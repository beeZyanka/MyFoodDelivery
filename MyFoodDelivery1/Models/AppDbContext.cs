using Microsoft.EntityFrameworkCore;

namespace MyFoodDelivery1.Models
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<Restaurant> Restaurants { get; set; }

        public DbSet<Dish> Dishes { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderItem> OrderItems { get; set; }
    }
}
