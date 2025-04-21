using System.ComponentModel.DataAnnotations;

namespace SC_701_PAW_Lunes.ViewModels
{
    public class CategoryViewModel
    {
        [Required(ErrorMessage = "El ID de categoría es obligatorio")]
        [Display(Name = "ID de Categoría")]
        public int Id_Cat { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Nombre { get; set; }

        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }

        public string Tallas { get; set; }
    }
}
