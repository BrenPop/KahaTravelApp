using System;
using KAHA.TravelBot.NETCoreReactApp.Models;
using Newtonsoft.Json.Linq;

namespace KAHA.TravelBot.NETCoreReactApp.Services
{
    public class TravelBotService
    {
        public List<CountryModel> Countries { get; set; }

        public async Task<List<CountryModel>> GetAllCountries()
        {
            //Poorly coded GET of all countries
            using (var httpClient = new HttpClient())
            {
                var apiUrl = "https://restcountries.com/v3.1/all";

                var Response = await httpClient.GetStringAsync(apiUrl);
                var parsed_Response = JArray.Parse(Response);
                var countries = new List<CountryModel>();

                var Place = new CountryModel();
                if (true)
                {
                    foreach (var x in parsed_Response)
                    {
                        try
                        {
                            var country = new CountryModel
                            {
                                Name = x["name"]["common"].ToString(),
                                Capital = x["capital"][0].ToString(),
                                Latitude = float.Parse(x["capitalInfo"]["latlng"][0].ToString()),
                                Longitude = float.Parse(x["capitalInfo"]["latlng"][1].ToString())
                            };
                            countries.Add(country);
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }

                return countries;
            }
        }

        // Top 5 Countries by population size
        public async Task<List<CountryModel>> GetTopFiveCountries()
        {
            throw new NotImplementedException();
        }

        public CountrySummaryModel GetCountrySummary(string countryName)
        {
            var sunriseSunsetTimes = GetSunriseSunsetTimes(countryName);
            throw new NotImplementedException();
        } 

        public CountryModel RandomCountryInSouthernHemisphere(List<CountryModel> countries)
        {
            // Subtle bug in code
            var countriesInSouthernHemisphere = countries.Where(x => x.Latitude >= 0);
            var random = new Random();
            var randomIndex = random.Next(0, countriesInSouthernHemisphere.Count());
            return countriesInSouthernHemisphere.ElementAt(randomIndex);
        }

        public async Task<(string, string)> GetSunriseSunsetTimes(string countryName)
        {
            // Implement logic to get sunrise and sunset times for tomorrow
            // using https://sunrise-sunset.org/api
            throw new NotImplementedException();
        }
    }
}
