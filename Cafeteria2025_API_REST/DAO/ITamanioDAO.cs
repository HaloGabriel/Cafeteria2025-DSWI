using Cafeteria2025_API_REST.Models;

namespace Cafeteria2025_API_REST.DAO
{
    public interface ITamanioDAO
    {
        Task<IEnumerable<Tamano>> Listar();
        Task<Tamano?> Buscar(byte id);
        Task<bool> Insertar(Tamano tam);
        Task<bool> Actualizar(byte id, Tamano tam);
        Task<bool> Eliminar(byte id);
    }
}
