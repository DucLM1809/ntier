using Ntier.Shared.Models;

namespace Ntier.DataAccess.Repository.Interfaces;

public interface IUserRepository
{
    Task<User> GetUserByEmail(string email);
    Task<User> AddUser(User user);
}
