namespace Cafeteria2025_API_REST.Models
{
    public class OpcionGrupo
    {
        public int IdGrupo { get; set; }
        public string Nombre { get; set; }
        public string? Descripcion { get; set; }
        public bool EsRequerido { get; set; }
        public int? Maximo { get; set; }
        public string TipoControl { get; set; }
        public bool Activo { get; set; }
    }
}
