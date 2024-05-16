using Server.Core.src.Entity;
using Server.Core.src.ValueObject;

namespace Server.Core.src.Entity;

public class Order : BaseEntity
{

    public DateTime OrderDate { get; set; } = DateTime.Now;
    public Status Status { get; set; } = Status.processing;
    public Guid UserId { get; set; }
    public User User { get; set; }
    public DateTime? DateOfDelivery { get; set; }
    public Guid AddressId { get; set; }
    public Address Address { get; set; }
    public IEnumerable<OrderProduct> OrderProducts{ get; set; }

    // public Order(Guid userId, Guid addressId)
    // {
    //     Id = Guid.NewGuid();
    //     OrderDate = DateTime.Now;
    //     Status = Status.processing;
    //     UserId = userId;
    //     AddressId = addressId;
    // }
}
