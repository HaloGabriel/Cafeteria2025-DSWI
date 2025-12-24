namespace Cafeteria2025_API_REST.Models.Dtos.Response
{
    public class GetProductoResponse
    {
        public int id { get; set; }
        public string name { get; set; } = string.Empty;
        public string categoryName { get; set; } = string.Empty;
        public decimal price { get; set; }
        public int stock { get; set; }
    }
}
