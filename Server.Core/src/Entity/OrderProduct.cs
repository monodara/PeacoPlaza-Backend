using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Core.src.Entity;

public class OrderProduct : BaseEntity
{
    public Guid? OrderId { get; set; }
    public Order? Order { get; set; }
    public Guid ProductId { get; set; }
    public Product? Product { get; set; }
    public int Quantity { get; set; }

    // public OrderProduct(Guid orderId, Guid productId, int quantity)
    // {
    //     Id = Guid.NewGuid();
    //     OrderId = orderId;
    //     ProductId = productId;
    //     Quantity = quantity;
    //     CreatedAt = DateTime.Now;
    //     UpdatedAt = DateTime.Now;
    // }
}
