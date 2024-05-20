using AutoMapper;
using Server.Core.src.Common;
using Server.Core.src.Entity;
using Server.Core.src.RepoAbstract;
using Server.Service.src.DTO;
using Server.Service.src.ServiceAbstract.EntityServiceAbstract;
using Server.Service.src.Shared;

namespace Server.Service.src.ServiceImplement.EntityServiceImplement
{
    public class ProductImageService : IProductImageService
    {
        private readonly IProductImageRepo _productImageRepo;
        protected IMapper _mapper;
        public ProductImageService(IProductImageRepo productImageRepo, IMapper mapper)
        {
            _productImageRepo = productImageRepo;
            _mapper = mapper;
        }
        public async Task<ProductImageReadDTO> CreateProductImage(ProductImageCreateDTO prodImg)
        {
            var result = await _productImageRepo.CreateOneAsync(_mapper.Map<ProductImageCreateDTO, ProductImage>(prodImg));
            return _mapper.Map<ProductImage, ProductImageReadDTO>(result);
        }

        public async Task<bool> DeleteProductImage(Guid id)
        {
            var foundItem = await _productImageRepo.GetOneByIdAsync(id);
            if (foundItem is not null)
            {
                await _productImageRepo.DeleteOneByIdAsync(foundItem);
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<IEnumerable<ProductImageReadDTO>> GetAllProductImagesAsync(Guid productId)
        {
            Console.WriteLine(productId);
            var r = await _productImageRepo.GetImageByProductAsync(productId);
            return _mapper.Map<IEnumerable<ProductImage>, IEnumerable<ProductImageReadDTO>>(r);
        }

        public async Task<ProductImageReadDTO> GetProductImageById(Guid id)
        {
            var result = await _productImageRepo.GetOneByIdAsync(id);
            if (result is not null)
            {
                return _mapper.Map<ProductImage, ProductImageReadDTO>(result);
            }
            else
            {
                throw CustomException.NotFoundException("Id not found");
            }
        }

        public async Task<ProductImageReadDTO> UpdateProductImage(Guid id, ProductImageUpdateDTO prodImg)
        {
            var foundItem = await _productImageRepo.GetOneByIdAsync(id);
            if (foundItem is not null)
            {
                var result = await _productImageRepo.UpdateOneByIdAsync(_mapper.Map(prodImg, foundItem));
                return _mapper.Map<ProductImage, ProductImageReadDTO>(result);
            }
            else
            {
                throw CustomException.NotFoundException("Id not found");
            }
        }
    }
}