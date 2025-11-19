namespace SalesOrderAPI.Domain.Entities
{
    public class Item
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }

        // Navigation property
        public ICollection<SalesOrderItem>? SalesOrderItems { get; set; }
    }
}