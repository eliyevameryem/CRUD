namespace Pronia.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string SKU { get; set; }
        public int CategoryId { get; set; }
        public int Price { get; set; }
        public string Desc { get; set; }


        public Category Category { get; set; }
      
        public List<ProductImage> ProductImages { get; set; }
        public List<ProductTag> ProductTags { get; set; }
        public List<ProductSize> ProductSizes { get; set; }


    }
}
