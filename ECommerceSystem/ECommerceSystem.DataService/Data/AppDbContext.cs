using ECommerceSystem.Entities.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceSystem.DataService.Data
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> option) : base(option)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Cart>().HasMany(c => c.Products).WithMany(p => p.Carts).UsingEntity<Cart_Product>(
                j => j.HasOne(cp => cp.Product).WithMany(p => p.Cart_Products).HasForeignKey(op => op.ProductId),
                j => j.HasOne(cp => cp.Cart).WithMany(c => c.Cart_Products).HasForeignKey(cp => cp.CartId),
                j => j.HasKey(cp => new { cp.ProductId, cp.CartId }));

            modelBuilder.Entity<Order>().HasMany(o => o.Products).WithMany(p => p.Orders).UsingEntity<Order_Product>(
                j => j.HasOne(op => op.Product).WithMany(p => p.Order_Products).HasForeignKey(op => op.ProductId),
                j => j.HasOne(op => op.Order).WithMany(o => o.Order_Products).HasForeignKey(op => op.OrderId),
                j => j.HasKey(op => new { op.ProductId , op.OrderId }));

            modelBuilder.Entity<Product_Category>().HasKey(pc => new { pc.ProductId, pc.CategoryId });
            modelBuilder.Entity<Product>().HasMany(p => p.Categories).WithMany(c => c.Products).UsingEntity<Product_Category>();
            modelBuilder.Entity<Product_Category>().HasOne(pc => pc.Category).WithMany(c => c.Product_Categories).HasForeignKey(pc => pc.CategoryId);
            modelBuilder.Entity<Product_Category>().HasOne(pc => pc.Product).WithMany(p => p.Product_Categories).HasForeignKey(pc => pc.ProductId);
        }

        public DbSet<Cart> Carts { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<Product> Products { get; set; }
    }
}
