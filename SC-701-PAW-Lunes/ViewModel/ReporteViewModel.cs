using SC_701_PAW_Lunes.Models;

namespace SC_701_PAW_Lunes.ViewModel
{
    public class ReporteViewModel
    {
        public List<ReporteVentas> VentasPorProducto { get; set; }
        public List<ReporteRotacion> RotacionInventario { get; set; }
        public List<ReporteCategoria> VentasPorCategoria { get; set; }
    }
}
