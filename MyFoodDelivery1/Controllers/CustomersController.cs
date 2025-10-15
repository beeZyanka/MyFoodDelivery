using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyFoodDelivery1.Models;
using MyFoodDelivery1.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFoodDelivery1.Controllers
{
    public class CustomersController : Controller
    {
        private readonly AppDbContext _context;

        public CustomersController(AppDbContext context)
        {
            _context = context;
        }
        
        [HttpGet]
        public IActionResult PlaceOrder(int? id)
        {
            // Перевіряємо чи авторизовано користувача
            var customerIdClaim = User.FindFirst("Id");
            if (customerIdClaim == null)
            {
                return RedirectToAction("Login","Account");
            }
            // Обираємо страви обраного ресторана
            var items = _context.Dishes
                .Where(p => p.RestaurantId == id).Include(d => d.Restaurant)
                .ToList();
            // Мапимо вью-модель
            var model = new ListOrderItemViewModel
            {
                Items = items.Select(i => new OrderItemViewModel
                {
                    ItemId = i.Id,
                    ItemName = i.Name,
                    Price = i.Price,
                    Cnt = 0
                }).ToList(),
                Restaurant = _context.Restaurants.First(r => r.Id == id).Name
            };
            
            return View(model);
        }
        
        [HttpPost]
        public IActionResult PlaceOrder(ListOrderItemViewModel model)
        {
            // перевіряємо автентифікацію 
            var customerIdClaim = User.FindFirst("Id");
            if (customerIdClaim == null)
            {
                return RedirectToAction("Login", "Account");
            }
            int customerId = int.Parse(customerIdClaim.Value);

            // обираємо ті страви, що мають не нульову кількість
            var selectedItems = model.Items.Where(x => x.Cnt > 0).ToList();
            if (!selectedItems.Any())
            {
                ModelState.AddModelError("", "Жодної страви не обрано, будь ласка, зробіть свій вибір");
                return View(model); 
            }
            // Формуємо модель нового замовлення
            var order = new Order
            {
                CustomerId = customerId,
                Status = OrderStatus.New,
                OrderDate =  DateTime.Now,
                OrderItems = new List<OrderItem>()
            };
            // Формуємо OrderItem з кожної обраної страви
            foreach (var selected_form in selectedItems)
            {
                // вибираємо страву з БД
                var dbItem = _context.Dishes.FirstOrDefault(i => i.Id == selected_form.ItemId );
                if (dbItem == null)
                {
                    continue; 
                }
                // формуємо вміст OrderItem
                var orderItem = new OrderItem
                {
                    DishId = dbItem.Id,
                    Cnt = selected_form.Cnt,
                    Price = dbItem.Price
                };
                
                order.OrderItems.Add(orderItem);
            }
            // загальна сума замовл
            order.PriceTotal = order.OrderItems.Sum(s => s.Price * s.Cnt);
            // зберігаємо в БД
            _context.Orders.Add(order);
            _context.SaveChanges();
            
            return RedirectToAction("MyOrders");
        }

        public IActionResult MyOrders()
        {
            var customerIdClaim = User.FindFirst("Id");
            if (customerIdClaim == null)
            {
                return RedirectToAction("Login");
            }
            int customerId = int.Parse(customerIdClaim.Value);
            // обираємо всі замовлення клієнта
            var orders = _context.Orders
                .AsNoTracking()
                .Include(o => o.OrderItems)
                    .ThenInclude(od => od.Dish)
                    .ThenInclude(dr => dr.Restaurant)
                .Where(o => o.CustomerId == customerId).OrderBy(s => s.Status).ThenByDescending(s => s.OrderDate)
                .ToList();

            var customerOrders = new List<CustomerOrderViewModel>();
            foreach (var order in orders)
            {
                //формуємо вью-модель
                var orderVm = new CustomerOrderViewModel
                {
                    OrderId = order.Id,
                    OrderTotalPrice = order.PriceTotal,
                    Status = order.Status.ToString(),
                    OrderDate = order.OrderDate.ToString("g"),
                    RestaurantName = order.OrderItems.FirstOrDefault().Dish.Restaurant.Name,
                    
                    Items = new List<CustomerOrderItemViewModel>()
                };
                foreach (var oi in order.OrderItems)
                {
                    orderVm.Items.Add(new CustomerOrderItemViewModel
                    {
                        ItemName = oi.Dish.Name,
                        Cnt = oi.Cnt,
                        Price = oi.Price
                    });
                }
                customerOrders.Add(orderVm);
            }
            ViewBag.ActivePage = 1;
            return View(customerOrders);
        }

        #region
        //// GET: Customers
        //public async Task<IActionResult> Index()
        //{
        //    return View(await _context.Customers.ToListAsync());
        //}

        //// GET: Customers/Details/5
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var customer = await _context.Customers
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (customer == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(customer);
        //}

        //// GET: Customers/Create
        //public IActionResult Create()
        //{
        //    return View();
        //}

        //// POST: Customers/Create
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Id,Name,Email,Password,PhoneNumber,Address")] Customer customer)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(customer);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(customer);
        //}

        //// GET: Customers/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var customer = await _context.Customers.FindAsync(id);
        //    if (customer == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(customer);
        //}

        //// POST: Customers/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Email,Password,PhoneNumber,Address")] Customer customer)
        //{
        //    if (id != customer.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(customer);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!CustomerExists(customer.Id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(customer);
        //}

        //// GET: Customers/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var customer = await _context.Customers
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (customer == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(customer);
        //}

        //// POST: Customers/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var customer = await _context.Customers.FindAsync(id);
        //    if (customer != null)
        //    {
        //        _context.Customers.Remove(customer);
        //    }

        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool CustomerExists(int id)
        //{
        //    return _context.Customers.Any(e => e.Id == id);
        //}
        #endregion
    }
}
