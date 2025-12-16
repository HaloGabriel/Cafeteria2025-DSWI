using Cafeteria2025_API_REST.Models;

namespace Cafeteria2025_API_REST.DAO
{
    public interface IOpcionDAO
    {
        void Insertar(Opcion opcion);
        IEnumerable<Opcion> ListarPorGrupo(int idGrupo);
    }
}
