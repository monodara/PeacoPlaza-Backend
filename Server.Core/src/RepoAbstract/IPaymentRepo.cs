using Server.Core.src.Common;
using Server.Core.src.Entity;

namespace Server.Core.src.RepoAbstract;

public interface IPaymentRepo
{
    public Task<List<Payment>> GetAllPaymentsOfOrders(QueryOptions options, Guid userId);
    public Task<Payment> CreatePaymentOfOrder(Payment payment);
}
