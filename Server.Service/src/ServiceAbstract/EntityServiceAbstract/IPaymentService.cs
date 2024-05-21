using Server.Core.src.Common;
using Server.Core.src.Entity;
using Server.Service.src.DTO;

namespace Server.Service.src.ServiceAbstract.EntityServiceAbstract;

public interface IPaymentService
{
    public Task<IEnumerable<ReadPaymentDto>> GetAllPaymentsOfOrders(QueryOptions options, Guid userId);
    public Task<ReadPaymentDto> CreatePaymentOfOrder(CreatePaymentDto payment);
}
