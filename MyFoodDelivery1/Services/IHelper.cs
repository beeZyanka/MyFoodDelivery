using MyFoodDelivery1.Models;

namespace MyFoodDelivery1.Services
{
    public interface IHelper
    {
        
        public string GetOrderStatusUkrName(OrderStatus orderStatus);
        public string GetOrderStatusUkrName(string orderStatus);
    }
}
