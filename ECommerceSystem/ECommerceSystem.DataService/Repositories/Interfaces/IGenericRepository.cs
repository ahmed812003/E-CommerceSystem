using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceSystem.DataService.Repositories.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();

        Task<T?> FindByIdAsync(int Id);

        Task<bool> AddAsync (T Entity);

        bool Update (T Entity);

        Task<bool> DeleteAsync (int Id);
    }
}
