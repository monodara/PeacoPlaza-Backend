using Server.Core.src.Common;
using Server.Core.src.Entity;
using Server.Service.src.DTO;

namespace Server.Test.src.Service.Data;

public class OrderServiceTestData : TheoryData<CreateOrderDTO>
{
    Guid userId = Guid.Parse("b6a509af-a85a-4958-8589-7b4f0119ede8");
    Guid addressId = Guid.Parse("51614cd7-b453-4398-ae98-9d5419d66307");

    List<ProductsList> productsList =
    [
        new ProductsList(Guid.NewGuid(),2),
        new ProductsList(Guid.NewGuid(),4)
    ];

    public OrderServiceTestData()
    {
        var createOrderDTO = new CreateOrderDTO(userId, addressId, productsList);

        Add(createOrderDTO);
    }
}
