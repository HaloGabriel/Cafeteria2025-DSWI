using Cafeteria2025_API_REST.Models;
using Microsoft.Data.SqlClient;

namespace Cafeteria2025_API_REST.DAO.Impl
{
    public class MetodoPagoDAOImpl : IMetodoPagoDAO
    {
        private readonly IConfiguration config;
        public MetodoPagoDAOImpl(IConfiguration config)
        {
            this.config = config;
        }
        public async Task<IEnumerable<MetodoPagoList>> ListarActivos()
        {
            List<MetodoPagoList> temporal = new List<MetodoPagoList>();
            using var cn = new SqlConnection(config["ConnectionStrings:CafeteriaSQL"]);
            using var cmd = new SqlCommand("USP_ListarMetodosPagoActivos", cn);
            await cn.OpenAsync();
            using var dr = await cmd.ExecuteReaderAsync();
            while(await dr.ReadAsync())
            {
                temporal.Add(new MetodoPagoList()
                {
                    IdMetodoPago = dr.GetInt32(0),
                    Nombre = dr.GetString(1)
                });
            }
            return temporal;
        }
    }
}
