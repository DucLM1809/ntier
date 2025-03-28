using Ntier.Shared.Enums;

namespace Ntier.Shared.Filters;

public class UserFilter
{
    public string? Email { get; set; }
    public Role? Role { get; set; }
}