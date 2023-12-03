using Microsoft.AspNetCore.Mvc;
using Pronia.DAL;
using Pronia.Models;

namespace Pronia.ViewComponents
{
    public class ProductViewComponent:ViewComponent
    {
        AppDbContext _context;

        public ProductViewComponent(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAysnc()
        {
            List<Product> products = _context.Products.Take(4).ToList();
            return View(products);
        }
    }
}
