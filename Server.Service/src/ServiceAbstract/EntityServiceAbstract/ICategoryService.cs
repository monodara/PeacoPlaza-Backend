using Server.Core.src.Common;
using Server.Service.src.DTO;

namespace Server.Service.src.ServiceAbstract.EntityServiceAbstract
{
    public interface ICategoryService
    {
        public Task<IEnumerable<CategoryReadDTO>> GetAllCategoriesAsync(QueryOptions options);
        public Task<CategoryReadDTO> GetCategoryById(Guid id);
        public Task<CategoryReadDTO> CreateCategory(CategoryCreateDTO category);
        public Task<CategoryReadDTO> UpdateCategory(Guid id, CategoryUpdateDTO category);
        public Task<bool> DeleteCategory(Guid id);
    }
}