using Microsoft.AspNetCore.Mvc;
using MyFoodDelivery1.Models;

namespace MyFoodDelivery1.Controllers
{
    public class HomeController : Controller
    {
        public HomeController(AppDbContext context)
        {
            Context = context;
        }

        public AppDbContext Context { get; }

        public IActionResult Index()
        {
            
            ViewBag.ActivePage = 0;
            return View();
        }
        
        
    }
}
