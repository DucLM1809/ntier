using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Ntier.DataAccess.Extensions;
using Ntier.DataAccess.Repository.Interfaces;
using Ntier.Shared.Models;

namespace Ntier.DataAccess.Repository;

public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
{
    private readonly DataContext _context;
    private readonly DbSet<T> _dbSet;

    public GenericRepository(DataContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public IQueryable<T> GetAll(CancellationToken cancellationToken)
    {
        return _dbSet.AsQueryable();
    }

    public IQueryable<T> Find(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken)
    {
        return _dbSet.Where(predicate).AsQueryable();
    }

    public async Task<List<T>> GetFilteredAsync(Expression<Func<T, bool>>? predicate = null,
        QueryParameters? queryParams = null, CancellationToken cancellationToken = default)
    {
        IQueryable<T> query = _dbSet;

        // Apply Filtering (if predicate exists)
        if (predicate != null) query = query.Where(predicate);

        // Apply Sorting and Pagination (if queryParams exists)
        if (queryParams != null)
            query = query
                .ApplySorting(queryParams.SortBy, queryParams.SortOrder)
                .ApplyPagination(queryParams.Page, queryParams.PageSize);

        return await query.ToListAsync(cancellationToken);
    }

    public async Task<T> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _dbSet.FindAsync(id, cancellationToken);
    }

    public async Task AddAsync(T entity, CancellationToken cancellationToken)
    {
        await _dbSet.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(T entity, CancellationToken cancellationToken)
    {
        _dbSet.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var entity = await GetByIdAsync(id, cancellationToken);

        if (entity != null)
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}