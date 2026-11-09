namespace RaceDay.MVC.Models
{
    public class MyEnrolmentViewModel
    {
        public int RegistrationID { get; set; }

        public DateTime RegistrationDate { get; set; }

        public string PaymentStatus { get; set; } = string.Empty;

        public string CategoryName { get; set; } = string.Empty;

        public double Distance { get; set; }

        public int EventID { get; set; }

        public string EventName { get; set; } = string.Empty;

        public DateTime EventDate { get; set; }

        public string EventLocation { get; set; } = string.Empty;
    }
}