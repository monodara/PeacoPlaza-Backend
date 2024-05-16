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
    private readonly DbSet<Order> _orders;
    private readonly DbSet<Product> _products;
    private readonly DbSet<OrderProduct> _orderproducts;

    public OrderRepo(AppDbContext context)
    {
        _context = context;
        _orders = context.Orders;
        _products = context.Products;
        _orderproducts = context.OrderProducts;
    }

    public async Task<Order> CreateOrderAsync(Order order)
    {
        using (var transaction = await _context.Database.BeginTransactionAsync())
        {
            try
            {
                foreach (var orderProduct in order.OrderProducts)
                {
                    var foundProduct = await _products.FindAsync(orderProduct.ProductId);
                    orderProduct.OrderId = order.Id;
                    Console.WriteLine(order.Id);
                    await _orderproducts.AddAsync(orderProduct);
                    foundProduct.Inventory -= orderProduct.Quantity;
                }
                await _orders.AddAsync(order);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return order;
            }
            catch (DbUpdateException)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }

    public async Task<bool> DeleteOrderByIdAsync(Guid orderId)
    {
        Order order = await GetOrderByIdAsync(orderId);
        _orders.Remove(order);
        // delete orderProduct
        var orderProducts = _orderproducts.Where(op=>op.OrderId == orderId);
        foreach(var op in orderProducts){
            _orderproducts.Remove(op);
        }
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<Order>> GetAllOrdersAsync(QueryOptions options, Guid userId)
    {
        var pgNo = options.PageNo; //1
        var pgSize = options.PageSize; // 10
        if (userId == Guid.Empty) throw new ArgumentNullException("UserId cannot be empty");
        var userRole = _context.Users.FirstOrDefault(u => u.Id == userId)!.Role;
        // var query = _context.Orders.Include("OrderProducts.Product");
        IQueryable<Order> query = _context.Orders
    .Include(o => o.OrderProducts)
        .ThenInclude(op => op.Product);
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
            Console.WriteLine(userId);
            var orders = query
                        .Where(o => o.UserId == userId)
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
        return await _context.Orders.FirstOrDefaultAsync(o => o.Id == orderId);
    }

    public async Task<bool> UpdateOrderByIdAsync(Guid orderId, Order order)
    {
        var orderFound = await GetOrderByIdAsync(orderId);
        // if (order != null && orderFound != null)
        // {
        //     orderFound.Status = order.Status;
        //     orderFound.DateOfDelivery = order.DateOfDelivery;
        //     orderFound.AddressId = order.AddressId;
        // }
        _orders.Update(order);
        await _context.SaveChangesAsync();

        return true;
    }
}
