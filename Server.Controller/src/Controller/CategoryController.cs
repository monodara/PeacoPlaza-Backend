using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Core.src.Common;
using Server.Service.src.DTO;
using Server.Service.src.ServiceAbstract.EntityServiceAbstract;

namespace Server.Controller.src.Controller
{
    [ApiController]
    [Route("api/v1/categories")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryServices;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryServices = categoryService;
        }

        [HttpGet]
        public async Task<IEnumerable<CategoryReadDto>> GetAllCategoriesAsync([FromQuery] QueryOptions options)
        {
            return await _categoryServices.GetAllCategoriesAsync(options);
        }

        [HttpGet("{id}")]
        public async Task<CategoryReadDto> GetCategoryByIdAsync([FromRoute] Guid id)
        {
            return await _categoryServices.GetCategoryById(id);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<CategoryReadDto> CreateCategoryAsync([FromBody] CategoryCreateDto category)
        {
            return await _categoryServices.CreateCategory(category);
        }
        [Authorize(Roles = "Admin")]
        [HttpPatch("{id}")]
        public async Task<CategoryReadDto> UpdateCategoryAsync([FromRoute] Guid id, [FromBody] CategoryUpdateDto category)
        {
            return await _categoryServices.UpdateCategory(id, category);
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<bool> DeleteCategoryAsync([FromRoute] Guid id)
        {
            return await _categoryServices.DeleteCategory(id);
        }
    }
}