using Ntier.Shared.Models;

namespace Ntier.Business.Service;

public interface INutrientService
{
    Task<List<Nutrient>> GetAllNutrientsAsync(QueryParameters queryParameters, CancellationToken cancellationToken);
    Task<Nutrient> GetNutrientByIdAsync(int id, CancellationToken cancellationToken);
    Task<Nutrient> AddNutrientAsync(Nutrient nutrient, CancellationToken cancellationToken);
    Task<Nutrient> UpdateNutrientAsync(Nutrient nutrient, CancellationToken cancellationToken);
    Task DeleteNutrientAsync(int id, CancellationToken cancellationToken);
}