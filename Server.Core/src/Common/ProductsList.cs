using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Core.src.Common;

public class ProductsList
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }

    public ProductsList(Guid productId, int quantity)
    {
        ProductId = productId;
        Quantity = quantity;
    }
}
