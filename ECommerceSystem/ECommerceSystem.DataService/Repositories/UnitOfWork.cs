using ECommerceSystem.DataService.Data;
using ECommerceSystem.DataService.Repositories.Interfaces;
using ECommerceSystem.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceSystem.DataService.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        public ICartRepository Cart { get; }

        public ICategoryRepository Category { get; }

        public IOrderRepository Order { get; }

        public IProductRepository Product { get; }

        public IProductCategoryReository ProductCategory { get; }

        public IOrderProductRepository OrderProduct { get; }

        public ICartProductRepository CartProduct { get; }

        private readonly AppDbContext _context;

        public UnitOfWork(AppDbContext _context)
        {
            this._context = _context;
            Cart = new CartRepository(_context);
            Category = new CategoryRepository(_context);
            Order = new OrderRepository(_context);
            Product = new ProductRepository(_context);
            ProductCategory = new ProductCategoryRepository(_context);
            OrderProduct = new OrderProdectRepository(_context);
            CartProduct = new CartProductRepository(_context);
        }

        public async Task<bool> Complete()
        {
            var Result = await _context.SaveChangesAsync();
            return Result > 0;
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
