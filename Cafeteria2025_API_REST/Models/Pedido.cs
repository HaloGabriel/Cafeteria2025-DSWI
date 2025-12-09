namespace Cafeteria2025_API_REST.Models
{
    public class Pedido
    {
        public int IdPedido { get; set; }
        public Usuario? UsuaPedido { get; set; }
        public DateTime FechaPedido { get; set; }
        public EstadoPedido? Estado { get; set; }
        public MetodoPago? MtdPago { get; set; }
        public decimal TotalPagar { get; set; }
        public string? NombreClienteRecoge { get; set; }
        public DateTime HoraRecojoEstimado { get; set; }
        public string? NotasGenerales { get; set; }
        public bool Activo { get; set; }
        public string? CodigoRecojo { get; set; }
        public bool EsRecojoInmediato { get; set; }
        public string? NumeroComprobante { get; set; }
        public DateTime FechaActualizacion { get; set; }
        public string? UsuarioActualizacion { get; set; }
    }
}
