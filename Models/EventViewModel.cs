using System.ComponentModel.DataAnnotations;

namespace RaceDay.MVC.Models
{
    public class EventViewModel
    {
        public int EventID { get; set; }

        [Required(ErrorMessage = "Event name is required.")]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        [Required(ErrorMessage = "Location is required.")]
        public string Location { get; set; } = string.Empty;

        [Required]
        public DateTime EventDate { get; set; }

        [Required]
        public string EventType { get; set; } = "Run";

        public string? RouteDetails { get; set; }

        public string? WeatherInfo { get; set; }
    }
}