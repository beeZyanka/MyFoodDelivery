using MyFoodDelivery1.Models;

namespace MyFoodDelivery1.Services
{
    public class HelperService :  IHelper
    {
        
        public string GetOrderStatusUkrName(OrderStatus orderStatus)
        {
            string result = string.Empty;
            switch (orderStatus)
            {
                case OrderStatus.New:
                    result = "Нове";
                    break;
                case OrderStatus.InProgress:
                    result = "В роботі";
                    break;
                case OrderStatus.Completed:
                    result = "Виконано";
                    break;
                case OrderStatus.Cancelled:
                    result = "Скасовано";
                    break;
            }
            return result;
        }

        public string GetOrderStatusUkrName(string orderStatus)
        {
            string result = string.Empty;
            switch (orderStatus)
            {
                case "New":
                    result = "Нове";
                    break;
                case "InProgress":
                    result = "В роботі";
                    break;
                case "Completed":
                    result = "Виконано";
                    break;
                case "Cancelled":
                    result = "Скасовано";
                    break;
            }
            return result;
        }
    }
}
