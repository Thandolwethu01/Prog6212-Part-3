using System.ComponentModel.DataAnnotations;

namespace RaceDay.MVC.Models
{
    public class CategoryViewModel
    {
        public int CategoryID { get; set; }

        [Required(ErrorMessage = "Category name is required.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Distance is required.")]
        public double Distance { get; set; }

        public string? AgeRestriction { get; set; }
    }
}