using Cafeteria2025_API_REST.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Cafeteria2025_API_REST.DAO.Impl
{
    public class UsuarioDAOImpl : IUsuarioDAO
    {
        private readonly IConfiguration _config;

        public UsuarioDAOImpl(IConfiguration config)
        {
            _config = config;
        }

        /* =========================
           LISTAR USUARIOS ACTIVOS
        ========================= */
        public async Task<IEnumerable<Usuario>> Listar()
        {
            List<Usuario> lista = new();

            using SqlConnection cn = new(_config["ConnectionStrings:CafeteriaSQL"]);
            using SqlCommand cmd = new("USP_Listar_Usuarios", cn);

            cmd.CommandType = CommandType.StoredProcedure;

            await cn.OpenAsync();
            using var dr = await cmd.ExecuteReaderAsync();

            while (await dr.ReadAsync())
            {
                lista.Add(new Usuario
                {
                    IdUsuario = dr.GetInt32(0),
                    Nombre = dr.IsDBNull(1) ? null : dr.GetString(1),
                    Apellido = dr.IsDBNull(2) ? null : dr.GetString(2),
                    Email = dr.IsDBNull(3) ? null : dr.GetString(3),
                    PasswordHash = dr.IsDBNull(4) ? null : dr.GetString(4),
                    Telefono = dr.IsDBNull(5) ? null : dr.GetString(5),
                    IdRol = dr.GetByte(6),
                    Activo = dr.GetBoolean(7),
                    FechaRegistro = dr.GetDateTime(8),
                    FechaActualizacion = dr.IsDBNull(9) ? null : dr.GetDateTime(9),
                    UsuarioActualizacion = dr.IsDBNull(10) ? null : dr.GetString(10)
                });
            }

            return lista;
        }



        /* =========================
           BUSCAR POR ID
        ========================= */
        public async Task<Usuario?> Buscar(int id)
        {
            using SqlConnection cn = new(_config["ConnectionStrings:CafeteriaSQL"]);
            using SqlCommand cmd = new("USP_Buscar_Usuario_Por_Id", cn);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@id", id);

            await cn.OpenAsync();
            using var dr = await cmd.ExecuteReaderAsync();

            if (!await dr.ReadAsync()) return null;

            return new Usuario
            {
                IdUsuario = dr.GetInt32(0),
                Nombre = dr.IsDBNull(1) ? null : dr.GetString(1),
                Apellido = dr.IsDBNull(2) ? null : dr.GetString(2),
                Email = dr.IsDBNull(3) ? null : dr.GetString(3),
                PasswordHash = dr.IsDBNull(4) ? null : dr.GetString(4),
                Telefono = dr.IsDBNull(5) ? null : dr.GetString(5),
                IdRol = dr.GetByte(6),
                Activo = dr.GetBoolean(7),
                FechaRegistro = dr.GetDateTime(8),
                FechaActualizacion = dr.IsDBNull(9) ? null : dr.GetDateTime(9),
                UsuarioActualizacion = dr.IsDBNull(10) ? null : dr.GetString(10)
            };
        }

        /* =========================
           INSERTAR
        ========================= */
        public async Task<bool> Insertar(Usuario usu, string usuarioCreacion)
        {
            using SqlConnection cn = new(_config["ConnectionStrings:CafeteriaSQL"]);
            using SqlCommand cmd = new("USP_Insertar_Usuario", cn);

            cmd.CommandType = CommandType.StoredProcedure;

            // 👇 NOMBRES EXACTOS DEL SP
            cmd.Parameters.AddWithValue("@nom", usu.Nombre);
            cmd.Parameters.AddWithValue("@ape", usu.Apellido);
            cmd.Parameters.AddWithValue("@email", usu.Email);
            cmd.Parameters.AddWithValue("@pass", usu.PasswordHash);
            cmd.Parameters.AddWithValue("@tel", (object?)usu.Telefono ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@rol", usu.IdRol);
            cmd.Parameters.AddWithValue("@user", usuarioCreacion);

            await cn.OpenAsync();
            return await cmd.ExecuteNonQueryAsync() > 0;
        }


        /* =========================
           ACTUALIZAR
        ========================= */
        public async Task<bool> Actualizar(int id, Usuario usu, string usuarioActualiza)
        {
            using SqlConnection cn = new(_config["ConnectionStrings:CafeteriaSQL"]);
            using SqlCommand cmd = new("USP_Actualizar_Usuario", cn);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@nom", usu.Nombre);
            cmd.Parameters.AddWithValue("@ape", usu.Apellido);
            cmd.Parameters.AddWithValue("@email", usu.Email);
            cmd.Parameters.AddWithValue("@pass", usu.PasswordHash);
            cmd.Parameters.AddWithValue("@tel", (object?)usu.Telefono ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@rol", usu.IdRol);
            cmd.Parameters.AddWithValue("@user", usuarioActualiza);

            await cn.OpenAsync();
            return await cmd.ExecuteNonQueryAsync() > 0;
        }


        /* =========================
           DESACTIVAR (ELIMINACIÓN LÓGICA)
        ========================= */
        public async Task Desactivar(int idUsuario)
        {
            using SqlConnection cn = new(_config["ConnectionStrings:CafeteriaSQL"]);
            using SqlCommand cmd = new("USP_Desactivar_Usuario", cn);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@idUsuario", idUsuario);

            await cn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }
    }
}
