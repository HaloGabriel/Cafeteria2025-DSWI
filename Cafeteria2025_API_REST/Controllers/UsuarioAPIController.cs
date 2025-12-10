using Cafeteria2025_API_REST.DAO;
using Cafeteria2025_API_REST.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cafeteria2025_API_REST.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioAPIController : ControllerBase
    {
        private readonly IUsuarioDAO _usudao;
        public UsuarioAPIController(IUsuarioDAO usu)
        {
            this._usudao = usu;
        }



        [HttpGet("Listar Usuarios")]
        public async Task<IActionResult> Listar()
        => Ok(await _usudao.Listar());

        [HttpGet("{id}")]
        public async Task<IActionResult> Buscar(int id)
        {
            var u = await _usudao.Buscar(id);
            return u is null ? NotFound() : Ok(u);
        }

        [HttpPost("Añadir nuevo Usuario")]
        public async Task<IActionResult> Crear([FromBody] Usuario usu)
        {
            var ok = await _usudao.Insertar(usu, "admin");
            return Ok(ok);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] Usuario usu)
        {
            var ok = await _usudao.Actualizar(id, usu, "admin");
            return Ok(ok);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var ok = await _usudao.Eliminar(id, "admin");
            return Ok(ok);
        }


    }
}
