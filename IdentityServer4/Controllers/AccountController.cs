using IdentityServer4.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;

namespace IdentityServer4.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IEmailSender _emailSender;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
        }

        private void AddError(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(String.Empty, error.Description);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Register()
        {
            RegisterViewModels viewModels = new RegisterViewModels();
            return View(viewModels);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModels viewModels)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser
                {
                    UserName = viewModels.Email,
                    Email = viewModels.Email,
                    //Name = viewModels.Name
                };

                if (viewModels.Password != viewModels.ConfirmPassword)
                {
                    ModelState.AddModelError(String.Empty, "رمز عبور و تکرار آن یکسان نمی باشد،مجدد تلاش کنید.");
                }

                var result = await _userManager.CreateAsync(user, viewModels.Password);

                if (result.Succeeded)
                {
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackurl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: HttpContext.Request.Scheme);


                    await _emailSender.SendEmailAsync(viewModels.Email, "فعال سازی حساب کاربری", "جهت بازیابی بر روی لینک <a href=\""
                        + callbackurl + "\"></a> کلیک کنید.");

                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction(nameof(Index), "Home");
                }
                else
                {
                    AddError(result);
                }
            }

            return View(viewModels);
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound();
            }
            var result = await _userManager.ConfirmEmailAsync(user, code);

            return View(result.Succeeded ? "ConfirmEmail" : NotFound());
        }

        [HttpGet]
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }


        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModels? viewModels, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            returnUrl = returnUrl ?? Url.Content("~/");

            if (ModelState.IsValid)
            {
                if (viewModels != null)
                {

                    var result = await _signInManager.PasswordSignInAsync(viewModels.Email, viewModels.Password, viewModels.RememberMe,
                        lockoutOnFailure: true);

                    if (result.Succeeded)
                    {
                        //return Redirect(returnUrl);
                        return Redirect(returnUrl);
                    }

                    if (result.IsLockedOut)
                    {
                        return View("LockOut");
                    }
                    else
                    {
                        ModelState.AddModelError(String.Empty, "ورود با خطا مواجه شد،مجدد تلاش کنید.");
                        return View();

                    }
                }
            }
            return View(viewModels);
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel viewModels)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(viewModels.Email);
                if (user == null)
                {
                    return NotFound();
                }

                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                var callbackurl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code },
                    protocol: HttpContext.Request.Scheme);

                await _emailSender.SendEmailAsync(viewModels.Email, "بازیابی رمز عبور", "جهت بازیابی رمز عبور بر روی لینک <a href=\""
                    + callbackurl + "\"></a> کلیک کنید.");

                return RedirectToAction(nameof(ForgotPasswordConfirmation));

            }
            return View();
        }


        [HttpGet]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ResetPassword(string code = null)
        {
            return code == null ? View("Error") : View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModels viewModels)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(viewModels.Email);

                if (user == null)
                {
                    return NotFound();
                }

                var result = await _userManager.ResetPasswordAsync(user, viewModels.Code, viewModels.Password);

                if (result.Succeeded)
                {
                    return RedirectToAction("ResetPasswordConfirmation");
                }
            }
            return View();
        }
    }
}
