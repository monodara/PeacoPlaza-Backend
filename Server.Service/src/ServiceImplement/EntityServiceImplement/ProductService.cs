using AutoMapper;
using Server.Core.src.Common;
using Server.Core.src.Entity;
using Server.Core.src.RepoAbstract;
using Server.Service.src.DTO;
using Server.Service.src.ServiceAbstract.EntityServiceAbstract;
using Server.Service.src.Shared;

namespace Server.Service.src.ServiceImplement.EntityServiceImplement
{
    public class ProductService : IProductService
    {
        private readonly IProductRepo _productRepo;
        private readonly ICategoryRepo _categoryRepo;
        protected IMapper _mapper;
        private IMapper mapper;

        public ProductService(IProductRepo productRepo, IMapper mapper, ICategoryRepo categoryRepo)
        {
            _productRepo = productRepo;
            _mapper = mapper;
            _categoryRepo = categoryRepo;
        }

        public async Task<ProductReadDto> CreateProductAsync(ProductCreateDto product)
        {
            var result = await _productRepo.CreateOneAsync(_mapper.Map<ProductCreateDto, Product>(product));
            return _mapper.Map<Product, ProductReadDto>(result);
        }

        public async Task<bool> DeleteProductAsync(Guid id)
        {
            var foundItem = await _productRepo.GetOneByIdAsync(id);
            if (foundItem is not null)
            {
                await _productRepo.DeleteOneByIdAsync(foundItem);
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<IEnumerable<ProductReadDto>> GetAllProductsAsync(QueryOptions options)
        {
            var result = await _productRepo.GetAllAsync(options);
            return result.Select(p=>new ProductReadDto().Transform(p));
            // return _mapper.Map<IEnumerable<Product>, IEnumerable<ProductReadDto>>(r);
        }

        public async Task<IEnumerable<ProductReadDto>> GetAllProductsByCategoryAsync(Guid categoryId)
        {
            var foundCategory = await _categoryRepo.GetOneByIdAsync(categoryId);
            if (foundCategory is not null)
            {
                var result = _productRepo.GetByCategory(categoryId);
                return _mapper.Map<IEnumerable<Product>, IEnumerable<ProductReadDto>>(result);
            }
            else
            {
                throw CustomException.NotFoundException("Category not found");
            }
        }

        public async Task<IEnumerable<ProductReadDto>> GetMostPurchasedProductsAsync(int top)
        {
            var result = await _productRepo.GetMostPurchasedAsync(top);
            return result.Select(p => new ProductReadDto().Transform(p));
        }

        public async Task<ProductReadDto> GetProductByIdAsync(Guid id)
        {
            var result = await _productRepo.GetOneByIdAsync(id);
            return new ProductReadDto().Transform(result);
        }

        public async Task<ProductReadDto> UpdateProductAsync(Guid id, ProductUpdateDto product)
        {
            var foundItem = await _productRepo.GetOneByIdAsync(id);
            if (foundItem is not null)
            {
                var result = await _productRepo.UpdateOneByIdAsync(_mapper.Map(product, foundItem));
                return _mapper.Map<Product, ProductReadDto>(result);
            }
            else
            {
                throw CustomException.NotFoundException("Id not found");
            }
        }

        public async Task<IEnumerable<ProductReadDto>> GetAllProductsByCategoryAndSubcategoriesAsync(Guid categoryId)
        {
            var products = await GetAllProductsByCategoryAsync(categoryId);

            var allSubcategories = await _categoryRepo.GetSubcategories(categoryId);
            foreach (var subcategory in allSubcategories)
            {
                var subcategoryProducts = await GetAllProductsByCategoryAsync(subcategory.Id);
                products = products.Concat(subcategoryProducts);
            }

            return products;
        }

        public async Task<IEnumerable<ProductReadDto>> GetTopRatedProductsAsync(int top)
        {
            var result = await _productRepo.GetTopRatedProductsAsync(top);
            return result.Select(p => new ProductReadDto().Transform(p));
        }

        public async Task<int> GetProductsCountAsync(QueryOptions options)
        {
            return await _productRepo.GetProductsCountAsync(options);
        }
    }
}