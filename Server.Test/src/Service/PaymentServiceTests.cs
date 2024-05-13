using Moq;
using Server.Core.src.Entity;
using Server.Core.src.RepoAbstract;
using Server.Core.src.ValueObject;
using Server.Service.src.DTO;
using Server.Service.src.ServiceAbstract.EntityServiceAbstract;
using Server.Service.src.ServiceImplement.EntityServiceImplement;

namespace Server.Test.src.Service;

public class PaymentServiceTests
{
    private IPaymentService _paymentService;
    Mock<IPaymentRepo> _mockPaymentRepo = new Mock<IPaymentRepo>();

    [Fact]
    public async void CreatePaymentOfOrder_ReturnsPaymentInfo()
    {
        var orderId = Guid.NewGuid();
        var createPayment = new CreatePaymentDTO(orderId, PaymentMethod.creditcard);
        var payment = createPayment.CreatePayment();

        _mockPaymentRepo.Setup(x => x.CreatePaymentOfOrder(It.IsAny<Payment>())).ReturnsAsync(payment);
        _paymentService = new PaymentService(_mockPaymentRepo.Object);

        var paymentDetails = await _paymentService.CreatePaymentOfOrder(createPayment);

        Assert.Equal(paymentDetails.OrderId, orderId);

    }
}
