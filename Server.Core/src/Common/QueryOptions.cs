
using Server.Core.src.ValueObject;
namespace Server.Core.src.Common;

public class QueryOptions
{
    public int PageSize { get; set; } = int.MaxValue;
    public int PageNo { get; set; } = 1;
    public SortType? SortBy { get; set; } = SortType.ByTitle;
    public SortOrder? OrderBy { get; set; } = SortOrder.Ascending;
    public string SearchKey { get; set; } = string.Empty; // ""
    public string MinPrice { get; set; } = string.Empty;
    public string MaxPrice { get; set;} = string.Empty;
    public string? CategoriseBy { get; set; }
}