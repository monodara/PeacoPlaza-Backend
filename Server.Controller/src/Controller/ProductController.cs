using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Server.Core.src.Common;
using Server.Core.src.ValueObject;
using Server.Service.src.DTO;
using Server.Service.src.ServiceAbstract.EntityServiceAbstract;

namespace Server.Controller.src.Controller
{
    [ApiController]
    [Route("api/v1/products")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productServices;

        public ProductController(IProductService productService)
        {
            _productServices = productService;
        }

        [HttpGet]
        public async Task<IEnumerable<ProductReadDto>> GetAllProductsAsync([FromQuery] QueryOptions options)
        {
            return await _productServices.GetAllProductsAsync(options);
        }
        [HttpGet("{id}")]
        public async Task<ProductReadDto> GetProductByIdAsync([FromRoute] Guid id)
        {
            return await _productServices.GetProductById(id);
        }
        [HttpGet("category/{categoryId}")]
        public async Task<IEnumerable<ProductReadDto>> GetAllProductsByCategoryAsync([FromRoute] Guid categoryId)
        {
            return await _productServices.GetAllProductsByCategoryAndSubcategoriesAsync(categoryId);
        }
        [HttpGet("most_purchased/{topNumber:int}")]
        public async Task<IEnumerable<ProductReadDto>> GetMostPurchasedAsync([FromRoute] int topNumber)
        {
            return await _productServices.GetMostPurchasedProductsAsync(topNumber);
        }
        [HttpGet("top_rated/{topNumber:int}")]
        public async Task<IEnumerable<ProductReadDto>> GetTopRatedAsync([FromRoute] int topNumber)
        {
            return await _productServices.GetTopRatedProductsAsync(topNumber);
        }   
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ProductReadDto> CreateProductAsync([FromBody] ProductCreateDto product)
        {
            return await _productServices.CreateProduct(product);
        }
        [Authorize(Roles = "Admin")]
        [HttpPatch("{id}")]
        public async Task<ProductReadDto> UpdateProductAsync([FromRoute] Guid id, [FromBody] ProductUpdateDto category)
        {
            return await _productServices.UpdateProduct(id, category);
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<bool> DeleteProductAsync([FromRoute] Guid id)
        {
            return await _productServices.DeleteProduct(id);
        }
    }

    
}