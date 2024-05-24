using Server.Core.src.Entity;

namespace Server.Service.src.DTO;

public class CategoryReadDto : BaseEntity
{
    public Guid Id;
    public string Name { get; set; }
    public string Image { get; set; }
    public Guid? ParentCategoryId { get; set; }
    public CategoryReadDto Transform(Category category)
    {
        Id = category.Id;
        Name = category.Name;
        Image = category.Image;
        ParentCategoryId = category.ParentCategoryId;
        return this;
    }
}

public class CategoryCreateDto
{
    public string Name { get; set; }
    public string Image { get; set; }
    public Guid? ParentCategoryId{get; set;}
}

public class CategoryUpdateDto
{
    public string Name { get; set; }
    public string Image { get; set; }
    public Guid? ParentCategoryId{ get; set; }
    public Category UpdateCategory(Category oldCate)
    {
        if(string.IsNullOrEmpty(Name)) oldCate.Name = Name;
        if (string.IsNullOrEmpty(Image)) oldCate.Image = Image;
        oldCate.ParentCategoryId = ParentCategoryId;
        return oldCate;
    }
}