using Pronia.Models;
using System.ComponentModel.DataAnnotations;

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
        
        public List<Tag>? Tags { get; set; }
        public List<int>? TagIds { get; set; }
        public List<Size>? Sizes { get; set; }
        public List<int>? SizesIds { get; set; }
        [Required]
        public IFormFile MainPhoto { get; set; }
        [Required]
        public IFormFile HoverPhoto { get; set; }
        public List<IFormFile>? Photo {  get; set; }
        
    }
}
