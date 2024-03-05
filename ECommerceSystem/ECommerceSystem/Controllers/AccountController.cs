using ECommerceSystem.DataService.Repositories.Interfaces;
using ECommerceSystem.DataService.Services.Interfaces;
using ECommerceSystem.Entities.DtoModels.Create;
using ECommerceSystem.Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IUnitOfWork _unit;
        public AccountController(IAuthService _authService, IUnitOfWork _unit)
        {
            this._authService = _authService;
            this._unit = _unit;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody]CreateUser model)
        {
            if (!ModelState.IsValid)
                return BadRequest(model);

            var result = await _authService.RegisterAsync(model);

            if (!result.IsAuthenticated)
                return BadRequest(result.message);

            return Ok(result);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginUser model)
        {
            if (!ModelState.IsValid)
                return BadRequest(model);

            var result = await _authService.GetTokenAsync(model);

            if (!result.IsAuthenticated)
                return BadRequest(result.message);

            return Ok(result);
        }

        [HttpPost("Addrole")]
        public async Task<IActionResult> AddRoleAsync([FromBody] AddRole model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.AddRoleAsync(model);

            if (!string.IsNullOrEmpty(result))
                return BadRequest(result);

            return Ok(model);
        }

    }
}
