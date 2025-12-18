using Cafeteria2025_API_REST.Models;

namespace Cafeteria2025_API_REST.DAO
{
    public interface IMetodoPagoDAO
    {
        Task<IEnumerable<MetodoPagoList>> ListarActivos();
    }
}
