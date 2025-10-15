using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyFoodDelivery1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFoodDelivery1.Controllers
{
    public class RestaurantsController : Controller
    {
        private readonly AppDbContext _context;

        public RestaurantsController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> GetRestaurants()
        {
            ViewBag.ActivePage = 0;
            return View(await _context.Restaurants.ToListAsync());
        }



        #region

        //// GET: Restaurants
        //public async Task<IActionResult> Index()
        //{
        //    return View(await _context.Restaurants.ToListAsync());
        //}

        //// GET: Restaurants/Details/5
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var restaurant = await _context.Restaurants
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (restaurant == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(restaurant);
        //}

        //// GET: Restaurants/Create
        //public IActionResult Create()
        //{
        //    return View();
        //}

        //// POST: Restaurants/Create
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Id,Name,Address")] Restaurant restaurant)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(restaurant);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(restaurant);
        //}

        //// GET: Restaurants/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var restaurant = await _context.Restaurants.FindAsync(id);
        //    if (restaurant == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(restaurant);
        //}

        //// POST: Restaurants/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Address")] Restaurant restaurant)
        //{
        //    if (id != restaurant.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(restaurant);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!RestaurantExists(restaurant.Id))
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
        //    return View(restaurant);
        //}

        //// GET: Restaurants/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var restaurant = await _context.Restaurants
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (restaurant == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(restaurant);
        //}



        //private bool RestaurantExists(int id)
        //{
        //    return _context.Restaurants.Any(e => e.Id == id);
        //}
        #endregion
    }
}
