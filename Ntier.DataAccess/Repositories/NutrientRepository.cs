using Ntier.DataAccess.Repositories.Interfaces;
using Ntier.DataAccess.Repository;
using Ntier.Shared.Models;

namespace Ntier.DataAccess.Repositories;

public class NutrientRepository : GenericRepository<Nutrient>, INutrientRepository
{
    private readonly DataContext _context;

    public NutrientRepository(DataContext context) : base(context)
    {
        _context = context;
    }
}