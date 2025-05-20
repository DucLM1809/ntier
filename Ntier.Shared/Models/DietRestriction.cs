namespace Ntier.Shared.Models;

public class DietRestriction : BaseEntity
{
    public int Id { get; set; }
    public string? High { get; set; }
    public string? Low { get; set; }
    public string? Avoid { get; set; }
}