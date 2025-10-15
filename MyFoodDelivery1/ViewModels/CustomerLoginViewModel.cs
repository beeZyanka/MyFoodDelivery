using System.ComponentModel.DataAnnotations;

namespace MyFoodDelivery1.ViewModels
{
    public class CustomerLoginViewModel
    {
        [Required]
        [EmailAddress]
        [UIHint("EmailAddress")]
        [Display(Name = "Email-адреса")]
        public string Email { get; set; }

        [Required]
        [UIHint("Password")]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        
        
    }
}
