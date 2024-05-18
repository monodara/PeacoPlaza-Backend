using Server.Core.src.Common;
using Server.Core.src.RepoAbstract;
using Server.Service.src.DTO;
using Server.Service.src.ServiceAbstract.EntityServiceAbstract;

namespace Server.Service.src.ServiceImplement.EntityServiceImplement;

public class OrderService : IOrderService
{
    private readonly IOrderRepo _orderRepository;

    public OrderService(IOrderRepo orderRepository)
    {
        _orderRepository = orderRepository;
    }
    public async Task<IEnumerable<OrderReadDto>> GetAllOrdersAsync(QueryOptions options, Guid userId)
    {
        if (userId == Guid.Empty) throw new ArgumentNullException("User Id cannot be null");
        var orders = await _orderRepository.GetAllOrdersAsync(options, userId);
        return orders.Select(o => new OrderReadDto().ReadOrder(o));
    }
    public async Task<IEnumerable<OrderReadDto>> GetAllOrdersByUserAsync(QueryOptions options, Guid userId)
    {
        if (userId == Guid.Empty) throw new ArgumentNullException("User Id cannot be empty");
        var results = await _orderRepository.GetAllOrdersByUserAsync(options, userId);
        return results.Select(o => new OrderReadDto().ReadOrder(o));
    }
    public async Task<OrderReadDto> GetOrderByIdAsync(Guid orderId)
    {
        if (orderId == Guid.Empty) throw new ArgumentException("OrderId should be valid");

        var orderFound = await _orderRepository.GetOrderByIdAsync(orderId);
        if (orderFound is null)
        {
            throw new ArgumentException("orderId is not correct");
        }
        return new OrderReadDto().ReadOrder(orderFound);
    }
    public async Task<OrderReadDto> CreateOrderAsync(Guid userId, OrderCreateDto orderCreateDto)
    {
        var addressId = orderCreateDto.AddressId;
        if (userId == Guid.Empty && addressId == Guid.Empty)
        {
            throw new ArgumentException("UserId & Address Id must be provided.");
        }
        var order = orderCreateDto.CreateOrder();
        order.UserId = userId;
        var createdOrder = await _orderRepository.CreateOrderAsync(order);
        return new OrderReadDto().ReadOrder(createdOrder);
    }
    public async Task<bool> UpdateOrderByIdAsync(Guid orderId, OrderUpdateDto newOrder)
    {
        if (orderId == Guid.Empty) throw new ArgumentException("Order id cannot be empty");
        var oldOrder = await _orderRepository.GetOrderByIdAsync(orderId);
        var result = await _orderRepository.UpdateOrderByIdAsync(orderId, newOrder.UpdateOrder(oldOrder));
        if (result == false) throw new InvalidOperationException("Could not update the order");

        return true;
    }
    public async Task<bool> DeleteOrderByIdAsync(Guid orderId)
    {
        return await _orderRepository.DeleteOrderByIdAsync(orderId);
    }
}
