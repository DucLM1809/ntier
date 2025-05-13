using Ntier.Shared.Models;

namespace Ntier.DataAccess.Repository.Interfaces;

public interface IRefreshTokenRepository : IGenericRepository<RefreshToken>
{
    Task<RefreshToken> GetByTokenAsync(string token, CancellationToken cancellationToken);
    Task<List<RefreshToken>> GetUserTokenAsync(int userId, CancellationToken cancellationToken);
    Task RevokeTokenAsync(RefreshToken token, CancellationToken cancellationToken);
    Task RemoveExpiredTokensAsync(CancellationToken cancellationToken);
}