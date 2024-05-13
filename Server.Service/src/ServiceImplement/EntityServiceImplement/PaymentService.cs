
using Server.Core.src.Common;
using Server.Core.src.RepoAbstract;
using Server.Service.src.DTO;
using Server.Service.src.ServiceAbstract.EntityServiceAbstract;

namespace Server.Service.src.ServiceImplement.EntityServiceImplement;

public class PaymentService : IPaymentService
{
    private readonly IPaymentRepo _paymentrepo;

    public PaymentService(IPaymentRepo paymentrepo)
    {
        _paymentrepo = paymentrepo;
    }
    public async Task<ReadPaymentDTO> CreatePaymentOfOrder(CreatePaymentDTO payment)
    {
        if (payment == null)
            throw new ArgumentNullException("Payment information cannot be null or empty");
        var paymentObj = payment.CreatePayment();
        var paymentDetail = await _paymentrepo.CreatePaymentOfOrder(paymentObj);
        if (paymentDetail == null) throw new InvalidOperationException("Payment cannot be completed");
        return new ReadPaymentDTO().ReadPayment(paymentDetail);
    }

    public async Task<IEnumerable<ReadPaymentDTO>> GetAllPaymentsOfOrders(QueryOptions options, Guid userId)
    {
        if (userId == Guid.Empty)
            throw new ArgumentNullException("User Id cannot be empty");
        var paymentsDetails = await _paymentrepo.GetAllPaymentsOfOrders(options, userId);
        return paymentsDetails.Select(payment => new ReadPaymentDTO().ReadPayment(payment));
    }
}
