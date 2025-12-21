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


        // ===============================
        // LISTAR USUARIOS
        // ===============================
        [HttpGet("Listar Usuarios")]
        public async Task<IActionResult> Listar()
        => Ok(await _usudao.Listar());

        // ===============================
        // PAGINACIÓN USUARIOS
        // ===============================
        [HttpGet("Paginacion")]
        public async Task<IActionResult> Paginacion(int p = 1, int t = 10)
        => Ok(await _usudao.Paginacion(p, t));

        // ===============================
        // BUSCAR POR ID
        // ===============================
        [HttpGet("{id}")]
        public async Task<IActionResult> Buscar(int id)
        {
            var u = await _usudao.Buscar(id);
            return u is null ? NotFound() : Ok(u);
        }

        // ===============================
        // AÑADIR NUEVO USUARIO
        // ===============================
        [HttpPost("Añadir nuevo Usuario")]
        public async Task<IActionResult> Crear([FromBody] Usuario usu)
        {
            var ok = await _usudao.Insertar(usu, "admin");
            return Ok(ok);
        }

        // ===============================
        // ACTUALIZAR POR ID
        // ===============================
        [HttpPut("{id}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] Usuario usu)
        {
            var ok = await _usudao.Actualizar(id, usu, "admin");
            return Ok(ok);
        }

        // ===============================
        // DESACTIVAR POR ID
        // ===============================

        [HttpDelete("{id}")]
        public async Task<IActionResult> Desactivar(int id)
        {
            await _usudao.Desactivar(id);
            return Ok("Usuario desactivado");
        }


    }
}
