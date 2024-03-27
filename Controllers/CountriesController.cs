using KAHA.TravelBot.NETCoreReactApp.Models;
using KAHA.TravelBot.NETCoreReactApp.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace KAHA.TravelBot.NETCoreReactApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountriesController : ControllerBase
    {
        // GET: api/<CountriesController>
        [HttpGet("top5")]
        public async Task<IEnumerable<CountryModel>> GetTopFive()
        {
            var travelBotService = new TravelBotService();
            return await travelBotService.GetTopFiveCountries();
        }

        // GET api/<CountriesController>/Zimbabwe
        [HttpGet("{countryName}")]
        public string GetSummary(string countryName)
        {
            return "value";
        }

        // POST api/<CountriesController>
        [HttpGet("random")]
        public void GetRandomCountry()
        {
        }
    }
}
