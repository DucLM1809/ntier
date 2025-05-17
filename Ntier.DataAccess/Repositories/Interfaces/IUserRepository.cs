using Ntier.Shared.Models;

namespace Ntier.DataAccess.Repository.Interfaces;

public interface IUserRepository : IGenericRepository<User>
{
    Task<User> GetUserByEmail(string email, CancellationToken cancellationToken);
    Task<User> AddUser(User user, CancellationToken cancellationToken);
    Task<User> UpdateUser(User user, CancellationToken cancellationToken);
    Task DeleteUser(int userId, CancellationToken cancellationToken);
    Task<User> GetUserByIdAsync(int userId, CancellationToken cancellationToken);
}