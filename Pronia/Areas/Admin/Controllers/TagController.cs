using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pronia.DAL;
using Pronia.Models;

namespace Pronia.Areas.Admin.Controllers

{
    [Area("Admin")]

    public class TagController : Controller
    {
        AppDbContext _context;

        public TagController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            List<Tag> tag = await _context.Tags.ToListAsync();
            return View(tag);
        }

        public IActionResult Create()
        {
            return View();

        }
        [HttpPost]
        public IActionResult Create(Tag tag)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            _context.Tags.Add(tag);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Update(int id)
        {
            Tag tag = _context.Tags.Find(id);
            return View(tag);

        }
        [HttpPost]
        public IActionResult Update(Tag tag)
        {
            Tag exist = _context.Tags.Find(tag.Id);
            exist.Name = tag.Name;
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Delete(int id)
        {
            Tag tag = _context.Tags.Find(id);
            _context.Tags.Remove(tag);
            _context.SaveChanges();
            return RedirectToAction("Index");

        }
    }
}
