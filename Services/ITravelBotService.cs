using KAHA.TravelBot.NETCoreReactApp.Models;

namespace KAHA.TravelBot.NETCoreReactApp.Services
{
    public interface ITravelBotService
    {
        Task<IEnumerable<CountryModel>> GetAllCountries();
        Task<CountrySummaryModel> GetCountrySummary(string countryName);
        Task<(string Sunrise, string Sunset)> GetSunriseSunsetTimes(CountryModel country);
        Task<IEnumerable<CountryModel>> GetTopFiveCountries();
        Task<CountryModel> RandomCountryInSouthernHemisphere();
        Task<CountrySummaryModel> GetRandomCountry();
    }
}