namespace Cafeteria2025_API_REST.DAO
{
    public interface IPedidoDAO
    {
        int ObtenerOCrearPedidoGenerado(int idUsuario);
        void AgregarProducto(int idPedido, int idProducto, int cantidad);
        IEnumerable<object> ListarPedidoGenerado(int idUsuario);
        void ConfirmarPedido(int idPedido, int idMetodoPago);
        void CancelarPedido(int idPedido);
        void CambiarEstadoPedido(int idPedido, int idEstadoNuevo);
        IEnumerable<object> ListarPedidosOperativos();
        void AgregarProductoPersonalizado(int idUsuario, int idProducto, int cantidad, List<int> opciones);
        IEnumerable<object> ListarHistorialPedidosUsuario(int idUsuario);
        IEnumerable<object> ReportePedidosGeneral();


    }
}
