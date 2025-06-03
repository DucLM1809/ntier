using Ntier.Shared.Models;

namespace Ntier.Business.Service;

public interface IFoodNutrientService
{
    Task<List<FoodNutrient>> GetFilteredFoodNutrientsAsync(QueryParameters queryParameters,
        CancellationToken cancellationToken = default);

    Task<FoodNutrient> GetFoodNutrientByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<FoodNutrient> AddFoodNutrientAsync(FoodNutrient foodNutrient, CancellationToken cancellationToken);
    Task<FoodNutrient> UpdateFoodNutrientAsync(int id, FoodNutrient foodNutrient, CancellationToken cancellationToken);
    Task DeleteFoodNutrientAsync(int id, CancellationToken cancellationToken);
}