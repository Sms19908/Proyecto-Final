namespace SC_701_PAW_Lunes.ViewModel
{
    public class UserViewModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }

        public string Direccion { get; set; }
        public string Roles { get; set; }
        
        public Boolean Active { get; set; }
        public Boolean PasswordRecoveryMode { get; set; }

        public bool IsCurrentUser { get; set; }
    }
}
