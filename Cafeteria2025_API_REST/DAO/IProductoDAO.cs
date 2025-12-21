using Cafeteria2025_API_REST.Models;

namespace Cafeteria2025_API_REST.DAO
{
    public interface IProductoDAO
    {
        Task<IEnumerable<ProductoList>> Listar();
        Task<PaginacionRespuestaDto<ProductoList>> Paginacion(int pagina, int tamanoPagina);
        Task<ProductoDetalle?> BuscarPorId(int id);
        Task<ProductoUpdate?> BuscarPorId2(int id);
        Task Insertar(ProductoCreate reg);
        Task Actualizar(ProductoUpdate reg);
        Task Desactivar(int idProducto, string? userUpdate);
        IEnumerable<object> ListarOpcionesPorProducto(int idProducto);
    }
}
