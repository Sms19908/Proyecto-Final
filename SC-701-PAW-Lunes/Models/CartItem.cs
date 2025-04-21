using System;

namespace SC_701_PAW_Lunes.Models
{
    public class CartItem
    {
        public int ProductoId { get; set; }
        public string Nombre { get; set; }
        public decimal Precio { get; set; }
        public int Cantidad { get; set; }

        public decimal Subtotal => Precio * Cantidad;
    }
}
