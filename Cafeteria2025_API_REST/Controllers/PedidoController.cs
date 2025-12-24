using Cafeteria2025_API_REST.DAO;
using Cafeteria2025_API_REST.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Cafeteria2025_API_REST.Controllers
{
    [ApiController]
    [Route("api/pedidos")]
    [Authorize]

    public class PedidoController : ControllerBase
    {
        private readonly IPedidoDAO _dao;

        public PedidoController(IPedidoDAO dao)
        {
            _dao = dao;
        }

        // ===============================
        // AGREGAR PRODUCTO
        // ===============================
        [Authorize(Roles = "admin,cliente")]
        [HttpPost("agregar")]
        public IActionResult Agregar(int idProducto, int cantidad)
        {
            int idUsuario = ObtenerIdUsuario();
            int idPedido = _dao.ObtenerOCrearPedidoGenerado(idUsuario);

            _dao.AgregarProducto(idPedido, idProducto, cantidad);
            return Ok("Producto agregado al carrito");
        }

        // ===============================
        // VER CARRITO
        // ===============================
        [Authorize(Roles = "admin,cliente")]
        [HttpGet("carrito")]
        public IActionResult VerCarrito()
        {
            int idUsuario = ObtenerIdUsuario();
            return Ok(_dao.ListarPedidoGenerado(idUsuario));
        }

        // ===============================
        // CHECKOUT (PAGO)
        // ===============================
        [Authorize(Roles = "admin,cliente")]
        [HttpPost("checkout")]
        public IActionResult Checkout(int idPedido, int idMetodoPago)
        {
            int idUsuario = ObtenerIdUsuario();
            _dao.ConfirmarPedido(idPedido, idMetodoPago, idUsuario);

            return Ok("Pedido confirmado");
        }

        // ===============================
        // HISTORIAL
        // ===============================
        [Authorize(Roles = "admin,cliente")]
        [HttpGet("historial")]
        public IActionResult Historial()
        {
            int idUsuario = ObtenerIdUsuario();
            return Ok(_dao.ListarHistorialPedidosUsuario(idUsuario));
        }

        [Authorize(Roles = "admin,cliente")]
        [HttpGet("historial/paginado")]
        public IActionResult HistorialPaginado( [FromQuery] int pagina = 1, [FromQuery] int tamano = 10)
        {
            int idUsuario = int.Parse(
                User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value
            );

            return Ok(_dao.PaginacionHistorialPedidosUsuario(idUsuario, pagina, tamano));
        }


        // ===============================
        // CANCELAR
        // ===============================
        [Authorize(Roles = "admin,vendedor")]
        [HttpPost("cancelar")]
        public IActionResult Cancelar(int idPedido)
        {
            _dao.CancelarPedido(idPedido);
            return Ok("Pedido cancelado");
        }

        // ================================
        // PAGINACIÓN PEDIDOS OPERATIVOS
        // ================================
        [HttpGet("operativos/paginacion")] public IActionResult PaginacionPedidosOperativos([FromQuery] int p = 1, [FromQuery] int t = 10)
        {
            return Ok(_dao.PaginacionPedidosOperativos(p, t));
        }

        // ===============================
        // CAMBIAR ESTADO
        // ===============================
        [Authorize(Roles = "admin,vendedor")]
        [HttpPut("estado")]
        public IActionResult CambiarEstado(int idPedido, int idEstado)
        {
            _dao.CambiarEstadoPedido(idPedido, idEstado);
            return Ok("Estado actualizado");
        }

        // ===============================
        // OPERATIVOS
        // ===============================
        [Authorize(Roles = "admin")]
        [HttpGet("operativos")]
        public IActionResult Operativos()
        {
            return Ok(_dao.ListarPedidosOperativos());
        }

        [Authorize(Roles = "admin")]
        [HttpGet("operativos/paginado")]
        public IActionResult PedidosOperativosPaginado( [FromQuery] int pagina = 1, [FromQuery] int tamano = 10)
        {
            return Ok(_dao.PaginacionPedidosOperativos(pagina, tamano));
        }

        // ===================================
        // PAGINACIÓN HISTORIAL POR CLIENTE
        // ===================================
        [HttpGet("historial/paginacion/{idUsuario}")]
        public IActionResult Paginacionistorial(int idUsuario, [FromQuery] int p = 1, [FromQuery] int t = 10)
        {
            return Ok(_dao.PaginacionHistorialPedidosUsuario(idUsuario, p, t));
        }

        // ===============================
        // REPORTE
        // ===============================
        [Authorize(Roles = "admin")]
        [HttpGet("reporte/general")]
        public IActionResult ReporteGeneral()
        {
            return Ok(_dao.ReportePedidosGeneral());
        }

        // ===============================
        // HELPER
        // ===============================
        private int ObtenerIdUsuario()
        {
            return int.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier)!.Value
            );
        }
    }
}