using Microsoft.EntityFrameworkCore;
using Server.Core.src.Common;
using Server.Core.src.Entity;
using Server.Core.src.RepoAbstract;
using Server.Infrastructure.src.Database;

namespace Server.Infrastructure.src.Repo
{
    public class CategoryRepo : BaseRepo<Category>, ICategoryRepo
    {
        public CategoryRepo(AppDbContext databaseContext) : base(databaseContext)
        {
        }

        public async Task<IEnumerable<Category>> GetSubcategories(Guid categoryId)
        {
            // get direct sub-category
            var subcategories = await _data.Where(c => c.ParentCategoryId == categoryId).ToListAsync();

            // recursively get sub-categories of sub-category
            foreach (var category in subcategories)
            {
                var children = await GetSubcategories(category.Id);
                subcategories.AddRange(children);
            }

            return subcategories;
        }
    }
}