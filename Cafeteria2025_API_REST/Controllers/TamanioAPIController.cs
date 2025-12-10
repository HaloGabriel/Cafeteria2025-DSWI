using Cafeteria2025_API_REST.DAO;
using Cafeteria2025_API_REST.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cafeteria2025_API_REST.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TamanioAPIController : ControllerBase
    {
        private ITamanioDAO _tamDAO;
        public TamanioAPIController(ITamanioDAO repo)
        {
            _tamDAO = repo;
        }

        [HttpGet("Listar Tamaños")]
        public async Task<IActionResult> Listar()
=> Ok(await _tamDAO.Listar());

        [HttpGet("{id}")]
        public async Task<IActionResult> Buscar(byte id)
        {
            var t = await _tamDAO.Buscar(id);
            return t is null ? NotFound() : Ok(t);
        }

        [HttpPost("Insertar tamaño")]
        public async Task<IActionResult> Insertar([FromBody] Tamano tam)
            => Ok(await _tamDAO.Insertar(tam));

        [HttpPut("{id}")]
        public async Task<IActionResult> Actualizar(byte id, [FromBody] Tamano tam)
            => Ok(await _tamDAO.Actualizar(id, tam));

        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar(byte id)
            => Ok(await _tamDAO.Eliminar(id));


    }
}
