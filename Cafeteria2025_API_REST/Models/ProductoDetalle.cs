namespace Cafeteria2025_API_REST.Models
{
    public class ProductoDetalle
    {
            public int IdProducto { get; set; }
            public string? Nombre { get; set; }
            public string? Descripcion { get; set; }
            public decimal PrecioBase { get; set; }
            public string? Tamano { get; set; }
            public int Stock { get; set; }
            public string? Categoria { get; set; }
            public string? ImagenUrl { get; set; }
            public bool EsPersonalizable { get; set; }
            public bool Activo { get; set; }
            public DateTime FechaRegistro { get; set; }

    }
}
