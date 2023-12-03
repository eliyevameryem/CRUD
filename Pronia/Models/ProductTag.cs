using Pronia.Models.Entity;

namespace Pronia.Models
{
    public class ProductTag:BaseEntity
    {
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int Tagid { get; set; }
        public Tag Tag { get; set; }
    }
}
