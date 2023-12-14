using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pronia.DAL;
using Pronia.Models;
using Pronia.Utilities;
using Pronia.ViewsModels;
using System.Runtime.CompilerServices;

namespace Pronia.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(AppDbContext context,UserManager<AppUser> userManager,SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager =signInManager;
            _roleManager = roleManager;
        }

        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }
            AppUser user = new AppUser()
            {
                Name = registerVM.Name,
                Email = registerVM.Email,
                Surname = registerVM.Surname,
                UserName= registerVM.Username
            };

            var result=await _userManager.CreateAsync(user,registerVM.Password);
            if(!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {

                    ModelState.AddModelError("", error.Description);
                    return View();
                }
            }
                await _signInManager.SignInAsync(user, isPersistent:registerVM.IsRemained);
            return RedirectToAction(nameof(Index), "Home");
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM,string ReturnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var existuser = await _userManager.FindByNameAsync(loginVM.UsernameOrEmail);
            if (existuser == null)
            {
                existuser = await _userManager.FindByEmailAsync(loginVM.UsernameOrEmail);
                if (existuser == null)
                {
                    ModelState.AddModelError("", "Username ve ya Password duzgun deyl");
                }
            }

            var singInCheck =  _signInManager.CheckPasswordSignInAsync(existuser, loginVM.Password, false).Result;
            if(singInCheck.IsLockedOut)
            {
                ModelState.AddModelError("", "Birazdan yeniden cehd edin");
            }
            if (!singInCheck.Succeeded)
            {
                ModelState.AddModelError("", "Username ve ya Password duzgun deyl");
                return View();

            }
                await _signInManager.SignInAsync(existuser, loginVM.RememberMe);
            if(ReturnUrl!=null)
            {
                return Redirect(ReturnUrl);
            }

            return RedirectToAction(nameof(Index), "Home");
        }

        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Index),"Home");
        }

        public async Task<IActionResult> CreateRole()
        {
            foreach (var role in Enum.GetValues(typeof(UserRole)))
            {
                if (await _roleManager.RoleExistsAsync(role.ToString()))
                {
                    await _roleManager.CreateAsync(new IdentityRole { Name=role.ToString() });

                }
            }
            return View();
        }

        [Authorize]
        public async Task<IActionResult> MyAccount()
        {
            AppUser user=await _userManager.FindByNameAsync(User.Identity.Name);
            if(user==null)
            {
                return RedirectToAction("Index", "Home");
            }

            List<Order> userOrders = await _context.Orders
            .Where(o => o.AppuserId == user.Id)
            .Include(o => o.BasketItems) 
            .ToListAsync();

            MyAccountVM accountVM = new MyAccountVM()
            {
                Orders=userOrders,
            };

            return View(accountVM);
        }


    }
}
