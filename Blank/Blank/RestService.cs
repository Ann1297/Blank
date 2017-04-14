using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Diagnostics;
using Newtonsoft.Json.Linq;

namespace Blank
{
    public class RestService
    {
        HttpClient client;

        public RestService()
        {
            client = new HttpClient();
        }

        public async Task<List<Country>> GetCountriesAsync()
        {
            List<Country> countries = new List<Country>();

            var uri = "https://api.vk.com/method/database.getCountries?need_all=1&count=236&lang=ru&v=5.6";

            var response = await client.GetAsync(new Uri(uri));
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                JObject json = JObject.Parse(content);

                // get JSON items objects into a list
                List<JToken> items = json["response"]["items"].Children().ToList();

                // serialize JSON items into .NET objects                
                foreach (JToken item in items)
                {
                    Country country = item.ToObject<Country>();
                    countries.Add(country);
                }
            }

            return countries;
        }

        public async Task<List<City>> GetCtiesAsync(string query)
        {
            List<City> cities = new List<City>();

            if (FillingPage.ContactData.Country != null)
            {
                string uri;

                if (String.IsNullOrWhiteSpace(query))
                {
                    uri = "https://api.vk.com/method/database.getCities?need_all=0&country_id="
                        + FillingPage.ContactData.Country.Id + "&lang=ru&v=5.6";
                }
                else
                {
                    uri = "https://api.vk.com/method/database.getCities?q=" + query
                        + "&need_all=1&country_id=" + FillingPage.ContactData.Country.Id
                        + "&lang=ru&v=5.6";
                }

                var response = await client.GetAsync(new Uri(uri));
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    JObject json = JObject.Parse(content);

                    // get JSON items objects into a list
                    List<JToken> items = json["response"]["items"].Children().ToList();

                    // serialize JSON items into .NET objects                
                    foreach (JToken item in items)
                    {
                        City city = item.ToObject<City>();
                        cities.Add(city);
                    }
                }
            }

            return cities;
        }

        public async Task<List<University>> GetUniversitiesListAsync(string query)
        {
            List<University> universities = new List<University>();

            if (FillingPage.ContactData.City != null && FillingPage.ContactData.Country != null)
            {
                string uri;
                if (!String.IsNullOrWhiteSpace(query))
                {
                    uri = "https://api.vk.com/method/database.getUniversities?q=" + query
                        + "&country_id=" + FillingPage.ContactData.Country.Id
                        + "&city_id=" + FillingPage.ContactData.City.Id + "&lang=ru&v=5.6";
                }
                else
                {
                    uri = "https://api.vk.com/method/database.getUniversities?country_id="
                        + FillingPage.ContactData.Country.Id + "&city_id="
                        + FillingPage.ContactData.City.Id + "&lang=ru&v=5.6";
                }

                var response = await client.GetAsync(new Uri(uri));
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    JObject json = JObject.Parse(content);

                    // get JSON items objects into a list
                    List<JToken> items = json["response"]["items"].Children().ToList();

                    // serialize JSON items into .NET objects                
                    foreach (JToken item in items)
                    {
                        University univer = item.ToObject<University>();
                        universities.Add(univer);
                    }
                }
            }

            return universities;
        }
    }
}
