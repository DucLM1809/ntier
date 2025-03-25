using System.Linq.Expressions;
using System.Reflection;
using Ntier.Shared.Enums;

namespace Ntier.DataAccess.Extensions;

public static class QueryableExtension
{
    public static IQueryable<T> ApplyPagination<T>(this IQueryable<T> query, int page, int pageSize)
    {
        return query.Skip((page - 1) * pageSize).Take(pageSize);
    }

    public static IQueryable<T> ApplySorting<T>(this IQueryable<T> query, string sortBy, SortOrder sortOrder)
    {
        if (string.IsNullOrEmpty(sortBy)) return query;

        var property =
            typeof(T).GetProperty(sortBy, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
        if (property == null) return query;

        var parameter = Expression.Parameter(typeof(T), "x");
        var propertyAccess = Expression.Property(parameter, property);
        var orderByExpression = Expression.Lambda(propertyAccess, parameter);

        var methodName = sortOrder == SortOrder.Asc ? "OrderBy" : "OrderByDescending";

        // Get the correct OrderBy / OrderByDescending method with 2 parameters
        var method = typeof(Queryable).GetMethods()
            .Where(m => m.Name == methodName && m.GetParameters().Length == 2)
            .FirstOrDefault()?
            .MakeGenericMethod(typeof(T), property.PropertyType);

        if (method == null) return query; // Fail gracefully if method is not found

        return (IQueryable<T>)method.Invoke(null, new object[] { query, orderByExpression })!;
    }
}