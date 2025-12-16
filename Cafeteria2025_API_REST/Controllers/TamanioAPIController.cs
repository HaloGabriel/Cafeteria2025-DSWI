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

        // ===============================
        // LISTAR TAMAÑOS
        // ===============================
        [HttpGet("Listar Tamaños")] public async Task<IActionResult> Listar()
=> Ok(await _tamDAO.Listar());

        // ===============================
        // BUSCAR POR ID
        // ===============================
        [HttpGet("{id}")] public async Task<IActionResult> Buscar(byte id)
        {
            var t = await _tamDAO.Buscar(id);
            return t is null ? NotFound() : Ok(t);
        }

        // ===============================
        // INSERTAR TAMAÑOS
        // ===============================
        [HttpPost("Insertar tamaño")] public async Task<IActionResult> Insertar([FromBody] Tamano tam)
            => Ok(await _tamDAO.Insertar(tam));

        // ===============================
        // ACTUALIZAR POR ID
        // ===============================
        [HttpPut("{id}")] public async Task<IActionResult> Actualizar(byte id, [FromBody] Tamano tam)
            => Ok(await _tamDAO.Actualizar(id, tam));

        // ===============================
        // DESACTIVAR POR ID
        // ===============================
        [HttpDelete("{id}")]
        public async Task<IActionResult> Desactivar(byte id)
        {
            await _tamDAO.Desactivar(id);
            return Ok("Tamaño desactivado");
        }

    }
}
