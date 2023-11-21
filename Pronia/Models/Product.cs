namespace Pronia.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string SKU { get; set; }
        public string CategoryId { get; set; }
        public string Desc { get; set; }


        public Category Category { get; set; }
        public string Tag { get; set; }
        public List<ProductImage> ProductImages { get; set; }
        public List<ProductTag> ProductTags { get; set; }
        public List<ProductSize> ProductSizes { get; set; }


    }
}
