using Cafeteria2025_API_REST.Models;

namespace Cafeteria2025_API_REST.DAO
{
    public interface IRolDAO
    {
        Task<IEnumerable<Rol>> Listar();
    }
}
