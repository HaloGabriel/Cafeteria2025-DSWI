using Microsoft.Data.SqlClient;
using System.Data;

namespace Cafeteria2025_API_REST.DAO.Impl
{
    public class PedidoDAOImpl : IPedidoDAO
    {
        private readonly IConfiguration config;
        public PedidoDAOImpl(IConfiguration config)
        {
            this.config = config;
        }

        public int ObtenerOCrearPedidoGenerado(int idUsuario)
        {
            using SqlConnection cn = new SqlConnection(config["ConnectionStrings:CafeteriaSQL"]);
            cn.Open();

            SqlCommand cmd = new SqlCommand("USP_ObtenerOCrearPedidoGenerado", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@idUsuario", idUsuario);

            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        public void AgregarProducto(int idPedido, int idProducto, int cantidad)
        {
            using SqlConnection cn = new SqlConnection(config["ConnectionStrings:CafeteriaSQL"]);
            cn.Open();

            SqlCommand cmd = new SqlCommand("USP_AgregarProductoPedido", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@idPedido", idPedido);
            cmd.Parameters.AddWithValue("@idProducto", idProducto);
            cmd.Parameters.AddWithValue("@cantidad", cantidad);

            cmd.ExecuteNonQuery();
        }

        public IEnumerable<object> ListarPedidoGenerado(int idUsuario)
        {
            List<object> lista = new();

            using SqlConnection cn = new SqlConnection(config["ConnectionStrings:CafeteriaSQL"]);
            cn.Open();

            SqlCommand cmd = new SqlCommand("USP_ListarPedidoGenerado", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@idUsuario", idUsuario);

            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                lista.Add(new
                {
                    IdDetallePedido = dr.GetInt32(0),
                    Producto = dr.GetString(1),
                    Cantidad = dr.GetInt32(2),
                    Precio = dr.GetDecimal(3),
                    Subtotal = dr.GetDecimal(4)
                });
            }

            return lista;
        }

        public void ConfirmarPedido(int idPedido, int idMetodoPago)
        {
            using SqlConnection cn = new SqlConnection(config["ConnectionStrings:CafeteriaSQL"]);
            cn.Open();

            SqlCommand cmd = new SqlCommand("USP_ConfirmarPedido", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@idPedido", idPedido);
            cmd.Parameters.AddWithValue("@idMetodoPago", idMetodoPago);

            cmd.ExecuteNonQuery();
        }

        public void CancelarPedido(int idPedido)
        {
            using SqlConnection cn = new SqlConnection(config["ConnectionStrings:CafeteriaSQL"]);
            cn.Open();

            SqlCommand cmd = new SqlCommand("USP_CancelarPedido", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@idPedido", idPedido);

            cmd.ExecuteNonQuery();
        }

        public void CambiarEstadoPedido(int idPedido, int idEstadoNuevo)
        {
            using SqlConnection cn = new SqlConnection(config["ConnectionStrings:CafeteriaSQL"]);
            cn.Open();

            SqlCommand cmd = new SqlCommand("USP_CambiarEstadoPedido", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@idPedido", idPedido);
            cmd.Parameters.AddWithValue("@idNuevoEstado", idEstadoNuevo);

            cmd.ExecuteNonQuery();
        }

        public IEnumerable<object> ListarPedidosOperativos()
        {
            List<object> lista = new();

            using SqlConnection cn = new SqlConnection(config["ConnectionStrings:CafeteriaSQL"]);
            cn.Open();

            SqlCommand cmd = new SqlCommand("USP_ListarPedidosOperativos", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                lista.Add(new
                {
                    IdPedido = dr.GetInt32(0),
                    Cliente = dr.GetString(1),
                    Fecha = dr.GetDateTime(2),
                    Estado = dr.GetString(3),
                    Total = dr.GetDecimal(4),
                    CodigoRecojo = dr.GetString(5)
                });
            }

            return lista;
        }

        public void AgregarProductoPersonalizado(int idUsuario, int idProducto, int cantidad, List<int> opciones)
        {
            using SqlConnection cn = new SqlConnection(config["ConnectionStrings:CafeteriaSQL"]);
            cn.Open();

            using SqlCommand cmd = new SqlCommand(
                "USP_AgregarProductoPersonalizado", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@idUsuario", idUsuario);
            cmd.Parameters.AddWithValue("@idProducto", idProducto);
            cmd.Parameters.AddWithValue("@cantidad", cantidad);

            DataTable tvp = new DataTable();
            tvp.Columns.Add("IdOpcion", typeof(int));
            foreach (var op in opciones)
                tvp.Rows.Add(op);

            SqlParameter p = cmd.Parameters.AddWithValue("@Opciones", tvp);
            p.SqlDbType = SqlDbType.Structured;
            p.TypeName = "TVP_Opciones";

            cmd.ExecuteNonQuery();
        }

        public IEnumerable<object> ListarHistorialPedidosUsuario(int idUsuario)
        {
            var lista = new List<object>();

            using SqlConnection cn = new(config["ConnectionStrings:CafeteriaSQL"]);
            using SqlCommand cmd = new("USP_Listar_Historial_Pedidos_Usuario", cn);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@idUsuario", idUsuario);

            cn.Open();
            using var dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                lista.Add(new
                {
                    IdPedido = dr.GetInt32(0),
                    Fecha = dr.GetDateTime(1),
                    Estado = dr.GetString(2),
                    Total = dr.GetDecimal(3),
                    ClienteRecoge = dr.GetString(4),
                    CodigoRecojo = dr.GetString(5)
                });
            }

            return lista;
        }

        public IEnumerable<object> ReportePedidosGeneral()
        {
            var lista = new List<object>();

            using SqlConnection cn = new(config["ConnectionStrings:CafeteriaSQL"]);
            using SqlCommand cmd = new("USP_Reporte_Pedidos_General", cn);

            cmd.CommandType = CommandType.StoredProcedure;

            cn.Open();
            using var dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                lista.Add(new
                {
                    Estado = dr.GetString(0),
                    Cantidad = dr.GetInt32(1),
                    Total = dr.IsDBNull(2) ? 0 : dr.GetDecimal(2)
                });
            }

            return lista;
        }

    }
}
