using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyFoodDelivery1.Models;
using MyFoodDelivery1.Services;
using MyFoodDelivery1.ViewModels;
using static NuGet.Packaging.PackagingConstants;

namespace MyFoodDelivery1.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;
        //масив для позначення рейтингу ресторану
        private readonly int[] _rateRestaurant = { 1, 2, 3, 4, 5 };

        public AdminController(AppDbContext context)
        {
            _context = context;


        }

        public IActionResult Index()
        {
            return View();
        }
       
        #region Адміністрування ресторанів
        //повертає сторінку з загальним списком ресторанів (для адміністратора)
        public async Task<IActionResult> RestaurantTotal()
        {
            //ActivePage використовується в Layout для виділення активних вкладок
            //0 - ресторани; 1 - меню; 2 - замовлення;
            ViewBag.ActivePage = 0;
            return View(await _context.Restaurants.ToListAsync());
        }

        //повертає сторінку для введення ресторану
        public IActionResult RestaurantCreate()
        {
            //Передаємо на view selectlist рейтингу
            ViewData["Rating"] = new SelectList(_rateRestaurant, _rateRestaurant[4]);
            return View();
        }

        //пост метод введення ресторану
        [HttpPost]
        public async Task<IActionResult> RestaurantCreate(Restaurant restaurant)
        {
            //Якщо модель валідна
            if (ModelState.IsValid)
            {
                //додаємо введений ресторан
                //зберігаємо в БД
                //переходимо до action RestaurantTotal
                _context.Add(restaurant);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(RestaurantTotal));
            }
            //ні - повертаємо на сторінку вводу, з урахуванням введеного рейтингу
            ViewData["Rating"] = new SelectList(_rateRestaurant, restaurant.Rating);
            return View(restaurant);
        }
        
        //повертає сторінку видалення ресторану
        public async Task<IActionResult> RestaurantDelete(int id)
        {
            //перевіряємо на існування обраного ресторану в БД            
            var restaurant = await _context.Restaurants
                .FirstOrDefaultAsync(m => m.Id == id);
            if (restaurant == null)
            {
                return NotFound();
            }
            //перевіряємо чи є замовлення по даному ресторану
            var xx = _context.OrderItems
                .AsNoTracking()
                .Include(o => o.Dish)
                    .ThenInclude(r => r.Restaurant)
                .ToList().Any(oi => (oi.Dish.Restaurant!=null && oi.Dish.Restaurant.Id == id));
            if (xx)
            {
                ViewBag.Obj = 0; //0 -ресторан; 1 -меню
                return View("CantDel");
            }
            else
            { 
                return View(restaurant);
            }
        }
        public IActionResult CantDel()
        {
            return View();
        }
        //Пост метод видалення ресторану
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            
            //видаляємо з БД
            var restaurant = await _context.Restaurants.FindAsync(id);
            if (restaurant != null)
            {
                _context.Restaurants.Remove(restaurant);
            }
            //зберігаємо в БД
            await _context.SaveChangesAsync();

            //переходимо до action RestaurantTotal
            return RedirectToAction(nameof(RestaurantTotal));
        }

        //повертає сторінку редагування ресторану
        public async Task<IActionResult> RestaurantEdit(int id)
        {

            var restaurant = await _context.Restaurants.FindAsync(id);
            if (restaurant == null)
            {
                return NotFound();
            }
            ViewData["Rating"] = new SelectList(_rateRestaurant, restaurant.Rating);
            return View(restaurant);
        }

        //Пост метод редагування ресторану
        [HttpPost]
        public async Task<IActionResult> RestaurantEdit(int id, Restaurant restaurant)
        {
            //перевірка моделі на валідність
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(restaurant);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Restaurants.Any(e => e.Id == restaurant.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(RestaurantTotal));
            }
            ViewData["Rating"] = new SelectList(_rateRestaurant, restaurant.Rating);
            return View(restaurant);
        }
        #endregion

        #region Адміністрування меню
        //повертає сторінку з загальним списком меню (для адміністратора)
        
        public async Task<IActionResult> DishTotal()
        {
                var appDbContext = _context.Dishes
                                            .Include(d => d.Restaurant)
                                            .OrderBy(s => s.RestaurantId);
                //для виділення активним вкладки "меню" в layout
                ViewBag.ActivePage = 1;
                return View(await appDbContext.ToListAsync());
        }
        #region Test
        //public async Task<IActionResult> DishTotalTest(int? id)
        //{
        //    //var appDbContext;
        //    if (id == null || id == 0)
        //    {
        //        var appDbContext = _context.Dishes
        //                                    .Include(d => d.Restaurant)
        //                                    .OrderBy(s => s.RestaurantId);
        //        //для виділення активним вкладки "меню" в layout
        //        ViewBag.ActivePage = 1;
        //        ViewBag.SingleRest = 0;
        //        return View(await appDbContext.ToListAsync());

        //    }
        //    else
        //    {
        //        //обираємо меню одного ресторана з БД
        //        var appDbContext = _context.Dishes
        //                                    .Where(p => p.RestaurantId == id)
        //                                    .Include(d => d.Restaurant)
        //                                    .OrderBy(s => s.RestaurantId);
        //        //для виділення активним вкладки "меню" в layout
        //        ViewBag.ActivePage = 1;
        //        //змінна, що ідентифікує одиничний ресторан
        //        ViewBag.SingleRest = 1;
        //        ViewBag.RestId = id;
        //        ViewBag.RestName = appDbContext.First().Restaurant.Name;
        //        return View(await appDbContext.ToListAsync());

        //    }

        //}
        //public IActionResult DishCreateTest(int? rest)
        //{
        //    //список ресторанів на вью
        //    ViewData["RestaurantId"] = new SelectList(_context.Restaurants, "Id", "Name", rest);
        //    ViewBag.RestId = rest;
        //    return View();
        //}
        #endregion
        //сторінка створення нової страви
        public IActionResult DishCreate()
        {
            //список ресторанів на вью
            ViewData["RestaurantId"] = new SelectList(_context.Restaurants, "Id", "Name");
            
            return View();
        }

        //пост метод додавання страви
        [HttpPost]
        public async Task<IActionResult> DishCreate(Dish dish)
        {
            dish.Restaurant = _context.Restaurants.FirstOrDefault(m => m.Id == dish.RestaurantId);

            if (ModelState.IsValid)
            {
                _context.Add(dish);
                await _context.SaveChangesAsync();
                return RedirectToAction("DishTotal", new { id = dish.RestaurantId });
            }
            ViewData["RestaurantId"] = new SelectList(_context.Restaurants, "Id", "Name", dish.RestaurantId);
            return View(dish);
        }

        public async Task<IActionResult> DishDelete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dish = await _context.Dishes
                .Include(d => d.Restaurant)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dish == null)
            {
                return NotFound();
            }
            //перевіряємо чи є замовлення по даній страві
            var xx = _context.OrderItems
                .AsNoTracking()
                .Include(o => o.Dish)
                .ToList().Any(oi => (oi.Dish != null && oi.Dish.Id == id));
            if (xx)
            {
                ViewBag.Obj = 1; //0 -ресторан; 1 -меню
                return View("CantDel");
            }
            else
            {
                return View(dish);
            }
            return View(dish);
        }

        [HttpPost, ActionName("DishDelete")]
        public async Task<IActionResult> DishDeleteConfirmed(int id)
        {
            var dish = await _context.Dishes.FindAsync(id);
            if (dish != null)
            {
                _context.Dishes.Remove(dish);
            }

            await _context.SaveChangesAsync();
            
            return RedirectToAction("DishTotal", new { id = dish.RestaurantId });
        }
     
        public async Task<IActionResult> DishEdit(int id, int rest)
        {
            var dish = await _context.Dishes.FindAsync(id);
            if (dish == null)
            {
                return NotFound();
            }
            //ViewData["RestaurantId"] = new SelectList(_context.Restaurants, "Id", "Name", dish.RestaurantId);
            ViewBag.RestId = rest;
            ViewBag.RestName = _context.Restaurants.First(r => r.Id == rest).Name;
            return View(dish);
        }
        
        [HttpPost]
        public async Task<IActionResult> DishEdit(int id, int rest, Dish dish)
        {
            if (id != dish.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    dish.RestaurantId = rest;
                    _context.Update(dish);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Restaurants.Any(e => e.Id == dish.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(DishTotal));
            }
            //ViewData["RestaurantId"] = new SelectList(_context.Restaurants, "Id", "Name", dish.RestaurantId);
            ViewBag.RestId = rest;
            ViewBag.RestName = _context.Restaurants.First(r => r.Id == rest).Name;
            return View(dish);
        }
        #endregion

        #region Адміністрування замовлень
        public async Task<IActionResult> OrdersTotal()
        {
            var appDbContext = _context.Orders
                                .AsNoTracking()
                                .Include(c => c.Customer)
                                .Include(oi => oi.OrderItems)
                                    .ThenInclude(od => od.Dish)
                                    .ThenInclude(dr => dr.Restaurant)
                                    .OrderBy(s => s.CustomerId)
                                .ThenBy(s => s.Status).ThenByDescending(s => s.OrderDate)
                ;
            ViewBag.ActivePage = 2;
            return View(await appDbContext.ToListAsync());
        }

        
        public IActionResult OrderAccept(int id)
        {
            var order = _context.Orders.Find(id);
            if (order != null && (order.Status == OrderStatus.New || order.Status == OrderStatus.Cancelled))
            {
                order.Status = OrderStatus.InProgress;
                _context.SaveChanges();
            }
            return RedirectToAction("OrdersTotal");
        }

        
        public IActionResult OrderComplete(int id)
        {
            var order = _context.Orders.Find(id);
            if (order != null && order.Status == OrderStatus.InProgress)
            {
                order.Status = OrderStatus.Completed;
                _context.SaveChanges();
            }
            return RedirectToAction("OrdersTotal");
        }

       
        public IActionResult OrderReject(int id)
        {
            var order = _context.Orders.Find(id);
            if (order != null && (order.Status == OrderStatus.InProgress || order.Status == OrderStatus.New))
            {
                order.Status = OrderStatus.Cancelled;
                _context.SaveChanges();
            }
            return RedirectToAction("OrdersTotal");
        }
        #endregion

        

    }
}
