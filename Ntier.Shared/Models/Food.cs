using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ntier.Shared.Models
{
    public class Food : BaseEntity
    {
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "varchar(255)")]
        public string Name { get; set; } = null!;

        public string? Image { get; set; }

        public string? Description { get; set; }

        public string? Source { get; set; }

        public ICollection<FoodNutrient> FoodNutrients { get; set; } = new List<FoodNutrient>();
    }
}