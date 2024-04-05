using KAHA.TravelBot.NETCoreReactApp.Models;
using KAHA.TravelBot.NETCoreReactApp.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

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

        // GET: api/<CountriesController>
        [HttpGet("all")]
        public async Task<IEnumerable<CountryModel>> all()
        {
            return await travelBotService.GetAllCountries();
        }

        // GET: api/<CountriesController>
        [HttpGet("top5")]
        public async Task<IEnumerable<CountryModel>> GetTopFive()
        {
            return await travelBotService.GetTopFiveCountries();
        }

        // GET api/<CountriesController>/summary/Zimbabwe
        [HttpGet("summary/{countryName}")]
        public async Task<CountrySummaryModel> GetSummary(string countryName)
        {
            return await travelBotService.GetCountrySummary(countryName);
        }

        // POST api/<CountriesController>
        [HttpGet("random")]
        public async Task<CountrySummaryModel> GetRandomCountry()
        {
            return await travelBotService.GetRandomCountry();
        }
    }
}
