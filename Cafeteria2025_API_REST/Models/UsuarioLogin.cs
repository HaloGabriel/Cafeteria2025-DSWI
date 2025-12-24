namespace Cafeteria2025_API_REST.Models
{
    /// <summary>
    /// USO SOLO PARA UTENTICACIÓN
    /// </summary>
    public class UsuarioLogin
    {
        public int IdUsuario { get; set; }
        public string Email { get; set; } = null!;
        public byte IdRol { get; set; }
        public string RolNombre { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
    }
}
