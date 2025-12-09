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

        public async void Actualizar(Producto reg)
        {
            using var cn = new SqlConnection(config["ConnectionStrings:CafeteriaSQL"]);
            using var cmd = new SqlCommand("USP_Actualizar_Producto", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@idproducto", reg.IdProducto);
            cmd.Parameters.AddWithValue("@nombre", reg.Nombre);
            cmd.Parameters.AddWithValue("@desc", reg.Descripcion);
            cmd.Parameters.AddWithValue("@precio", reg.PrecioBase);
            cmd.Parameters.AddWithValue("@idtamano", reg.TmnProd?.IdTamano);
            cmd.Parameters.AddWithValue("@stock", reg.Stock);
            cmd.Parameters.AddWithValue("@idcategoria", reg.CateProd?.IdCategoria);
            cmd.Parameters.AddWithValue("@imgurl", reg.ImagenUrl);
            cmd.Parameters.AddWithValue("@espersonalizable", reg.EsPersonalizable);
            cmd.Parameters.AddWithValue("@activo", reg.Activo);
            cmd.Parameters.AddWithValue("@userupdate", reg.UsuarioActualizacion);
            await cn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<Producto> BuscarPorId(int id)
        {
            Producto producto = new Producto();
            using var cn = new SqlConnection(config["ConnectionStrings:CafeteriaSQL"]);
            using var cmd = new SqlCommand("USP_Buscar_Producto_Por_ID", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@idproducto", id);
            await cn.OpenAsync();
            using var dr = await cmd.ExecuteReaderAsync();
            if(await dr.ReadAsync())
            {
                producto.IdProducto = dr.GetInt32(0);
                producto.Nombre = dr.GetString(1);
                producto.Descripcion = await dr.IsDBNullAsync(2) ? null : dr.GetString(2);
                producto.PrecioBase = dr.GetDecimal(3);
                producto.TmnProd = new Tamano()
                {
                    Nombre = await dr.IsDBNullAsync(4) ? null : dr.GetString(4)
                };
                producto.Stock = dr.GetInt32(5);
                producto.CateProd = new Categoria()
                {
                    Descripcion = dr.GetString(6)
                };
                producto.ImagenUrl = await dr.IsDBNullAsync(7) ? null : dr.GetString(7);
                producto.EsPersonalizable = dr.GetBoolean(8);
                producto.Activo = dr.GetBoolean(9);
                producto.FechaRegistro = dr.GetDateTime(10);
            }
            return producto;
        }

        public async Task<Producto> BuscarPorId2(int id)
        {
            Producto producto = new Producto();
            using var cn = new SqlConnection(config["ConnectionStrings:CafeteriaSQL"]);
            using var cmd = new SqlCommand("USP_Buscar_Producto_Por_ID2", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@idproducto", id);
            await cn.OpenAsync();
            using var dr = await cmd.ExecuteReaderAsync();
            if (await dr.ReadAsync())
            {
                producto.IdProducto = dr.GetInt32(0);
                producto.Nombre = dr.GetString(1);
                producto.Descripcion = await dr.IsDBNullAsync(2) ? null : dr.GetString(2);
                producto.PrecioBase = dr.GetDecimal(3);
                producto.TmnProd = new Tamano()
                {
                    IdTamano = await dr.IsDBNullAsync(4) ? 0 : dr.GetInt32(4)
                };
                producto.Stock = dr.GetInt32(5);
                producto.CateProd = new Categoria()
                {
                    IdCategoria = dr.GetInt32(6)
                };
                producto.ImagenUrl = await dr.IsDBNullAsync(7) ? null : dr.GetString(7);
                producto.EsPersonalizable = dr.GetBoolean(8);
                producto.Activo = dr.GetBoolean(9);
            }
            return producto;
        }

        public async void Insertar(Producto reg)
        {
            using var cn = new SqlConnection(config["ConnectionStrings:CafeteriaSQL"]);
            using var cmd = new SqlCommand("USP_Insertar_Producto", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@nombre", reg.Nombre);
            cmd.Parameters.AddWithValue("@desc", reg.Descripcion);
            cmd.Parameters.AddWithValue("@precio", reg.PrecioBase);
            cmd.Parameters.AddWithValue("@idtamano", reg.TmnProd?.IdTamano);
            cmd.Parameters.AddWithValue("@stock", reg.Stock);
            cmd.Parameters.AddWithValue("@idcategoria", reg.CateProd?.IdCategoria);
            cmd.Parameters.AddWithValue("@imgurl", reg.ImagenUrl);
            cmd.Parameters.AddWithValue("@espersonalizable", reg.EsPersonalizable);
            await cn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<IEnumerable<Producto>> Listar()
        {
            List<Producto> temporal = new List<Producto>();
            using var cn = new SqlConnection(config["ConnectionStrings:CafeteriaSQL"]);
            using var cmd = new SqlCommand("USP_Listar_Productos", cn);
            await cn.OpenAsync();
            using var dr = await cmd.ExecuteReaderAsync();
            while(await dr.ReadAsync())
            {
                temporal.Add(new Producto()
                {
                    IdProducto = dr.GetInt32(0),
                    Nombre = dr.GetString(1),
                    CateProd = new Categoria()
                    {
                        Descripcion = dr.GetString(2)
                    },
                    PrecioBase = dr.GetDecimal(3),
                    Stock = dr.GetInt32(4)
                });
            }
            return temporal;
        }
    }
}
