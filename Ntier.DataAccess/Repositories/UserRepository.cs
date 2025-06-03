using Microsoft.EntityFrameworkCore;
using Ntier.DataAccess.Repository.Interfaces;
using Ntier.Shared.Models;

namespace Ntier.DataAccess.Repository;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    private readonly DataContext _context;

    public UserRepository(DataContext context) : base(context)
    {
        _context = context;
    }

    public async Task<User?> GetUserByEmail(string email)
    {
        return await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email == email);
    }
}