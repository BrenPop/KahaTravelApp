namespace KAHA.TravelBot.NETCoreReactApp.Models
{
    public class SunsetSunriseApiResponse
    {
        public SunriseSunsetResults? Results { get; set; }
        public string? Status { get; set; }
        public string? Tzid { get; set; }
    }
}
