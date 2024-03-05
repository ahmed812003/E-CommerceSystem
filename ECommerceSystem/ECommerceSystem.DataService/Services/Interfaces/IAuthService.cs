using ECommerceSystem.Entities.DtoModels.Create;
using ECommerceSystem.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceSystem.DataService.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AuthModel> RegisterAsync(CreateUser model);

        Task<AuthModel> GetTokenAsync(LoginUser model);
        Task<string> AddRoleAsync(AddRole model);
    }
}
