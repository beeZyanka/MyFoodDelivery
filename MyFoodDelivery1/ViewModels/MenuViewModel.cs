using System.ComponentModel.DataAnnotations;

namespace MyFoodDelivery1.ViewModels
{
    public class MenuViewModel
    {
        public int RestaurantId { get; set; }
        
        [Display(Name = "Ресторан")]
        public string RestaurantName { get; set; }
        [Display(Name = "Про ресторан")]
        public string RestaurantAbout { get; set; }


        public List<RestaurantDishViewModel> Dishes { get; set; }
    }
    public class RestaurantDishViewModel
    {
        public int DishId {  get; set; }
        
        [Display(Name = "Назва страви")]
        public string DishName { get; set; }
        
        [Display(Name = "Опис страви")]
        
        public string DishDescription { get; set; }
        [Display(Name = "Ціна (грн)")]
        
        public decimal Price { get; set; }
    }
}
