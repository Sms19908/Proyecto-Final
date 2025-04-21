namespace SC_701_PAW_Lunes.Models
{
    public class ReporteVentas
    {
        public string Nombre { get; set; }
        public int CantidadVendida { get; set; }
    }

    public class ReporteRotacion
    {
        public string Nombre { get; set; }
        public int RotacionInventario { get; set; }
    }

    public class ReporteCategoria
    {
        public string Categoria { get; set; }
        public decimal TotalVentas { get; set; }
    }
}
