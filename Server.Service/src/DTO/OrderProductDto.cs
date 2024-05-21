

using Server.Core.src.Entity;

namespace Server.Service.src.DTO
{
    public class OrderProductReadDto
    {
        public ProductReadDto Product { get; set; }
        public int Quantity { get; set; }
    }

    public class OrderProductCreatedDto
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        
    }
}