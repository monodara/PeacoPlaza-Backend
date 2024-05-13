using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Server.Service.src.DTO;
using Server.Service.src.ServiceAbstract.EntityServiceAbstract;

namespace Server.Controller.src.Controller
{

    [ApiController]
    [Route("api/v1/wishlists")]
    public class WishlistController : ControllerBase
    {
        private readonly IWishlistService _wishlistService;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public WishlistController(IWishlistService wishlistService, IHttpContextAccessor httpContextAccessor)
        {
            _wishlistService = wishlistService;
            _httpContextAccessor = httpContextAccessor;
        }

        [Authorize]
        [HttpGet]
        public async Task<IEnumerable<WishlistReadDto>> GetWishlistByUsersAsync()
        {
            var userClaims = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userClaims == null) throw new InvalidOperationException("Please login to use this facility!");
            var userId = Guid.Parse(userClaims);
            return await _wishlistService.GetWishlistByUserAsync(userId);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<WishlistReadDto> GetWishlistByIdAsync([FromRoute] Guid id)
        {
            var userClaims = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userClaims == null) throw new InvalidOperationException("Please login to use this facility!");
            return await _wishlistService.GetWishlistByIdAsync(id);
        }
        [Authorize]
        [HttpPost]
        public async Task<WishlistReadDto> CreateWishlistAsync([FromBody] WishlistCreateDto wishlist)
        {
            var userClaims = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userClaims == null) throw new InvalidOperationException("Please login to use this facility!");
            var userId = Guid.Parse(userClaims);
            return await _wishlistService.CreateWishlistAsync(userId, wishlist);
        }
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<bool> DeleteWishlistByIdAsync([FromRoute] Guid id)
        {
            var userClaims = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userClaims == null) throw new InvalidOperationException("Please login to use this facility!");
            return await _wishlistService.DeleteWishlistByIdAsync(id);
        }
        [Authorize]
        [HttpPatch("{id}")]
        public async Task<WishlistReadDto> UpdateWishlistByIdAsync([FromBody] WishlistUpdateDto wishlist)
        {
            var userClaims = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userClaims == null) throw new InvalidOperationException("Please login to use this facility!");
            return await _wishlistService.UpdateWishlistByIdAsync(wishlist);
        }
        [Authorize]
        [HttpPost("{id}/add_product")]
        public async Task<bool> AddProductToWishlishAsync([FromBody] Guid productId, [FromRoute] Guid wishlistId)
        {
            var userClaims = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userClaims == null) throw new InvalidOperationException("Please login to use this facility!");
            return await _wishlistService.AddProductToWishlishAsync(productId, wishlistId);
        }
        [Authorize]
        [HttpDelete("{id}/delete_product")]
        public async Task<bool> DeleteProductFromWishlishAsync([FromRoute] Guid wishlistId, [FromBody] Guid productId)
        {
            var userClaims = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userClaims == null) throw new InvalidOperationException("Please login to use this facility!");
            return await _wishlistService.DeleteProductFromWishlishAsync(productId, wishlistId);
        }

    }

}