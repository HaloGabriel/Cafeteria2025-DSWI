using Cafeteria2025_API_REST.DAO;
using Cafeteria2025_API_REST.Models;
using Cafeteria2025_API_REST.Models.Dtos.Request;
using Cafeteria2025_API_REST.Models.Dtos.Response;
using Mapster;
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
            var response = await productoDAO.Listar();
            var lista = response.Adapt<List<GetProductoResponse>>();
            return Ok(lista);
        }
        [HttpGet("Busqueda/{id}")] public async Task<ActionResult> Busqueda(int id = 0)
        {
            var response = await productoDAO.BuscarPorId(id);
            var producto = response.Adapt<GetProductoResponse2>();
            return Ok(producto);
        }
        [HttpGet("Busqueda2/{id}")]
        public async Task<ActionResult> Busqueda2(int id = 0)
        {
            var response = await productoDAO.BuscarPorId2(id);
            var producto = response.Adapt<GetProductoResponse3>();
            return Ok(producto);
        }
        [HttpPost] public async Task<ActionResult> Insertar(PostProductoRequest reg)
        {
            var producto = reg.Adapt<Producto>();
            await Task.Run(() => productoDAO.Insertar(producto));
            return Ok("¡Producto registrado!");
        }
        [HttpPut] public async Task<ActionResult> Actualizar(PutProductoRequest reg)
        {
            var producto = reg.Adapt<Producto>();
            await Task.Run(() => productoDAO.Actualizar(producto));
            return Ok("¡Producto actualizado!");
        }
    }
}
