using System.ComponentModel.DataAnnotations;

namespace IdentityServer4.Models
{
    public class LoginViewModels
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Display(Name = "مرا به خاطر بسپار.")]
        public bool RememberMe { get; set; }
    }
}
