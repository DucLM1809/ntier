using Ntier.Shared.Models;

namespace Ntier.Business.Service;

public interface IFoodService
{
    Task<List<Food>> GetFilteredFoodsAsync(QueryParameters queryParameters, CancellationToken cancellationToken);
    Task<Food> GetFoodByIdAsync(int id, CancellationToken cancellationToken);
    Task<Food> AddFoodAsync(Food food, CancellationToken cancellationToken);
    Task<Food> UpdateFoodAsync(int id, Food food, CancellationToken cancellationToken);
    Task DeleteFoodAsync(int id, CancellationToken cancellationToken);
}