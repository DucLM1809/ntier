namespace Ntier.Shared.Models;

public class MedicalConditionUser : BaseEntity
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    public int MedicalConditionId { get; set; }
    public MedicalCondition MedicalCondition { get; set; } = null!;
}