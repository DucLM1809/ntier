namespace Ntier.Shared.Models;

public class MedicalCondition : BaseEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public ICollection<MedicalConditionUser> MedicalConditionUsers { get; set; } = new List<MedicalConditionUser>();
}