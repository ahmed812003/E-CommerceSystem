using ECommerceSystem.DataService.Services.Interfaces;
using ECommerceSystem.Entities.DtoModels.Create;
using ECommerceSystem.Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceSystem.DataService.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly IEmailSender _emailSender;

        public AuthService(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, IEmailSender emailSender)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _emailSender = emailSender;
        }

        //done
        // Register new user without authentication
        public async Task<ProcessResult> RegisterAsync(CreateUser model)
        {
            if (await _userManager.FindByEmailAsync(model.Email) != null)
                return new ProcessResult { Message = "This Email Already Registered!" };

            if (await _userManager.FindByNameAsync(model.UserName) != null)
                return new ProcessResult { Message = "This Username Already Registered!" };


            var user = new AppUser
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.Phone,
                Email = model.Email,
                UserName = model.UserName
            };

            var Result = await _userManager.CreateAsync(user, model.Password);
            if (!Result.Succeeded)
            {
                string Error = string.Empty;
                foreach (var error in Result.Errors)
                    Error += $"{error.Description} , ";
                return new ProcessResult { Message = Error };
            }

            await _userManager.AddToRoleAsync(user, "User");

            return new ProcessResult
            {
                IsSuccesed = true
            };
        }

        // done 
        // login user and get Token
        public async Task<AuthModel> GetTokenAsync(LoginUser model)
        {
            var authModel = new AuthModel();

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user is null || !await _userManager.CheckPasswordAsync(user, model.Password))
            {
                authModel.message = "Email or Password is incorrect!";
                return authModel;
            }

            if (!user.EmailConfirmed)
            {
                authModel.message = "Email needed To be Comfirmed";
                return authModel;
            }

            var jwtSecurityToken = await CreateJwtToken(user);
            var rolesList = await _userManager.GetRolesAsync(user);

            authModel.IsAuthenticated = true;
            authModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            authModel.Email = model.Email;
            authModel.UserName = user.UserName;
            authModel.ExpireOn = jwtSecurityToken.ValidTo;
            authModel.Roles = rolesList.ToList();
            return authModel;
        }

        // done
        public async Task<ProcessResult> EmailConfirmationAsync(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return new ProcessResult { Message = "Invalid Email" };
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new ProcessResult { Message = $"Unable to load user with ID '{userId}'." };
            }

            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ConfirmEmailAsync(user, code);

            var processResult = new ProcessResult();
            processResult.Message = result.Succeeded ? "Thank you for confirming your email." : "Error confirming your email.";
            processResult.IsSuccesed = true;

            return (processResult);
        }

        //done
        public async Task<ProcessResult> ForgetPassword(string email)
        {
            if (string.IsNullOrEmpty(email))
                return new ProcessResult { Message = "Email Is Not Found" };

            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
                return new ProcessResult { Message = "No User With This Email" };

            if (!user.EmailConfirmed)
                return new ProcessResult { Message = "Please Confirm Email" };

            return new ProcessResult
            {
                Message = user.Id,
                IsSuccesed = true
            };
        }

        //done
        public async Task<ProcessResult> ResetPasswordAsync(string userId, string code, string newPassword)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(newPassword))
            {
                return new ProcessResult { Message = "Password or UserId cannot be Empty or Null" };
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new ProcessResult { Message = $"Unable to load user with ID '{userId}'." };
            }

            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ResetPasswordAsync(user, code, newPassword);

            var processResult = new ProcessResult();
            processResult.Message = result.Succeeded ? "Password Reset Succesfully" : "Unable to reset password";
            processResult.IsSuccesed = true;

            return processResult;
        }

        //done
        public async Task<string> AddRoleAsync(AddRole model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);

            if (user is null || !await _roleManager.RoleExistsAsync(model.RoleName))
                return "Invalid username or Role";

            if (await _userManager.IsInRoleAsync(user, model.RoleName))
                return "User already assigned to this role";

            var result = await _userManager.AddToRoleAsync(user, model.RoleName);

            return result.Succeeded ? string.Empty : "Something went wrong";
        }

        //done
        private async Task<JwtSecurityToken> CreateJwtToken(AppUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();

            foreach (var role in roles)
                roleClaims.Add(new Claim("roles", role));

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            }
            .Union(userClaims)
            .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _configuration["JWT:Issuer"],
                audience: _configuration["JWT:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(30),
                signingCredentials: signingCredentials);

            return jwtSecurityToken;
        }

    }
}
