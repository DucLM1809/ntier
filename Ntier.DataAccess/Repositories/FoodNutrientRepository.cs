using Ntier.DataAccess.Repositories.Interfaces;
using Ntier.DataAccess.Repository;
using Ntier.Shared.Models;

namespace Ntier.DataAccess.Repositories;

public class FoodNutrientRepository : GenericRepository<FoodNutrient>, IFoodNutrientRepository
{
    private readonly DataContext _context;

    public FoodNutrientRepository(DataContext context) : base(context)
    {
        _context = context;
    }
}