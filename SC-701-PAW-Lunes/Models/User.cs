using Microsoft.AspNetCore.Identity;

namespace SC_701_PAW_Lunes.Models
{
    public class User : IdentityUser
    {
        public String NombreCompleto { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string SelectedRole { get; set; }
        public string Direccion { get; set; }

        public Boolean Active { get; set; } = true;

        public Boolean PasswordRecoveryMode { get; set; } = false;

    }
}
