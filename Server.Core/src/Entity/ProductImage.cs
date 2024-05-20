using System.ComponentModel.DataAnnotations;

namespace Server.Core.src.Entity;

public class ProductImage : BaseEntity
{
    public Guid ProductId { get; set; }
    // navigation
    public Product Product { get; set; }
    public byte[] Data { get; set; }
    // public ProductImage(string url, Guid productId)
    // {
    //     Id = Guid.NewGuid();
    //     Url = url;
    //     ProductId = productId;
    // }
}