namespace Cafeteria2025_API_REST.Models
{
    public class Usuario
    {
        public int IdUsuario { get; set; }
        public string? Nombre { get; set; }
        public string? Apellido { get; set; }
        public string? Email { get; set; }
        public string? PasswordHash { get; set; }
        public string? Telefono { get; set; }
        public Rol? RolUsua { get; set; }
        public bool Activo { get; set; }
        public DateTime FechaRegistro { get; set; }
        public DateTime FechaActualizacion { get; set; }
        public string? UsuarioActualizacion { get; set; }
    }
}
