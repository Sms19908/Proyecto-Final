using System.ComponentModel.DataAnnotations;

namespace SC_701_PAW_Lunes.Models
{
    public class Favorites
    {
        [Key]
        public int Id_Favorites { get; set; }
        [Required]
        public int Id_User { get; set; } //id del usuario para filtrar los favoritos
        [Required]
        public int Id_Inv { get; set; } //Id del producto favorito
    }
}
