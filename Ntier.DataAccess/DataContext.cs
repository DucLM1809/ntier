using Microsoft.EntityFrameworkCore;
using Ntier.DataAccess.Extensions;
using Ntier.Shared.Models;

namespace Ntier.DataAccess;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; } = null!;
    public DbSet<RefreshToken> RefreshTokens { get; set; } = null!;
    public DbSet<Food> Foods { get; set; } = null!;
    public DbSet<Nutrient> Nutrients { get; set; } = null!;
    public DbSet<FoodNutrient> FoodNutrients { get; set; } = null!;
    public DbSet<MedicalCondition> MedicalConditions { get; set; } = null!;
    public DbSet<DietRestriction> DietRestrictions { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply global filter for soft delete
        SoftDeleteModelBuilderExtension.ApplySoftDeleteQueryFilter(modelBuilder);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            if (entry.State == EntityState.Added) entry.Entity.CreatedAt = DateTimeOffset.UtcNow;
            entry.Entity.UpdatedAt = DateTimeOffset.UtcNow;
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}