using Ntier.Shared.Models;

namespace Ntier.Business.Service;

public interface IJwtService
{
    string GenerateToken(User user);
}
