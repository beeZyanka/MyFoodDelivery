using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyFoodDelivery1.Models
{
    public class Order
    {
        public int Id { get; set; }

        [Display(Name = "Клієнт")] public int CustomerId {  get; set; }
        public Customer? Customer { get; set; }

        public DateTime OrderDate { get; set; }
        public OrderStatus Status { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal PriceTotal {  get; set; }

        public List<OrderItem> OrderItems { get; set; }
    }

    
}
