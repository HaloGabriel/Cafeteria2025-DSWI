namespace Cafeteria2025_API_REST.Models
{
    public class DetallePedido
    {
        public int IdDetallePedido { get; set; }
        public Pedido? Cabecera { get; set; }
        public Producto? ProdDetalle { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitarioFinal { get; set; }
        public decimal Subtotal { get
            {
                return Cantidad * PrecioUnitarioFinal;
            } 
        }
    }
}
