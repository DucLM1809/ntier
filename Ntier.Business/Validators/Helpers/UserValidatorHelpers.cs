using Microsoft.EntityFrameworkCore;
using Ntier.DataAccess;

namespace Ntier.Business.Validators.Helpers;

public static class UserValidatorHelpers
{
  public static async Task<bool> BeUniqueEmail(DataContext context, string email, CancellationToken cancellationToken)
  {
    return !await context.Users.AnyAsync(u => u.Email == email, cancellationToken);
  }
}
