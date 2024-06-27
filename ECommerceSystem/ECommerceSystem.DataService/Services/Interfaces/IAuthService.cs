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
        Task<ProcessResult> RegisterAsync(CreateUser model);

        Task<AuthModel> GetTokenAsync(LoginUser model);

        Task<ProcessResult> EmailConfirmationAsync(string userId, string code);

        Task<ProcessResult> ForgetPassword(string email);

        Task<ProcessResult> ResetPasswordAsync(string userId, string code, string newPassword);

        Task<string> AddRoleAsync(AddRole model);
    }
}
