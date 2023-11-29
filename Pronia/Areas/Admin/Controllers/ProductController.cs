using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pronia.Areas.Admin.ViewModelProduct;
using Pronia.DAL;
using Pronia.Models;
using Pronia.Utilities;
using System.Runtime.CompilerServices;

namespace Pronia.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private AppDbContext _context;
        private IWebHostEnvironment _env;

        public ProductController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }


        public async Task<IActionResult> Index()
        {
            List<Product> products = await _context.Products
                .Include(x => x.Category)
                .Include(pi => pi.ProductImages)
                .Include(pt => pt.ProductTags).ThenInclude(t => t.Tag)
                .Include(ps => ps.ProductSizes).ThenInclude(s => s.Size)
                .ToListAsync();
            return View(products);
        }


        public async Task<IActionResult> Delete(int id)
        {
            Product product = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);
            if (product == null) return NotFound();

            _context.Products.Remove(product);

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");

        }

        public async Task<IActionResult> Create()
        {
            ProductCreateVM productCreateVM = new ProductCreateVM();

            productCreateVM.Categories = await _context.Categories.ToListAsync();
            productCreateVM.Tags = await _context.Tags.ToListAsync();
            productCreateVM.Sizes = await _context.Sizes.ToListAsync();
            return View(productCreateVM);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductCreateVM productCreateVM)
        {
            productCreateVM.Categories = await _context.Categories.ToListAsync();
            productCreateVM.Tags = await _context.Tags.ToListAsync();
            productCreateVM.Sizes = await _context.Sizes.ToListAsync();


            if (productCreateVM == null) return NotFound();

            if (!ModelState.IsValid)
            {
                return View(productCreateVM);
            }

            bool result = await _context.Categories.AnyAsync(x => x.Id == productCreateVM.CategoryId);
            if (!result)
            {
                ModelState.AddModelError("CategoryId", "Duzgun kategori elave elemediniz");
            }

            Product newproduct = new Product()
            {
                Name = productCreateVM.Name,
                CategoryId = productCreateVM.CategoryId,
                SKU = productCreateVM.SKU,
                Price = productCreateVM.Price,
                Desc = productCreateVM.Desc,
                ProductSizes = new List<ProductSize>(),
                ProductTags = new List<ProductTag>(),
                ProductImages = new List<ProductImage>()

            };



            if (productCreateVM.SizesIds != null)
            {
                foreach (var sizeId in productCreateVM.SizesIds)
                {
                    Size size = await _context.Sizes.FirstOrDefaultAsync(x => x.Id == sizeId);
                    if (size == null)
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
                        Product = newproduct
                    };

                    newproduct.ProductSizes.Add(productSize);
                }
            }

            if (productCreateVM.TagIds != null)
            {
                foreach (var tagid in productCreateVM.TagIds)
                {
                    Tag tag = await _context.Tags.FirstOrDefaultAsync(x => x.Id == tagid);
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
                        Product = newproduct
                    };

                    //newproduct.ProductSizes.Add(productCreateVM);


                }
            }

            if (!productCreateVM.MainPhoto.CheckFileLength(20000000))
            {
                ModelState.AddModelError("MainPhoto", "Duzgun olculu sekil daxil elemediniz");
                return View(productCreateVM);
            }
            if (!productCreateVM.MainPhoto.CheckFileType("image/"))
            {
                ModelState.AddModelError("MainPhoto", "Sekil daxil edin");
                return View(productCreateVM);
            }
            if (!productCreateVM.HoverPhoto.CheckFileLength(20000000))
            {
                ModelState.AddModelError("HoverPhoto", "Duzgun olculu sekil daxil elemediniz");
                return View();
            }
            if (!productCreateVM.HoverPhoto.CheckFileType("image/"))
            {
                ModelState.AddModelError("HoverPhoto", "Sekil daxil edin");
                return View(productCreateVM);
            }
            ProductImage mainphoto = new ProductImage()
            {
                IsPrime = true,
                Product = newproduct,
                ImageUrl = productCreateVM.MainPhoto.UploadFile(_env.WebRootPath, @"\UploadImage\Product\")
            };
            ProductImage hoverphoto = new ProductImage()
            {
                IsPrime = false,
                Product = newproduct,
                ImageUrl = productCreateVM.HoverPhoto.UploadFile(_env.WebRootPath, @"\UploadImage\Product\")
            };
            foreach (IFormFile imgFile in productCreateVM.Photo)
            {
                if (!imgFile.CheckFileType("image/"))
                {
                    continue;
                }
                if (!imgFile.CheckFileLength(2000000))
                {
                    continue;
                }
                //ProductImage photo = new ProductImage()
                //{
                //    IsPrime = null,
                //    Product = newproduct,
                //    ImageUrl = productCreateVM.imgFile.UploadFile(_env.WebRootPath, @"\UploadImage\Product\")
                //};
                //newproduct.ProductImages.Add(imgFile);
            }

            newproduct.ProductImages.Add(mainphoto);
            newproduct.ProductImages.Add(hoverphoto);



            await _context.Products.AddAsync(newproduct);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Update(int id)
        {
            if (id == null || id <= 0) return BadRequest();

            Product exist = await _context.Products
                .Include(c => c.Category)
                .Include(pt => pt.ProductTags).ThenInclude(t => t.Tag)
                .Include(pi => pi.ProductImages)
                .Where(c => c.Id == id)
                .FirstOrDefaultAsync();

            if (exist == null) return NotFound();

            UpdateProductVM updateproductVM = new UpdateProductVM()
            {
                Id = exist.Id,
                Name = exist.Name,
                SKU = exist.SKU,
                Desc = exist.Desc,
                Price = exist.Price,
                CategoryId = exist.CategoryId,
                TagIds = exist.ProductTags.Select(p => p.Tagid).ToList(),
                ProductImagesVM = new List<ProductImageVM>()

            };

            foreach (var item in exist.ProductImagesVM)
            {
                ProductImageVM productImageVM = new ProductImageVM()
                {
                    Id = item.Id,
                    IsPrime = item.IsPrime,
                    ImgUrl = item.ImgUrl,

                };
                updateproductVM.ProductImagesVM.Add(productImageVM);
            }
            return View(updateproductVM);
        }





        [HttpPost]
        public async Task<IActionResult> Update(int id, UpdateProductVM updateproductVM)
        {
            updateproductVM.Categories = await _context.Categories.ToListAsync();

            Product exist = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);
            if (exist == null) return NotFound();

            if (!ModelState.IsValid)
            {
                return View(updateproductVM);
            }

            bool result = await _context.Products.AnyAsync(x => x.Id == updateproductVM.CategoryId);
            if (!result)
            {
                ModelState.AddModelError("CategoryId", "Duzgun kategori elave elemediniz");
            }
            if (updateproductVM.SizesIds != null)
            {
                foreach (var sizeId in updateproductVM.SizesIds)
                {
                    Size size = await _context.Sizes.FirstOrDefaultAsync(x => x.Id == sizeId);
                    if (size == null)
                    {
                        ModelState.AddModelError("SizeIds", "Duzgun olcunu daxil elemediniz");
                        updateproductVM.Categories = await _context.Categories.ToListAsync();
                        updateproductVM.Tags = await _context.Tags.ToListAsync();
                        updateproductVM.Sizes = await _context.Sizes.ToListAsync();
                        return View(updateproductVM);
                    }
                    ProductSize productSize = new ProductSize()
                    {
                        SizeId = size.Id,
                        Product = exist
                    };

                    exist.ProductSizes.Add(productSize);
                }
            }

            foreach (var tagid in updateproductVM.TagIds)
            {
                Tag tag = await _context.Tags.FirstOrDefaultAsync(x => x.Id == tagid);
                if (tag == null)
                {
                    ModelState.AddModelError("TagIds", "Duzgun tagi daxil elemediniz");
                    updateproductVM.Categories = await _context.Categories.ToListAsync();
                    updateproductVM.Tags = await _context.Tags.ToListAsync();
                    updateproductVM.Sizes = await _context.Sizes.ToListAsync();
                    return View(updateproductVM);
                }
                ProductTag productTag = new ProductTag()
                {
                    Tagid = tagid,
                    Product = exist
                };

                exist.ProductTags.Add(productTag);

            }

            exist.Name = updateproductVM.Name;
            exist.SKU = updateproductVM.SKU;
            exist.Price = updateproductVM.Price;
            exist.Desc = updateproductVM.Desc;
            exist.CategoryId = updateproductVM.CategoryId;


            if (updateproductVM.MainPhoto != null)
            {
                if (!updateproductVM.MainPhoto.CheckFileLength(20000000))
                {
                    ModelState.AddModelError("MainPhoto", "Duzgun olculu sekil daxil elemediniz");
                    return View(updateproductVM);
                }
                if (!updateproductVM.MainPhoto.CheckFileType("image/"))
                {
                    ModelState.AddModelError("MainPhoto", "Sekil daxil edin");
                    return View(updateproductVM);
                }
                var existMainPhoto = exist.ProductImages.FirstOrDefault(p => p.IsPrime == true);
                existMainPhoto.ImageUrl.Remove(existMainPhoto);
                ProductImage image = new ProductImage()
                {
                    ImageUrl = updateproductVM.MainPhoto.UploadFile(_env.WebRootPath, @"\Upload\Product"),
                    Product = exist.Id,
                    IsPrime = true
                };
                exist.ProductImages.Add(image);
            }
            if (updateproductVM.HoverPhoto != null)
            {
                if (!updateproductVM.HoverPhoto.CheckFileLength(20000000))
                {
                    ModelState.AddModelError("HoverPhoto", "Duzgun olculu sekil daxil elemediniz");
                    return View(updateproductVM);
                }
                if (!updateproductVM.MainPhoto.CheckFileType("image/"))
                {
                    ModelState.AddModelError("HoverPhoto", "Sekil daxil edin");
                    return View(updateproductVM);
                }
                var existHoverPhoto = exist.ProductImages.FirstOrDefault(p => p.IsPrime == false);
                existHoverPhoto.ImageUrl.Remove(existHoverPhoto);
                ProductImage image = new ProductImage()
                {
                    ImageUrl = updateproductVM.HoverPhoto.UploadFile(_env.WebRootPath, @"\Upload\Product"),
                    Product = exist.Id,
                    IsPrime = false
                };
                exist.ProductImages.Add(image);
            }
            if (updateproductVM.Photo != null)
            {
                foreach (IFormFile imgFile in updateproductVM.Photo)
                {
                    if (!imgFile.CheckFileType("image/"))
                    {

                        continue;
                    }
                    if (!imgFile.CheckFileLength(2097152))
                    {

                        continue;
                    }
                    ProductImage productImage = new ProductImage()
                    {
                        IsPrime = null,
                        ProductId = exist.Id,
                        ImageUrl = imgFile.UploadFile(_env.WebRootPath, "/Upload/Product/")
                    };
                    exist.ProductImages.Add(productImage);

                }


                await _context.SaveChangesAsync();




               
            }

            return RedirectToAction("Index");
        }
    }
}
