using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SC_701_PAW_Lunes.Data;
using SC_701_PAW_Lunes.Models;
using SC_701_PAW_Lunes.ViewModels;

namespace SC_701_PAW_Lunes.Controllers
{
    [Authorize]
    public class CategoryController : Controller
    {
        private readonly PAWDbContext _context;
        private readonly ILogger<CategoryController> _logger;

        public CategoryController(PAWDbContext context, ILogger<CategoryController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _context.Category.ToListAsync();
            return View(categories);
        }

        public IActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(CategoryViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);
            

            var category = new Category
            {
                Id_Cat = vm.Id_Cat,
                Nombre = vm.Nombre,
                Descripcion = vm.Descripcion,
                Tallas = vm.Tallas
            };

            _context.Add(category);
            await _context.SaveChangesAsync();

            TempData["Mensaje"] = "Categoría creada correctamente.";
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Editar(int id)
        {
            var category = await _context.Category.FindAsync(id);
            if (category == null) return NotFound();
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(int id, Category category)
        {
            if (id != category.Id_Cat) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(category);
                    await _context.SaveChangesAsync();
                    TempData["Mensaje"] = "Categoría editada exitosamente.";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error al editar categoría: {ex.Message}");
                    ModelState.AddModelError(string.Empty, "Error al actualizar la categoría.");
                }
            }

            return View(category);
        }

        public async Task<IActionResult> Eliminar(int id)
        {
            var category = await _context.Category.FindAsync(id);
            if (category == null) return NotFound();

            try
            {
                _context.Category.Remove(category);
                await _context.SaveChangesAsync();
                TempData["Mensaje"] = "Categoría eliminada.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al eliminar categoría: {ex.Message}");
                return BadRequest("Error al eliminar la categoría.");
            }
        }
    }
}
