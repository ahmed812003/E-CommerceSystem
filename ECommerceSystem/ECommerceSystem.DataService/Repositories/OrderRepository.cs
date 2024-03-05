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
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        public OrderRepository(AppDbContext _context) : base(_context)
        {
        }

        public async Task<IEnumerable<Order>> GetByDate(DateTime date)
        {
            return await _Dbset.Where(o => o.CreatedOn == date).ToListAsync();
        }
    }
}
