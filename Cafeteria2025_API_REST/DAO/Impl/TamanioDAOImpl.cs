using Cafeteria2025_API_REST.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Cafeteria2025_API_REST.DAO.Impl
{
    public class TamanioDAOImpl : ITamanioDAO
    {
        private readonly IConfiguration _config;

        public TamanioDAOImpl(IConfiguration config)
        {
            _config = config;
        }

        /* =========================
           LISTAR
        ========================= */
        public async Task<IEnumerable<Tamano>> Listar()
        {
            List<Tamano> lista = new();

            using SqlConnection cn = new(_config["ConnectionStrings:CafeteriaSQL"]);
            using SqlCommand cmd = new("USP_Listar_Tamanos", cn);

            cmd.CommandType = CommandType.StoredProcedure;

            await cn.OpenAsync();
            using var dr = await cmd.ExecuteReaderAsync();

            while (await dr.ReadAsync())
            {
                lista.Add(new Tamano
                {
                    IdTamano = dr.GetByte(0),
                    Nombre = dr.GetString(1),
                    Descripcion = dr.IsDBNull(2) ? null : dr.GetString(2),
                    CostoAdicional = dr.GetDecimal(3),
                    Activo = dr.GetBoolean(4)
                });
            }

            return lista;
        }

        /* =========================
           BUSCAR POR ID
        ========================= */
        public async Task<Tamano?> Buscar(byte id)
        {
            using SqlConnection cn = new(_config["ConnectionStrings:CafeteriaSQL"]);
            using SqlCommand cmd = new("USP_Buscar_Tamano_Por_Id", cn);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@id", id);

            await cn.OpenAsync();
            using var dr = await cmd.ExecuteReaderAsync();

            if (!await dr.ReadAsync()) return null;

            return new Tamano
            {
                IdTamano = dr.GetByte(0),
                Nombre = dr.GetString(1),
                Descripcion = dr.IsDBNull(2) ? null : dr.GetString(2),
                CostoAdicional = dr.GetDecimal(3),
                Activo = dr.GetBoolean(4)
            };
        }

        /* =========================
           INSERTAR
        ========================= */
        public async Task<bool> Insertar(Tamano tam)
        {
            using SqlConnection cn = new(_config["ConnectionStrings:CafeteriaSQL"]);
            using SqlCommand cmd = new("USP_Insertar_Tamano", cn);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@nom", tam.Nombre);
            cmd.Parameters.AddWithValue("@desc", (object?)tam.Descripcion ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@costo", tam.CostoAdicional);
            cmd.Parameters.AddWithValue("@activo", tam.Activo);

            await cn.OpenAsync();
            return await cmd.ExecuteNonQueryAsync() > 0;
        }

        /* =========================
           ACTUALIZAR
        ========================= */
        public async Task<bool> Actualizar(byte id, Tamano tam)
        {
            using SqlConnection cn = new(_config["ConnectionStrings:CafeteriaSQL"]);
            using SqlCommand cmd = new("USP_Actualizar_Tamano", cn);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@nom", tam.Nombre);
            cmd.Parameters.AddWithValue("@desc", (object?)tam.Descripcion ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@costo", tam.CostoAdicional);
            cmd.Parameters.AddWithValue("@activo", tam.Activo);

            await cn.OpenAsync();
            return await cmd.ExecuteNonQueryAsync() > 0;
        }

        /* =========================
           DESACTIVAR (ELIMINACIÓN LÓGICA)
        ========================= */
        public async Task Desactivar(byte idTamano)
        {
            using SqlConnection cn = new(_config["ConnectionStrings:CafeteriaSQL"]);
            using SqlCommand cmd = new("USP_Desactivar_Tamano", cn);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@idTamano", idTamano);

            await cn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }
    }
}
