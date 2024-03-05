using ECommerceSystem.DataService.Repositories.Interfaces;
using ECommerceSystem.Entities.DtoModels;
using ECommerceSystem.Entities.DtoModels.Create;
using ECommerceSystem.Entities.DtoModels.Display;
using ECommerceSystem.Entities.DtoModels.Update;
using ECommerceSystem.Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly IUnitOfWork _unit;

        public CategoriesController(IUnitOfWork _unit)
        {
            this._unit = _unit;
        }

        [Authorize(Roles = "User,Admin")]
        // admin , user
        [HttpGet("GetAllCategories")]
        public async Task<IActionResult> GetAllCategories()
        {
            var Categories = await _unit.Category.GetAllAsync();
            var Display = Categories.Select(cat => new DisplayCategory
            {
                Id = cat.Id,
                Name = cat.Name,
                Description = cat.Description
            });
            return Ok(Display);
        }

        [Authorize(Roles = "Admin")]
        //admin , user
        [HttpGet("GetCategory/{Id}")]
        public async Task<IActionResult> GetCategoryById(int Id)
        {
            var Category = await _unit.Category.FindByIdAsync(Id);
            if (Category == null)
                return BadRequest($"The Category With Id {Id} Doesn't Exist!");
            var Display = new DisplayCategory
            {
                Id = Category.Id,
                Name = Category.Name,
                Description = Category.Description
            };
            return Ok(Display);
        }

        [Authorize(Roles = "Admin")]
        //admin
        [HttpPost("AddCategory")]
        public async Task<IActionResult> AddCategory(CreateCategory model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if(await _unit.Category.FindByNameAsync(model.Name) != null)
                return BadRequest($"The Category With Name {model.Name} Already Exist!");

            var Category = new Category
            {
                Name = model.Name,
                Description = model.Description
            };

            var Result = await _unit.Category.AddAsync(Category);
            if(!Result)
                return BadRequest();
            Result = await _unit.Complete();
            if (!Result)
                return BadRequest();

            var Display = new DisplayCategory
            {
                Id = Category.Id,
                Name = Category.Name,
                Description = Category.Description
            };

            return Ok(Display);
        }

        [Authorize(Roles = "Admin")]
        //admin
        [HttpPut("updateCategory")]
        public async Task<IActionResult> updateCategory(UpdateCategory model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var Category = await _unit.Category.FindByIdAsync(model.Id);
            if (Category == null)
                return BadRequest($"The Category With Id {model.Id} Doesn't Exist!");
            if (model.Name != null && model.Name != string.Empty && Category.Name != model.Name)
                Category.Name = model.Name;
            if (model.Description != null && model.Description != string.Empty && Category.Description != model.Description)
                Category.Description = model.Description;

            var Result = _unit.Category.Update(Category);
            if (!Result)
                return BadRequest();
            Result = await _unit.Complete();
            if (!Result)
                return BadRequest();

            var Display = new DisplayCategory
            {
                Id = Category.Id,
                Name = Category.Name,
                Description = Category.Description
            };
            return Ok(Display);
        }

        [Authorize(Roles = "Admin")]
        //admin
        [HttpDelete("DeleteCategory/{Id}")]
        public async Task<IActionResult> DeleteCategory(int Id) 
        {
            var Result = await _unit.Category.DeleteAsync(Id);
            if (!Result)
                return BadRequest();
            Result = await _unit.Complete();
            if (!Result)
                return BadRequest();
            return Ok();
        }  
    }
}
