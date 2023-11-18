using System.ComponentModel.DataAnnotations;

namespace IdentityServer4.Models
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
