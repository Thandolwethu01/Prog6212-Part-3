namespace RaceDay.MVC.Models
{
    public class MyResultViewModel
    {
        public int ResultID { get; set; }

        public int RegistrationID { get; set; }

        public string EventName { get; set; } = string.Empty;

        public string CategoryName { get; set; } = string.Empty;

        public TimeSpan FinishTime { get; set; }

        public int Position { get; set; }

        public string? SplitTimes { get; set; }
    }
}