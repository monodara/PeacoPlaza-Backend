using Server.Core.src.Entity;

namespace Server.Service.src.DTO;

public class ProductImageReadDTO : BaseEntity
{
    public byte[] Data { get; set; }
}

public class ProductImageCreateDTO
{
    public byte[] Data { get; set; }
}

public class ProductImageUpdateDTO
{
    public byte[] Data { get; set; }
}