using Cafeteria2025_API_REST.Models;

namespace Cafeteria2025_API_REST.DAO
{
    public interface ICategoriaDAO
    {
        Task<IEnumerable<Categoria>> Listar();
        Task<IEnumerable<CategoriaSelectList>> ListarDescripcionAsc();
        Task<Categoria> Buscar(int id);
        Task Insertar(CategoriaCreate reg);
        Task Actualizar(CategoriaUpdate reg);
    }
}
