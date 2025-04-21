using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SC_701_PAW_Lunes.Models;
using SC_701_PAW_Lunes.ViewModel;

namespace SC_701_PAW_Lunes.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ILogger<AccountController> _logger;

        public AccountController(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            if (TempData["userError"] != null)
            {
                ViewData["userError"] = TempData["userError"];
                TempData.Keep("userError");
            }

            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }


        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var user = new User
                {
                    UserName = model.Email,
                    Email = model.Email,
                    NombreCompleto = model.FullName,
                    Active = string.Equals(model.SelectedRole, "USER", StringComparison.OrdinalIgnoreCase),
                    PasswordRecoveryMode = false,
                    Direccion = model.Direccion,
                    Password = model.Password,
                    SelectedRole = model.SelectedRole,
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return View(model);
                }

                // Asignar el rol al usuario
                var roleResult = await _userManager.AddToRoleAsync(user, model.SelectedRole);
                if (!roleResult.Succeeded)
                {
                    foreach (var error in roleResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return View(model);
                }

                // Si es usuario normal, redirigir al inventario
                if (string.Equals(model.SelectedRole, "USER", StringComparison.OrdinalIgnoreCase))
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }

                // Si es administrador, necesita activación
                TempData["userMessage"] = "Ahora un administrador debe activar tu cuenta para poder acceder correctamente como administrador.";
                return RedirectToAction("Login", "Account");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al registrar usuario: {ex.Message}");
                ModelState.AddModelError(string.Empty, "Ocurrió un error inesperado al intentar registrarte. Inténtalo nuevamente.");
                return View(model);
            }
        }

        private List<SelectListItem> GetRoleOptions()
        {
            return new List<SelectListItem>
             {
                new SelectListItem { Value = "USER", Text = "Regular User" },
                new SelectListItem { Value = "ADMIN", Text = "Administrator" }
            };
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string returnUrl = null)
        {

            if (TempData["UserMessage"] != null)
            {
                ViewData["UserMessage"] = TempData["UserMessage"];
                TempData.Keep("UserMessage"); 
            }

            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.Email);

                if (user == null)
                {
                    TempData["userMessage"] = "Usuario no encontrado, intenta de nuevo";
                    return RedirectToAction("Login", "Account");
                }

                if (user != null && !user.Active)
                {
                    TempData["userMessage"] = "Usuario aún no ha sido activado, pide a un administrador activar tu cuenta";
                    return RedirectToAction("Login", "Account");
                }

                var result = await _signInManager.PasswordSignInAsync(
                    model.Email,
                    model.Password,
                    model.RememberMe,
                    lockoutOnFailure: false);

                if (!result.Succeeded)
                {
                    TempData["userMessage"] = "Usuario o contraseña incorrectos, intenta otra vez";
                    return RedirectToAction("Login", "Account");
                }

                return RedirectToAction("Index", "Home");
            }

            return View(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Logout()
        {

            // Sign out from all authentication schemes
            await _signInManager.SignOutAsync();

            // Clear all cookies
            foreach (var cookie in Request.Cookies.Keys)
            {
                Response.Cookies.Delete(cookie);
            }

            // Prevent caching of the page
            Response.Headers["Cache-Control"] = "no-cache, no-store";
            Response.Headers["Pragma"] = "no-cache";
            Response.Headers["Expires"] = "0";

            return RedirectToAction("Login", "Account");
        }
    }
}
