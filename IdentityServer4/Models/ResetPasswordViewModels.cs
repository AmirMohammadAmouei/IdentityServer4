using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace IdentityServer4.Models
{
    public class ResetPasswordViewModels
    {
        [Required(ErrorMessage ="ایمیل خود را وارد کنید.")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "رمز عبور حداقل باید شامل 2 کاراکتر باشد.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password",ErrorMessage ="رمز عبور با تکرار آن مطابقت ندارد.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }
}
