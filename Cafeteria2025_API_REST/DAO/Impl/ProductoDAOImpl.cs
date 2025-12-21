using Cafeteria2025_API_REST.Models;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Threading.Tasks;

namespace Cafeteria2025_API_REST.DAO.Impl
{
    public class ProductoDAOImpl: IProductoDAO
    {
        private readonly IConfiguration config;
        public ProductoDAOImpl(IConfiguration config)
        {
            this.config = config;
        }

        public async Task Actualizar(ProductoUpdate reg)
        {
            using SqlConnection cn = new SqlConnection(config["ConnectionStrings:CafeteriaSQL"]);
            using SqlCommand cmd = new SqlCommand("USP_Actualizar_Producto", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@idproducto", reg.IdProducto);
            cmd.Parameters.AddWithValue("@nombre", reg.Nombre);
            cmd.Parameters.AddWithValue("@desc", (object?)reg.Descripcion ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@precio", reg.PrecioBase);
            cmd.Parameters.AddWithValue("@idtamano", (object?)reg.IdTamano ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@stock", reg.Stock);
            cmd.Parameters.AddWithValue("@idcategoria", reg.IdCategoria);
            cmd.Parameters.AddWithValue("@imgurl", (object?)reg.ImagenUrl ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@espersonalizable", reg.EsPersonalizable);
            cmd.Parameters.AddWithValue("@activo", reg.Activo);
            cmd.Parameters.AddWithValue("@userupdate", (object?)reg.UsuarioActualizacion ?? DBNull.Value);

            await cn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<ProductoDetalle?> BuscarPorId(int id)
        {
            using SqlConnection cn = new SqlConnection(config["ConnectionStrings:CafeteriaSQL"]);
            using SqlCommand cmd = new SqlCommand("USP_Buscar_Producto_Por_ID", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@idproducto", id);

            await cn.OpenAsync();
            using SqlDataReader dr = await cmd.ExecuteReaderAsync();

            if (await dr.ReadAsync())
            {
                return new ProductoDetalle
                {
                    IdProducto = dr.GetInt32(0),
                    Nombre = dr.GetString(1),
                    Descripcion = dr.IsDBNull(2) ? null : dr.GetString(2),
                    PrecioBase = dr.GetDecimal(3),
                    Tamano = dr.IsDBNull(4) ? null : dr.GetString(4),
                    Stock = dr.GetInt32(5),
                    Categoria = dr.GetString(6),
                    ImagenUrl = dr.IsDBNull(7) ? null : dr.GetString(7),
                    EsPersonalizable = dr.GetBoolean(8),
                    Activo = dr.GetBoolean(9),
                    FechaRegistro = dr.GetDateTime(10)
                };
            }

            return null;
        }

        public async Task<ProductoUpdate?> BuscarPorId2(int id)
        {
            using SqlConnection cn = new SqlConnection(config["ConnectionStrings:CafeteriaSQL"]);
            using SqlCommand cmd = new SqlCommand("USP_Buscar_Producto_Por_ID2", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@idproducto", id);

            await cn.OpenAsync();
            using SqlDataReader dr = await cmd.ExecuteReaderAsync();

            if (await dr.ReadAsync())
            {
                return new ProductoUpdate
                {
                    IdProducto = dr.GetInt32(0),
                    Nombre = dr.GetString(1),
                    Descripcion = dr.IsDBNull(2) ? null : dr.GetString(2),
                    PrecioBase = dr.GetDecimal(3),
                    IdTamano = dr.IsDBNull(4) ? null : dr.GetByte(4),
                    Stock = dr.GetInt32(5),
                    IdCategoria = dr.GetInt32(6),
                    ImagenUrl = dr.IsDBNull(7) ? null : dr.GetString(7),
                    EsPersonalizable = dr.GetBoolean(8),
                    Activo = dr.GetBoolean(9)
                };
            }

            return null;
        }

        public async Task Desactivar(int idProducto, string? userUpdate)
        {
            using SqlConnection cn = new SqlConnection(config["ConnectionStrings:CafeteriaSQL"]);
            using SqlCommand cmd = new SqlCommand("USP_Desactivar_Producto", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@idproducto", idProducto);
            cmd.Parameters.AddWithValue("@userupdate", (object?)userUpdate ?? DBNull.Value);

            await cn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task Insertar(ProductoCreate reg)
        {
            using SqlConnection cn = new SqlConnection(config["ConnectionStrings:CafeteriaSQL"]);
            using SqlCommand cmd = new SqlCommand("USP_Insertar_Producto", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@nombre", reg.Nombre);
            cmd.Parameters.AddWithValue("@desc", (object?)reg.Descripcion ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@precio", reg.PrecioBase);
            cmd.Parameters.AddWithValue("@idtamano", (object?)reg.IdTamano ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@stock", reg.Stock);
            cmd.Parameters.AddWithValue("@idcategoria", reg.IdCategoria);
            cmd.Parameters.AddWithValue("@imgurl", (object?)reg.ImagenUrl ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@espersonalizable", reg.EsPersonalizable);

            await cn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<IEnumerable<ProductoList>> Listar()
        {
            List<ProductoList> lista = new();

            using SqlConnection cn = new SqlConnection(config["ConnectionStrings:CafeteriaSQL"]);
            using SqlCommand cmd = new SqlCommand("USP_Listar_Productos", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            await cn.OpenAsync();
            using SqlDataReader dr = await cmd.ExecuteReaderAsync();

            while (await dr.ReadAsync())
            {
                lista.Add(new ProductoList
                {
                    IdProducto = dr.GetInt32(0),
                    Nombre = dr.GetString(1),
                    Categoria = dr.GetString(2),
                    PrecioBase = dr.GetDecimal(3),
                    Stock = dr.GetInt32(4)
                });
            }

            return lista;
        }
        public IEnumerable<object> ListarOpcionesPorProducto(int idProducto)
        {
            var lista = new List<object>();

            using SqlConnection cn = new SqlConnection(config["ConnectionStrings:CafeteriaSQL"]);
            cn.Open();

            SqlCommand cmd = new SqlCommand("USP_ListarOpcionesPorProducto", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@idProducto", idProducto);

            using SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                lista.Add(new
                {
                    IdGrupo = dr.GetInt32(0),
                    Grupo = dr.GetString(1),
                    TipoControl = dr.GetString(2),
                    EsRequerido = dr.GetBoolean(3),
                    Maximo = dr.IsDBNull(4) ? (int?)null : dr.GetInt32(4),
                    IdOpcion = dr.GetInt32(5),
                    NombreOpcion = dr.GetString(6),
                    CostoAdicional = dr.GetDecimal(7)
                });
            }

            return lista;
        }

        public async Task<PaginacionRespuestaDto<ProductoList>> Paginacion(int pagina, int tamanoPagina)
        {
            PaginacionRespuestaDto<ProductoList> response = new();
            using var cn = new SqlConnection(config["ConnectionStrings:CafeteriaSQL"]);
            using var cmd = new SqlCommand("USP_PaginacionProductos", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@pagina", pagina);
            cmd.Parameters.AddWithValue("@nroRegistros", tamanoPagina);
            await cn.OpenAsync();
            using var dr = await cmd.ExecuteReaderAsync();
            if(await dr.ReadAsync())
            {
                int totalRegistros = dr.GetInt32(0);
                response.TotalRegistros = totalRegistros;
                response.TotalPaginas = totalRegistros % tamanoPagina == 0 ?
                                        totalRegistros / tamanoPagina :
                                        totalRegistros / tamanoPagina + 1;
                response.PaginaActual = pagina;
                response.TamanoPagina = tamanoPagina;
            }
            await dr.NextResultAsync();
            while(await dr.ReadAsync())
            {
                response.Datos.Add(new ProductoList()
                {
                    IdProducto = dr.GetInt32(0),
                    Nombre = dr.GetString(1),
                    Categoria = dr.GetString(2),
                    PrecioBase = dr.GetDecimal(3),
                    Stock = dr.GetInt32(4)
                });
            }
            return response;
        }
    }
}
