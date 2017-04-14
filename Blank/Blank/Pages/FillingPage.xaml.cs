using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Blank
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FillingPage : ContentPage
    {
        //public fields
        public static Contact ContactData { get; set; }

        //private variables
        private List<Country> countriesList;
        private List<City> citiesList;
        private List<University> universitiesList;
        private List<Entry> entryList;

        public FillingPage()
        {
            InitializeComponent();

            //create new instance of Contact to do binding for FillingPage and DataPage
            ContactData = new Contact();
            BindingContext = ContactData;

            //get all Entries from StackLayout into one list
            //not very elegant solution -__-
            entryList = new List<Entry>();
            foreach (var children in stackLayout.Children)
            {
                StackLayout layout;
                if (children is StackLayout)
                {
                    layout = children as StackLayout;
                    foreach (var c in layout.Children)
                    {
                        if (c is Entry)
                        {
                            entryList.Add(c as Entry);
                        }
                    }
                }
            }

            //subscribe one event handler for all entires in list
            foreach (var entry in entryList)
            {
                entry.TextChanged += Entry_TextChanged;
            }
        }

        private void Entry_TextChanged(object sender, TextChangedEventArgs e)
        {
            int currentEntryIndex = entryList.IndexOf(sender as Entry);

            if (currentEntryIndex < entryList.Count - 1) //handle case when the current entry is the last
            {
                if (!String.IsNullOrEmpty(e.NewTextValue))
                {
                    //I dont know if to do binding
                    entryList[currentEntryIndex + 1].IsEnabled = true;
                }
                else
                {
                    entryList[currentEntryIndex + 1].IsEnabled = false;
                }
            }
        }

        private void OnNavigateButtonClicked(object sender, EventArgs e)
        {
            var dataPage = new DataPage();
            dataPage.BindingContext = ContactData;

            Navigation.PushAsync(dataPage);
        }

        private void ListViewManage(Button button, ListView listView)
        {
            //Change the picture on button when button is clicked
            button.Text = (button.Text == "Λ") ? button.Text = "V" : button.Text = "Λ";

            //Hide or show countriesListView when button is clicked
            listView.IsVisible = !listView.IsVisible;
        }

        #region Countries 
        private void OnCountriesEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            List<string> countriesTitles;

            if (!String.IsNullOrEmpty(citiesEntry.Text))
                citiesEntry.Text = String.Empty;

            if (countriesList != null)
            {
                countriesTitles = new List<string>(countriesList.Select(i => i.Title));

                countriesListView.BeginRefresh();

                if (String.IsNullOrWhiteSpace(e.NewTextValue))
                    countriesListView.ItemsSource = countriesTitles;
                else
                    countriesListView.ItemsSource = countriesTitles.Where(i => i.Contains(e.NewTextValue));

                countriesListView.EndRefresh();
            }
        }

        private async void OnCountriesEntry_Focused(object sender, FocusEventArgs e)
        {
            countriesList = await App.Service.GetCountriesAsync();

            if (countriesList != null) //refresh countriesListView when countriesList is got
            {
                countriesListView.BeginRefresh();
                countriesListView.ItemsSource = countriesList.Select(i => i.Title);
                countriesListView.EndRefresh();
            }

            if (!countriesListView.IsVisible)
                ListViewManage(countriesButton, countriesListView);
        }

        private void OnCountriesEntry_Unfocused(object sender, FocusEventArgs e)
        {
            if (countriesListView.IsVisible)
                ListViewManage(countriesButton, countriesListView);
        }

        private void OnCountriesButton_Clicked(object sender, EventArgs e)
        {
            ListViewManage(sender as Button, countriesListView);
        }
                
        private void OnCountriesListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            countriesEntry.Text = e.SelectedItem.ToString();

            if (countriesListView.IsVisible)
                ListViewManage(countriesButton, sender as ListView);

            ContactData.Country = countriesList.Where(i => i.Title.Equals(countriesEntry.Text)).FirstOrDefault();
        }        
        #endregion

        #region Cities 
        private async void OnCitiesEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            citiesList = await App.Service.GetCtiesAsync(e.NewTextValue);

            universitiesEntry.Text = String.Empty;

            if (citiesList != null)
            {
                citiesListView.BeginRefresh();
                citiesListView.ItemsSource = new List<string>(citiesList.Select(i => i.Title));
                citiesListView.EndRefresh();
            }
        }

        private async void OnCitiesEntry_Focused(object sender, FocusEventArgs e)
        {
            citiesList = await App.Service.GetCtiesAsync(citiesEntry.Text);
                        
            if (citiesList != null)
            {
                citiesListView.BeginRefresh();
                citiesListView.ItemsSource = citiesList.Select(i => i.Title);
                citiesListView.EndRefresh();
            }

            //show citiesListView if it's hidden when citiesEntry is focused
            if (!citiesListView.IsVisible)
                ListViewManage(citiesButton, citiesListView);
        }

        private void OnCitiesEntry_Unfocused(object sender, FocusEventArgs e)
        {
            //hide citiesListView if it's shown when citiesEntry is focused
            if (citiesListView.IsVisible)
                ListViewManage(citiesButton, citiesListView);
        }

        private void OnCitiesButton_Clicked(object sender, EventArgs e)
        {
            ListViewManage(sender as Button, citiesListView);
        }

        private void OnCitiesListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            citiesEntry.Text = e.SelectedItem.ToString();

            if (citiesListView.IsVisible)
                ListViewManage(citiesButton, sender as ListView);

            ContactData.City = citiesList.Where(i => i.Title.Equals(citiesEntry.Text)).FirstOrDefault();
        }
        #endregion

        #region Universities
        private async void OnUniversitiesEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            universitiesList = await App.Service.GetUniversitiesListAsync(e.NewTextValue);
                        
            if (universitiesList != null)
            {
                universitiesListView.BeginRefresh();
                universitiesListView.ItemsSource = universitiesList.Select(i => i.Title);
                universitiesListView.EndRefresh();
            }
        }

        private async void OnUniversitiesEntry_Focused(object sender, FocusEventArgs e)
        {
            universitiesList = await App.Service.GetUniversitiesListAsync(universitiesEntry.Text);
                        
            if (universitiesList != null)
            {
                universitiesListView.BeginRefresh();
                universitiesListView.ItemsSource = universitiesList.Select(i => i.Title);
                universitiesListView.EndRefresh();
            }

            if (!universitiesListView.IsVisible)
                ListViewManage(universitiesButton, universitiesListView);
        }

        private void OnUniversitiesEntry_Unfocused(object sender, FocusEventArgs e)
        {
            if (universitiesListView.IsVisible)
                ListViewManage(universitiesButton, universitiesListView);
        }

        private void OnUniversitiesButton_Clicked(object sender, EventArgs e)
        {
            ListViewManage(sender as Button, universitiesListView);
        }
        
        private void OnUniversitiesListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            universitiesEntry.Text = e.SelectedItem.ToString();

            if (universitiesListView.IsVisible)
                ListViewManage(universitiesButton, universitiesListView);

            ContactData.University = universitiesList.Where(i => i.Title.Equals(universitiesEntry.Text)).FirstOrDefault();
        }
        #endregion
    }
}
