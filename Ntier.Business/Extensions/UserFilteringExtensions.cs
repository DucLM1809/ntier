using System.Linq.Expressions;
using Ntier.Business.Helpers;
using Ntier.Shared.Enums;
using Ntier.Shared.Filters;
using Ntier.Shared.Models;

namespace Ntier.Business.Service.Extensions;

public static class UserFilteringExtensions
{
    public static Expression<Func<User, bool>> BuildFilter(UserFilter filter)
    {
        Expression<Func<User, bool>> emailFilter = null;
        Expression<Func<User, bool>> roleFilter = null;

        if (!string.IsNullOrWhiteSpace(filter.Email)) emailFilter = user => user.Email.Contains(filter.Email);

        if (filter.Role.HasValue) roleFilter = user => (Role)user.Role == filter.Role;

        return ExpressionBuilder.Build(emailFilter, roleFilter);
    }
}