using Ntier.DataAccess.Repositories.Interfaces;
using Ntier.DataAccess.Repository;
using Ntier.Shared.Models;

namespace Ntier.DataAccess.Repositories;

public class MedicalConditionUserRepository : GenericRepository<MedicalConditionUser>, IMedicalConditionUserRepository
{
    private readonly DataContext _context;

    public MedicalConditionUserRepository(DataContext context) : base(context)
    {
        _context = context;
    }
}