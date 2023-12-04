using Microsoft.EntityFrameworkCore;
using Pronia.DAL;
using Pronia.Service;

namespace Pronia
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<AppDbContext>(opt=>
            
                opt.UseSqlServer(builder.Configuration.GetConnectionString("Default")));
            builder.Services.AddSingleton<IHttpContextAccessor, IHttpContextAccessor>();    
            
            builder.Services.AddScoped<LayoutService>();



            var app = builder.Build();


            app.MapControllerRoute(
            name: "areas",
            pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}"
            );

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=home}/{action=index}/{id?}"
                );

            app.UseStaticFiles();

            

            app.Run();
        }
    }
}