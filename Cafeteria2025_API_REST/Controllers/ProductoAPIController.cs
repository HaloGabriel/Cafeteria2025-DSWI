using Cafeteria2025_API_REST.DAO;
using Cafeteria2025_API_REST.Models;
using Microsoft.AspNetCore.Mvc;

namespace Cafeteria2025_API_REST.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductoAPIController : ControllerBase
    {
        private readonly IProductoDAO productoDAO;
        public ProductoAPIController(IProductoDAO productoDAO)
        {
            this.productoDAO = productoDAO;
        }

        // ===============================
        // LISTAR PRODUCTOS (LIGERO)
        // ===============================
        [HttpGet] public async Task<ActionResult<IEnumerable<ProductoList>>> Listar()
        {
            var lista = await productoDAO.Listar();
            return Ok(lista);
        }

        // =========================================
        // LISTAR PRODUCTOS (LIGERO) (PAGINACIÓN)
        // =========================================
        [HttpGet("paginacion")] public async Task<ActionResult<PaginacionRespuestaDto<Producto>>> Paginacion([FromQuery] int p = 1, [FromQuery] int t = 10)
        {
            var response = await productoDAO.Paginacion(p, t);
            return Ok(response);
        }

        // ===============================
        // VER DETALLE DE PRODUCTO
        // ===============================
        [HttpGet("{id:int}")] public async Task<ActionResult<ProductoDetalle>> Detalle(int id)
        {
            var producto = await productoDAO.BuscarPorId(id);

            if (producto == null)
                return NotFound("Producto no encontrado");

            return Ok(producto);
        }

        // ===============================
        // OBTENER PRODUCTO PARA EDITAR
        // ===============================
        [HttpGet("editar/{id:int}")] public async Task<ActionResult<ProductoUpdate>> ObtenerParaEditar(int id)
        {
            var producto = await productoDAO.BuscarPorId2(id);

            if (producto == null)
                return NotFound("Producto no encontrado");

            return Ok(producto);
        }

        // ===============================
        // CREAR PRODUCTO
        // ===============================
        [HttpPost] public async Task<ActionResult> Crear([FromBody] ProductoCreate reg)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await productoDAO.Insertar(reg);
            return Ok("¡Producto registrado correctamente!");
        }

        // ===============================
        // ACTUALIZAR PRODUCTO
        // ===============================
        [HttpPut] public async Task<ActionResult> Actualizar([FromBody] ProductoUpdate reg)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await productoDAO.Actualizar(reg);
            return Ok("¡Producto actualizado correctamente!");
        }

        // ===============================
        // DESACTIVAR PRODUCTO (BAJA LÓGICA)
        // ===============================
        [HttpDelete("{id:int}")] public async Task<ActionResult> Desactivar(int id, [FromQuery] string? userUpdate)
        {
            await productoDAO.Desactivar(id, userUpdate);
            return Ok("¡Producto desactivado correctamente!");
        }

        // ===============================
        // VER OPCIONES POR PRODUCTO
        // ===============================
        [HttpGet("{idProducto}/opciones")]public IActionResult Opciones(int idProducto)
        {
            return Ok(productoDAO.ListarOpcionesPorProducto(idProducto));
        }

    }
}
