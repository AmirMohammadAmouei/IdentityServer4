using System.ComponentModel.DataAnnotations;

namespace IdentityServer4.Models
{
    public class RegisterViewModels
    {
        [Required(ErrorMessage = "نام کاربری را وارد کنید")]
        public string? Name { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "طول رمز  عبور حداقل 6کاراکتر می باشد.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "رمز عبور با تکرار آن مطابقت ندارد،مجددا تلاش فرمایید.")]
        public string ConfirmPassword { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public bool RememberMe { get; set; }
    }
}
