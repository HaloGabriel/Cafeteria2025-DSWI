using Azure;
using Cafeteria2025_API_REST.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Cafeteria2025_API_REST.DAO.Impl
{
    public class CategoriaDAOImpl: ICategoriaDAO
    {
        private readonly IConfiguration config;
        public CategoriaDAOImpl(IConfiguration config)
        {
            this.config = config;
        }

        public async Task Actualizar(CategoriaUpdate reg)
        {
            using var cn = new SqlConnection(config["ConnectionStrings:CafeteriaSQL"]);
            using var cmd = new SqlCommand("USP_Actualizar_Categoria", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@idcategoria", reg.IdCategoria);
            cmd.Parameters.AddWithValue("@desc", reg.Descripcion);
            cmd.Parameters.AddWithValue("@activo", reg.Activo);
            await cn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<Categoria> Buscar(int id)
        {
            Categoria categoria = new Categoria();
            using var cn = new SqlConnection(config["ConnectionStrings:CafeteriaSQL"]);
            using var cmd = new SqlCommand("USP_Buscar_Categoria_Por_ID", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@idcategoria", id);
            await cn.OpenAsync();
            using var dr = await cmd.ExecuteReaderAsync();
            if(await dr.ReadAsync())
            {
                categoria.IdCategoria = dr.GetInt32(0);
                categoria.Descripcion = dr.GetString(1);
                categoria.Activo = dr.GetBoolean(2);
                categoria.FechaRegistro = dr.GetDateTime(3);
            }
            return categoria;
        }

        public async Task Insertar(CategoriaCreate reg)
        {
            using var cn = new SqlConnection(config["ConnectionStrings:CafeteriaSQL"]);
            using var cmd = new SqlCommand("USP_Insertar_Categoria", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@desc", reg.Descripcion);
            await cn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<IEnumerable<Categoria>> Listar()
        {
            List<Categoria> temporal = new List<Categoria>();
            using var cn = new SqlConnection(config["ConnectionStrings:CafeteriaSQL"]);
            using var cmd = new SqlCommand("USP_Listar_Categorias", cn);
            await cn.OpenAsync();
            using var dr = await cmd.ExecuteReaderAsync();
            while(await dr.ReadAsync())
            {
                temporal.Add(new Categoria()
                {
                    IdCategoria = dr.GetInt32(0),
                    Descripcion = dr.GetString(1),
                    Activo = dr.GetBoolean(2),
                    FechaRegistro = dr.GetDateTime(3)
                });
            }
            return temporal;
        }

        public async Task<IEnumerable<CategoriaSelectList>> ListarDescripcionAsc()
        {
            List<CategoriaSelectList> temporal = new List<CategoriaSelectList>();
            using var cn = new SqlConnection(config["ConnectionStrings:CafeteriaSQL"]);
            using var cmd = new SqlCommand("USP_Listar_Categorias_Descripcion_Asc", cn);
            await cn.OpenAsync();
            using var dr = await cmd.ExecuteReaderAsync();
            while (await dr.ReadAsync())
            {
                temporal.Add(new CategoriaSelectList()
                {
                    IdCategoria = dr.GetInt32(0),
                    Descripcion = dr.GetString(1)
                });
            }
            return temporal;
        }

        public async Task<PaginacionRespuestaDto<Categoria>> Paginacion(int pagina, int tamanoPagina)
        {
            PaginacionRespuestaDto<Categoria> response = new();
            using var cn = new SqlConnection(config["ConnectionStrings:CafeteriaSQL"]);
            using var cmd = new SqlCommand("USP_Paginacion_Categorias", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@pagina", pagina);
            cmd.Parameters.AddWithValue("@tamanoPagina", tamanoPagina);
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
            while (await dr.ReadAsync())
            {
                response.Datos.Add(new Categoria()
                {
                    IdCategoria = dr.GetInt32(0),
                    Descripcion = dr.GetString(1),
                    Activo = dr.GetBoolean(2),
                    FechaRegistro = dr.GetDateTime(3)
                });
            }
            return response;
        }
    }
}
