namespace RaceDay.MVC.Models
{
    public class EventDetailsViewModel
    {
        public int EventID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string Location { get; set; } = string.Empty;
        public DateTime EventDate { get; set; }
        public string EventType { get; set; } = string.Empty;
        public string? RouteDetails { get; set; }
        public string? WeatherInfo { get; set; }

        public List<CategoryViewModel> Categories { get; set; } = new();
    }
}