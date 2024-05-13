using System.Collections;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.EntityFrameworkCore;
using Server.Core.src.Common;
using Server.Core.src.Entity;
using Server.Core.src.RepoAbstract;
using Server.Core.src.ValueObject;
using Server.Infrastructure.src.Database;

namespace Server.Infrastructure.src.Repo;

public class OrderRepo : IOrderRepo
{
    private readonly AppDbContext _context;

    public OrderRepo(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Order> CreateOrderAsync(Order order, List<ProductsList> productsList)
    {
        _context.Orders.Add(order);
        await _context.SaveChangesAsync();

        foreach (var item in productsList)
        {
            _context.OrderedProducts.Add(new OrderProduct(order.Id, item.ProductId, item.Quantity));
        }
        await _context.SaveChangesAsync();

        return order;
    }

    public async Task<bool> DeleteOrderByIdAsync(Guid orderId)
    {
        Order order = await GetOrderByIdAsync(orderId);
        if (order != null)
        {
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return true;
        }
        return false;
    }

    public async Task<IEnumerable<Order>> GetAllOrdersAsync(QueryOptions options, Guid userId)
    {
        var pgNo = options.PageNo; //1
        var pgSize = options.PageSize; // 10

        if (userId == Guid.Empty) throw new ArgumentNullException("UserId cannot be empty");

        var userRole = _context.Users.FirstOrDefault(u => u.Id == userId)!.Role;

        IQueryable<Order> query = _context.Orders;

        if (userRole == Role.Admin)
        {
            var orders = query
                            .OrderByDescending(o => o.OrderDate)
                            .Skip((pgNo - 1) * pgSize)
                            .Take(pgSize);

            return await orders.ToListAsync();
        }
        else
        {
            var orders = query.Where(o => o.UserId == userId)
                .OrderByDescending(o => o.OrderDate)
                .Skip((pgNo - 1) * pgSize)
                .Take(pgSize);

            return await orders.ToListAsync();
        }


    }

    public async Task<IEnumerable<Order>> GetAllOrdersByUserAsync(QueryOptions options, Guid userId)
    {
        return await GetAllOrdersAsync(options, userId);
    }

    public async Task<Order> GetOrderByIdAsync(Guid orderId)
    {
        Console.WriteLine("From Repo");
        if (orderId == Guid.Empty) throw new ArgumentException("Order Id cannot be null");

        return await _context.Orders.FirstOrDefaultAsync(o => o.Id == orderId);
    }

    public async Task<bool> UpdateOrderByIdAsync(Guid orderId, Order order)
    {
        var orderFound = await GetOrderByIdAsync(orderId);
        if (order != null && orderFound != null)
        {
            orderFound.Status = order.Status;
            orderFound.DateOfDelivery = order.DateOfDelivery;
            orderFound.AddressId = order.AddressId;
        }
        await _context.SaveChangesAsync();

        return true;
    }
}
