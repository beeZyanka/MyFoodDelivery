using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyFoodDelivery1.Models
{
    public class Dish
    {
        public int Id { get; set; }

        [Display(Name = "Назва страви")] 
        public string Name { get; set; }
        
        [Display(Name = "Опис страви")] 
        public string Description { get; set; }

        [Display(Name = "Ціна (грн)")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [Display(Name = "Ресторан")] public int RestaurantId { get; set; }
        [Display(Name = "Ресторан")] public Restaurant? Restaurant { get; set; }
    }
}
