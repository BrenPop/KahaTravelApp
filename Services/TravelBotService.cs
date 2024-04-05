using KAHA.TravelBot.NETCoreReactApp.Models;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace KAHA.TravelBot.NETCoreReactApp.Services
{
    public class TravelBotService : ITravelBotService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<TravelBotService> _logger;
        private readonly IMemoryCache _memoryCache;

        private const string CountriesApiUrl = "https://restcountries.com/v3.1";
        private const string SunriseSunsetApiUrl = "https://api.sunrise-sunset.org/json";

        public List<CountryModel> Countries { get; set; }
        private static DateTime CacheExpiration;

        public TravelBotService(HttpClient httpClient, ILogger<TravelBotService> logger, IMemoryCache memoryCache)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
        }

        public async Task<IEnumerable<CountryModel>> GetAllCountries()
        {
            try
            {
                var cacheKey = $"all_countries_" + DateTime.Today.ToString("yyyy-MM-dd");

                if (!_memoryCache.TryGetValue(cacheKey, out IEnumerable<CountryModel> countries))
                {
                    var response = await _httpClient.GetStringAsync($"{CountriesApiUrl}/all");
                    var parsedResponse = JArray.Parse(response);
                    countries = parsedResponse.Select(c => ParseCountryModel(c)).ToList();
                    _memoryCache.Set(cacheKey, countries);
                }

                return countries;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving countries: {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<CountryModel>> GetTopFiveCountries()
        {
            try
            {
                var allCountries = await GetAllCountries();

                return allCountries.OrderByDescending(c => c.Population).Take(5);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving top 5 countries: {ex.Message}");

                throw;
            }
        }

        public async Task<CountrySummaryModel> GetCountrySummary(string countryName)
        {
            try
            {
                var countrySummary = await GetCountryByName(countryName);
                if (countrySummary == null)
                    return null;

                var (sunrise, sunset) = await GetSunriseSunsetTimes(countrySummary);
                countrySummary.Sunrise = sunrise;
                countrySummary.Sunset = sunset;
                countrySummary.DistanceFromKAHA = CalculateDistanceFromKahaInKm(countrySummary);

                return countrySummary;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving country summary: {ex.Message}");

                throw;
            }
        }

        public async Task<CountryModel> RandomCountryInSouthernHemisphere()
        {
            var countries = await GetAllCountries();
            var random = new Random();
            var countriesInSouthernHemisphere = countries.Where(x => x.Latitude <= 0).ToList();
            var randomIndex = random.Next(0, countriesInSouthernHemisphere.Count);

            return countriesInSouthernHemisphere[randomIndex];
        }

        public async Task<(string Sunrise, string Sunset)> GetSunriseSunsetTimes(CountryModel country)
        {
            try
            {
                var apiUrl = $"{SunriseSunsetApiUrl}?lat={country.Latitude}&lng={country.Longitude}&date=today&tzid=Africa/Johannesburg";
                var response = await _httpClient.GetAsync(apiUrl);
                response.EnsureSuccessStatusCode();
                var sunsetSunriseApiResponse = JsonConvert.DeserializeObject<SunsetSunriseApiResponse>(await response.Content.ReadAsStringAsync());

                return (sunsetSunriseApiResponse?.Results?.Sunrise ?? string.Empty, sunsetSunriseApiResponse?.Results?.Sunset ?? string.Empty);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving sunrise/sunset times: {ex.Message}");

                throw;
            }
        }

        public async Task<CountrySummaryModel> GetRandomCountry()
        {
            IEnumerable<CountryModel> allCountries = await GetAllCountries();

            var random = new Random();
            var randomIndex = random.Next(0, allCountries.ToList().Count);
            var randomCountry = allCountries.ToList()[randomIndex];

            return await GetCountrySummary(randomCountry.Name);
        }

        private async Task<CountrySummaryModel> GetCountryByName(string countryName)
        {
            try
            {
                var response = await _httpClient.GetStringAsync($"{CountriesApiUrl}/name/{countryName}");
                var parsedResponse = JArray.Parse(response);

                return parsedResponse.Select(ParseCountrySummaryModel).FirstOrDefault();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving country {countryName}: {ex.Message}");

                return null;
            }
        }

        private CountryModel ParseCountryModel(JToken token)
        {
            return new CountryModel
            {
                Name = token["name"]?["common"]?.ToString(),
                Capital = token["capital"]?[0]?.ToString(),
                Population = token["population"]?.ToObject<int>() ?? 0,
                Latitude = token["capitalInfo"]?["latlng"]?[0]?.ToObject<float>() ?? 0f,
                Longitude = token["capitalInfo"]?["latlng"]?[1]?.ToObject<float>() ?? 0f,
                TimeZone = token["timezones"]?[0]?.ToString() ?? "Africa/Johannesburg"
            };
        }

        private CountrySummaryModel ParseCountrySummaryModel(JToken token)
        {
            var countrySummaryModel = new CountrySummaryModel
            {
                Name = token["name"]?["common"]?.ToString(),
                Capital = token["capital"]?[0]?.ToString(),
                Population = token["population"]?.ToObject<int>() ?? 0,
                Latitude = token["capitalInfo"]?["latlng"]?[0]?.ToObject<float>() ?? 0f,
                Longitude = token["capitalInfo"]?["latlng"]?[1]?.ToObject<float>() ?? 0f,
                Area = token["area"]?.ToString(),
                CarSide = token["car"]?["side"]?.ToString(),
                LandLocked = token?["landlocked"]?.ToObject<bool>() ?? false,
            };

            foreach (var l in token?["languages"])
            {
                CountryLanguageModel language = new CountryLanguageModel
                {
                    Name = l.First().ToString(),
                };

                countrySummaryModel?.Languages?.Add(language);
            }

            countrySummaryModel.LanguageCount = countrySummaryModel.Languages.Count;

            return countrySummaryModel;
        }

        private int CalculateDistanceFromKahaInKm(CountryModel country)
        {
            var kahaLatitude = -33.9759679f;
            var kahaLongitude = 18.466389f;

            // calculate distance from country longitude and latitude
            var distanceInKm = (int)Math.Round(
                6371 * Math.Acos(
                    Math.Sin(country.Latitude * Math.PI / 180) * Math.Sin(kahaLatitude * Math.PI / 180) +
                    Math.Cos(country.Latitude * Math.PI / 180) * Math.Cos(kahaLatitude * Math.PI / 180) *
                    Math.Cos((kahaLongitude - country.Longitude) * Math.PI / 180)));

            return distanceInKm;
        }
    }
}
