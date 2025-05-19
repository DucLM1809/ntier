using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ntier.Shared.Models
{
    public class Nutrient : BaseEntity
    {
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "varchar(255)")]
        public string Name { get; set; } = null!;

        [Required]
        [Column(TypeName = "varchar(255)")]
        public string Unit { get; set; } = null!;

        public ICollection<FoodNutrient> FoodNutrients { get; set; } = new List<FoodNutrient>();
    }
}