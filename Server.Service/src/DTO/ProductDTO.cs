using Server.Core.src.Entity;

namespace Server.Service.src.DTO;

public class ProductReadDto
{
    public Guid Id { get; set;}
    public string Title { get; set; }
    public decimal Price { get; set; }
    public string Description { get; set; }
    public IEnumerable<ProductImageReadDto> ProductImages { get; set; }
    public int Inventory { get; set; }
    public decimal Weight { get; set; }
    public CategoryReadDto Category { get; set; }
    public ProductReadDto Transform(Product product)
    {
        Id = product.Id;
        Title = product.Title;
        Price = product.Price;
        Description = product.Description;
        ProductImages = product.ProductImages.Select(img=>new ProductImageReadDto().Transform(img)).ToList();
        Inventory = product.Inventory;
        Weight = product.Weight;
        var categoryReadDto = new CategoryReadDto();
        categoryReadDto.Transform(product.Category);
        Category = categoryReadDto;
        return this;
    }
}

public class ProductCreateDto
{
    public string Title { get; set; }
    public decimal Price { get; set; }
    public string Description { get; set; }
    public int Inventory { get; set; }
    public decimal Weight { get; set; }
    public Guid CategoryId { get; set; }
}

public class ProductUpdateDto
{
    public string Title { get; set; }
    public decimal Price { get; set; }
    public string Description { get; set; }
    public IEnumerable<ProductImageCreateDto>? Images { get; set; }
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