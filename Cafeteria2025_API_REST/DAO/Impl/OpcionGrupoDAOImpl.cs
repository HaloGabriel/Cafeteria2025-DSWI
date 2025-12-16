using Cafeteria2025_API_REST.DAO;
using Cafeteria2025_API_REST.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Cafeteria2025_API_REST.DAO.Impl
{
    public class OpcionGrupoDAOImpl : IOpcionGrupoDAO
    {
        private readonly IConfiguration _config;
        public OpcionGrupoDAOImpl(IConfiguration config)
        {
            _config = config;
        }

        public void Insertar(OpcionGrupo g)
        {
            using SqlConnection cn = new SqlConnection(_config["ConnectionStrings:CafeteriaSQL"]);
            cn.Open();

            SqlCommand cmd = new SqlCommand("USP_InsertarOpcionGrupo", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@nombre", g.Nombre);
            cmd.Parameters.AddWithValue("@descripcion", g.Descripcion);
            cmd.Parameters.AddWithValue("@esRequerido", g.EsRequerido);
            cmd.Parameters.AddWithValue("@maximo", (object?)g.Maximo ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@tipoControl", g.TipoControl);

            cmd.ExecuteNonQuery();
        }

        public IEnumerable<OpcionGrupo> Listar()
        {
            List<OpcionGrupo> lista = new();

            using SqlConnection cn = new SqlConnection(_config["ConnectionStrings:CafeteriaSQL"]);
            cn.Open();

            SqlCommand cmd = new SqlCommand(
                "SELECT * FROM OpcionGrupo WHERE Activo = 1", cn);

            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                lista.Add(new OpcionGrupo
                {
                    IdGrupo = dr.GetInt32(0),
                    Nombre = dr.GetString(1),
                    Descripcion = dr.IsDBNull(2) ? null : dr.GetString(2),
                    EsRequerido = dr.GetBoolean(3),
                    Maximo = dr.IsDBNull(4) ? null : dr.GetInt32(4),
                    TipoControl = dr.GetString(5),
                    Activo = dr.GetBoolean(6)
                });
            }

            return lista;
        }
    }
}
