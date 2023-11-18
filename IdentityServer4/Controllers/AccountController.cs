using IdentityServer4.Entity;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace IdentityServer4.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
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
            return View();
        }


        [HttpGet]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }
    }
}
