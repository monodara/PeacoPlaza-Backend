using Server.Core.src.Entity;

namespace Server.Service.src.DTO;

public class ProductReadDTO : BaseEntity
{
    public string Title { get; set; }
    public decimal Price { get; set; }
    public string Description { get; set; }
    public IEnumerable<ProductImageReadDTO> ProductImages { get; set; }
    public int Inventory { get; set; }
    public decimal Weight { get; set; }
    public CategoryReadDTO Category { get; set; }
    public ProductReadDTO Transform(Product product)
    {
        Title = product.Title;
        Price = product.Price;
        Description = product.Description;
        ProductImages = product.ProductImages.Select(img=>new ProductImageReadDTO().Transform(img)).ToList();
        Inventory = product.Inventory;
        Weight = product.Weight;
        var categoryReadDTO = new CategoryReadDTO();
        categoryReadDTO.Transform(product.Category);
        Category = categoryReadDTO;
        return this;
    }
}

public class ProductCreateDTO
{
    public string Title { get; set; }
    public decimal Price { get; set; }
    public string Description { get; set; }
    public int Inventory { get; set; }
    public decimal Weight { get; set; }
    public Guid CategoryId { get; set; }
}

public class ProductUpdateDTO
{
    public string Title { get; set; }
    public decimal Price { get; set; }
    public string Description { get; set; }
    public IEnumerable<ProductImageCreateDTO>? Images { get; set; }
    public int Inventory { get; set; }
    public decimal Weight { get; set; }
    public Guid CategoryId { get; set; }
    public Product UpdateProduct(Product oldProduct)
    {
        oldProduct.Title = Title;
        oldProduct.Price = Price;
        oldProduct.Description = Description;
        oldProduct.Inventory = Inventory;
        oldProduct.Weight = Weight;
        oldProduct.CategoryId = CategoryId;
        return oldProduct;
    }
}