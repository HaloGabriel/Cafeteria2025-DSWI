namespace Cafeteria2025_API_REST.Models
{
    public class ProductoCreate
    {
        public string Nombre { get; set; } = null!;
        public string? Descripcion { get; set; }
        public decimal PrecioBase { get; set; }
        public int? IdTamano { get; set; }
        public int Stock { get; set; }
        public int IdCategoria { get; set; }
        public string? ImagenUrl { get; set; }
        public bool EsPersonalizable { get; set; }
    }
}
