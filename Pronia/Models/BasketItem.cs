using Pronia.Models.Entity;

namespace Pronia.Models
{
    public class BasketItem:BaseEntity
    {
        public int ProductId { get; set; }
        public int Count { get; set; }
        public int Price { get; set; }
        public int? OrderId { get; set; }
        public string AppUserId { get; set; }   
        public Product Product { get; set; }
        public Order Order { get; set; }
        public AppUser AppUser { get; set; }




    }
}
