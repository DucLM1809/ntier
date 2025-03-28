using Ntier.Shared.Models;

namespace Ntier.DataAccess.Repository.Interfaces;

public interface IRefreshTokenRepository : IGenericRepository<RefreshToken>
{
    Task<RefreshToken> GetByTokenAsync(string token);
    Task<List<RefreshToken>> GetUserTokenAsync(int userId);
    Task RevokeTokenAsync(RefreshToken token);
    Task RemoveExpiredTokensAsync();
}