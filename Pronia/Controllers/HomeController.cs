using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pronia.DAL;
using Pronia.Models;
using Pronia.ViewsModels;

namespace Pronia.Controllers
{
    public class HomeController : Controller
    {
        AppDbContext _context;
        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            HomeVM HomeVM = new HomeVM()
            {
                Products = await _context.Products.Include(p => p.ProductImages).ToListAsync(),
                ShopinSections=await _context.ShopinSections.ToListAsync(),
                Sliders = await _context.Sliders.ToListAsync()

            };

            return View(HomeVM);
        }
    }
}
