using Moq;
using Server.Core.src.Common;
using Server.Core.src.Entity;
using Server.Core.src.RepoAbstract;
using Server.Core.src.ValueObject;
using Server.Service.src.DTO;
using Server.Service.src.ServiceImplement.EntityServiceImplement;
using Server.Test.src.Service.Data;

namespace Server.Test.src.Core;

public class OrderServiceTests
{
    private OrderService _orderService;// SUT
    //private IOrderRepo _orderRepo; // Dependencies

    //fake objects from MOQ
    private Mock<IOrderRepo> _orderRepoMock = new Mock<IOrderRepo>();
    private Mock<IUserRepo> _userRepoMock = new Mock<IUserRepo>();

    // [Theory]
    // [ClassData(typeof(OrderServiceTestData))]
    // public async Task CreateOrderAsync_CreateOrder_ReturnsOrder(OrderCreateDto orderCreateDto)
    // {
    //     var order = orderCreateDto.CreateOrder();
    //     var productsList = orderCreateDto.OrderProducts;

    //     _orderRepoMock.Setup(x => x.CreateOrderAsync(It.IsAny<Order>())).ReturnsAsync(order);

    //     _orderService = new OrderService(_orderRepoMock.Object);
    //     var expectedResult = await _orderService.CreateOrderAsync(orderCreateDto);

    //     Assert.NotNull(expectedResult);

    // }

    [Fact]
    public async Task GetOrderById_ReturnsOrder()
    {
        Guid userId = Guid.Parse("f2404c06-a354-4f0e-8cd1-6aa91d875205");
        Guid addressId = Guid.Parse("51614cd7-b453-4398-ae98-9d5419d66307");
        var order = new Order{UserId=userId, AddressId=addressId};

        _orderRepoMock.Setup(x => x.GetOrderByIdAsync(order.Id)).ReturnsAsync(order);

        _orderService = new OrderService(_orderRepoMock.Object);

        var result = await _orderService.GetOrderByIdAsync(order.Id);

        Assert.NotNull(result);

    }

    [Fact]
    public async Task UpdateOrderById_ReturnsTrue()
    {
        Guid userId = Guid.Parse("f2404c06-a354-4f0e-8cd1-6aa91d875205");
        Guid addressId = Guid.Parse("51614cd7-b453-4398-ae98-9d5419d66307");

        var order = new Order{UserId=userId, AddressId=addressId};

        var orderUpdateDto = new OrderUpdateDto(Status.shipped, DateTime.Now);
        var newOrder = orderUpdateDto.UpdateOrder(order);

        _orderRepoMock.Setup(x => x.GetOrderByIdAsync(order.Id))
                  .ReturnsAsync(order);

        _orderRepoMock.Setup(x => x.UpdateOrderByIdAsync(order.Id, It.IsAny<Order>()))
                  .ReturnsAsync(true);

        _orderService = new OrderService(_orderRepoMock.Object);

        var result = await _orderService.UpdateOrderByIdAsync(order.Id, orderUpdateDto);

        Assert.True(result);
    }

    [Fact]
    public async Task DeleteOrderById_ReturnsTrue()
    {
        Guid userId = Guid.Parse("f2404c06-a354-4f0e-8cd1-6aa91d875205");
        Guid addressId = Guid.Parse("51614cd7-b453-4398-ae98-9d5419d66307");

        var order = new Order{UserId =userId, AddressId=addressId};

        _orderRepoMock.Setup(x => x.GetOrderByIdAsync(order.Id)).ReturnsAsync(order);

        _orderRepoMock.Setup(x => x.DeleteOrderByIdAsync(order.Id)).ReturnsAsync(true);

        _orderService = new OrderService(_orderRepoMock.Object);

        var result = await _orderService.DeleteOrderByIdAsync(order.Id);

        Assert.True(result);

    }

    [Fact]
    public async Task GetAllOrdersAsync_ReturnsAllOrders_BasedOnUser()
    {
        QueryOptions queryOptions = new QueryOptions();
        var user = new User("JohnMiller", "john.miller@mail.com", "miller123", Role.Admin);

        List<Order> orders = new List<Order>()
        {
            new Order{UserId = user.Id, Id = Guid.Parse("ee6e3581-7694-4f39-be74-8ce366adcf8c")},
            new Order{ UserId = user.Id,Id = Guid.Parse("da8a97cf-a1c3-4ddc-8cbb-59a968dbff77")},
            new Order{ UserId = user.Id,Id = Guid.Parse("c4d1ad67-5ab2-4c57-826c-0b6af2d2d394")}
        };

        _orderRepoMock.Setup(x => x.GetAllOrdersAsync(queryOptions, user.Id)).ReturnsAsync(orders);

        _orderService = new OrderService(_orderRepoMock.Object);
        var ordersOfUser = await _orderService.GetAllOrdersAsync(queryOptions, user.Id);

        Assert.Equal(3, ordersOfUser.ToList().Count);
    }

}
