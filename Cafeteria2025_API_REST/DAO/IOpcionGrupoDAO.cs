using Cafeteria2025_API_REST.Models;

namespace Cafeteria2025_API_REST.DAO
{
    public interface IOpcionGrupoDAO
    {
        void Insertar(OpcionGrupo grupo);
        IEnumerable<OpcionGrupo> Listar();
    }
}
