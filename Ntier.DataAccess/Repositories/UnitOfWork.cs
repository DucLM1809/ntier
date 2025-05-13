using Ntier.DataAccess.Repositories.Interfaces;
using Ntier.DataAccess.Repository;
using Ntier.DataAccess.Repository.Interfaces;

namespace Ntier.DataAccess.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext _context;
        private IUserRepository _userRepository;
        private IRefreshTokenRepository _refreshTokenRepository;

        public UnitOfWork(DataContext context)
        {
            _context = context;
        }

        public IUserRepository Users => _userRepository ??= new UserRepository(_context);
        public IRefreshTokenRepository RefreshTokens => _refreshTokenRepository ??= new RefreshTokenRepository(_context);

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}