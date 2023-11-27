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
                .Include(x => x.Category)
                .Include(pt=>pt.ProductTags).ThenInclude(t=>t.Tag)
                .Include(ps=>ps.ProductSizes).ThenInclude(s=>s.Size)
                .ToListAsync();
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
            ProductCreateVM productCreateVM = new ProductCreateVM();

            productCreateVM.Categories = await _context.Categories.ToListAsync();
            productCreateVM.Tags=await _context.Tags.ToListAsync();
            productCreateVM.Sizes=await _context.Sizes.ToListAsync();
            return View(productCreateVM);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductCreateVM productCreateVM, Product product) 
        {
            productCreateVM.Categories = await _context.Categories.ToListAsync();
            productCreateVM.Tags = await _context.Tags.ToListAsync();
            productCreateVM.Sizes = await _context.Sizes.ToListAsync();


            if (productCreateVM == null) return NotFound();

            if (!ModelState.IsValid)
            {
                return View(productCreateVM);
            }

            bool result = await _context.Products.AnyAsync(x => x.Id == productCreateVM.CategoryId);
            if(!result) 
            {
                ModelState.AddModelError("CategoryId", "Duzgun kategori elave elemediniz");
            }

            Product newproduct = new Product() 
            {
                Name= productCreateVM.Name,
                CategoryId= productCreateVM.CategoryId,
                SKU= productCreateVM.SKU,
                Price= productCreateVM.Price,
                Desc= productCreateVM.Desc,
                ProductSizes=new List<ProductSize>(),
                ProductTags=new List<ProductTag>()
            };

            if(productCreateVM.SizesIds !=null)
            {
                foreach(var sizeId in productCreateVM.SizeIds)
                {
                    Size size = await _context.Sizes.FirstOrDefaultAsync(x => x.Id == sizeId);
                    if(size==null)
                    {
                        ModelState.AddModelError("SizeIds", "Duzgun olcunu daxil elemediniz");
                        productCreateVM.Categories = await _context.Categories.ToListAsync();
                        productCreateVM.Tags = await _context.Tags.ToListAsync();
                        productCreateVM.Sizes = await _context.Sizes.ToListAsync();
                        return View(productCreateVM);
                    }
                    ProductSize productSize = new ProductSize()
                    {
                        SizeId = sizeId,
                        Product = product
                    };

                    product.ProductSizes.Add(productSize);  


                }
            }

            if (productCreateVM.TagIds != null)
            {
                foreach (var tagid in productCreateVM.TagIds)
                {
                    Tag tag = await _context.Tags.FirstOrDefaultAsync(x => x.Id == sizeId);
                    if (tag == null)
                    {
                        ModelState.AddModelError("TagIds", "Duzgun tagi daxil elemediniz");
                        productCreateVM.Categories = await _context.Categories.ToListAsync();
                        productCreateVM.Tags = await _context.Tags.ToListAsync();
                        productCreateVM.Sizes = await _context.Sizes.ToListAsync();
                        return View(productCreateVM);
                    }
                    ProductTag productTag = new ProductTag()
                    {
                        Tagid = tagid,
                        Product = product
                    };

                    product.ProductSizes.Add(productTag);


                }
            }


            await _context.Products.AddAsync(newproduct);
            await _context.SaveChangesAsync();
           
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Update(int id) 
        {
            UpdateProductVM productVM = new UpdateProductVM();
            productVM.Categories = await _context.Categories.ToListAsync();

            Product exist = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);
            if (exist == null) return NotFound();

            Product product = new Product()
            {
                Name = exist.Name,
                CategoryId = exist.CategoryId,
                SKU = exist.SKU,
                Price = exist.Price,
                Desc = exist.Desc,
            };

            return View(productVM);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id,UpdateProductVM productVM)
        {
            productVM.Categories = await _context.Categories.ToListAsync();

            Product exist = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);
            if (exist == null) return NotFound();

            if (!ModelState.IsValid)
            {
                return View(productVM);
            }

            bool result = await _context.Products.AnyAsync(x => x.Id == productVM.CategoryId);
            if (!result)
            {
                ModelState.AddModelError("CategoryId", "Duzgun kategori elave elemediniz");
            }

            exist.Name = productVM.Name;
            exist.SKU = productVM.SKU;
            exist.Price = productVM.Price;
            exist.Desc = productVM.Desc;
            exist.CategoryId = productVM.CategoryId;
           

            await _context.SaveChangesAsync();


           

            return RedirectToAction("Index");
        }
    }
}
