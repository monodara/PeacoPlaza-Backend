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
    private readonly IHttpContextAccessor _httpContextAccessor;

    public OrderController(IOrderService orderService, IHttpContextAccessor httpContextAccessor)
    {
        _orderService = orderService;
        _httpContextAccessor = httpContextAccessor;
    }

    [Authorize] //means authenticate
    [HttpGet]
    public async Task<IEnumerable<ReadOrderDTO>> GetAllOrdersAsync([FromQuery] QueryOptions options)
    {
        var userClaims = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userClaims == null) throw new InvalidOperationException("Please login to use this facility!");

        var userId = Guid.Parse(userClaims);

        return await _orderService.GetAllOrdersAsync(options, userId);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("admin/{id}")]
    public async Task<IEnumerable<ReadOrderDTO>> GetAllOrdersByUserAsync([FromQuery] QueryOptions options, [FromRoute] Guid id)
    {
        return await _orderService.GetAllOrdersByUserAsync(options, id);
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<ReadOrderDTO> GetOrderByIdAsync([FromRoute] Guid id)
    {
        return await _orderService.GetOrderByIdAsync(id);
    }

    [Authorize]
    [HttpPost]
    public async Task<ReadOrderDTO> CreateOrderAsync([FromBody] CreateOrderDTO order)
    {
        Console.WriteLine(order.AddressId);
        return await _orderService.CreateOrderAsync(order);
    }

    [Authorize(Roles = "Admin")]
    [HttpPatch("{id}")]
    public async Task<bool> UpdateOrderByIdAsync([FromRoute] Guid id, [FromBody] UpdateOrderDTO updateOrderDTO)
    {
        return await _orderService.UpdateOrderByIdAsync(id, updateOrderDTO);
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<bool> DeleteOrderByIdAsync([FromRoute] Guid id)
    {
        return await _orderService.DeleteOrderByIdAsync(id);
    }
}
