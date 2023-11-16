using System.ComponentModel.DataAnnotations;

namespace IdentityServer4.Models
{
    public class LoginViewModels
    {
        [Required(ErrorMessage = "ایمیل را وارد کنید")]
        public string Email { get; set; }
        [Required(ErrorMessage = "رمز عبور را وارد کنید")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Display(Name = "مرا به خاطر بسپار.")]
        public bool RememberMe { get; set; }
    }
}
