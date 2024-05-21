using Server.Core.src.Entity;
using Server.Core.src.ValueObject;

namespace Server.Service.src.DTO;
public class ReadPaymentDto
{
    public Guid OrderId { get; set; }
    public PaymentMethod PayMethod { get; set; }

    public ReadPaymentDto ReadPayment(Payment payment)
    {
        return new ReadPaymentDto()
        {
            OrderId = payment.OrderId,
            PayMethod = payment.PayMethod
        };
    }
}

public class CreatePaymentDto
{
    public Guid OrderId { get; set; }
    public PaymentMethod PayMethod { get; set; }

    public CreatePaymentDto(Guid orderId, PaymentMethod payMethod)
    {
        OrderId = orderId;
        PayMethod = payMethod;
    }

    public Payment CreatePayment()
    {
        return new Payment(OrderId, PayMethod);
    }
}