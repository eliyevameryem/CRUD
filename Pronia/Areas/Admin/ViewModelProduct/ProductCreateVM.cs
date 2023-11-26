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
        
    }
}
