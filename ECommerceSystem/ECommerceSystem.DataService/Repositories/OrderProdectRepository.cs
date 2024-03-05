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
    public class OrderProdectRepository : GenericRepository<Order_Product>, IOrderProductRepository
    {
        public OrderProdectRepository(AppDbContext _context) : base(_context)
        {
        }
    }
}
