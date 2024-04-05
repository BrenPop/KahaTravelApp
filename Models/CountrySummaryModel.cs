namespace KAHA.TravelBot.NETCoreReactApp.Models
{
    // TODO: CountrySummaryModel to be implemented
    public class CountrySummaryModel : CountryModel
    {
        public string? Sunrise {  get; set; }
        public string? Sunset { get; set; }
        public string? Area { get; set; }
        public string? CarSide { get; set; }
        public bool LandLocked { get; set; }
        public int LanguageCount { get; set; }
        public List<CountryLanguageModel>? Languages {  get; set; } = new List<CountryLanguageModel>();
        public int DistanceFromKAHA { get; set; }
    }
}
