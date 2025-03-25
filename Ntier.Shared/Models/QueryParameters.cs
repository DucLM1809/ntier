using Ntier.Shared.Enums;

namespace Ntier.Shared.Models;

public class QueryParameters
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? SortBy { get; set; }
    public SortOrder SortOrder { get; set; } = SortOrder.Asc;
}