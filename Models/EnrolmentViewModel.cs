using System.ComponentModel.DataAnnotations;

namespace RaceDay.MVC.Models
{
    public class EnrolmentViewModel
    {
        [Required]
        public int CategoryID { get; set; }
    }
}