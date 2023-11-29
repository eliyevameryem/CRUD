using Pronia.Models;
using System.ComponentModel.DataAnnotations;

namespace Pronia.Areas.Admin.ViewModelProduct
{
    public class UpdateProductVM
    {
        public int Id { get; set; }
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
       
        public IFormFile? MainPhoto { get; set; }
       
        public IFormFile? HoverPhoto { get; set; }
        public List<IFormFile>? Photo { get; set; }
        public List<ProductImageVM>? ProductImagesVM { get; set; }
        public List<int>? ImageIds {  get; set; }

    }

    public class ProductImageVM
    {
        public int Id { get; set; }
        public bool? IsPrime { get; set; }
        public string ImgUrl { get; set; }
    }

}
