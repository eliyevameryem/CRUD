using Microsoft.Identity.Client;
using Pronia.Models;

namespace Pronia.ViewsModels
{
    public class DetailVM
    {
        public Product Product { get; set; }
        public List<Product> Products { get; set; }
        public List<Size> Sizes { get; set; }
        public List<Product> RelatedProduct { get; set;}
            
    }
}
