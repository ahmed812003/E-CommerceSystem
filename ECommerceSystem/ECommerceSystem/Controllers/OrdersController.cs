using ECommerceSystem.DataService.Repositories.Interfaces;
using ECommerceSystem.Entities.DtoModels.Create;
using ECommerceSystem.Entities.DtoModels.Display;
using ECommerceSystem.Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using System.Security.Claims;

namespace ECommerceSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IUnitOfWork _unit;
        private readonly UserManager<AppUser> _userManager;

        public OrdersController(IUnitOfWork _unit , UserManager<AppUser> _userManager)
        {
            this._unit = _unit;
            this._userManager = _userManager;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("GetOrders")]
        public async Task<IActionResult> GetOrders()
        {
            var Orders = await _unit.Order.GetAllAsync();
            var Display = Orders.Select(o => new DisplayOrder
            {
                Id = o.Id,
                Phone = o.Phone,
                TotalPrice = o.TotalPrice,
                Done = o.Done, 
                CreatedOn = o.CreatedOn,
                DeliveredOn = o.DeliveredOn ,
                UserId = o.UserId 
            });
            return Ok(Display);
        }

        [Authorize(Roles = "User , Admin")]
        [HttpGet("GetOrder/{Id}")]
        public async Task<IActionResult> GetOrder(int Id)
        {
            var Order = await _unit.Order.FindByIdAsync(Id);
            if (Order == null)
                return BadRequest($"The Order With Id {Id} Doesn't Exist!");
            var Display = new DisplayOrder
            {
                Id = Order.Id,
                Phone = Order.Phone,
                TotalPrice = Order.TotalPrice,
                Done = Order.Done,
                CreatedOn = Order.CreatedOn,
                DeliveredOn = Order.DeliveredOn,
                UserId = Order.UserId
            };
            return Ok(Display);
        }

        [Authorize(Roles = "User , Admin")]
        [HttpPost("AddOrder")]
        public async Task<IActionResult> AddOrder([FromBody] List<CreateOrderProduct> products)
        {
            //string? UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            string? UserId = "c19b9924-6399-4128-abe8-c5a591d10c3c";
            if (UserId == null)
                return BadRequest("Register Or Login Please!");
            var user = await _userManager.FindByIdAsync(UserId);

            decimal totalPrice = 0;
            foreach (var Pro in products)
            {
                totalPrice += (Pro.Amount * Pro.Price);
            }
            var Order = new Order
            {
                TotalPrice = totalPrice,
                Done = false,
                CreatedOn = DateTime.Now,
                UserId = UserId,
                Phone = user.PhoneNumber
            };
            var Result = await _unit.Order.AddAsync(Order);
            if (!Result)
                return BadRequest();
            Result = await _unit.Complete();
            if (!Result)
                return BadRequest();

            var OrderProducts = new List<Order_Product>();
            foreach (var Pro in products)
            {
                var OrderProdect = new Order_Product
                {
                    OrderId = Order.Id,
                    ProductId = Pro.Id,
                    Price = Pro.Price,
                    Amount = Pro.Amount,
                    TotalPrice = Pro.Price * Pro.Amount
                };
                Result = await _unit.OrderProduct.AddAsync(OrderProdect);
                if (!Result)
                    return BadRequest();
            }
            Result = await _unit.Complete();
            if (!Result)
                return BadRequest();

            var Cart = await _unit.Cart.FindByUserId(UserId);
            var CartProducts = await _unit.CartProduct.GetAllAsync(Cart.Id);
            if (CartProducts == null)
                return BadRequest("null");
            foreach (var pro in CartProducts)
            {
                Result = await _unit.CartProduct.DeleteProductAsync(pro.ProductId, pro.CartId);
                if (!Result)
                    return BadRequest();
            }
            Result = await _unit.Complete();
            if (!Result)
                return BadRequest();

            return Ok(products);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("Delete/{Id}")]
        public async Task<IActionResult> DeleteCategory(int Id)
        {
            var Result = await _unit.Order.DeleteAsync(Id);
            if (!Result)
                return BadRequest();
            Result = await _unit.Complete();
            if (!Result)
                return BadRequest();
            return Ok();
        }

    }
}
