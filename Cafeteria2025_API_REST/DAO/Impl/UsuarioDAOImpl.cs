using Cafeteria2025_API_REST.Models;
using Microsoft.Data.SqlClient;

namespace Cafeteria2025_API_REST.DAO.Impl
{
    /// <summary>
    /// 
    /// RECORDAR HACER LOS PROCEDURES
    /// 
    /// </summary>
    public class UsuarioDAOImpl : IUsuarioDAO
    {
        private readonly IConfiguration _config;
        public UsuarioDAOImpl(IConfiguration config)
        {
            this._config = config;
        }

        public async Task<bool> Actualizar(int id, Usuario usu, string usuarioActualiza)
        {
            using SqlConnection cn = new(_config["ConnectionStrings:CafeteriaSQL"]);
            using SqlCommand cmd = new(@"
                UPDATE Usuario
                SET Nombre = @nom, Apellido = @ape, Email = @email,
                    PasswordHash = @pass, Telefono = @tel, IdRol = @rol,
                    FechaActualizacion = SYSDATETIME(),
                    UsuarioActualizacion = @user
                WHERE IdUsuario = @id", cn);

            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@nom", usu.Nombre);
            cmd.Parameters.AddWithValue("@ape", usu.Apellido);
            cmd.Parameters.AddWithValue("@email", usu.Email);
            cmd.Parameters.AddWithValue("@pass", usu.PasswordHash);
            cmd.Parameters.AddWithValue("@tel", (object?)usu.Telefono ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@rol", usu.RolUsua);
            cmd.Parameters.AddWithValue("@user", usuarioActualiza);

            await cn.OpenAsync();
            return await cmd.ExecuteNonQueryAsync() > 0;
        }

        public async Task<Usuario?> Buscar(int id)
        {
            using SqlConnection cn = new(_config["ConnectionStrings:CafeteriaSQL"]);
            using SqlCommand cmd = new(@"
                SELECT IdUsuario, Nombre, Apellido, Email, PasswordHash,
                       Telefono, IdRol, Activo, FechaRegistro,
                       FechaActualizacion, UsuarioActualizacion
                FROM Usuario WHERE IdUsuario = @id", cn);

            cmd.Parameters.AddWithValue("@id", id);

            await cn.OpenAsync();
            using var dr = await cmd.ExecuteReaderAsync();

            if (!dr.Read()) return null;

            return new Usuario
            {
                IdUsuario = dr.GetInt32(0),
                Nombre = dr.GetString(1),
                Apellido = dr.GetString(2),
                Email = dr.GetString(3),
                PasswordHash = dr.GetString(4),
                Telefono = dr.IsDBNull(5) ? null : dr.GetString(5),
                RolUsua = new Rol()
                {
                    IdRol = dr.GetByte(6)
                },
                Activo = dr.GetBoolean(7),
                FechaRegistro = dr.GetDateTime(8),
                FechaActualizacion = dr.IsDBNull(9) ? null : dr.GetDateTime(9),
                UsuarioActualizacion = dr.IsDBNull(10) ? null : dr.GetString(10)
            };
        }

        public async Task<bool> Eliminar(int id, string usuarioActualiza)
        {
            using SqlConnection cn = new(_config["ConnectionStrings:CafeteriaSQL"]);
            using SqlCommand cmd = new(@"
                UPDATE Usuario
                SET Activo = 0,
                    FechaActualizacion = SYSDATETIME(),
                    UsuarioActualizacion = @user
                WHERE IdUsuario = @id", cn);

            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@user", usuarioActualiza);

            await cn.OpenAsync();
            return await cmd.ExecuteNonQueryAsync() > 0;
        }

        public async Task<bool> Insertar(Usuario usu, string usuarioCreacion)
        {
            using SqlConnection cn = new(_config["ConnectionStrings:CafeteriaSQL"]);
            using SqlCommand cmd = new(@"
                INSERT INTO Usuario (Nombre, Apellido, Email, PasswordHash,
                                     Telefono, IdRol, Activo, FechaRegistro,
                                     UsuarioActualizacion)
                VALUES (@nom, @ape, @email, @pass, @tel, @rol, 1,
                        SYSDATETIME(), @user)", cn);

            cmd.Parameters.AddWithValue("@nom", usu.Nombre);
            cmd.Parameters.AddWithValue("@ape", usu.Apellido);
            cmd.Parameters.AddWithValue("@email", usu.Email);
            cmd.Parameters.AddWithValue("@pass", usu.PasswordHash);
            cmd.Parameters.AddWithValue("@tel", (object?)usu.Telefono ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@rol", usu.RolUsua);
            cmd.Parameters.AddWithValue("@user", usuarioCreacion);

            await cn.OpenAsync();
            return await cmd.ExecuteNonQueryAsync() > 0;
        }

        public async Task<IEnumerable<Usuario>> Listar()
        {
            List<Usuario> lista = new();
            using SqlConnection cn = new(_config["ConnectionStrings:CafeteriaSQL"]);
            using SqlCommand cmd = new(@"
                SELECT IdUsuario, Nombre, Apellido, Email, PasswordHash,
                       Telefono, IdRol, Activo, FechaRegistro,
                       FechaActualizacion, UsuarioActualizacion
                FROM Usuario", cn);

            await cn.OpenAsync();
            using var dr = await cmd.ExecuteReaderAsync();

            while (await dr.ReadAsync())
            {
                lista.Add(new Usuario
                {
                    IdUsuario = dr.GetInt32(0),
                    Nombre = dr.GetString(1),
                    Apellido = dr.GetString(2),
                    Email = dr.GetString(3),
                    PasswordHash = dr.GetString(4),
                    Telefono = dr.IsDBNull(5) ? null : dr.GetString(5),
                    RolUsua = new Rol()
                    {
                        IdRol = dr.GetByte(6)
                    },
                    Activo = dr.GetBoolean(7),
                    FechaRegistro = dr.GetDateTime(8),
                    FechaActualizacion = dr.IsDBNull(9) ? null : dr.GetDateTime(9),
                    UsuarioActualizacion = dr.IsDBNull(10) ? null : dr.GetString(10)
                });
            }
            return lista;
        }
    }
}
