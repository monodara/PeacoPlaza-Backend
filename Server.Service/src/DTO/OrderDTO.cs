using Server.Core.src.Common;
using Server.Core.src.Entity;
using Server.Core.src.ValueObject;

namespace Server.Service.src.DTO;

public class OrderReadDto
{
    public Guid Id { get; set; }
    public DateTime OrderDate { get; set; }
    public Status Status { get; set; }
    public Guid UserId { get; set; }
    public DateTime? DateOfDelivery { get; set; }
    public IEnumerable<OrderProduct> OrderProducts { get; set; }

    public OrderReadDto ReadOrder(Order order)
    {
        return new OrderReadDto
        {
            Id = order.Id,
            OrderDate = order.OrderDate,
            Status = order.Status,
            UserId = order.UserId,
            DateOfDelivery = order.DateOfDelivery,
            // OrderProducts = order.OrderProducts,
        };
    }
}
public class OrderCreateDto
{
    public Guid AddressId { get; set; }
    public IEnumerable<OrderProduct> OrderProducts { get; set; }

    public Order CreateOrder()
    {
        return new Order { AddressId = AddressId, OrderProducts = OrderProducts };
    }
}
public class OrderUpdateDto
{
    public Status Status { get; set; }
    public DateTime? DateOfDelivery { get; set; } = DateTime.Now;
    public OrderUpdateDto(Status status, DateTime dateOfDelivery)
    {
        Status = status;
        DateOfDelivery = dateOfDelivery;
    }
    public Order UpdateOrder(Order oldOrder)
    {
        oldOrder.Status = Status;
        if (DateOfDelivery != null) oldOrder.DateOfDelivery = DateOfDelivery;
        return oldOrder;
    }
}
