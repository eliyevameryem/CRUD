
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pronia.DAL;
using Pronia.Models;

namespace pronia.controllers
{
    public class SettingController : Controller
    {
        AppDbContext _context;

        public SettingController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            List<Setting> setting = await _context.Settings.ToListAsync();
            return View(setting);
        }

        public IActionResult Create()
        {
            return View();

        }
        [HttpPost]
        public IActionResult Create(Setting setting)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            _context.Settings.Add(setting);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Update(int id)
        {
            Setting setting = _context.Settings.Find(id);
            return View(setting);

        }
        [HttpPost]
        public IActionResult Update(Setting setting)
        {
            Setting exist = _context.Settings.Find(setting);
            exist.Key=setting.Key;
            exist.Value=setting.Value;
           _context.SaveChanges();
            return RedirectToAction("index");
        }
        public IActionResult Delete(int id)
        {
            Setting setting = _context.Settings.Find(id);
            _context.Settings.Remove(setting);
            _context.SaveChanges();
            return RedirectToAction("index");

        }
    }
}
