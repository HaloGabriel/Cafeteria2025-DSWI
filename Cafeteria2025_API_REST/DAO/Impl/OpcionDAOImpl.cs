using Cafeteria2025_API_REST.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Cafeteria2025_API_REST.DAO.Impl
{
    public class OpcionDAOImpl : IOpcionDAO
    {
        private readonly IConfiguration config;
        public OpcionDAOImpl(IConfiguration config)
        {
            this.config = config;
        }

        public void Insertar(Opcion o)
        {
            using SqlConnection cn = new SqlConnection(config["ConnectionStrings:CafeteriaSQL"]);
            cn.Open();

            SqlCommand cmd = new SqlCommand("USP_InsertarOpcion", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@idGrupo", o.IdGrupo);
            cmd.Parameters.AddWithValue("@nombre", o.NombreOpcion);
            cmd.Parameters.AddWithValue("@costo", o.CostoAdicional);

            cmd.ExecuteNonQuery();
        }

        public IEnumerable<Opcion> ListarPorGrupo(int idGrupo)
        {
            List<Opcion> lista = new();

            using SqlConnection cn = new SqlConnection(config["ConnectionStrings:CafeteriaSQL"]);
            cn.Open();

            SqlCommand cmd = new SqlCommand(
                "SELECT * FROM Opcion WHERE IdGrupo = @idGrupo AND Activo = 1", cn);
            cmd.Parameters.AddWithValue("@idGrupo", idGrupo);

            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                lista.Add(new Opcion
                {
                    IdOpcion = dr.GetInt32(0),
                    IdGrupo = dr.GetInt32(1),
                    NombreOpcion = dr.GetString(2),
                    CostoAdicional = dr.GetDecimal(3),
                    Activo = dr.GetBoolean(4)
                });
            }

            return lista;
        }
    }
}
