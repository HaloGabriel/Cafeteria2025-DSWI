using Cafeteria2025_API_REST.DAO;
using Cafeteria2025_API_REST.Helpers;
using Cafeteria2025_API_REST.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Cafeteria2025_API_REST.Controllers
{
    [Route("api/auth")]
    [AllowAnonymous]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly IUsuarioDAO _usuarioDAO;
        private readonly IConfiguration _config;

        public AuthController(IUsuarioDAO usuarioDAO, IConfiguration config)
        {
            _usuarioDAO = usuarioDAO;
            _config = config;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Models.LoginRequest req)
        {
            var usuario = await _usuarioDAO.Login(req.Email, req.Password);

            if (usuario == null)
                return Unauthorized("Credenciales incorrectas");

            var token = JwtHelper.GenerarToken(usuario, _config);

            return Ok(new
            {
                token,
                usuario.IdUsuario,
                usuario.Email,
                Rol = usuario.RolNombre
            });
        }
    }
}