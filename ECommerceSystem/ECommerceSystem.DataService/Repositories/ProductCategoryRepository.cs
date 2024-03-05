using ECommerceSystem.DataService.Data;
using ECommerceSystem.DataService.Repositories.Interfaces;
using ECommerceSystem.Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceSystem.DataService.Repositories
{
    public class ProductCategoryRepository : GenericRepository<Product_Category>, IProductCategoryReository
    {
        public ProductCategoryRepository(AppDbContext _context) : base(_context)
        {

        }

        public async Task<bool> AddProductCategories(int ProductId, List<int> CategoriesIds)
        {
            foreach(var id in CategoriesIds)
            {
                var ProductCategory = await _Dbset.FindAsync(ProductId, id);
                if(ProductCategory == null)
                {
                    await base.AddAsync(new Product_Category { ProductId = ProductId, CategoryId = id });
                }
            }
            return true;
        }

        public async Task<bool> RemoveProductCategories(int ProductId)
        {
            var ProductCategories = await _Dbset.Where(pc => pc.ProductId == ProductId).ToListAsync();
            foreach (var category in ProductCategories)
            {
                _Dbset.Remove(category);
            }
            return true;
        }
    }
}
