using Microsoft.Data.SqlClient;
using System.Data;

namespace Cafeteria2025_API_REST.DAO.Impl
{
    public class ProductoOpcionDAOImpl : IProductoOpcionDAO
    {
        private readonly IConfiguration _config;
        public ProductoOpcionDAOImpl(IConfiguration config)
        {
            this._config = config;
        }
        public void Asignar(int idProducto, int idOpcion)
        {
            using SqlConnection cn = new SqlConnection(_config["ConnectionStrings:CafeteriaSQL"]);
            cn.Open();

            SqlCommand cmd = new SqlCommand(
                "USP_AsignarOpcionProducto", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@idProducto", idProducto);
            cmd.Parameters.AddWithValue("@idOpcion", idOpcion);

            cmd.ExecuteNonQuery();
        }
    }
}
