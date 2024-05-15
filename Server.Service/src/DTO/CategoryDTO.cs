using Server.Core.src.Entity;

namespace Server.Service.src.DTO;

public class CategoryReadDTO : BaseEntity
{
    public string Name { get; set; }
    public string Image { get; set; }
    public Guid? ParentCategoryId { get; set; }
    public void Transform(Category category)
    {
        category.Name = Name;
        category.Image = Image;
        ParentCategoryId = category.ParentCategoryId;
    }
}

public class CategoryCreateDTO
{
    public string Name { get; set; }
    public string Image { get; set; }
    public Guid? ParentCategoryId{get; set;}
}

public class CategoryUpdateDTO
{
    public string Name { get; set; }
    public string Image { get; set; }
    public Category UpdateCategory(Category oldCate)
    {
        oldCate.Name = Name;
        oldCate.Image = Image;
        return oldCate;
    }
}