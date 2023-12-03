using Pronia.Models.Entity;

namespace Pronia.Models
{
    public class Category:BaseEntity
    {
        public string Name { get; set; }
        List<Product> Products { get; set;}
    }
}
