using ECommerceSystem.DataService.Data;
using ECommerceSystem.DataService.Repositories.Interfaces;
using ECommerceSystem.Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceSystem.DataService.Repositories
{
    public class CartProductRepository : GenericRepository<Cart_Product>, ICartProductRepository
    {
        public CartProductRepository(AppDbContext _context) : base(_context)
        {
        }

        public async Task<IEnumerable<Cart_Product?>?> GetAllAsync(int Id)
        {
            return await _Dbset.ToListAsync();
        }

        public async Task<bool> DeleteProductAsync(int productId, int cartId)
        {
            var product = await _Dbset.FindAsync(productId, cartId);
            if (product == null)
                return false;
            _Dbset.Remove(product);
            return true;
        }
    }
}
