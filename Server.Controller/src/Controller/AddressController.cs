using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Server.Core.src.Common;
using Server.Service.src.DTO;
using Server.Service.src.ServiceAbstract.EntityServiceAbstract;

namespace Server.Controller.src.Controller
{

    [ApiController]
    [Route("api/v1/addresses")]
    public class AddressController : ControllerBase
    {
        private readonly IAddressService _addressService;
        private readonly IAuthorizationService _authorizationService;


        public AddressController(IAddressService addressService,  IAuthorizationService authorizationService)
        {
            _addressService = addressService;
            _authorizationService = authorizationService;
        }
        [Authorize]
        [HttpGet] 
        public async Task<IEnumerable<AddressReadDto>> GetAddressesByUserAsync([FromQuery] QueryOptions options)
        {
            var userClaims = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userClaims == null) throw new InvalidOperationException("Please login to use this facility!");
            var userId = Guid.Parse(userClaims);
            return await _addressService.GetAddressesByUserAsync(userId, options);
        }
        [Authorize]
        [HttpGet("{id}")]
        public async Task<AddressReadDto> GetAddressByIdAsync([FromRoute] Guid id)
        {
            var address = await _addressService.GetAddressByIdAsync(id);
            Console.WriteLine(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            Console.WriteLine(address.UserId);
            var authorizationResult = await _authorizationService.AuthorizeAsync(HttpContext.User, address, "ResourceOwner");
            Console.WriteLine(authorizationResult.Succeeded);
            if (authorizationResult.Succeeded){
                return address;
            }
            throw new UnauthorizedAccessException("The address doesn't belong to you.");
        }
        [Authorize]
        [HttpPost]
        public async Task<AddressReadDto> CreateAddressAsync([FromBody] AddressCreateDto address)
        {
            var userClaims = HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userClaims == null) throw new InvalidOperationException("Please login to use this facility!");
            var userId = Guid.Parse(userClaims);
            return await _addressService.CreateAddressAsync(userId, address);
        }
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<bool> DeleteAddressByIdAsync([FromRoute] Guid id)
        {
            // var userClaims = HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            // if (userClaims == null) throw new InvalidOperationException("Please login to use this facility!");
            // return await _addressService.DeleteAddressByIdAsync(id);
            var address = await _addressService.GetAddressByIdAsync(id);
            var authorizationResult = await _authorizationService.AuthorizeAsync(HttpContext.User, address, "ResourceOwner");
            if (authorizationResult.Succeeded)
            {
                return await _addressService.DeleteAddressByIdAsync(id);
            }
            throw new UnauthorizedAccessException("The address doesn't belong to you.");
        }
        [Authorize]
        [HttpPatch("{id}")]
        public async Task<AddressReadDto> UpdateAddressByIdAsync([FromRoute] Guid id, [FromBody] AddressUpdateDto addressUpdateDto)
        {
            // var userClaims = HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            // if (userClaims == null) throw new InvalidOperationException("Please login to use this facility!");
            // return await _addressService.UpdateAddressByIdAsync(address);
            var address = await _addressService.GetAddressByIdAsync(id);
            var authorizationResult = await _authorizationService.AuthorizeAsync(HttpContext.User, address, "ResourceOwner");
            if (authorizationResult.Succeeded)
            {
                return await _addressService.UpdateAddressByIdAsync(addressUpdateDto);
            }
            throw new UnauthorizedAccessException("The address doesn't belong to you.");
        }
        [Authorize]
        [HttpPatch("{id}/set_default")]
        public async Task<bool> SetDefaultAddressAsync([FromRoute] Guid addressId)
        {
            var userClaims = HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userClaims == null) throw new InvalidOperationException("Please login to use this facility!");//unauthenticated
            var userId = Guid.Parse(userClaims);
            return await _addressService.SetDefaultAddressAsync(userId, addressId);
        }
        [Authorize]
        [HttpGet("default")]
        public async Task<AddressReadDto> GetDefaultAddressAsync()
        {
            var userClaims = HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userClaims == null) throw new InvalidOperationException("Please login to use this facility!");//unauthenticated
            var userId = Guid.Parse(userClaims);
            return await _addressService.GetDefaultAddressAsync(userId);
        }

    }

}