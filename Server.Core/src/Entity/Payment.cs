using Server.Core.src.ValueObject;

namespace Server.Core.src.Entity;

public class Payment : BaseEntity
{
    public Guid Id { get; set; }
    public Guid OrderId { get; set; }
    public Order Order { get; set; }
    public PaymentMethod PayMethod { get; set; }

    public Payment()
    {
        // Initialize any properties here if needed
        Id = Guid.NewGuid();
        CreatedAt = DateTime.Now;
    }
    public Payment(Guid orderId, PaymentMethod method) : this()
    {
        OrderId = orderId;
        PayMethod = method;
    }
}
