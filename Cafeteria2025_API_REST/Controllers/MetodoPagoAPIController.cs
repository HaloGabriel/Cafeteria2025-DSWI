using Cafeteria2025_API_REST.DAO;
using Cafeteria2025_API_REST.Models;
using Microsoft.AspNetCore.Mvc;

namespace Cafeteria2025_API_REST.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MetodoPagoAPIController: ControllerBase
    {
        private IMetodoPagoDAO metodoPagoDAO;
        public MetodoPagoAPIController(IMetodoPagoDAO metodoPagoDAO)
        {
            this.metodoPagoDAO = metodoPagoDAO;
        }

        [HttpGet("Listar")] public async Task<ActionResult<List<MetodoPagoList>>> ListarActivos()
        {
            var lista = await metodoPagoDAO.ListarActivos();
            return Ok(lista);
        }
    }
}
