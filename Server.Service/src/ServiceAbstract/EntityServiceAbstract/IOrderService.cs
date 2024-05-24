using Server.Core.src.Common;
using Server.Service.src.DTO;

namespace Server.Service.src.ServiceAbstract.EntityServiceAbstract;
public interface IOrderService
{
    public Task<IEnumerable<OrderReadDto>> GetAllOrdersAsync(QueryOptions options, Guid userId);
    public Task<IEnumerable<OrderReadDto>> GetAllOrdersByUserAsync(QueryOptions options, Guid userId);
    public Task<OrderReadDto> GetOrderByIdAsync(Guid orderId);
    public Task<bool> CreateOrderAsync(Guid userId, OrderCreateDto orderCreateDto);
    public Task<bool> UpdateOrderByIdAsync(Guid orderId, OrderUpdateDto orderUpdateDto);
    public Task<bool> DeleteOrderByIdAsync(Guid orderId);
}
