using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SC_701_PAW_Lunes.Models;
using SC_701_PAW_Lunes.Data;
using Microsoft.AspNetCore.Authorization;

namespace SC_701_PAW_Lunes.Controllers
{
    [Authorize]
    public class InventoryController : Controller
    {
        private readonly PAWDbContext _context;
        private readonly ILogger<InventoryController> _logger;

        public InventoryController(PAWDbContext context, ILogger<InventoryController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            _logger.LogInformation($"User {User.Identity?.Name} accessed Inventory");
            var inventoryList = await _context.Inventory.ToListAsync();
            return View(inventoryList);
        }

        public async Task<IActionResult> Detalle(int id)
        {
            var inventory = await _context.Inventory.FindAsync(id);
            if (inventory == null) return NotFound();
            return View(inventory);
        }

        public IActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(Inventory inventory)
        {
            if (!ModelState.IsValid)
                return View(inventory);

            try
            {
                _context.Add(inventory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al crear inventario: {ex.Message}");
                ModelState.AddModelError(string.Empty, "Ocurrió un error al guardar el inventario.");
                return View(inventory);
            }
        }

        public async Task<IActionResult> Editar(int id)
        {
            var inventory = await _context.Inventory.FindAsync(id);
            if (inventory == null) return NotFound();
            return View(inventory);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(int id, Inventory inventory)
        {
            if (id != inventory.Id_Inv) return NotFound();

            if (!ModelState.IsValid)
                return View(inventory);

            try
            {
                _context.Update(inventory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al editar inventario: {ex.Message}");
                ModelState.AddModelError(string.Empty, "Error al actualizar el inventario.");
                return View(inventory);
            }
        }

        public async Task<IActionResult> Eliminar(int id)
        {
            var inventory = await _context.Inventory.FindAsync(id);
            if (inventory == null) return NotFound();

            try
            {
                _context.Inventory.Remove(inventory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al eliminar inventario: {ex.Message}");
                return BadRequest("Error al eliminar el inventario.");
            }
        }
    }
}
