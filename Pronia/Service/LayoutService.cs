using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Pronia.DAL;
using Pronia.Models;
using Pronia.ViewsModels;


namespace Pronia.Service
{
    public class LayoutService
    {
        AppDbContext _context;
        private readonly IHttpContextAccessor _http;
        public LayoutService(AppDbContext context, IHttpContextAccessor http)
        {
            _context = context;
            _http = http;
        }



        public async Task<Dictionary<string,string>> GetSettings()
        {
            var settings = await _context.Settings.ToDictionaryAsync(s=>s.Key,s=>s.Value);
            return settings;
        }
        public List<BasketitemVM> GetBasket()
        {
            List<BasketitemVM> basket = new List<BasketitemVM>();
            var jsonBasket = _http.HttpContext.Request.Cookies["Basket"];
            if (jsonBasket != null)
            {
                List<BasketCookieVM> basketCookie = JsonConvert.DeserializeObject<List<BasketCookieVM>>(jsonBasket);

                foreach (var item in basketCookie)
                {
                    Product product = _context.Products.Include(p => p.ProductImages.Where(pi => pi.IsPrime == true)).FirstOrDefault(p => p.Id == item.Id);

                    if (product == null)
                    {

                        continue;
                    }

                    basket.Add(new BasketitemVM()
                    {
                        Id = item.Id,
                        Name = product.Name,
                        Price = product.Price,
                        ImgUrl = product.ProductImages.FirstOrDefault().ImageUrl,
                        Count = item.Count
                    });
                }
            }
            return basket;


        }
    }
}
