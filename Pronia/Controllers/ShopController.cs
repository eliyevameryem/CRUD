using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Pronia.DAL;
using Pronia.Models;
using Pronia.ViewsModels;

namespace Pronia.Controllers
{
    public class ShopController : Controller
    {
        private AppDbContext _db;

        public List<Product> RelatedProduct { get; private set; }

        public ShopController(AppDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {

            return View();
        }
        public async Task<IActionResult> Detail(int id)
        {
            Product product = await _db.Products.Include(c => c.Category).Include(p => p.ProductSizes).ThenInclude(ps => ps.Size).Include(i => i.ProductImages).FirstOrDefaultAsync(p => p.Id == id);
            DetailVM detailVM = new DetailVM()
            {
                Product = product,
                RelatedProduct = await _db.Products.Include(x => x.ProductTags).ThenInclude(x => x.Tag)
                   .Include(x => x.Category)
                    .Where(x => x.CategoryId == product.CategoryId && x.Id != product.Id).ToListAsync()
            };

            return View(detailVM);
            

        Product currentProduct = await _db.Products.Include(c => c.Category).Include(p => p.ProductSizes).ThenInclude(ps => ps.Size).FirstOrDefaultAsync(p => p.Id == id);
        RelatedProduct = await _db.Products
            .Include(x=>x.ProductTags).ThenInclude(x=>x.Tag)
            .Include(x=>x.Category)
            .Where(x=>x.CategoryId==currentProduct.CategoryId && x.Id!= currentProduct.Id).ToListAsync();

        if(currentProduct==null)
        {
            return NotFound();
    }
        return View(currentProduct);
        }

}
}


