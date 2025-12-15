namespace Cafeteria2025_API_REST.Models.Dtos.Request
{
    public class PostProductoRequest
    {
        public string name { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
        public decimal price { get; set; }
        public int idSize { get; set; }
        public int stock { get; set; }
        public int idCategory { get; set; }
        public string imageUrl { get; set; } = string.Empty;
        public bool customizable { get; set; }
    }
}
