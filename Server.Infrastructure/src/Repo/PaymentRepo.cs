using Microsoft.EntityFrameworkCore;
using Server.Core.src.Common;
using Server.Core.src.Entity;
using Server.Core.src.RepoAbstract;
using Server.Core.src.ValueObject;
using Server.Infrastructure.src.Database;

namespace Server.Infrastructure.src.Repo;

public class PaymentRepo : IPaymentRepo
{
    private readonly AppDbContext _context;
    public PaymentRepo(AppDbContext context)
    {
        _context = context;
    }
    public async Task<Payment> CreatePaymentOfOrder(Payment payment)
    {
        _context.Payments.Add(payment);
        await _context.SaveChangesAsync();
        return payment;
    }

    public async Task<List<Payment>> GetAllPaymentsOfOrders(QueryOptions options, Guid userId)
    {
        var pgNo = options.PageNo;
        var pgSize = options.PageSize;
        IQueryable<Payment> query = _context.Payments;

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        var userRole = user!.Role;

        IQueryable<Payment> payments;

        if (userRole == Role.Admin)
        {
            payments = query.OrderByDescending(p => p.OrderId)
                            .Skip((pgNo - 1) * pgSize)
                            .Take(pgSize);
        }
        else
        {
            var ordersOfUser = await _context.Orders.Where(o => o.UserId == userId)
                                    .OrderByDescending(o => o.OrderDate)
                                    .Select(o => o.Id)
                                    .Skip((pgNo - 1) * pgSize)
                                    .Take(pgSize)
                                    .ToListAsync();

            payments = query.Where(p => ordersOfUser.Contains(p.OrderId))
                            .OrderByDescending(p => p.OrderId);

        }

        return await payments.ToListAsync();
    }
}
