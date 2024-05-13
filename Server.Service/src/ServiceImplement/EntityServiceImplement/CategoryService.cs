using AutoMapper;
using Server.Core.src.Common;
using Server.Core.src.Entity;
using Server.Core.src.RepoAbstract;
using Server.Service.src.DTO;
using Server.Service.src.ServiceAbstract.EntityServiceAbstract;
using Server.Service.src.Shared;

namespace Server.Service.src.ServiceImplement.AuthServiceImplement
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepo _categoryRepo;
        protected IMapper _mapper;
        public CategoryService(ICategoryRepo categoryRepo, IMapper mapper)
        {
            _categoryRepo = categoryRepo;
            _mapper = mapper;
        }
        public async Task<IEnumerable<CategoryReadDTO>> GetAllCategoriesAsync(QueryOptions options)
        {
            var r = await _categoryRepo.GetAllAsync(options);
            return _mapper.Map<IEnumerable<Category>, IEnumerable<CategoryReadDTO>>(r);
        }
        public async Task<CategoryReadDTO> GetCategoryById(Guid id)
        {
            var result = await _categoryRepo.GetOneByIdAsync(id);
            if (result is not null)
            {
                return _mapper.Map<Category, CategoryReadDTO>(result);
            }
            else
            {
                throw CustomException.NotFoundException("Id not found");
            }
        }
        public async Task<CategoryReadDTO> CreateCategory(CategoryCreateDTO category)
        {
            var result = await _categoryRepo.CreateOneAsync(_mapper.Map<CategoryCreateDTO, Category>(category));
            return _mapper.Map<Category, CategoryReadDTO>(result);
        }
        async Task<CategoryReadDTO> ICategoryService.UpdateCategory(Guid id, CategoryUpdateDTO category)
        {
            var foundItem = await _categoryRepo.GetOneByIdAsync(id);
            if (foundItem is not null)
            {
                var result = await _categoryRepo.UpdateOneByIdAsync(_mapper.Map(category, foundItem));
                return _mapper.Map<Category, CategoryReadDTO>(result);
            }
            else
            {
                throw CustomException.NotFoundException("Id not found");
            }
        }

        async Task<bool> ICategoryService.DeleteCategory(Guid id)
        {
            var foundItem = await _categoryRepo.GetOneByIdAsync(id);
            if (foundItem is not null)
            {
                await _categoryRepo.DeleteOneByIdAsync(foundItem);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}