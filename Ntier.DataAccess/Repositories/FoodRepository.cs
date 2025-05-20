using Ntier.DataAccess.Repositories.Interfaces;
using Ntier.DataAccess.Repository;
using Ntier.Shared.Models;

namespace Ntier.DataAccess.Repositories;

public class FoodRepository : GenericRepository<Food>, IFoodRepository
{
    private readonly DataContext _context;

    public FoodRepository(DataContext context) : base(context)
    {
        _context = context;
    }
}