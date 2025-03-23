using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RestaurantDomain.Models;
using RestaurantInfrastructure.Controllers;
using RestaurantInfrastructure.Models;

namespace RestaurantInfrastructure
{
    public class UsersController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public UsersController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // GET: Users/Register
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // POST: Users/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var existingUser = await _userManager.FindByEmailAsync(model.Email);
                if (existingUser != null)
                {
                    // Clear email validation errors, and add custom error for already taken email.
                    ModelState.Remove("Email");
                    ModelState.AddModelError("Email", "The email address is already taken.");
                    return View(model);
                }

                var user = new User
                {
                    UserName = model.Email,
                    Email = model.Email,
                    Name = model.Name
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                foreach (var error in result.Errors)
                {
                    if (error.Code.Contains("Password"))
                    {
                        ModelState.AddModelError("Password", "Password must contain at least one digit, one uppercase letter, one lowercase letter, one special character, and be at least 8 characters long.");
                    }
                }
            }

            return View(model);
        }

        // GET: Users/Login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // POST: Users/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: true);

                if (result.Succeeded)
                {
                    return LocalRedirect(returnUrl);
                }
                if (result.IsLockedOut)
                {
                    ModelState.AddModelError(string.Empty, "Ваш аккаунт заблоковано. Будь-ласка, спробуйте іншого разу.");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Помилка входу. Будь-ласка, перевірте правильність пошти та паролю.");
                    return View(model);
                }
            }
            return View(model);
        }

        // POST: Users/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}