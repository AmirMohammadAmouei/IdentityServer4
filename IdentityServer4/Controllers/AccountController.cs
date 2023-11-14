using IdentityServer4.Entity;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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
                var user = new ApplicationUser
                {
                    UserName = viewModels.Name,
                    Email = viewModels.Email,
                    Name = viewModels.Name
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }


        [HttpGet]
        public IActionResult Login()
        {
            LoginViewModels viewodModels = new LoginViewModels();
            return View(viewodModels);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModels viewModels)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(viewModels.Email, viewModels.Password,
                    viewModels.RememberMe, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index), "Home");
                }
                else
                {
                    ModelState.AddModelError(String.Empty, "ورود با خطا مواجه شد،مجدد تلاش کنید.");
                    return View();

                }
            }
            return View(viewModels);
        }

    }
}
