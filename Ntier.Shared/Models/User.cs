namespace Ntier.Shared.Models;

public class User : BaseEntity
{
    public int Id { get; set; }
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public int Role { get; set; }
}