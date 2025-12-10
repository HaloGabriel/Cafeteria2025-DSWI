using Cafeteria2025_API_REST.DAO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cafeteria2025_API_REST.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolAPIController : ControllerBase
    {
        private IRolDAO _daoRepo;
        public RolAPIController(IRolDAO repo)
        {
            this._daoRepo = repo;
        }

        [HttpGet("Listar Roles")]
        public async Task<IActionResult> Listar()
        {
            var lista = await _daoRepo.Listar();
            return Ok(lista);
        }
    }
}
