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
    }
}