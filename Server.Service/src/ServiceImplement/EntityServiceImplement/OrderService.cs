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
    public async Task<IEnumerable<ReadOrderDTO>> GetAllOrdersAsync(QueryOptions options, Guid userId)
    {
        if (userId == Guid.Empty) throw new ArgumentNullException("User Id cannot be null");
        var orders = await _orderRepository.GetAllOrdersAsync(options, userId);
        return orders.Select(o => new ReadOrderDTO().ReadOrder(o));
    }
    public async Task<IEnumerable<ReadOrderDTO>> GetAllOrdersByUserAsync(QueryOptions options, Guid userId)
    {
        if (userId == Guid.Empty) throw new ArgumentNullException("User Id cannot be empty");
        var results = await _orderRepository.GetAllOrdersByUserAsync(options, userId);
        return results.Select(o => new ReadOrderDTO().ReadOrder(o));
    }
    public async Task<ReadOrderDTO> GetOrderByIdAsync(Guid orderId)
    {
        if (orderId == Guid.Empty) throw new ArgumentException("OrderId should be valid");

        var orderFound = await _orderRepository.GetOrderByIdAsync(orderId);
        if (orderFound is null)
        {
            throw new ArgumentException("orderId is not correct");
        }
        return new ReadOrderDTO().ReadOrder(orderFound);
    }
    public async Task<ReadOrderDTO> CreateOrderAsync(CreateOrderDTO createOrderDTO)
    {
        var userId = createOrderDTO.UserId;
        var addressId = createOrderDTO.AddressId;
        var productsList = createOrderDTO.ProductList;

        if (userId == Guid.Empty && addressId == Guid.Empty)
        {
            throw new ArgumentException("UserId & Address Id must be provided.");
        }

        var order = createOrderDTO.CreateOrder();

        var createdOrder = await _orderRepository.CreateOrderAsync(order, productsList);
        if (createdOrder == null) throw new InvalidOperationException("Failed to create order.");

        return new ReadOrderDTO().ReadOrder(createdOrder);
    }
    public async Task<bool> UpdateOrderByIdAsync(Guid orderId, UpdateOrderDTO newOrder)
    {
        if (orderId == Guid.Empty) throw new ArgumentException("Order id cannot be empty");

        var oldOrder = await _orderRepository.GetOrderByIdAsync(orderId);

        var result = await _orderRepository.UpdateOrderByIdAsync(orderId, newOrder.UpdateOrder(oldOrder));

        if (result == false) throw new InvalidOperationException("Could not update the order");

        return true;
    }
    public async Task<bool> DeleteOrderByIdAsync(Guid orderId)
    {
        if (orderId == Guid.Empty) throw new ArgumentException("Order id cannot be empty");

        return await _orderRepository.DeleteOrderByIdAsync(orderId);
    }
}
