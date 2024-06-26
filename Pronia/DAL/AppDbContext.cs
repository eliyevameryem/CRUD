﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Pronia.Models;

namespace Pronia.DAL
{
    public class AppDbContext:IdentityDbContext<AppUser>
    {
      public  AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        
    public DbSet<Category> Categories {  get; set; }
        public DbSet<Slider> Sliders { get; set; }
    public DbSet<Product> Products {  get; set; }
    public DbSet<ProductImage> ProductImages {  get; set; }
    public DbSet<ProductTag> ProductTags { get; set; }
    public DbSet<Tag> Tags { get; set; }

        public DbSet<Size> Sizes { get; set; }
        public DbSet<ProductSize> ProductSizes { get; set; }
        public DbSet<ShopinSection> ShopinSections { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<BasketItem> BasketItems { get; set; }

        public DbSet<Order> Orders { get; set; }




    }



}
