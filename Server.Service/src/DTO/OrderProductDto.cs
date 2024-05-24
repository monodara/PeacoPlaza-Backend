

using Server.Core.src.Entity;

namespace Server.Service.src.DTO
{
    public class OrderProductReadDto
    {
        public Guid Id { get; set;}
        public ProductReadDto Product { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public OrderProductReadDto Transform(OrderProduct orderProduct){
            Id = orderProduct.Id;
            ProductId = orderProduct.ProductId;
            if (orderProduct.Product is null){
            }
            if(orderProduct.Product != null) Product = new ProductReadDto().Transform(orderProduct.Product);
            Quantity = orderProduct.Quantity;
            return this;
        }
    }

    public class OrderProductCreateDto
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public OrderProduct CreateOrderProduct()
        {
            return new OrderProduct{ ProductId  = ProductId, Quantity = Quantity };
        }
        
    }
}