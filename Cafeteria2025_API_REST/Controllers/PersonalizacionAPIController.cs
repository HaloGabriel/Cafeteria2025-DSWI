using Cafeteria2025_API_REST.DAO;
using Cafeteria2025_API_REST.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cafeteria2025_API_REST.Controllers
{
    [ApiController]
    [Route("api/admin/personalizacion")]
    [Authorize]

    public class PersonalizacionAPIController : ControllerBase
    {
        private readonly IOpcionGrupoDAO _grupoDAO;
        private readonly IOpcionDAO _opcionDAO;
        private readonly IProductoOpcionDAO _productoOpcionDAO;

       public PersonalizacionAPIController( IOpcionGrupoDAO grupoDAO, IOpcionDAO opcionDAO, IProductoOpcionDAO productoOpcionDAO)
        {
            _grupoDAO = grupoDAO;
            _opcionDAO = opcionDAO;
            _productoOpcionDAO = productoOpcionDAO;
        }

        /* =======================
           OPCION GRUPO
        ======================= */

        [HttpPost("grupo")]
        [Authorize(Roles = "admin")]
        public IActionResult CrearGrupo([FromBody] OpcionGrupo grupo)
        {
            _grupoDAO.Insertar(grupo);
            return Ok("Grupo creado");
        }

        [HttpGet("grupo")]
        [Authorize(Roles = "admin,cliente")]
        public IActionResult ListarGrupos()
        {
            return Ok(_grupoDAO.Listar());
        }

        /* =======================
           OPCIONES
        ======================= */

        [HttpPost("opcion")]
        [Authorize(Roles = "admin")]
        public IActionResult CrearOpcion([FromBody] Opcion opcion)
        {
            _opcionDAO.Insertar(opcion);
            return Ok("Opción creada");
        }

        [HttpGet("opcion/{idGrupo}")]
        [Authorize(Roles = "admin,cliente")]
        public IActionResult ListarOpcionesPorGrupo(int idGrupo)
        {
            return Ok(_opcionDAO.ListarPorGrupo(idGrupo));
        }

        /* =======================
           PRODUCTO - OPCION
        ======================= */

        [HttpPost("producto-opcion")]
        [Authorize(Roles = "admin")]
        public IActionResult AsignarOpcionProducto(
            int idProducto, int idOpcion)
        {
            _productoOpcionDAO.Asignar(idProducto, idOpcion);
            return Ok("Opción asignada al producto");
        }
    }
}
