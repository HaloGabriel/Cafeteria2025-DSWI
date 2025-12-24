namespace Cafeteria2025_API_REST.Models
{
    public class PaginacionRespuestaDto<T>
    {
        public int PaginaActual { get; set; }
        public int TamanoPagina { get; set; }
        public int TotalRegistros { get; set; }
        public int TotalPaginas { get; set; }
        public List<T> Datos { get; set; } = [];
    }
}
