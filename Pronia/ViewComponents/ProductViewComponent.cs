using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pronia.DAL;
using Pronia.Models;

namespace Pronia.ViewComponents
{
    public class ProductViewComponent : ViewComponent
    {
        AppDbContext _context;

        public ProductViewComponent(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<Product> products = await _context.Products.Include(x => x.ProductImages).Take(4).ToListAsync();
            return View(products);
        }
    }
}
