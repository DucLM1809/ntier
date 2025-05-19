namespace Ntier.Shared.Models
{
    public class FoodNutrient : BaseEntity
    {
        public int Id { get; set; }
        public int FoodId { get; set; }
        public int NutrientId { get; set; }
        public float Amount { get; set; } = 0;
        public Food Food { get; set; } = null!;
        public Nutrient Nutrient { get; set; } = null!;
    }
}