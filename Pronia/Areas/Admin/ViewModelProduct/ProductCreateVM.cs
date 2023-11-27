using Pronia.Models;
namespace Pronia.Areas.Admin.ViewModelProduct
{
    public class ProductCreateVM
    {       
        public string Name { get; set; }
        public string SKU { get; set; }
        public int CategoryId { get; set; }
        public int Price { get; set; }
        public string Desc { get; set; }  
        public List<Category>? Categories { get; set; }
        public List<int> CategoryIds { get; set; }
        public List<Tag>? Tags { get; set; }
        public List<int> TagIds { get; set; }
        public List<Size>? Sizes { get; set; }
        public List<int?> SizesIds { get; set; }
        
    }
}
