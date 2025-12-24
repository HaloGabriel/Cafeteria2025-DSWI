using Cafeteria2025_API_REST.DAO;
using Cafeteria2025_API_REST.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cafeteria2025_API_REST.Controllers
{
    [ApiController]
    [Route("api/Categoria")]
    [Authorize]
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
        [Authorize(Roles = "admin,cliente")]
        [HttpGet("Lista")]
        public async Task<ActionResult<List<Categoria>>> Lista()
        {
            var lista = await categoriaDAO.Listar();
            return Ok(lista);
        }

        // ===============================
        // PAGINACIÓN CATEGORIAS
        // ===============================
        [Authorize(Roles = "admin,cliente")]
        [HttpGet("Lista/Paginacion")]
        public async Task<ActionResult<List<Categoria>>> Paginacion(int p = 1, int t = 10)
        {
            var lista = await categoriaDAO.Paginacion(p, t);
            return Ok(lista);
        }

        // ===============================
        // LISTAR CATEGORIAS POR DESCRIPCION
        // ===============================
        [Authorize(Roles = "admin,cliente")]
        [HttpGet("Lista/Sort/Descripcion")]
        public async Task<ActionResult<CategoriaSelectList>> ListaDescripcionAsc()
        {
            var lista = await categoriaDAO.ListarDescripcionAsc();
            return Ok(lista);
        }

        // ===============================
        // BUSCAR POR ID
        // ===============================
        [Authorize(Roles = "admin,cliente")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Categoria>> BuscarPorId(int id = 0)
        {
            var categoria = await categoriaDAO.Buscar(id);

            if (categoria == null)
                return NotFound("¡Categoría no encontrada!");

            return Ok(categoria);
        }

        // ===============================
        // REGISTRAR CATEGORIA
        // ===============================
        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<ActionResult> Registrar(CategoriaCreate reg)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await categoriaDAO.Insertar(reg);
            return Ok("¡Categoría registrada!");
        }

        // ===============================
        // ACTUALIZAR CATEGORIA
        // ===============================
        [Authorize(Roles = "admin")]
        [HttpPut]
        public async Task<ActionResult> Actualizar(CategoriaUpdate reg)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await categoriaDAO.Actualizar(reg);
            return Ok("¡Categoría actualizada!");
        }
    }
}
