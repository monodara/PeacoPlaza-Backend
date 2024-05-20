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

        public async Task<ProductReadDTO> CreateProduct(ProductCreateDTO product)
        {
            var result = await _productRepo.CreateOneAsync(_mapper.Map<ProductCreateDTO, Product>(product));
            return _mapper.Map<Product, ProductReadDTO>(result);
        }

        public async Task<bool> DeleteProduct(Guid id)
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

        public async Task<IEnumerable<ProductReadDTO>> GetAllProductsAsync(QueryOptions options)
        {
            var result = await _productRepo.GetAllAsync(options);
            return result.Select(p=>new ProductReadDTO().Transform(p));
            // return _mapper.Map<IEnumerable<Product>, IEnumerable<ProductReadDTO>>(r);
        }

        public async Task<IEnumerable<ProductReadDTO>> GetAllProductsByCategoryAsync(Guid categoryId)
        {
            var foundCategory = await _categoryRepo.GetOneByIdAsync(categoryId);
            if (foundCategory is not null)
            {
                var result = _productRepo.GetByCategory(categoryId);
                return _mapper.Map<IEnumerable<Product>, IEnumerable<ProductReadDTO>>(result);
            }
            else
            {
                throw CustomException.NotFoundException("Category not found");
            }
        }

        public async Task<IEnumerable<ProductReadDTO>> GetMostPurchasedProductsAsync(int top)
        {
            var result = await _productRepo.GetMostPurchasedAsync(top);
            return _mapper.Map<IEnumerable<Product>, IEnumerable<ProductReadDTO>>(result);
        }

        public async Task<ProductReadDTO> GetProductById(Guid id)
        {
            var result = await _productRepo.GetOneByIdAsync(id);
            if (result is not null)
            {
                return _mapper.Map<Product, ProductReadDTO>(result);
            }
            else
            {
                throw CustomException.NotFoundException("Id not found");
            }
        }

        public async Task<ProductReadDTO> UpdateProduct(Guid id, ProductUpdateDTO product)
        {
            var foundItem = await _productRepo.GetOneByIdAsync(id);
            if (foundItem is not null)
            {
                var result = await _productRepo.UpdateOneByIdAsync(_mapper.Map(product, foundItem));
                return _mapper.Map<Product, ProductReadDTO>(result);
            }
            else
            {
                throw CustomException.NotFoundException("Id not found");
            }
        }

        public async Task<IEnumerable<ProductReadDTO>> GetAllProductsByCategoryAndSubcategoriesAsync(Guid categoryId)
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

        public async Task<IEnumerable<ProductReadDTO>> GetTopRatedProductsAsync(int top)
        {
            var result = await _productRepo.GetTopRatedProductsAsync(top);
            return _mapper.Map<IEnumerable<Product>, IEnumerable<ProductReadDTO>>(result);
        }
    }
}