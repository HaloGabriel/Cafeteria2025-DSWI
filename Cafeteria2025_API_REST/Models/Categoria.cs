namespace Cafeteria2025_API_REST.Models
{
    public class Categoria
    {
        public int IdCategoria { get; set; }
        public string? Descripcion { get; set; }
        public bool Activo { get; set; }
        public DateTime FechaRegistro { get; set; }
    }
}
