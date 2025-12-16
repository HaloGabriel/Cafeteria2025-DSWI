using Cafeteria2025_API_REST.Models;

namespace Cafeteria2025_API_REST.DAO
{
    public interface IUsuarioDAO
    {
        Task<IEnumerable<Usuario>> Listar();
        Task<Usuario?> Buscar(int id);
        Task<bool> Insertar(Usuario usu, string usuarioCreacion);
        Task<bool> Actualizar(int id, Usuario usu, string usuarioActualiza);
        Task Desactivar(int idUsuario);
    }
}
