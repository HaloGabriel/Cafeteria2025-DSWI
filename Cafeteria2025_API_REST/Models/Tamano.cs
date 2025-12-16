namespace Cafeteria2025_API_REST.Models
{
    public class Tamano
    {
        public byte IdTamano { get; set; }
        public string Nombre { get; set; }
        public string? Descripcion { get; set; }
        public decimal CostoAdicional { get; set; }
        public bool Activo { get; set; }
    }
}
