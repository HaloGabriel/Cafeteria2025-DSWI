using Cafeteria2025_API_REST.Models;

namespace Cafeteria2025_API_REST.DAO
{
    public interface IProductoDAO
    {
        Task<IEnumerable<Producto>> Listar();
        Task<Producto> BuscarPorId(int id);
        Task<Producto> BuscarPorId2(int id);
        void Insertar(Producto reg);
        void Actualizar(Producto reg);
    }
}
