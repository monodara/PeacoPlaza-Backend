
using Server.Core.src.ValueObject;
namespace Server.Core.src.Common;

public class QueryOptions
{
    public int PageSize { get; set; } = 10;
    public int PageNo { get; set; } = 0;
    public SortType? sortType { get; set; } = SortType.byTitle;
    public SortOrder? sortOrder { get; set; } = SortOrder.asc;
    public string SearchKey { get; set; } = string.Empty; // ""
}