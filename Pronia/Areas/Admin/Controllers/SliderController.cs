using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Pronia.DAL;
using Pronia.Models;
using Pronia.Utilities;

namespace Pronia.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SliderController : Controller
    {
        AppDbContext _context;
        private IWebHostEnvironment _env;

        public SliderController(AppDbContext context,IWebHostEnvironment env)
        {
            _context = context;
            _env=env;
        }

        public async Task<IActionResult> Index()
        {
            List<Slider> slider = await _context.Sliders.ToListAsync();
            return View(slider);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Slider slider) 
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            //if (!slider.ImageFile.CheckFileType("image/"))
            //{
            //    ModelState.AddModelError("ImageUrel", "File duzgun daxil elemediniz");
            //    return View();
            //}

            if (slider.ImageFile.CheckFileLength(200))
            {
                ModelState.AddModelError("ImageFile", "Maksimum 2mb olcude sekil yukleye bilersiz");
                return View();
            }

            
            string filname = slider.ImageFile.FileName;

            filname = Guid.NewGuid().ToString()+filname;

            string path = _env.WebRootPath+@"/UploadImage/SliderImage" + filname;


            using (FileStream stream = new FileStream(path, FileMode.Create))
             {
                slider.ImageFile.CopyTo(stream); 
             }

            slider.ImageUrl = filname;



             _context.Sliders.AddAsync(slider);
             _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));


            return View();
        }
    }
}
