namespace Ntier.Shared.Models;

public class ExpertProfile : BaseEntity
{
    public int Id { get; set; }
    public string CertImage { get; set; } = string.Empty;
    public string? Description { get; set; }
    public ICollection<User> Users { get; set; } = new List<User>();
}