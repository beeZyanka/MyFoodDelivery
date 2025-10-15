using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyFoodDelivery1.Models;
using MyFoodDelivery1.ViewModels;

namespace MyFoodDelivery1.Controllers
{
    public class DishesController : Controller
    {
        private readonly AppDbContext _context;

        public DishesController(AppDbContext context)
        {
            _context = context;
        }
        //public IActionResult MenuOld(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }
        //    var dishes = _context.Dishes.Where(p => p.RestaurantId == id).Include(d => d.Restaurant);

        //    if (dishes == null)
        //    {
        //        return NotFound();
        //    }
           
        //    var rest = _context.Restaurants.FirstOrDefault(r => r.Id == id);
        //    ViewBag.RestaurantId = rest.Id;
        //    ViewBag.RestaurantName = rest.Name;
        //    ViewBag.About = rest.About;
        //    ViewBag.ActivePage = 0;
        //    return View(dishes);

        //}
        public IActionResult Menu(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            var dishes = _context.Dishes.Where(p => p.RestaurantId == id).Include(d => d.Restaurant);
            var rest = _context.Restaurants.FirstOrDefault(r => r.Id == id);
            if (dishes == null)
            {
                return NotFound();
            }
            var model = new MenuViewModel { 
                RestaurantId = id,
                RestaurantName = rest.Name,
                RestaurantAbout = rest.About,
                Dishes = new List<RestaurantDishViewModel>()

               
            };
            foreach (var d in dishes)
            {
                model.Dishes.Add(new RestaurantDishViewModel
                {
                    DishId = d.Id,
                    DishName = d.Name,
                    DishDescription = d.Description,
                    Price = d.Price
                });
            }
            
            ViewBag.ActivePage = 0;
            return View(model);



        }


    }
}
