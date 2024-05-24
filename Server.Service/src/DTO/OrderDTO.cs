using Server.Core.src.Common;
using Server.Core.src.Entity;
using Server.Core.src.ValueObject;

namespace Server.Service.src.DTO;

public class OrderReadDto
{
    public Guid Id { get; set; }
    public DateTime OrderDate { get; set; }
    public string Status { get; set; }
    public Guid UserId { get; set; }
    public DateTime? DateOfDelivery { get; set; }
    public IEnumerable<OrderProductReadDto> OrderProducts { get; set; }

    public OrderReadDto Transform(Order order)
    {
        Id = order.Id;
        OrderDate = order.OrderDate;
        Status = order.Status.ToString();
        UserId = order.UserId;
        DateOfDelivery = order.DateOfDelivery;
        if (order.OrderProducts != null)
        {
            OrderProducts = order.OrderProducts.Select(op => new OrderProductReadDto().Transform(op)).ToList();
        }
        else
        {
            OrderProducts = new List<OrderProductReadDto>();
        }
        return this;
    }
}


public class OrderCreateDto
{
    public Guid AddressId { get; set; }
    public IEnumerable<OrderProductCreateDto> OrderProducts { get; set; }

    public Order CreateOrder()
    {
        IEnumerable<OrderProduct> orderProducts = OrderProducts.Select(op => op.CreateOrderProduct()
        ).ToList();
        return new Order { Id = Guid.NewGuid(), AddressId = AddressId, OrderProducts = orderProducts };
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
