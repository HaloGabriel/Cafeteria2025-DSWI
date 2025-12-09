namespace Cafeteria2025_API_REST.Models
{
    public class ProductoOpcion
    {
        public int IdProductoOp { get; set; }
        public Producto? ProductoOp { get; set; }
        public Opcion? OpcionProd { get; set; }
    }
}
