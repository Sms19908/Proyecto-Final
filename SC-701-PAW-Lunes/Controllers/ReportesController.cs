using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SC_701_PAW_Lunes.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using SC_701_PAW_Lunes.Data;
    using SC_701_PAW_Lunes.Models;
    using SC_701_PAW_Lunes.ViewModel;
    using System.Linq;

    public class ReportesController : Controller
    {
        private readonly PAWDbContext _context;

        public ReportesController(PAWDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var modelo = new ReporteViewModel
            {
                VentasPorProducto = _context.Inventory
                    .Select(i => new ReporteVentas { Nombre = i.Nombre, CantidadVendida = i.Cantidad })
                    .ToList(),

                RotacionInventario = _context.Inventory
                    .Select(i => new ReporteRotacion { Nombre = i.Nombre, RotacionInventario = i.Cantidad })
                    .ToList(),

                VentasPorCategoria = _context.Category
                    .Select(c => new ReporteCategoria
                    {
                        Categoria = c.Nombre,
                        TotalVentas = _context.Inventory.Where(i => i.Id_Cat == c.Id_Cat)
                                                        .Sum(i => i.Precio * i.Cantidad)
                    })
                    .ToList()
            };

            return View(modelo);
        }
    }
}
