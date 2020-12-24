using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SomarthokBangla.Models;

namespace SomarthokBangla.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<SpecialTag>SpecialTag { get; set; }
        //public DbSet<Brand> Brand { get; set; }
        public DbSet<Manufacturer> Manufacturer { get; set; }
        public DbSet<Orders> Orders { get; set; }
        public DbSet<OrderDetails> OrderDetails { get; set; }
        //public DbSet<Review> Review { get; set; }
        public DbSet<ProductCategory> ProductCategory { get; set; }
        public DbSet<ProductTypes> ProductTypes { get; set; }
        public DbSet<Products> Products { get; set; }
        public DbSet<ApplicationUser> ApplicationUser { get; set; }

    }
}


