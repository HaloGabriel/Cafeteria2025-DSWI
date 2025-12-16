namespace Cafeteria2025_API_REST.Models.Dtos.Response
{
    public class GetProductoResponse2
    {
        public int id { get; set; }
        public string name { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
        public decimal price { get; set; }
        public string sizeName { get; set; } = string.Empty;
        public int stock { get; set; }
        public string categoryName { get; set; } = string.Empty;
        public string imageUrl { get; set; } = string.Empty;
        public bool customizable { get; set; }
        public bool enabled { get; set; }
        public DateTime registerDate { get; set; }
    }
}
