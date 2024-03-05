using ECommerceSystem.DataService.Data;
using ECommerceSystem.DataService.Repositories.Interfaces;
using ECommerceSystem.Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceSystem.DataService.Repositories
{
    public class CartRepository : GenericRepository<Cart>, ICartRepository
    {
        public CartRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Cart?> FindByUserId(string Id)
        {
            return await _Dbset.FirstOrDefaultAsync(c => c.UserId == Id);
        }
    }
}
