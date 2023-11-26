using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pronia.Areas.Admin.ViewModelProduct;
using Pronia.DAL;
using Pronia.Models;

namespace Pronia.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private AppDbContext _context;

        public ProductController(AppDbContext context)
        {
            _context = context;
        }

       
        public async Task<IActionResult> Index()
        {
            List<Product> products = await _context.Products
                .Include(x => x.Category).ToListAsync();
            return View(products);
        } 
        

        public async Task<IActionResult> Delete(int id)
        {
            Product product = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);
                if(product==null) return NotFound();

                _context.Products.Remove(product);

                await _context.SaveChangesAsync();
            return RedirectToAction("Index");

        }

        public async Task<IActionResult> Create()
        {
            ViewBag.CategoryId= await _context.Categories.ToListAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Product product) 
        {
            ViewBag.CategoryId = await _context.Categories.ToListAsync();

            if (product==null) return NotFound();

            if (!ModelState.IsValid)
            {
                return View();
            }

            bool result = await _context.Products.AnyAsync(x => x.Id == product.CategoryId);
            if(!result) 
            {
                ModelState.AddModelError("CategoryId", "Duzgun kategori elave elemediniz");
            }

            Product newproduct = new Product() 
            {
                Name= product.Name,
                CategoryId= product.CategoryId,
                SKU= product.SKU,
                Price= product.Price,
                Desc= product.Desc,
                ProductImages=new List<ProductImage>()
            };


            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
           
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Update(int id) 
        {
            ViewBag.CategoryId = await _context.Categories.ToListAsync();

            Product exist =await _context.Products.FirstOrDefaultAsync(x => x.Id == id);
            if (exist==null) return NotFound();

            Product product = new Product()
            {
                Name= exist.Name,
                CategoryId= exist.CategoryId,
                SKU= exist.SKU,
                Price= exist.Price,
                Desc= exist.Desc,
            };

           return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id,Product product)
        {
            ViewBag.CategoryId = await _context.Categories.ToListAsync();

            Product exist = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);
            if (exist == null) return NotFound();

            if (!ModelState.IsValid)
            {
                return View();
            }

            bool result = await _context.Products.AnyAsync(x => x.Id == product.CategoryId);
            if (!result)
            {
                ModelState.AddModelError("CategoryId", "Duzgun kategori elave elemediniz");
            }

            exist.Name = product.Name;
            exist.SKU = product.SKU;
            exist.Price = product.Price;
            exist.Desc = product.Desc;
            exist.CategoryId = product.CategoryId;

            await _context.SaveChangesAsync();


           

            return RedirectToAction("Index");
        }
    }
}
