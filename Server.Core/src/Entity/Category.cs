
using System.ComponentModel.DataAnnotations;

namespace Server.Core.src.Entity;

public class Category : BaseEntity
{
    [Required(ErrorMessage = "Category name is required")]
    public string Name { get; set; }
    [Required(ErrorMessage = "Category image is required")]
    public string Image { get; set; }
    public Guid? ParentCategoryId { get; set; }
    public Category? ParentCategory { get; set; }
    public IEnumerable<Product> Products { get; set; }
    public Category(string name, string image)
    {
        Id = Guid.NewGuid();
        Name = name;
        Image = image;
    }
}