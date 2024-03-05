using ECommerceSystem.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceSystem.DataService.Repositories.Interfaces
{
    public interface ICartProductRepository : IGenericRepository<Cart_Product>
    {
        Task<IEnumerable<Cart_Product?>?> GetAllAsync(int Id);

        Task<bool> DeleteProductAsync(int productId, int cartId);
    }
}
