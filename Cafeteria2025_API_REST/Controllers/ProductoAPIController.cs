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
        [HttpGet("Lista")] public async Task<ActionResult> Lista()
        {
            var lista = await productoDAO.Listar();
            return Ok(lista);
        }
        [HttpGet("Busqueda/{id}")] public async Task<ActionResult> Busqueda(int id = 0)
        {
            var producto = await productoDAO.BuscarPorId(id);
            return Ok(producto);
        }
        [HttpGet("Busqueda2/{id}")]
        public async Task<ActionResult> Busqueda2(int id = 0)
        {
            var producto = await productoDAO.BuscarPorId2(id);
            return Ok(producto);
        }
        [HttpPost] public async Task<ActionResult> Insertar(Producto reg)
        {
            await Task.Run(() => productoDAO.Insertar(reg));
            return Ok("¡Producto registrado!");
        }
        [HttpPut] public async Task<ActionResult> Actualizar(Producto reg)
        {
            await Task.Run(() => productoDAO.Actualizar(reg));
            return Ok("¡Producto actualizado!");
        }
    }
}
