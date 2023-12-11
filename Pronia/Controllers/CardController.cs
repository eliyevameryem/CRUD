using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
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

        UserManager<AppUser> _userManager;

        public CardController(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            List<BasketitemVM> basketItems = new List<BasketitemVM>();

            if (User.Identity.IsAuthenticated)
            {
                AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);

                List<BasketItem> userBasket= await _context.BasketItems
                    .Include(p=>p.Product).ThenInclude(x=>x.ProductImages).ToListAsync();

                foreach (var item in userBasket)
                {
                    basketItems.Add(new BasketitemVM()
                    {
                        Price=item.Price,
                        Count=item.Count,
                        ImgUrl=item.Product.ProductImages.FirstOrDefault().ImageUrl,
                        Name=item.Product.Name,
                    });
                }
            }

            else
            {
                List<BasketCookieVM> basketCookies = new List<BasketCookieVM>();
                if (Request.Cookies["Basket"] != null)
                {
                    basketCookies = JsonConvert.DeserializeObject<List<BasketCookieVM>>(Request.Cookies["Basket"]);
                    foreach (var item in basketCookies)
                    {
                        Product product = await _context.Products.FirstOrDefaultAsync(p => p.Id == item.Id);

                        if (product == null)
                        {

                            continue;
                        }

                        basketItems.Add(new BasketitemVM()
                        {
                            Id = item.Id,
                            Name = product.Name,
                            Price = product.Price,
                            ImgUrl = product.ProductImages.FirstOrDefault().ImageUrl,
                            Count = item.Count
                        });
                    }
                    return View(basketItems);
                }
            }
        }

            public async Task<IActionResult> AddBasket(int id)
            {
                var product = _context.Products.FirstOrDefault(p => p.Id == id);
                if (product == null) return NotFound();

                if (User.Identity.IsAuthenticated)
                {
                    AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
                    BasketItem existItem = _context.BasketItems
                        .FirstOrDefault(p => p.AppUserId == user.Id && p.ProductId == product.Id);
                    if (existItem != null)
                    {
                        existItem.Count++;
                    }
                    else
                    {
                        BasketItem basketItem = new BasketItem()
                        {
                            AppUser = user,
                            Product = product,
                            Count = 1,
                            Price = product.Price,
                        };

                        _context.BasketItems.Add(basketItem);
                    }
                    await _context.SaveChangesAsync();
                }


                else
                {

                    List<BasketCookieVM> basket;
                    if (Request.Cookies["Basket"] == null)
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
                        var existBasket = basket.FirstOrDefault(x => x.Id == id);
                        if (existBasket != null)
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
                        Id = id,
                        Count = 1
                    };
                    var json = JsonConvert.SerializeObject(basket);
                    Response.Cookies.Append("Basket", json);




                    List<BasketitemVM> basketitemVMs = new List<BasketitemVM>();

                    foreach (var item in basket)
                    {
                        Product newproduct = _context.Products.Include(p => p.ProductImages.Where(pi => pi.IsPrime == true)).FirstOrDefault(p => p.Id == item.Id);

                        if (newproduct == null)
                        {

                            continue;
                        }

                        //basket.Add(new BasketitemVM()
                        //{
                        //    Id = item.Id,
                        //    Name = newproduct.Name,
                        //    Price = newproduct.Price,
                        //    ImgUrl = newproduct.ProductImages.FirstOrDefault().ImageUrl,
                        //    Count = item.Count
                        //});
                    }
                }


                return RedirectToAction(nameof(Index), "Home");
            }

            public IActionResult GetBasket()
            {
                var cookie = Request.Cookies["Basket"];

                return Content(cookie);
            }

            public IActionResult RemoveBasket(int id)
            {
                string json = Request.Cookies["Basket"];
                if (json != null)
                {
                    List<BasketCookieVM> basketCookieVMs = JsonConvert.DeserializeObject<List<BasketCookieVM>>(json);
                    BasketCookieVM product = basketCookieVMs.FirstOrDefault(x => x.Id == id);
                    if (product != null)
                    {
                        basketCookieVMs.Remove(product);
                    }

                    Response.Cookies.Append("Basket", JsonConvert.SerializeObject(basketCookieVMs));
                }

                return RedirectToAction(nameof(Index), "Home");

            }


        }
    }



