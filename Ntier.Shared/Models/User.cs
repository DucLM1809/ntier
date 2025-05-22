using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ntier.Shared.Models;

public class User : BaseEntity
{
    public int Id { get; set; }

    [Required]
    [EmailAddress]
    [Column(TypeName = "varchar(255)")]
    public string Email { get; set; } = null!;

    [Required]
    [Column(TypeName = "varchar(255)")]
    public string Password { get; set; } = null!;

    public int Role { get; set; }
    public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();

    public DateTimeOffset? DateOfBirth { get; set; } = null;

    [Required] public string Name { get; set; } = null!;

    public string? Avatar { get; set; } = null;

    public float? Height { get; set; } = null;

    public int Gender { get; set; } = 0;

    public int? ExpertProfileId { get; set; } = null;
    public ExpertProfile? ExpertProfile { get; set; } = null;

    public ICollection<MedicalConditionUser>? MedicalConditionUser { get; set; } = null;
}