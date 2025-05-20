using Ntier.DataAccess.Repository.Interfaces;

namespace Ntier.DataAccess.Repositories.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IUserRepository Users { get; }
    IRefreshTokenRepository RefreshTokens { get; }
    IFoodRepository Foods { get; }
    INutrientRepository Nutrients { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}