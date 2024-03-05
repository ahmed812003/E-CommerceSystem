using ECommerceSystem.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceSystem.DataService.Repositories.Interfaces
{
    public interface IProductCategoryReository : IGenericRepository<Product_Category>
    {
        Task<bool> AddProductCategories(int ProductId, List<int> CategoriesIds);

        Task<bool> RemoveProductCategories(int ProductId);
    }
}
