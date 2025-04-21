using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SC_701_PAW_Lunes.Models
{
    public class Inventory
    {
        [Key]
        public int Id_Inv { get; set; } // PK

        [Required]
        public int Id_Cat { get; set; } // FK de categorias

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        [Required]
        public string Marca { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Precio { get; set; }

        public string Descripcion { get; set; }

        [Range(0, int.MaxValue)]
        public int Cantidad { get; set; }

        [ForeignKey("Id_Cat")]
        public Category Categoria { get; set; }
    }
    

}
