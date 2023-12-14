using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pronia.DAL;
using Pronia.Models;
using Pronia.Service;
using Pronia.ViewsModels;

namespace Pronia.Controllers
{
    public class OrderController : Controller
    {
        AppDbContext _context;

        UserManager<AppUser> _userManager;
        private readonly EmailService _emailService;

        public OrderController(AppDbContext context, UserManager<AppUser> userManager, EmailService emailService)
        {
            _context = context;
            _userManager = userManager;
           _emailService = emailService;
        }
        [Authorize]
        public async Task<IActionResult> Checkout()
        {
            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
            OrderVM orderVM = new OrderVM();
            orderVM.BasketItems = await _context.BasketItems
                .Where(b => b.AppUserId == user.Id && b.OrderId == null).Include(b => b.Product).ToListAsync();
            return View(orderVM);
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Checkout(OrderVM orderVM)
        {
            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (!ModelState.IsValid)
            {
                orderVM.BasketItems = await _context.BasketItems
             .Where(b => b.AppUserId == user.Id && b.OrderId == null).Include(b => b.Product).ToListAsync();
                ModelState.AddModelError("Address", "Addresi mutleq doldurun");
                return View(orderVM);
            }
            var userBasketItems = await _context.BasketItems
              .Where(b => b.AppUserId == user.Id && b.OrderId == null).Include(b => b.Product).ToListAsync();
            double TotalPrice = 0;
            for (int i = 0; i < userBasketItems.Count; i++)
            {
                TotalPrice += userBasketItems[i].Price * userBasketItems[i].Count;
            }
            Order order = new Order()
            {
                BasketItems = userBasketItems,
                Address = orderVM.Address,
                AppUser = user,
                CreateDate = DateTime.Now,
                TotalPrice = TotalPrice,
                Email = orderVM.Email,
                Name = orderVM.Name,
                Surname = orderVM.Surname,
            };

            _emailService.SendEmail("aliyevmeryem15@gmail.com", "Order", "Sifarisiniz tesdiqlendi");

            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();




            return RedirectToAction(nameof(Index), "Home");
        }

    }
}
