using KAHA.TravelBot.NETCoreReactApp.Services;
using Microsoft.Extensions.Caching.Memory;
using NUnit.Framework;

namespace KAHA.TravelBot.NETCoreReactApp.Tests
{
    public class TravelBotServiceTests
    {
        TravelBotService travelBotService;
        HttpClient httpClient;
        ILogger<TravelBotService> ILogger;
        IMemoryCache IMemoryCache;

        [SetUp]
        public void Setup()
        {
            httpClient = new HttpClient();
            ILogger = new LoggerFactory().CreateLogger<TravelBotService>();
            IMemoryCache = new MemoryCache(new MemoryCacheOptions());

            travelBotService = new TravelBotService(httpClient, ILogger, IMemoryCache);
        }

        [Test]
        public async Task GetAllCountries_ReturnListOfCountries()
        {
            // Act
            var countries = await travelBotService.GetAllCountries();

            // Assert
            Assert.That(countries, Is.Not.Null);
            Assert.That(countries.Count, Is.GreaterThan(0));
        }

        [Test]
        public async Task GetAllCountries_NoExceptionsThrown()
        {
            // Act & Assert
            Assert.DoesNotThrowAsync(async () => await travelBotService.GetAllCountries());
        }

        [Test]
        public async Task GetTopFiveCountries_ReturnsTopFiveCountries()
        {
            // Act
            var topFiveCountries = await travelBotService.GetTopFiveCountries();

            // Assert
            Assert.That(topFiveCountries, Is.Not.Null);
            Assert.That(topFiveCountries.Count, Is.EqualTo(5));
        }

        [Test]
        public async Task GetTopFiveCountries_NoExceptionsThrown()
        {
            // Act & Assert
            Assert.DoesNotThrowAsync(async () => await travelBotService.GetTopFiveCountries());
        }

        [Test]
        public async Task GetCountrySummary_ReturnsCountrySummary()
        {
            // Act
            var countrySummary = await travelBotService.GetCountrySummary("United States");

            // Assert
            Assert.That(countrySummary, Is.Not.Null);
        }

        [Test]
        public async Task GetCountrySummary_NoExceptionsThrown()
        {
            // Act & Assert
            Assert.DoesNotThrowAsync(async () => await travelBotService.GetCountrySummary("United States"));
        }

        // Write NUnit test for the travelBotService.RandomCountryInSouthernHemisphere method
        [Test]
        public async Task RandomCountryInSouthernHemisphere_ReturnsCountry()
        {
            // Act
            var randomSouthernHemisphereCountry = travelBotService.RandomCountryInSouthernHemisphere();

            // Assert
            Assert.That(randomSouthernHemisphereCountry, Is.Not.Null);
        }

        [Test]
        public async Task RandomCountryInSouthernHemisphere_NoExceptionsThrown()
        {
            // Act & Assert
            Assert.DoesNotThrowAsync(async () => await travelBotService.RandomCountryInSouthernHemisphere());
        }

        // Write NUnit test for the TravelBotService.GetSunriseSunsetTimes method
        [Test]
        public async Task GetSunriseSunsetTimes_ReturnsSunriseSunsetTimes()
        {
            // Act
            var country = await travelBotService.GetRandomCountry();
            var sunriseSunsetTimes = await travelBotService.GetSunriseSunsetTimes(country);

            // Assert
            Assert.That(sunriseSunsetTimes, Is.Not.Null);
        }

        [Test]
        public async Task GetSunriseSunsetTimes_NoExceptionsThrown()
        {
            // Act & Assert
            Assert.DoesNotThrowAsync(async () => await travelBotService.GetSunriseSunsetTimes(await travelBotService.GetRandomCountry()));
        }

        // Write NUnit test for the TravelBotService.GetRandomCountry method
        [Test]
        public async Task GetRandomCountry_ReturnsCountry()
        {
            // Act
            var randomCountry = await travelBotService.GetRandomCountry();

            // Assert
            Assert.That(randomCountry, Is.Not.Null);
        }

        [Test]
        public async Task GetRandomCountry_NoExceptionsThrown()
        {
            // Act & Assert
            Assert.DoesNotThrowAsync(async () => await travelBotService.GetRandomCountry());
        }
    }
}
