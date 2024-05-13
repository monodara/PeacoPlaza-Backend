using Server.Core.src.Common;
using Server.Service.src.DTO;

namespace Server.Service.src.ServiceAbstract.EntityServiceAbstract;
public interface IOrderService
{
    public Task<IEnumerable<ReadOrderDTO>> GetAllOrdersAsync(QueryOptions options, Guid userId);
    public Task<IEnumerable<ReadOrderDTO>> GetAllOrdersByUserAsync(QueryOptions options, Guid userId);
    public Task<ReadOrderDTO> GetOrderByIdAsync(Guid orderId);
    public Task<ReadOrderDTO> CreateOrderAsync(CreateOrderDTO createOrderDTO);
    public Task<bool> UpdateOrderByIdAsync(Guid orderId, UpdateOrderDTO updateOrderDTO);
    public Task<bool> DeleteOrderByIdAsync(Guid orderId);
}
