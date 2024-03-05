using ECommerceSystem.Entities.DtoModels.Update;
using ECommerceSystem.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceSystem.DataService.Repositories.Interfaces
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
       Task<Category?> FindByNameAsync(string Name); 
    }
}
