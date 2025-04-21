using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SC_701_PAW_Lunes.Models
{
    public class Category
    {
        [Key]
        public int Id_Cat { get; set; } // FK de categorías

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres.")]
        public string Nombre { get; set; }

        [StringLength(250, ErrorMessage = "La descripción no puede exceder los 250 caracteres.")]
        public string Descripcion { get; set; }

        [StringLength(100, ErrorMessage = "Las tallas no pueden exceder los 100 caracteres.")]
        public string Tallas { get; set; }

        public List<Inventory> Inventarios { get; set; }
    }
}
