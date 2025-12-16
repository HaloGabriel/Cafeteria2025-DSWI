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
        [HttpGet("Lista")] public async Task<ActionResult> Lista()
        {
            var response = await categoriaDAO.Listar();
            var lista = response.Adapt<List<GetCategoriaResponse>>();
            return Ok(lista);
        }

        // ===============================
        // LISTAR CATEGORIAS POR DESCRIPCION
        // ===============================
        [HttpGet("Lista/Sort/Descripcion")] public async Task<ActionResult> ListaDescripcionAsc()
        {
            var response = await categoriaDAO.ListarDescripcionAsc();
            var lista = response.Adapt<List<GetCategoriaResponse2>>();
            return Ok(lista);
        }

        // ===============================
        // BUSCAR POR ID
        // ===============================
        [HttpGet("{id}")] public async Task<ActionResult> BuscarPorId(int id = 0)
        {
            var response = await categoriaDAO.Buscar(id);
            var categoria = response.Adapt<GetCategoriaResponse>();
            return Ok(categoria);
        }

        // ===============================
        // REGISTRAR CATEGORIA
        // ===============================
        [HttpPost] public async Task<ActionResult> Registrar(Categoria reg)
        {
            var categoria = reg.Adapt<Categoria>();
            await Task.Run(() => categoriaDAO.Insertar(categoria));
            return Ok("¡Categoría registrada!");
        }

        // ===============================
        // ACTUALIZAR CATEGORIA
        // ===============================
        [HttpPut] public async Task<ActionResult> Actualizar(Categoria reg)
        {
            var categoria = reg.Adapt<Categoria>();
            await Task.Run(() => categoriaDAO.Actualizar(categoria));
            return Ok("¡Categoría actualizada!");
        }
    }
}
