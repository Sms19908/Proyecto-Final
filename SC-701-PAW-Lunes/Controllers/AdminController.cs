using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SC_701_PAW_Lunes.Models;
using SC_701_PAW_Lunes.ViewModel;

namespace SC_701_PAW_Lunes.Controllers
{
    [Authorize(Roles = "ADMIN")]
    public class AdminController : Controller
    {

        private readonly UserManager<User> _userManager;
        private readonly ILogger<AdminController> _logger;

        public AdminController(UserManager<User> userManager, ILogger<AdminController> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<IActionResult> UserAdministration()
        {
            var users = _userManager.Users.ToList();
            var userViewModels = new List<UserViewModel>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userViewModels.Add(new UserViewModel
                {
                    Id = user.Id,
                    Email = user.Email,
                    FullName = user.NombreCompleto,
                    Roles = string.Join(", ", roles),
                    IsCurrentUser = user.UserName == User.Identity.Name,
                    Active = user.Active,
                    PasswordRecoveryMode = user.PasswordRecoveryMode
                });
            }

            return View(userViewModels);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            // Prevent deleting current user
            if (user.UserName == User.Identity.Name)
            {
                TempData["ErrorMessage"] = "No puedes eliminar tu propio usuario";
                return RedirectToAction(nameof(UserAdministration));
            }

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                _logger.LogInformation($"User {user.Email} deleted by {User.Identity.Name}");
                TempData["SuccessMessage"] = "Usuario eliminado correctamente";
            }
            else
            {
                TempData["ErrorMessage"] = "Error al eliminar el usuario";
            }

            return RedirectToAction(nameof(UserAdministration));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>Activate(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            // Prevent activating current user
            if (user.UserName == User.Identity.Name)
            {
                TempData["ErrorMessage"] = "No puedes activar tu propio usuario";
                return RedirectToAction(nameof(Index));
            }

            user.Active = true;

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                _logger.LogInformation($"User {user.Email} activated by {User.Identity.Name}");
                TempData["SuccessMessage"] = "Usuario activado correctamente";
            }
            else
            {
                TempData["ErrorMessage"] = "Error al activar el usuario";
            }

            return RedirectToAction(nameof(UserAdministration));
        }


    }
}
