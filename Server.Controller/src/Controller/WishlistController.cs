using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Server.Service.src.DTO;
using Server.Service.src.ServiceAbstract.EntityServiceAbstract;
using Server.Service.src.Shared;

namespace Server.Controller.src.Controller
{

    [ApiController]
    [Route("api/v1/wishlists")]
    public class WishlistController : ControllerBase
    {
        private readonly IWishlistService _wishlistService;
        private readonly IAuthorizationService _authorizationService;

        public WishlistController(IWishlistService wishlistService, IAuthorizationService authorizationService)
        {
            _wishlistService = wishlistService;
            _authorizationService = authorizationService;
        }

        [Authorize]
        [HttpGet]
        public async Task<IEnumerable<WishlistReadDto>> GetWishlistByUsersAsync()
        {
            var userClaims = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userClaims))
            {
                throw CustomException.UnauthorizedException("User is not authenticated.");
            }
            var userId = Guid.Parse(userClaims);
            return await _wishlistService.GetWishlistByUserAsync(userId);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<WishlistReadDto> GetWishlistByIdAsync([FromRoute] Guid id)
        {
            var wishlist = await _wishlistService.GetWishlistByIdAsync(id);
            var authorizationResult = await _authorizationService.AuthorizeAsync(HttpContext.User, wishlist, "ResourceOwner");
            if (authorizationResult.Succeeded)
            {
                return wishlist;
            }
            throw new UnauthorizedAccessException("The wishlist doesn't belong to you.");
        }
        [Authorize]
        [HttpPost]
        public async Task<WishlistReadDto> CreateWishlistAsync([FromBody] WishlistCreateDto wishlist)
        {
            var userClaims = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userClaims))
            {
                throw CustomException.UnauthorizedException("User is not authenticated.");
            }
            var userId = Guid.Parse(userClaims);
            return await _wishlistService.CreateWishlistAsync(userId, wishlist);
        }
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<bool> DeleteWishlistByIdAsync([FromRoute] Guid id)
        {
            var wishlist = await _wishlistService.GetWishlistByIdAsync(id);
            var authorizationResult = await _authorizationService.AuthorizeAsync(HttpContext.User, wishlist, "ResourceOwner");
            if (authorizationResult.Succeeded)
            {
                return await _wishlistService.DeleteWishlistByIdAsync(id);
            }
            throw new UnauthorizedAccessException("The wishlist doesn't belong to you.");
        }
        [Authorize]
        [HttpPatch("{id}")]
        public async Task<WishlistReadDto> UpdateWishlistByIdAsync([FromRoute] Guid id, [FromBody] WishlistUpdateDto wishlistUpdateDto)
        {
            var wishlist = await _wishlistService.GetWishlistByIdAsync(id);
            var authorizationResult = await _authorizationService.AuthorizeAsync(HttpContext.User, wishlist, "ResourceOwner");
            if (authorizationResult.Succeeded)
            {
                return await _wishlistService.UpdateWishlistByIdAsync(id, wishlistUpdateDto);
            }
            throw new UnauthorizedAccessException("The wishlist doesn't belong to you.");
        }
        [Authorize]
        [HttpPost("{id}/add_product")]
        public async Task<bool> AddProductToWishlishAsync([FromBody] Guid productId, [FromRoute] Guid id)
        {
            var wishlist = await _wishlistService.GetWishlistByIdAsync(id);
            var authorizationResult = await _authorizationService.AuthorizeAsync(HttpContext.User, wishlist, "ResourceOwner");
            if (authorizationResult.Succeeded)
            {
                return await _wishlistService.AddProductToWishlishAsync(productId, id);
            }
            throw new UnauthorizedAccessException("The wishlist doesn't belong to you.");
        }
        [Authorize]
        [HttpDelete("{id}/delete_product")]
        public async Task<bool> DeleteProductFromWishlishAsync([FromRoute] Guid id, [FromBody] Guid productId)
        {
            var wishlist = await _wishlistService.GetWishlistByIdAsync(id);
            var authorizationResult = await _authorizationService.AuthorizeAsync(HttpContext.User, wishlist, "ResourceOwner");
            if (authorizationResult.Succeeded)
            {
                return await _wishlistService.DeleteProductFromWishlishAsync(productId, id);
            }
            throw new UnauthorizedAccessException("The wishlist doesn't belong to you.");

        }

    }

}