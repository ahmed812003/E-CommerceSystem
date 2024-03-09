using ECommerceSystem.DataService.Repositories.Interfaces;
using ECommerceSystem.Entities.DtoModels.Display;
using ECommerceSystem.Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommerceSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly IUnitOfWork _unit;
        private readonly UserManager<AppUser> _userManager;

        public CartController(IUnitOfWork _unit , UserManager<AppUser> _userManager)
        {
            this._unit = _unit;
            this._userManager = _userManager;
        }

        [Authorize(Roles = "User , Admin")]
        //user
        [HttpGet("GetCartProducts")]
        public async Task<IActionResult> GetProducts()
        {
            string? Username = User.FindFirstValue(ClaimTypes.NameIdentifier); // will give the user's userId
            //string? UserId = "c19b9924-6399-4128-abe8-c5a591d10c3c";
            if (Username == null)
                return BadRequest("Register Or Login Please!");
            var user = await _userManager.FindByNameAsync(Username);
            string UserId = user.Id;
            var Cart = await _unit.Cart.FindByUserId(UserId);
            if(Cart == null)
            {
                var Result = await _unit.Cart.AddAsync(new Cart { UserId = UserId });
                if (!Result)
                    return BadRequest();
                Result = await _unit.Complete();
                if (!Result)
                    return BadRequest();
                return Ok();
            }

            //Cart = await _unit.Cart.FindByUserId(UserId);
            var CartProducts = await _unit.CartProduct.GetAllAsync(Cart.Id);
            
            if (CartProducts == null)
                return BadRequest("null");
            var Displays = new List<DisplayProduct>();
            foreach(var pro in CartProducts)
            {
                var Pro =await _unit.Product.FindByIdAsync(pro.ProductId);
                var Display = new DisplayProduct
                {
                    Id = Pro.Id,
                    Name = Pro.Name,
                    Description = Pro.Description,
                    Price = Pro.Price,
                    //Image = Pro.Image,
                    Categoris = Pro.Categories.Select(c => c.Name).ToList()
                };
                Displays.Add(Display);
            }
            return Ok(Displays);
        }

        [Authorize(Roles = "User , Admin")]
        [HttpDelete("RemoveProduct")]
        public async Task<IActionResult> RemoveProduct(int Id)
        {
            string? Username = User.FindFirstValue(ClaimTypes.NameIdentifier); // will give the user's userId
            //string? UserId = "c19b9924-6399-4128-abe8-c5a591d10c3c";
            if (Username == null)
                return BadRequest("Register Or Login Please!");
            var user = await _userManager.FindByNameAsync(Username);
            string UserId = user.Id;
            var Cart = await _unit.Cart.FindByUserId(UserId);
            var result = await _unit.CartProduct.DeleteProductAsync(Id, Cart.Id);
            if (!result)
                return BadRequest($"The Product With Id {Id} Doesn't Exists!");
            result = await _unit.Complete();
            if (!result)
                return BadRequest();
            return Ok();
        }
    
    }
}
