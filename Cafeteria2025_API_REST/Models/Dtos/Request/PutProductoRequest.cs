namespace Cafeteria2025_API_REST.Models.Dtos.Request
{
    public class PutProductoRequest
    {
        public int id { get; set; }
        public string name { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
        public decimal price { get; set; }
        public int idSize { get; set; }
        public int stock { get; set; }
        public int idCategory { get; set; }
        public string imageUrl { get; set; } = string.Empty;
        public bool customizable { get; set; }
        public bool enabled { get; set; }
        public string userUpdate { get; set; } = string.Empty;
    }
}
