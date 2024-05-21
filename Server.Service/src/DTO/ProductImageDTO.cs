using Server.Core.src.Entity;

namespace Server.Service.src.DTO;

public class ProductImageReadDto : BaseEntity
{
    // public byte[] Data { get; set; }
    public string Data { get; set; }
    public ProductImageReadDto Transform(ProductImage productImage){
        Data = "data:image/jpeg;base64," + Convert.ToBase64String(productImage.Data);
        return this;
    }
}

public class ProductImageCreateDto
{
    public byte[] Data { get; set; }
}

public class ProductImageUpdateDto
{
    public byte[] Data { get; set; }
}