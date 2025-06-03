using Microsoft.EntityFrameworkCore;
using Ntier.DataAccess.Repository.Interfaces;
using Ntier.Shared.Models;

namespace Ntier.DataAccess.Repository;

public class RefreshTokenRepository : GenericRepository<RefreshToken>, IRefreshTokenRepository
{
    private readonly DataContext _context;

    public RefreshTokenRepository(DataContext context) : base(context)
    {
        _context = context;
    }

    public async Task<RefreshToken?> GetByTokenAsync(string token, CancellationToken cancellationToken)
    {
        return await _context.RefreshTokens
            .Include(rt => rt.User)
            .FirstOrDefaultAsync(rt => rt.Token == token, cancellationToken);
    }

    public async Task<List<RefreshToken>> GetUserTokenAsync(int userId, CancellationToken cancellationToken)
    {
        return await _context.RefreshTokens
            .Where(rt => rt.UserId == userId && !rt.IsRevoked)
            .ToListAsync(cancellationToken);
    }

    public async Task RevokeTokenAsync(RefreshToken token, CancellationToken cancellationToken)
    {
        token.IsRevoked = true;
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task RemoveExpiredTokensAsync(CancellationToken cancellationToken)
    {
        var expiredTokens = await _context.RefreshTokens
            .Where(rt => rt.ExpiryDate < DateTimeOffset.UtcNow)
            .ToListAsync(cancellationToken);

        _context.RefreshTokens.RemoveRange(expiredTokens);
        await _context.SaveChangesAsync(cancellationToken);
    }
}