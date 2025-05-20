using Ntier.Shared.Models;

namespace Ntier.DataAccess.Repository.Interfaces;

public interface IUserRepository : IGenericRepository<User>
{
    Task<User> GetUserByEmail(string email, CancellationToken cancellationToken);
}