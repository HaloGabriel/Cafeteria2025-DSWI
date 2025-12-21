using Cafeteria2025_API_REST.DAO;
using Cafeteria2025_API_REST.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cafeteria2025_API_REST.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PedidoController : ControllerBase
    {
        private readonly IPedidoDAO _dao;

        public PedidoController(IPedidoDAO dao)
        {
            _dao = dao;
        }

        // ===============================
        // AGREGAR AL PEDIDO
        // ===============================
        [HttpPost("agregar")]public IActionResult Agregar(int idUsuario, int idProducto, int cantidad)
        {
            int idPedido = _dao.ObtenerOCrearPedidoGenerado(idUsuario);
            _dao.AgregarProducto(idPedido, idProducto, cantidad);

            return Ok("Producto agregado al pedido (carrito)");
        }

        // ===============================
        // VER PEDIDO (CARRITO) DEL USUARIO
        // ===============================
        [HttpGet("carrito/{idUsuario}")]public IActionResult VerCarrito(int idUsuario)
        {
            return Ok(_dao.ListarPedidoGenerado(idUsuario));
        }

        // ===============================
        // DESCUENTO DEL STOCK (PAGO)
        // ===============================
        [HttpPost("checkout")]public IActionResult Checkout(int idPedido, int idMetodoPago)
        {
            try
            {
                _dao.ConfirmarPedido(idPedido, idMetodoPago);
                return Ok("Pedido confirmado y stock descontado");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // ===============================
        // CANCELAR PEDIDO
        // ===============================
        [HttpPost("cancelar")]public IActionResult Cancelar(int idPedido)
        {
            try
            {
                _dao.CancelarPedido(idPedido);
                return Ok("Pedido cancelado y stock devuelto");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // ===============================
        // COLOCAR ESTADO AL PEDIDO (PARA LOS BOTONES)
        // ===============================
        [HttpPut("estado")] public IActionResult CambiarEstado(int idPedido, int idEstado)
        {
            try
            {
                _dao.CambiarEstadoPedido(idPedido, idEstado);
                return Ok("Estado del pedido actualizado");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // ===============================
        // VER TODOS LOS PEDIDOS OPERATIVOS
        // ===============================
        [HttpGet("operativos")]public IActionResult PedidosOperativos()
        {
            return Ok(_dao.ListarPedidosOperativos());
        }

        // ================================
        // PAGINACIÓN PEDIDOS OPERATIVOS
        // ================================
        [HttpGet("operativos/paginacion")] public IActionResult PaginacionPedidosOperativos([FromQuery] int p = 1, [FromQuery] int t = 10)
        {
            return Ok(_dao.PaginacionPedidosOperativos(p, t));
        }

        // ===============================
        // AGREGAR PERSONALIZACIÓN AL PRODUCTO DEL PEDIDO
        // ===============================
        [HttpPost("agregar-personalizado")]public IActionResult AgregarPersonalizado([FromBody] AgregarProductoPersonalizado req)
        {
            try
            {
                _dao.AgregarProductoPersonalizado(
                    req.IdUsuario,
                    req.IdProducto,
                    req.Cantidad,
                    req.Opciones
                );

                return Ok("Producto agregado al carrito con personalización");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // ===============================
        // HISTORIAL POR CLIENTE
        // ===============================
        [HttpGet("historial/{idUsuario}")] public IActionResult Historial(int idUsuario)
        {
            return Ok(_dao.ListarHistorialPedidosUsuario(idUsuario));
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
        // REPORTES
        // ===============================
        [HttpGet("reporte/general")] public IActionResult ReporteGeneral()
        {
            return Ok(_dao.ReportePedidosGeneral());
        }


    }
}
