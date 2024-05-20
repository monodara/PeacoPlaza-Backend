
namespace Server.Core.src.Entity;

public class ProductImage : BaseEntity
{
    public byte[] Data { get; set; }
    public Guid ProductId { get; set; }
    // navigation
    public Product Product { get; set; }
    // public ProductImage(string url, Guid productId)
    // {
    //     Id = Guid.NewGuid();
    //     Url = url;
    //     ProductId = productId;
    // }
}