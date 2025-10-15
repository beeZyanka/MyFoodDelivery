using System.ComponentModel.DataAnnotations;

namespace MyFoodDelivery1.ViewModels
{
    
    public class CustomerOrderViewModel
    {

        [Display(Name = "# замовлення")]
        public int OrderId { get; set; }

        [Display(Name = "Загальна сума")]
        public decimal OrderTotalPrice { get; set; }

        [Display(Name = "Статус")]
        public string Status { get; set; }


        [Display(Name = "Ресторан")]
        public string RestaurantName { get; set; }


        [Display(Name = "Дата замовлення")]
        public string OrderDate { get; set; }


        public List<CustomerOrderItemViewModel> Items { get; set; }
    }
    public class CustomerOrderItemViewModel
    {

        [Display(Name = "Страви")]
        public string ItemName { get; set; }
        
        [Display(Name = "Кількість")]
        public int Cnt { get; set; }
        
        [Display(Name = "Сума")] 
        public decimal Price { get; set; }
    }
}
