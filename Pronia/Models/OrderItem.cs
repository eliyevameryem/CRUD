namespace Pronia.Models
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public string ProductName { get; set; }

        public int Count { get; set; }

        public int Price { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; }
    }
}
