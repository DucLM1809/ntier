using Ntier.DataAccess.Repositories.Interfaces;
using Ntier.DataAccess.Repository;
using Ntier.DataAccess.Repository.Interfaces;

namespace Ntier.DataAccess.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly DataContext _context;
    private IFoodNutrientRepository _foodNutrientRepository;
    private IFoodRepository _foodRepository;
    private IMedicalConditionUserRepository _medicalConditionUserRepository;
    private INutrientRepository _nutrientRepository;
    private IRefreshTokenRepository _refreshTokenRepository;
    private IUserRepository _userRepository;

    public UnitOfWork(DataContext context)
    {
        _context = context;
    }

    public INutrientRepository Nutrients => _nutrientRepository ??= new NutrientRepository(_context);
    public IFoodRepository Foods => _foodRepository ??= new FoodRepository(_context);
    public IUserRepository Users => _userRepository ??= new UserRepository(_context);
    public IRefreshTokenRepository RefreshTokens => _refreshTokenRepository ??= new RefreshTokenRepository(_context);
    public IFoodNutrientRepository FoodNutrients => _foodNutrientRepository ??= new FoodNutrientRepository(_context);

    public IMedicalConditionUserRepository MedicalConditionUsers =>
        _medicalConditionUserRepository ??= new MedicalConditionUserRepository(_context);

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}