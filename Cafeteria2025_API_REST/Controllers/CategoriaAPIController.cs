using Cafeteria2025_API_REST.DAO;
using Cafeteria2025_API_REST.Models;
using Microsoft.AspNetCore.Mvc;

namespace Cafeteria2025_API_REST.Controllers
{
    [ApiController]
    [Route("API/[controller]")]
    public class CategoriaAPIController : ControllerBase
    {
        private ICategoriaDAO categoriaDAO;
        public CategoriaAPIController(ICategoriaDAO categoriaDAO)
        {
            this.categoriaDAO = categoriaDAO;
        }

        [HttpGet("Lista")] public async Task<ActionResult> Lista()
        {
            var lista = await categoriaDAO.Listar();
            return Ok(lista);
        }
        [HttpGet("Lista/Sort/Descripcion")] public async Task<ActionResult> ListaDescripcionAsc()
        {
            var lista = await categoriaDAO.ListarDescripcionAsc();
            return Ok(lista);
        }
        [HttpGet("{id}")] public async Task<ActionResult> BuscarPorId(int id = 0)
        {
            var categoria = await categoriaDAO.Buscar(id);
            return Ok(categoria);
        }
        [HttpPost] public async Task<ActionResult> Registrar(Categoria reg)
        {
            await Task.Run(() => categoriaDAO.Insertar(reg));
            return Ok("¡Categoría registrada!");
        }
        [HttpPut] public async Task<ActionResult> Actualizar(Categoria reg)
        {
            await Task.Run(() => categoriaDAO.Actualizar(reg));
            return Ok("¡Categoría actualizada!");
        }
    }
}
