using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Pronia.DAL;
using Pronia.Models;
using Pronia.ViewsModels;

namespace Pronia.Controllers
{
    public class CardController : Controller
    {
        AppDbContext _context;

        public CardController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            if (Request.Cookies["Basket"]!=null)
            {


                List<BasketitemVM> basketitemVM = new List<BasketitemVM>();
                List<BasketCookieVM> basketCookieVM = JsonConvert.DeserializeObject<List<BasketCookieVM>>(Request.Cookies["Basket"]);
                if (basketCookieVM != null)
                {
                    foreach (var item in basketCookieVM)
                    {
                        Product product = _context.Products.Include(p => p.ProductImages.Where(pi => pi.IsPrime == true)).FirstOrDefault(x => x.Id == item.Id);
                        basketitemVM.Add(new BasketitemVM()
                        {
                            Id = item.Id,
                            Name = product.Name,
                            Price = product.Price,
                            ImgUrl = product.ProductImages.FirstOrDefault().ImageUrl,
                            Count = item.Count,
                        });

                    }
                }

                return View(basketitemVM);
            }
            return NotFound();
        }


        public IActionResult AddBasket(int id)
        {
            var product=_context.Products.FirstOrDefault(p => p.Id == id);
            if (product == null) return NotFound();

            List<BasketCookieVM> basket;
            if (Request.Cookies["Basket"]==null)
            {
                BasketCookieVM basketCookieVMs = new BasketCookieVM()
                {
                    Id = id,
                    Count = 1
                };
                basket = new List<BasketCookieVM>();
                    basket.Add(basketCookieVMs);
            }
            else
            {
                basket = JsonConvert.DeserializeObject<List<BasketCookieVM>>(Request.Cookies["Basket"]);
                var existBasket= basket.FirstOrDefault(x=>x.Id==id);
                if (existBasket!=null)
                {
                    existBasket.Count += 1;
                }
                else
                {
                    BasketCookieVM basketCookieVMs = new BasketCookieVM()
                    {
                        Id = id,
                        Count = 1
                    };
                    basket.Add(basketCookieVMs);
                }
            }


            BasketCookieVM basketCookieVM = new BasketCookieVM()
            {
                Id=id,
                    Count=1
            };
            var json=JsonConvert.SerializeObject(basketCookieVM);
            Response.Cookies.Append("Basket", json);
            return RedirectToAction(nameof(Index),"Home");               
        }

        public IActionResult GetBasket()
        {
            var cookie = Request.Cookies["Basket"];
            
            return Content(cookie);
        }
    }
}
