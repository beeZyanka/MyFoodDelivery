using System.ComponentModel.DataAnnotations.Schema;

namespace MyFoodDelivery1.Models
{
    public class OrderItem
    {
        public int Id { get; set; }

        public int DishId { get; set; }
        public Dish Dish { get; set; }

        public int OrderId { get; set; }
        public Order Order { get; set; }

        public int Cnt { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
    }
}
