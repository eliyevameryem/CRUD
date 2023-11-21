using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pronia.DAL;
using Pronia.Models;

namespace Pronia.Areas.Admin.Controllers
{

    public class CategoryController : Controller
    {
        AppDbContext _context;

        public CategoryController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            List<Category> categories =await _context.Categories.ToListAsync();
            return View(categories);
        }

        public IActionResult Create() 
        { 
            return View();

        }
        [HttpPost]
        public IActionResult Create(Category category) 
        {
            _context.Categories.Add(category);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Update(int id)
        {
            Category category = _context.Categories.Find(id);
            return View(category);

        }
        [HttpPost]
        public IActionResult Update(Category category) 
        {
            Category exist=_context.Categories.Find(category.Id);
            exist.Name= category.Name;
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Delete(int id)
        {
            Category category= _context.Categories.Find(id);
            _context.Categories.Remove(category);
            _context.SaveChanges();
            return RedirectToAction("Index");

        }
    }
}
