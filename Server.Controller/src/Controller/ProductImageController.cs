using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Core.src.Common;
using Server.Service.src.DTO;
using Server.Service.src.ServiceAbstract.EntityServiceAbstract;
using Server.Service.src.Shared;

namespace Server.Controller.src.Controller
{
    [ApiController]
    [Route("api/v1/images")]
    public class ProductImageController : ControllerBase
    {
        private readonly IProductImageService _productImageService;

        public ProductImageController(IProductImageService productImageService)
        {
            _productImageService = productImageService;
        }

        [HttpGet("product/{productId}")]
        public async Task<IEnumerable<ProductImageReadDto>> GetAllProductImagesAsync([FromRoute] Guid productId)
        {
            return await _productImageService.GetAllProductImagesAsync(productId);
        }

        [HttpGet("{id}")]
        public async Task<ProductImageReadDto> GetProductImageByIdAsync([FromRoute] Guid id)
        {
            return await _productImageService.GetProductImageById(id);
        }

        [HttpPost]
        public async Task<ProductImageReadDto> CreateProductImageByIdAsync([FromBody] ProductImageCreateDto productImg)
        {
            return await _productImageService.CreateProductImage(productImg);
        }
        [HttpPatch("{id:guid}")]
        public async Task<ProductImageReadDto> UpdateProductImageAsync([FromRoute] Guid id, [FromBody] ProductImageUpdateDto category)
        {
            return await _productImageService.UpdateProductImage(id, category);
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:guid}")]
        public async Task<bool> DeleteImageAsync([FromRoute] Guid id)
        {
            if (HttpContext.User!.Identity == null || !HttpContext.User.Identity.IsAuthenticated)
            {
                throw CustomException.UnauthorizedException("");
            }

            var userClaims = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userClaims == null)
            {
                throw CustomException.UnauthorizedException("");
            }
            return await _productImageService.DeleteProductImage(id);
        }
    }
}