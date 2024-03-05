using ECommerceSystem.DataService.Data;
using ECommerceSystem.DataService.Repositories.Interfaces;
using ECommerceSystem.Entities.DtoModels.Update;
using ECommerceSystem.Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceSystem.DataService.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(AppDbContext _context) : base(_context)
        {
        }

        public override async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _Dbset.Include(p => p.Categories).ToListAsync();
        }

        public override async Task<Product?> FindByIdAsync(int Id)
        {
            return await _Dbset.Include(p => p.Categories).FirstOrDefaultAsync(p => p.Id == Id);
        }

        public async Task<Product?> FindByNameAsync(string Name)
        {
            return await _Dbset.FirstOrDefaultAsync(p => p.Name == Name);
        }
    }
}
