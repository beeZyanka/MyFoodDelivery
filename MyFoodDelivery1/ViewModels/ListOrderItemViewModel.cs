using MyFoodDelivery1.Models;
using MyFoodDelivery1.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace MyFoodDelivery1.ViewModels
{
    public class ListOrderItemViewModel
    {
        public List<OrderItemViewModel> Items { get; set; }
        [Display(Name = "Ресторан")]
        public string Restaurant { get; set; }
    }
    public class OrderItemViewModel
    {
        public int ItemId { get; set; }

        [Display(Name = "Страва")]
        public string ItemName { get; set; }

        [Display(Name = "Ціна (грн)")] 
        public decimal Price { get; set; }

        [Display(Name = "Кількість")] public int Cnt { get; set; }
    }






}
