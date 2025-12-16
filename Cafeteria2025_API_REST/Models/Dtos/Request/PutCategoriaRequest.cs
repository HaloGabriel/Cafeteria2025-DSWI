namespace Cafeteria2025_API_REST.Models.Dtos.Request
{
    public class PutCategoriaRequest
    {
        public int id { get; set; }
        public string name { get; set; } = string.Empty;
        public bool enabled { get; set; }
    }
}
