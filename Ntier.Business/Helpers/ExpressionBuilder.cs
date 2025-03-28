using System.Linq.Expressions;

namespace Ntier.Business.Helpers;

public static class ExpressionBuilder
{
    public static Expression<Func<T, bool>> Build<T>(params Expression<Func<T, bool>>[] expressions)
    {
        Expression<Func<T, bool>> predicate = x => true; // Start with "always true"

        foreach (var expr in expressions)
            if (expr != null)
                predicate = CombineExpressions(predicate, expr);

        return predicate;
    }

    private static Expression<Func<T, bool>> CombineExpressions<T>(
        Expression<Func<T, bool>> first,
        Expression<Func<T, bool>> second)
    {
        var parameter = Expression.Parameter(typeof(T));

        var body = Expression.AndAlso(
            Expression.Invoke(first, parameter),
            Expression.Invoke(second, parameter)
        );

        return Expression.Lambda<Func<T, bool>>(body, parameter);
    }
}