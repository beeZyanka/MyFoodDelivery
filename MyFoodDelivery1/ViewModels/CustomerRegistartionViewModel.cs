using System.ComponentModel.DataAnnotations;

namespace MyFoodDelivery1.ViewModels
{
    public class CustomerRegistartionViewModel
    {
        [Required]
        [UIHint("text")]
        [Display(Name = "Ім'я")]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        [UIHint("EmailAddress")]
        [Display(Name = "Email-адреса")]
        public string Email { get; set; }

        [Required]
        [RegularExpression(@"^(?:\+380\d{9}|0\d{9})$", ErrorMessage = "Некоректний номер телефону.")]
        [Display(Name = "Телефон")]
        public string PhoneNumber { get; set; }

        [Required]
        [UIHint("text")]
        [Display(Name = "Адреса доставки")]
        public string Address { get; set; }
        
        
        [UIHint("Password")]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Display(Name = "Підтвердження паролю")]
        [UIHint("Password")]
        [Compare("Password")]
        public string PasswordConfirm { get; set; }


    }
}
