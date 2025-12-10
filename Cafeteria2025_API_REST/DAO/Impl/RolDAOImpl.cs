using Cafeteria2025_API_REST.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Cafeteria2025_API_REST.DAO.Impl
{
    /// <summary>
    /// 
    /// RECORDAR HACER LOS PROCEDURES
    /// 
    /// </summary>
    public class RolDAOImpl : IRolDAO
    {
        private readonly IConfiguration config;
        public RolDAOImpl(IConfiguration config)
        {
            this.config = config;
        }
        public async Task<IEnumerable<Rol>> Listar()
        {
            List<Rol> lista = new();

            using SqlConnection cn = new(config["ConnectionStrings:CafeteriaSQL"]);
            using SqlCommand cmd = new("SELECT IdRol, Nombre, Descripcion, Activo FROM Rol WHERE Activo = 1", cn);

            await cn.OpenAsync();
            using var dr = await cmd.ExecuteReaderAsync();

            while (await dr.ReadAsync())
            {
                lista.Add(new Rol
                {
                    IdRol = dr.GetByte(0),
                    Nombre = dr.GetString(1),
                    Descripcion = dr.IsDBNull(2) ? null : dr.GetString(2),
                    Activo = dr.GetBoolean(3)
                });
            }

            return lista;
        }
    }
}
