using Common.Types.Roles;

namespace Common.DTOs.Users
{
    public class UserLoginData
    {
        public string Contrasena { get; set; }
        public string? Token { get; set; }
        public bool? IsAuthorizedUser { get; set; }
        public DateTime? ExpireToken { get; set; }
        public string NombreUsuario { get; set; }
        public int? IdUsuario { get; set; }
    }
}
