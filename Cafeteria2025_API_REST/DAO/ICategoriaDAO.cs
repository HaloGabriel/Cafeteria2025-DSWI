using Cafeteria2025_API_REST.Models;

namespace Cafeteria2025_API_REST.DAO
{
    public interface ICategoriaDAO
    {
        Task<IEnumerable<Categoria>> Listar();
        Task<IEnumerable<Categoria>> ListarDescripcionAsc();
        Task<Categoria> Buscar(int id);
        void Insertar(Categoria reg);
        void Actualizar(Categoria reg);
    }
}
