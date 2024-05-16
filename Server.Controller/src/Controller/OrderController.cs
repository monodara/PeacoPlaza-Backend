using Server.Service.src.DTO;
using Microsoft.AspNetCore.Mvc;
using Server.Core.src.Common;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Server.Service.src.ServiceAbstract.EntityServiceAbstract;

namespace Server.Controller.src.Controller;

[ApiController]
[Route("api/v1/orders")]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;
    // private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IAuthorizationService _authorizationService;

    public OrderController(IOrderService orderService, IAuthorizationService authorizationService)
    {
        _orderService = orderService;
        _authorizationService = authorizationService;
        // _httpContextAccessor = httpContextAccessor;
    }

    [Authorize]
    [HttpGet]
    public async Task<IEnumerable<OrderReadDto>> GetAllOrdersAsync([FromQuery] QueryOptions options)
    {
        var userClaims = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userClaims == null) throw new InvalidOperationException("Please login to use this facility!");
        var userId = Guid.Parse(userClaims);
        return await _orderService.GetAllOrdersAsync(options, userId);
    }

    // [Authorize(Roles = "Admin")]
    // [HttpGet("admin/{id}")]
    // public async Task<IEnumerable<OrderReadDto>> GetAllOrdersByUserAsync([FromQuery] QueryOptions options, [FromRoute] Guid id)
    // {
    //     return await _orderService.GetAllOrdersByUserAsync(options, id);
    // }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<OrderReadDto> GetOrderByIdAsync([FromRoute] Guid id)
    {
        var order = await _orderService.GetOrderByIdAsync(id);
        var isAdmin = HttpContext.User.IsInRole("Admin");
        var authorizationResult = await _authorizationService.AuthorizeAsync(HttpContext.User, order, "ResourceOwner");
        if (isAdmin || authorizationResult.Succeeded)
        {
            return order;
        }
        throw new UnauthorizedAccessException("The order doesn't belong to you.");
    }

    [Authorize]
    [HttpPost]
    public async Task<OrderReadDto> CreateOrderAsync([FromBody] OrderCreateDto order)
    {
        // Console.WriteLine(order.AddressId);
        var userClaims = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userClaims == null) throw new InvalidOperationException("Please login to use this facility!");
        var userId = Guid.Parse(userClaims);
        return await _orderService.CreateOrderAsync(userId, order);
    }

    [Authorize]
    [HttpPatch("{id}")]
    public async Task<bool> UpdateOrderByIdAsync([FromRoute] Guid id, [FromBody] UpdateOrderDTO updateOrderDTO)
    {
        var order = await _orderService.GetOrderByIdAsync(id);
        var isAdmin = HttpContext.User.IsInRole("Admin");
        var authorizationResult = await _authorizationService.AuthorizeAsync(HttpContext.User, order, "ResourceOwner");
        if (isAdmin || authorizationResult.Succeeded)
        {
            return await _orderService.UpdateOrderByIdAsync(id, updateOrderDTO);
        }
        throw new UnauthorizedAccessException("You don't have the authorization to update the order.");
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<bool> DeleteOrderByIdAsync([FromRoute] Guid id)
    {
        var order = await _orderService.GetOrderByIdAsync(id);
        var authorizationResult = await _authorizationService.AuthorizeAsync(HttpContext.User, order, "ResourceOwner");
        if (authorizationResult.Succeeded)
        {
            return await _orderService.DeleteOrderByIdAsync(id);
        }
        throw new UnauthorizedAccessException("The order doesn't belong to you.");
    }

    
}
