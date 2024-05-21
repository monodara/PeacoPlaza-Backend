using Server.Core.src.Common;
using Server.Core.src.Entity;
using Server.Service.src.DTO;

namespace Server.Service.src.ServiceAbstract.EntityServiceAbstract
{
    public interface ICategoryService
    {
        public Task<IEnumerable<CategoryReadDto>> GetAllCategoriesAsync(QueryOptions options);
        public Task<CategoryReadDto> GetCategoryById(Guid id);
        public Task<CategoryReadDto> CreateCategory(CategoryCreateDto category);
        public Task<CategoryReadDto> UpdateCategory(Guid id, CategoryUpdateDto category);
        public Task<bool> DeleteCategory(Guid id);
        // public Task<IEnumerable<Category>> GetAllSubcategories(Guid categoryId);

    }
}