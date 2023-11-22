using Microsoft.AspNetCore.Mvc;
using Pronia.DAL;
using Pronia.Models;
using Pronia.Utilities;

namespace Pronia.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SliderController : Controller
    {
        AppDbContext _content;

        public SliderController(AppDbContext content)
        {
            _content = content;
        }

        public IActionResult Index()
        {
            return View();
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

            if (!slider.ImageFile.CheckFileType("image/"))
            {
                ModelState.AddModelError("ImageUrel", "File duzgun daxil elemediniz");
                return View();
            }
            if(slider.ImageFile.CheckFileLength(200))
            {
                ModelState.AddModelError("ImageFile", "Maksimum 2mb olcude sekil yukleye bilersiz");
                return View();
            }

            string filname = slider.ImageFile.FileName;

            filname = Guid.NewGuid().ToString()+filname;

            string path = "C:\\Users\\Asus\\Desktop\\thome\\Pronia\\wwwroot\\UploadImage\\SliderImage\\" + filname;


            using (FileStream stream = new FileStream(path, FileMode.Create))
             {
                slider.ImageFile.CopyTo(stream); 
             }

            slider.ImageUrl = filname;






            return View();
        }
    }
}
