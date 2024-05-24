using Microsoft.EntityFrameworkCore;
using Server.Core.src.Common;
using Server.Core.src.Entity;
using Server.Core.src.RepoAbstract;
using Server.Core.src.ValueObject;
using Server.Infrastructure.src.Database;

namespace Server.Infrastructure.src.Repo;

public class ProductRepo : BaseRepo<Product>, IProductRepo
{
    private DbSet<OrderProduct> _orderProducts;
    private DbSet<Category> _categories;
    private DbSet<ProductImage> _images;
    private AppDbContext _context;
    public ProductRepo(AppDbContext context) : base(context)
    {
        _context = context;
        _orderProducts = context.OrderProducts;
        _categories = context.Categories;
        _images = context.ProductImages;
    }

    public override async Task<IEnumerable<Product>> GetAllAsync(QueryOptions options)
    {
        IQueryable<Product> filteredData;
        if (options.CategoriseBy is not null && !string.IsNullOrEmpty(options.CategoriseBy))
        {
            var categoryId = Guid.Parse(options.CategoriseBy);
            var productsByCategory = await GetByCategoryAsync(categoryId);
            filteredData = productsByCategory;
        }
        else
        {
            filteredData = _data.Include("ProductImages").Include("Category");
        }
        // Apply search keyword filter if provided
        if (!string.IsNullOrEmpty(options.SearchKey))
        {
            filteredData = filteredData.Where(item => item.Title.ToUpper().Contains(options.SearchKey.ToUpper()));
        }
        // Apply minimum price filter if provided
        if (!string.IsNullOrEmpty(options.MinPrice))
        {
            decimal minPrice = Convert.ToDecimal(options.MinPrice);
            filteredData = filteredData.Where(item => item.Price >= minPrice);
        }
        // Apply maximum price filter if provided
        if (!string.IsNullOrEmpty(options.MaxPrice))
        {
            decimal maxPrice = Convert.ToDecimal(options.MaxPrice);
            filteredData = filteredData.Where(item => item.Price <= maxPrice);
        }

        if (options.SortBy == SortType.ByTitle && options.OrderBy == SortOrder.Ascending)
        {
            filteredData = filteredData.OrderBy(item => item.Title);
        }
        else if (options.SortBy == SortType.ByTitle && options.OrderBy == SortOrder.Descending)
        {
            filteredData = filteredData.OrderByDescending(item => item.Title);
        }
        else if (options.SortBy == SortType.ByPrice && options.OrderBy == SortOrder.Ascending)
        {
            filteredData = filteredData.OrderBy(item => item.Price);
        }
        else
        {
            filteredData = filteredData.OrderByDescending(item => item.Price);
        }
        return await filteredData.Skip(options.PageNo).Take(options.PageSize).ToListAsync();

    }


    public IEnumerable<Product> GetByCategory(Guid categoryId)
    {
        return _data.Include("ProductImages").Include("Category").Where(products => products.Category.Id == categoryId);
    }

    public override async Task<Product?> GetOneByIdAsync(Guid id)
    {
        var allData = _data.Include("ProductImages").Include("Category");
        return await allData.FirstOrDefaultAsync(product => product.Id == id);
    }

    public async Task<IEnumerable<Product>> GetMostPurchasedAsync(int topNumber)
    {
        var mostPurchasedProducts = await _orderProducts
            .GroupBy(orderProduct => orderProduct.Product.Id)
            .Select(group => new
            {
                ProductId = group.Key,
                TotalQuantity = group.Sum(item => item.Quantity)
            })
            .OrderByDescending(item => item.TotalQuantity)
            .Take(topNumber)
            .Join(_data.Include("ProductImages").Include("Category"),
                orderItem => orderItem.ProductId,
                product => product.Id,
                (orderItem, product) => product)
            .ToListAsync();

        return mostPurchasedProducts;
    }

    public async Task<IEnumerable<Product>> GetTopRatedProductsAsync(int topNumber)
    {
        var topRatedProducts = await _data.Include("ProductImages").Include("Category")
        .Select(product => new
        {
            Product = product,
            AverageRating = product.OrderProducts
                                    .Where(orderProduct => orderProduct.Review != null)
                                    .Average(orderProduct => orderProduct.Review.Rating)
        })
        .OrderByDescending(item => item.AverageRating)
        .Take(topNumber)
        .Select(item => item.Product)
        .ToListAsync();

        return topRatedProducts;
    }

    public async Task<int> GetProductsCountAsync(QueryOptions options)
    {
        IQueryable<Product> filteredData;
        if (options.CategoriseBy is not null && !string.IsNullOrEmpty(options.CategoriseBy))
        {
            var categoryId = Guid.Parse(options.CategoriseBy);
            var productsByCategory = await GetByCategoryAsync(categoryId);
            filteredData = productsByCategory.AsQueryable();
        }
        else
        {
            filteredData = _data.Include("ProductImages").Include("Category");
        }

        // Apply minimum price filter if provided
        if (!string.IsNullOrEmpty(options.MinPrice))
        {
            decimal minPrice = Convert.ToDecimal(options.MinPrice);
            filteredData = filteredData.Where(item => item.Price >= minPrice);
        }
        // Apply maximum price filter if provided
        if (!string.IsNullOrEmpty(options.MaxPrice))
        {
            decimal maxPrice = Convert.ToDecimal(options.MaxPrice);
            filteredData = filteredData.Where(item => item.Price <= maxPrice);
        }
        return await filteredData.CountAsync();
    }

    public async Task<IQueryable<Product>> GetByCategoryAsync(Guid categoryId)
    {
        var allCategoryIds = new List<Guid> { categoryId };
        allCategoryIds.AddRange(await GetAllSubCategoryIdsAsync(categoryId));

        return _data
            .Include(p => p.ProductImages)
            .Include(p => p.Category)
            .Where(p => allCategoryIds.Contains(p.Category.Id));
    }
    private async Task<List<Guid>> GetAllSubCategoryIdsAsync(Guid parentId)
    {
        var subCategoryIds = await _categories
                                           .Where(c => c.ParentCategoryId == parentId)
                                           .Select(c => c.Id)
                                           .ToListAsync();

        var allSubCategoryIds = new List<Guid>(subCategoryIds);
        foreach (var subCategoryId in subCategoryIds)
        {
            allSubCategoryIds.AddRange(await GetAllSubCategoryIdsAsync(subCategoryId));
        }
        return allSubCategoryIds;
    }

    public override async Task<bool> DeleteOneByIdAsync(Product deleteObject)
    {
        var productDeletion = await base.DeleteOneByIdAsync(deleteObject);
        var imgList = deleteObject.ProductImages;
        foreach (var img in imgList){
            _images.Remove(img);
            await _context.SaveChangesAsync();
        }
        return productDeletion && true;

    }
}