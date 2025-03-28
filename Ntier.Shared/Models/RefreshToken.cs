namespace Ntier.Shared.Models;

public class RefreshToken : BaseEntity
{
    public int Id { get; set; }
    public string Token { get; set; }
    public DateTimeOffset ExpiryDate { get; set; }
    public bool IsRevoked { get; set; }
    public string DeviceInfo { get; set; }
    public string IpAddress { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
}