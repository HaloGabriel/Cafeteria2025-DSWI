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
    public class TamanioDAOImpl : ITamanioDAO
    {
        private readonly IConfiguration _config;
        public TamanioDAOImpl(IConfiguration config)
        {
            this._config = config;
        }

        public async Task<bool> Actualizar(byte id, Tamano tam)
        {
            using SqlConnection cn = new(_config["ConnectionStrings:CafeteriaSQL"]);
            using SqlCommand cmd = new(@"
                UPDATE Tamano
                SET Nombre = @nom, Descripcion = @desc,
                    CostoAdicional = @costo, Activo = @activo
                WHERE IdTamano = @id", cn);

            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@nom", tam.Nombre);
            cmd.Parameters.AddWithValue("@desc", (object?)tam.Descripcion ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@costo", tam.CostoAdicional);
            cmd.Parameters.AddWithValue("@activo", tam.Activo);

            await cn.OpenAsync();
            return await cmd.ExecuteNonQueryAsync() > 0;
        }

        public async Task<Tamano?> Buscar(byte id)
        {
            using SqlConnection cn = new(_config["ConnectionStrings:CafeteriaSQL"]);
            using SqlCommand cmd = new("SELECT * FROM Tamano WHERE IdTamano = @id", cn);
            cmd.Parameters.AddWithValue("@id", id);

            await cn.OpenAsync();
            using var dr = await cmd.ExecuteReaderAsync();

            if (!dr.Read()) return null;

            return new Tamano
            {
                IdTamano = dr.GetByte(0),
                Nombre = dr.GetString(1),
                Descripcion = dr.IsDBNull(2) ? null : dr.GetString(2),
                CostoAdicional = dr.GetDecimal(3),
                Activo = dr.GetBoolean(4)
            };
        }

        public async Task<bool> Eliminar(byte id)
        {
            using SqlConnection cn = new(_config["ConnectionStrings:CafeteriaSQL"]);
            using SqlCommand cmd =
                new(@"UPDATE Tamano SET Activo = 0 WHERE IdTamano = @id", cn);

            cmd.Parameters.AddWithValue("@id", id);
            await cn.OpenAsync();
            return await cmd.ExecuteNonQueryAsync() > 0;
        }

        public async Task<bool> Insertar(Tamano tam)
        {
            using SqlConnection cn = new(_config["ConnectionStrings:CafeteriaSQL"]);
            using SqlCommand cmd = new(@"
                INSERT INTO Tamano (Nombre, Descripcion, CostoAdicional, Activo)
                VALUES (@nom, @desc, @costo, @activo)", cn);

            cmd.Parameters.AddWithValue("@nom", tam.Nombre);
            cmd.Parameters.AddWithValue("@desc", (object?)tam.Descripcion ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@costo", tam.CostoAdicional);
            cmd.Parameters.AddWithValue("@activo", tam.Activo);

            await cn.OpenAsync();
            return await cmd.ExecuteNonQueryAsync() > 0;
        }

        public async Task<IEnumerable<Tamano>> Listar()
        {
            List<Tamano> lista = new();
            using SqlConnection cn = new(_config["ConnectionStrings:CafeteriaSQL"]);
            using SqlCommand cmd = new("SELECT * FROM Tamano WHERE Activo = 1", cn);

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
    }
}
