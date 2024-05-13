using Microsoft.AspNetCore.Mvc;
using Server.Core.src.Common;
using Server.Service.src.DTO;
using Server.Service.src.ServiceAbstract.EntityServiceAbstract;

namespace Server.Controller.src.Controller
{
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productServices;

        public ProductController(IProductService productService)
        {
            _productServices = productService;
        }

        [HttpGet("api/v1/products")]
        public async Task<IEnumerable<ProductReadDTO>> GetAllProductsAsync([FromQuery] QueryOptions options)
        {
            Console.WriteLine("GetAllProductsAsync");
            try
            {
                return await _productServices.GetAllProductsAsync(options);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        [HttpGet("api/v1/product/{id}")]
        public async Task<ProductReadDTO> GetProductByIdAsync([FromRoute] Guid id)
        {
            return await _productServices.GetProductById(id);
        }
        [HttpGet("api/v1/products/category/{categoryId}")]
        public async Task<IEnumerable<ProductReadDTO>> GetAllProductsByCategoryAsync([FromRoute] Guid categoryId)
        {
            Console.WriteLine("GetAllProductsByCategoryAsync");
            try
            {
                return await _productServices.GetAllProductsByCategoryAsync(categoryId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        [HttpGet("top/{topNumber:int}")]
        public async Task<IEnumerable<ProductReadDTO>> GetMostPurchased([FromRoute] int top)
        {
            return await _productServices.GetMostPurchasedProductsAsync(top);
        }
        [HttpPost("api/v1/product/{id}")]
        public async Task<ProductReadDTO> CreateProductAsync([FromBody] ProductCreateDTO product)
        {
            return await _productServices.CreateProduct(product);
        }
        [HttpPatch("api/v1/product/{id}")]
        public async Task<ProductReadDTO> UpdateProductAsync([FromRoute] Guid id, [FromBody] ProductUpdateDTO category)
        {
            return await _productServices.UpdateProduct(id, category);
        }
        [HttpDelete("api/v1/product/{id}")]
        public async Task<bool> DeleteProductAsync([FromRoute] Guid id)
        {
            return await _productServices.DeleteProduct(id);
        }
    }
}