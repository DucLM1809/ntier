using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Ntier.Shared.Models;

namespace Ntier.DataAccess.Extensions
{
    public static class SoftDeleteModelBuilderExtension
    {
        public static ModelBuilder ApplySoftDeleteQueryFilter(ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
                {
                    // Use the IsDeletedFilter class to apply the global filter
                    modelBuilder.Entity(entityType.ClrType)
                        .HasQueryFilter(CreateFilter(entityType.ClrType));
                }
            }

            return modelBuilder;
        }

        private static LambdaExpression CreateFilter(Type entityType)
        {
            var parameter = Expression.Parameter(entityType, "e");
            var property = Expression.Property(parameter, nameof(BaseEntity.IsDeleted));
            var filter = Expression.Lambda(Expression.Equal(property, Expression.Constant(false)), parameter);
            return filter;
        }
    }
}