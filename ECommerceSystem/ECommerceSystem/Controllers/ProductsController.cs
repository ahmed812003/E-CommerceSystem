using ECommerceSystem.DataService.Repositories.Interfaces;
using ECommerceSystem.Entities.DtoModels;
using ECommerceSystem.Entities.DtoModels.Create;
using ECommerceSystem.Entities.DtoModels.Display;
using ECommerceSystem.Entities.DtoModels.Update;
using ECommerceSystem.Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Security.Claims;

namespace ECommerceSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IUnitOfWork _unit;
        private readonly UserManager<AppUser> _userManager;

        public ProductsController(IUnitOfWork _unit , UserManager<AppUser> _userManager)
        {
            this._unit = _unit;
            this._userManager = _userManager;
        }

        [Authorize(Roles = "User , Admin")]
        // admin , user
        [HttpGet("GetAllProducts")]
        public async Task<IActionResult> GetAllProducts()
        {
            var Products = await _unit.Product.GetAllAsync();
            var Displays = new List<DisplayProduct>();
            foreach(var P in Products)
            {
                var Display = new DisplayProduct
                {
                    Id = P.Id,
                    Name = P.Name,
                    Description = P.Description,
                    Price = P.Price,
                    Image = P.Image,
                    Categoris = P.Categories.Select(c => c.Name).ToList()
                };
                Displays.Add(Display);
            }
            
            return Ok(Displays);
        }

        [Authorize(Roles = "User , Admin")]
        //admin , user
        [HttpGet("GetProduct/{Id}")]
        public async Task<IActionResult> GetProduct(int Id)
        {
            var Product = await _unit.Product.FindByIdAsync(Id);
            if (Product == null)
                return BadRequest($"The Product With Id {Id} Doesn't Exists!");
            
            var Display = new DisplayProduct
            {
                Id = Product.Id,
                Name = Product.Name,
                Description = Product.Description,
                Price = Product.Price,
                Image = Product.Image,
                Categoris = Product.Categories.Select(c => c.Name).ToList()
            };

            return Ok(Display);
        }

        [Authorize(Roles = "Admin")]
        //admin
        [HttpPost("AddProduct")]
        public async Task<IActionResult> AddProduct([FromForm]CreateProduct model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var Product = await _unit.Product.FindByNameAsync(model.Name);
            if (Product != null)
                return BadRequest($"The Product With Name {model.Name} Already Exist!");

            var Categories = new List<Category>();
            foreach(var cate in model.Categories)
            {
                var category = await _unit.Category.FindByNameAsync(cate);
                if (category == null)
                    continue;
                Categories.Add(category);
            }

            using var stream = new MemoryStream();
            await model.Image.CopyToAsync(stream);

            Product = new Product
            {
                Name = model.Name,
                Description = model.Description,
                Price = model.Price,
                Image = stream.ToArray() 
            };

            var Result = await _unit.Product.AddAsync(Product);
            if (!Result)
                return BadRequest();

            Result = await _unit.Complete();
            if (!Result)
                return BadRequest();

            Result = await _unit.ProductCategory.AddProductCategories(Product.Id, Categories.Select(c => c.Id).ToList());
            if (!Result)
                return BadRequest();

            Result = await _unit.Complete();
            if (!Result)
                return BadRequest();

            var Display = new DisplayProduct
            {
                Id = Product.Id,
                Name = Product.Name,
                Description = Product.Description,
                Price = Product.Price,
                Image = Product.Image,
                Categoris = Categories.Select(c => c.Name).ToList()
            };

            return Ok(Display);
        }

        [Authorize(Roles = "User , Admin")]
        //user
        [HttpPost("AddProductToCart/{Id}")]
        public async Task<IActionResult> AddProductToCart(int Id)
        {
            string? Username = User.FindFirstValue(ClaimTypes.NameIdentifier); // will give the user's userId
            //string? UserId = "c19b9924-6399-4128-abe8-c5a591d10c3c";
            if (Username == null)
                return BadRequest("Register Or Login Please!");
            var user = await _userManager.FindByNameAsync(Username);
            string UserId = user.Id;
            var Cart = await _unit.Cart.FindByUserId(UserId);
            if (Cart == null)
            {
                var Result = await _unit.Cart.AddAsync(new Cart { UserId = UserId });
                if (!Result)
                    return BadRequest();
                Result = await _unit.Complete();
                if (!Result)
                    return BadRequest();
            }
            var product = await _unit.Product.FindByIdAsync(Id);
            if (product == null)
                return BadRequest($"The Product With Id {Id} Doesn't Exists!");

            var result = await _unit.CartProduct.AddAsync(new Cart_Product
            {
                CartId = Cart.Id,
                ProductId = Id
            });

            if (!result)
                return BadRequest();
            result = await _unit.Complete();
            if (!result)
                return BadRequest();
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        //admmin
        [HttpPut("updateProduct")]
        public async Task<IActionResult> UpdateProduct([FromForm] UpdateProduct model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var Product = await _unit.Product.FindByIdAsync(model.Id);
            if (Product == null)
                return BadRequest($"TheProduct With Id {model.Id} Doesn't Exists!");

            if (model.Name != null && model.Name != string.Empty && model.Name != Product.Name)
                Product.Name = model.Name;
            if (model.Description != null && model.Description != string.Empty && model.Description != Product.Description)
                Product.Description = model.Description;
            if (model.Price != 0 && model.Price != Product.Price)
                Product.Price = model.Price;
            if(model.Image != null)
            {
                using var Stream = new MemoryStream();
                await model.Image.CopyToAsync(Stream);
                Product.Image = Stream.ToArray();
            }
            
            if (model.Categories != null && model.Categories.Count != 0)
            {
                var Categories = new List<Category>();
                foreach (var cate in model.Categories)
                {
                    var category = await _unit.Category.FindByNameAsync(cate);
                    if (category == null)
                        continue;
                    Categories.Add(category);
                }
                var Result = await _unit.ProductCategory.RemoveProductCategories(model.Id);
                if (!Result)
                    return BadRequest();
                Result = await _unit.ProductCategory.AddProductCategories(model.Id, Categories.Select(c => c.Id).ToList());
                if (!Result)
                    return BadRequest();
            }
            var result = _unit.Product.Update(Product);
            if (!result)
                return BadRequest();
            result =await _unit.Complete();
            if (!result)
                return BadRequest();

            var Display = new DisplayProduct
            {
                Id = Product.Id,
                Name = Product.Name,
                Description = Product.Description,
                Price = Product.Price,
                Image = Product.Image,
                Categoris = Product.Categories.Select(c => c.Name).ToList()
            };

            return Ok(Display);
        }

        [Authorize(Roles = "Admin")]
        //admin
        [HttpDelete("DeleteProduct/{Id}")]
        public async Task<IActionResult> DeleteProduct(int Id)
        {
            var Result = await _unit.Product.DeleteAsync(Id);
            if (!Result)
                return BadRequest();
            Result = await _unit.Complete();
            if (!Result)
                return BadRequest();
            return Ok();
        }


    }
}
