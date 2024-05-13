using Server.Core.src.Entity;
using Server.Core.src.ValueObject;

namespace Server.Service.src.DTO;
public class ReadPaymentDTO
{
    public Guid OrderId { get; set; }
    public PaymentMethod PayMethod { get; set; }

    public ReadPaymentDTO ReadPayment(Payment payment)
    {
        return new ReadPaymentDTO()
        {
            OrderId = payment.OrderId,
            PayMethod = payment.PayMethod
        };
    }
}

public class CreatePaymentDTO
{
    public Guid OrderId { get; set; }
    public PaymentMethod PayMethod { get; set; }

    public CreatePaymentDTO(Guid orderId, PaymentMethod payMethod)
    {
        OrderId = orderId;
        PayMethod = payMethod;
    }

    public Payment CreatePayment()
    {
        return new Payment(OrderId, PayMethod);
    }
}