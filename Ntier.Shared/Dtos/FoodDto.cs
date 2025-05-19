namespace Ntier.Shared.Dtos;

public record FoodDto(string Name, string? Description, List<NutrientDto> FoodNutrients);