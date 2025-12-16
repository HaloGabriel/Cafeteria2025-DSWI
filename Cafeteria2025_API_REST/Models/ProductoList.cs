namespace Cafeteria2025_API_REST.Models
{
    public class ProductoList
    {
        public int IdProducto { get; set; }
        public string? Nombre { get; set; }
        public string? Categoria { get; set; }
        public decimal PrecioBase { get; set; }
        public int Stock { get; set; }
    }
}
