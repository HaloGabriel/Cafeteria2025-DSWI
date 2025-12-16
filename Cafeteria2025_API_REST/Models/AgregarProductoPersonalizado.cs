namespace Cafeteria2025_API_REST.Models
{
    public class AgregarProductoPersonalizado
    {
        public int IdUsuario { get; set; }
        public int IdProducto { get; set; }
        public int Cantidad { get; set; }
        public List<int> Opciones { get; set; } = new();
    }
}
