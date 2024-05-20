using Server.Core.src.Entity;

namespace Server.Service.src.DTO;

public class ProductImageReadDTO : BaseEntity
{
    // public byte[] Data { get; set; }
    public string Data { get; set; }
    public ProductImageReadDTO Transform(ProductImage productImage){
        Data = "data:image/jpeg;base64," + Convert.ToBase64String(productImage.Data);
        return this;
    }
}

public class ProductImageCreateDTO
{
    public byte[] Data { get; set; }
}

public class ProductImageUpdateDTO
{
    public byte[] Data { get; set; }
}