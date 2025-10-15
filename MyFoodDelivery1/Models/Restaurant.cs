using System.ComponentModel.DataAnnotations;

namespace MyFoodDelivery1.Models
{
    public class Restaurant
    {
        [Key]
        public int Id { get; set; }

        [Required][Display(Name = "Назва ресторану")] public string Name { get; set; }

        [Required][Display(Name = "Адреса ресторану")] public string Address { get; set; }

        [Required][Display(Name = "Рейтинг")] public int Rating { get; set; }

        [Display(Name = "Про ресторан")] public string About { get; set; }

        
    }
}
