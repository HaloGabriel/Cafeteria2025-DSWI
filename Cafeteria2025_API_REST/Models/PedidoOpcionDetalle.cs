namespace Cafeteria2025_API_REST.Models
{
    public class PedidoOpcionDetalle
    {
        public int IdPedidoOpcionDetalle { get; set; }
        public DetallePedido? DtllPdd { get; set; }
        public Opcion? OpDtllPed { get; set; }
        public decimal CostoAplicado { get; set; }
    }
}
