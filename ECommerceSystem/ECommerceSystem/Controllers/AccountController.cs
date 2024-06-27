using ECommerceSystem.DataService.Repositories.Interfaces;
using ECommerceSystem.DataService.Services.Interfaces;
using ECommerceSystem.Entities.DtoModels.Create;
using ECommerceSystem.Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Text;

namespace ECommerceSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailSender _emailSender;

        public AccountController(IAuthService _authService, UserManager<AppUser> userManager, IEmailSender emailSender)
        {
            this._authService = _authService;
            _userManager = userManager;
            _emailSender = emailSender;
        }

        [HttpGet("ConfirmEmail/{userId}/{code}")]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            var result = await _authService.EmailConfirmationAsync(userId, code);

            if (!result.IsSuccesed)
            {
                return BadRequest(result.Message);
            }

            return Ok(result.Message);
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(CreateUser model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.RegisterAsync(model);

            if (!result.IsSuccesed)
                return BadRequest(result.Message);

            var callbackUrl = await GenerateConfirmEmailUrl(model.Email);
            var encodedUrl = HtmlEncoder.Default.Encode(callbackUrl);
            await _emailSender.SendEmailAsync(model.Email, "Confirm your email",
                $"Please confirm your account by <a href='{encodedUrl}'>clicking here</a>.");

            return Ok("Please confirm your account");

            /*try
            {
                
            }
            catch
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                var deleteResult = await _userManager.DeleteAsync(user);
                if (!deleteResult.Succeeded)
                {
                    return BadRequest("Not Deleted");
                }
                return BadRequest("Register Failed, Please Register Again!");
            }*/
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginUser model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.GetTokenAsync(model);

            if (!result.IsAuthenticated)
                return BadRequest(result.message);

            return Ok(result);
        }

        [HttpPost("ForgetPassword")]
        public async Task<IActionResult> ForgetPassword([EmailAddress] string email)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await _authService.ForgetPassword(email);

            if (!result.IsSuccesed)
                return BadRequest(result.Message);

            string userId = result.Message;
            var callbackUrl = await GenerateResetPasswordUrl(userId);
            var encodedUrl = HtmlEncoder.Default.Encode(callbackUrl);
            await _emailSender.SendEmailAsync(email, "Reset Password",
                $"To Reset Password <a href='{encodedUrl}'>clicking here</a>.");

            return Ok("Check Your Email To Reset Password");
        }

        [HttpPost("ResetPassword/{userId}/{code}")]
        public async Task<IActionResult> ResetPassword(string userId, string code, string newPassword)
        {
            var result = await _authService.ResetPasswordAsync(userId, code, newPassword);

            if (!result.IsSuccesed)
            {
                return BadRequest(result.Message);
            }

            return Ok(result.Message);
        }

        [HttpPost("Addrole")]
        public async Task<IActionResult> AddRoleAsync(AddRole model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.AddRoleAsync(model);

            if (!string.IsNullOrEmpty(result))
                return BadRequest(result);

            return Ok(model);
        }

        private async Task<string> GenerateConfirmEmailUrl(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var callbackUrl = Request.Scheme + "://" + Request.Host +
                                    Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code });
            return callbackUrl;
        }

        private async Task<string> GenerateResetPasswordUrl(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var callbackUrl = Request.Scheme + "://" + Request.Host +
                                    Url.Action("ResetPassword", "Account", new { userId = userId, code = code });
            return callbackUrl;
        }

    }
}
