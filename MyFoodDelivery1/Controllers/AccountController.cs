using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyFoodDelivery1.Models;
using MyFoodDelivery1.Services;
using MyFoodDelivery1.ViewModels;
using System.Security.Claims;

namespace MyFoodDelivery1.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;

        public AccountController(AppDbContext context)
        {
            _context = context;
        }

        
        public IActionResult AccessDenied()
        {
            return View();
        }
        
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(CustomerLoginViewModel model)
        {
            //Перевіряємо валідність моделі
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            //Перевіряємо існування користувача з заданим email
            //та правильність паролю
            var customer = _context.Customers.
                FirstOrDefault(l => l.Email == model.Email);
            if (customer == null || (!PasswordHasher.IsCorrectPassword(customer, model.Password)))
            {
                ModelState.AddModelError("", "Некоректні дані для входу. Не знайдено користувача, або невірний пароль.");
                return View(model);
            }
            //Якщо перевірки пройдено переходимо до логування безпосередньо (LogInAsync)
            //2-й параметр - булева ознака (true - якщо Admin)
            //у якості адміна для спрощення є користувач admin@gmail.com
            await LogInAsync(customer,(customer.Email=="admin@gmail.com"));
            //За успішного логування переходимо на домашню сторінку
            return RedirectToAction("Index", "Home");

        }
        private async Task LogInAsync(Customer user, bool isAdmin)
        {
            //ініціаліцуємо Claims відповідними значеннями
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("Id", user.Id.ToString()),
                new Claim(ClaimTypes.Role, (isAdmin? "Admin": "User"))
            };
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Name, ClaimTypes.Role);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            await HttpContext.SignInAsync(claimsPrincipal);
        }

        public async Task<IActionResult> LogOut()
        {
            //стандартний метод HttpContext-у
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        
        //виклик view реєстрації
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(CustomerRegistartionViewModel model)
        {
            //Перевіряємо валідність моделі
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            //Перевірка на існування введенного email в БД
            bool emailExists = _context.Customers
                .AsNoTracking()
                .Any(r => r.Email == model.Email);
            if (emailExists)
            {
                ModelState.AddModelError("", "Даний e-mail вже використовується");
                return View(model);
            }
            // формуємо модель для Customer запису в БД
            // при цьому хешуємо пароль 
            var newCustomer = new Customer
            {
                Name = model.Name,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                Address = model.Address,
                Password = PasswordHasher.HashPassword(model.Password)

            };
            //Запис в БД
            _context.Customers.Add(newCustomer);
            _context.SaveChanges();
            //повертаємо на сторінку успішної реєстрації
            return View("RegisterSuccess");

        }
    }
}
