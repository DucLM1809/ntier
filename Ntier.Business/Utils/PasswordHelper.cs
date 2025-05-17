using System.Security.Cryptography;
using System.Text;

namespace Ntier.Business.Utils
{
  public static class PasswordHelper
  {
    public static string HashPassword(string password)
    {
      using var sha256 = SHA256.Create();
      return Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes(password)));
    }

    public static bool VerifyPassword(string password, string hashedPassword)
    {
      return HashPassword(password) == hashedPassword;
    }
  }
}
