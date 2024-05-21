using Server.Core.src.Common;
using Server.Service.src.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Server.Service.src.ServiceAbstract.EntityServiceAbstract;

namespace Server.Controller.src.Controller;

[ApiController]
[Route("api/v1/payment")]
public class PaymentController : ControllerBase
{
    private readonly IPaymentService _paymentService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public PaymentController(IPaymentService paymentService, IHttpContextAccessor httpContextAccessor)
    {
        _paymentService = paymentService;
        _httpContextAccessor = httpContextAccessor;
    }

    [Authorize]
    [HttpGet]
    public async Task<IEnumerable<ReadPaymentDto>> GetAllPaymentsOfOrders([FromQuery] QueryOptions options)
    {
        try
        {
            var userClaims = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userClaims == null) throw new InvalidOperationException("Please login to use this facility!");

            var userId = Guid.Parse(userClaims);

            return await _paymentService.GetAllPaymentsOfOrders(options, userId);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    [Authorize]
    [HttpPost]
    public async Task<ReadPaymentDto> CreatePaymentOfOrder([FromBody] CreatePaymentDto payment)
    {
        return await _paymentService.CreatePaymentOfOrder(payment);
    }
}
