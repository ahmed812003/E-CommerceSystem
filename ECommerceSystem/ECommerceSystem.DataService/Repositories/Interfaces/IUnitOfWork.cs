using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceSystem.DataService.Repositories.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        ICartRepository Cart { get; }
        ICategoryRepository Category { get; }
        IOrderRepository Order { get; }
        IProductRepository Product { get; }
        IProductCategoryReository ProductCategory { get; }
        IOrderProductRepository OrderProduct { get; }
        ICartProductRepository CartProduct { get; }
        Task<bool> Complete();

    }
}
