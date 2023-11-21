using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pronia.DAL;
using Pronia.Models;

namespace Pronia.Areas.Admin.Controllers
{
    public class ShopingController : Controller
    {
        AppDbContext _context;

        public ShopingController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            List<ShopinSection> shopinSections = await _context.ShopinSections.ToListAsync();
            return View(shopinSections);
        }

        public IActionResult Create()
        {
            return View();

        }
        [HttpPost]
        public IActionResult Create(ShopinSection shopinSection)
        {
            _context.ShopinSections.Add(shopinSection);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Update(int id)
        {
            ShopinSection shopinSection = _context.ShopinSections.Find(id);
            return View(shopinSection);

        }
        [HttpPost]
        public IActionResult Update(ShopinSection shopinSection)
        {
            ShopinSection exist = _context.ShopinSections.Find(shopinSection.Id);
            exist.Title = shopinSection.Title;
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Delete(int id)
        {
            ShopinSection shopinSection = _context.ShopinSections.Find(id);
            _context.ShopinSections.Remove(shopinSection);
            _context.SaveChanges();
            return RedirectToAction("Index");

        }
    }
}
