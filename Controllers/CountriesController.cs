using KAHA.TravelBot.NETCoreReactApp.Models;
using KAHA.TravelBot.NETCoreReactApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace KAHA.TravelBot.NETCoreReactApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountriesController : ControllerBase
    {
        readonly ITravelBotService travelBotService;

        public CountriesController(ITravelBotService travelBotService)
        {
            this.travelBotService = travelBotService;
        }

        // GET: api/countries/all
        [HttpGet("all")]
        public async Task<IEnumerable<CountryModel>> all()
        {
            return await travelBotService.GetAllCountries();
        }

        // GET: api/countries/top5
        [HttpGet("top5")]
        public async Task<IEnumerable<CountryModel>> GetTopFive()
        {
            return await travelBotService.GetTopFiveCountries();
        }

        // GET api/countries/South Africa
        [HttpGet("{countryName}")]
        public async Task<CountrySummaryModel> GetSummary(string countryName)
        {
            return await travelBotService.GetCountrySummary(countryName);
        }

        // POST api/countries/random
        [HttpGet("random")]
        public async Task<CountrySummaryModel> GetRandomCountry()
        {
            return await travelBotService.GetRandomCountry();
        }
    }
}
