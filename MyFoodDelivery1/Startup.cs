using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using MyFoodDelivery1.Models;
using MyFoodDelivery1.Services;

namespace MyFoodDelivery1
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            string connectionString = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));
            
            services.AddMvc();
            services.AddScoped<IHelper, HelperService>();
            #region comment
            // AddAuthentication конфігурує та додає в DI сервіси, необхідні для автентифікації
            // API автентифікації у ASP.NET Core підтримує використання багатьох схем автентифікації
            // Наприклад, в одному додатку може бути присутня можливість автентифікації за допомогою кукі,
            // за допомогою інтеграції з Google та Facebook
            // В метод AddAuthentication надається назва схеми за замовчуванням, далі відбуваються
            // конфігурація схем автентифікації, які використовуються у додатку
            #endregion
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/account/login"; // посилання на сторінку логіну, додаток переспрямовує
                                                          // користувача за цією адресою, якщо він спробує запросити ресурс,
                                                          // для якого у нього немає прав
                    options.AccessDeniedPath = "/account/accessdenied";
                });
            

        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseRouting();
            #region comment
            // При аутентифікації, додаток перевірить наявність Cookie автентифікації, 
            // спробує видобути з нього дані про користувача та розмістити їх у спеціальний об'єкт 
            // у контексті обробки запиту. (HttpContext.User)
            #endregion
            app.UseAuthentication();
            #region comment
            // На етапі авторизації додаток перевірить, чи є у користувача доступ до ресурсу, який запитується
            // Наприклад, якщо неавтентифікований користувач звернеться до методу дії, декорованному 
            // атрибутом Authorize, запит не пройде авторизацію та додаток переспрямує кристувача на сторінку для логіну
            #endregion
            app.UseAuthorization();

            app.UseStaticFiles();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "Default",
                    pattern: "{controller=home}/{action=index}"
                    );
            });

        }
    }
}
