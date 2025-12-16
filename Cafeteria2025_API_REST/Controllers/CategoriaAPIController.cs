using Cafeteria2025_API_REST.DAO;
using Cafeteria2025_API_REST.Models;
using Microsoft.AspNetCore.Mvc;

namespace Cafeteria2025_API_REST.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriaAPIController : ControllerBase
    {
        private ICategoriaDAO categoriaDAO;
        public CategoriaAPIController(ICategoriaDAO categoriaDAO)
        {
            this.categoriaDAO = categoriaDAO;
        }

        // ===============================
        // LISTAR CATEGORIAS
        // ===============================
        [HttpGet("Lista")] public async Task<ActionResult<List<Categoria>>> Lista()
        {
            var lista = await categoriaDAO.Listar();
            return Ok(lista);
        }

        // ===============================
        // LISTAR CATEGORIAS POR DESCRIPCION
        // ===============================
        [HttpGet("Lista/Sort/Descripcion")] public async Task<ActionResult<CategoriaSelectList>> ListaDescripcionAsc()
        {
            var lista = await categoriaDAO.ListarDescripcionAsc();
            return Ok(lista);
        }

        // ===============================
        // BUSCAR POR ID
        // ===============================
        [HttpGet("{id}")] public async Task<ActionResult<Categoria>> BuscarPorId(int id = 0)
        {
            var categoria = await categoriaDAO.Buscar(id);

            if (categoria == null)
                return NotFound("¡Categoría no encontrada!");

            return Ok(categoria);
        }

        // ===============================
        // REGISTRAR CATEGORIA
        // ===============================
        [HttpPost] public async Task<ActionResult> Registrar(CategoriaCreate reg)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await categoriaDAO.Insertar(reg);
            return Ok("¡Categoría registrada!");
        }

        // ===============================
        // ACTUALIZAR CATEGORIA
        // ===============================
        [HttpPut] public async Task<ActionResult> Actualizar(CategoriaUpdate reg)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await categoriaDAO.Actualizar(reg);
            return Ok("¡Categoría actualizada!");
        }
    }
}
