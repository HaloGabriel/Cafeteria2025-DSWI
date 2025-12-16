namespace Cafeteria2025_API_REST.Models
{
    public class Opcion
    {
        public int IdOpcion { get; set; }
        public int IdGrupo { get; set; }
        public string NombreOpcion { get; set; }
        public decimal CostoAdicional { get; set; }
        public bool Activo { get; set; }
    }
}
