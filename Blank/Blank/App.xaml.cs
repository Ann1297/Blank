using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace Blank
{
    public partial class App : Application
    {
        static public RestService Service { get; set; }

        public App()
        {
            InitializeComponent();

            Service = new RestService();
            MainPage = new NavigationPage(new FillingPage());
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
