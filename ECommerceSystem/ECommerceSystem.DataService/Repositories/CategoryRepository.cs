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
    public class CategoryRepository : GenericRepository<Category> , ICategoryRepository
    {
        public CategoryRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Category?> FindByNameAsync(string Name)
        {
            return await _Dbset.FirstOrDefaultAsync(c => c.Name == Name);
        }
    }
}
