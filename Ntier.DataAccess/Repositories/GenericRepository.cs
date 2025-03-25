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

    public IQueryable<T> GetAll()
    {
        return _dbSet.AsQueryable();
    }

    public IQueryable<T> Find(Expression<Func<T, bool>> predicate)
    {
        return _dbSet.Where(predicate).AsQueryable();
    }

    public async Task<List<T>> GetFilteredAsync(Expression<Func<T, bool>>? predicate = null,
        QueryParameters? queryParams = null)
    {
        IQueryable<T> query = _dbSet;

        // Apply Filtering (if predicate exists)
        if (predicate != null) query = query.Where(predicate);

        // Apply Sorting and Pagination (if queryParams exists)
        if (queryParams != null)
            query = query
                .ApplySorting(queryParams.SortBy, queryParams.SortOrder)
                .ApplyPagination(queryParams.Page, queryParams.PageSize);

        return await query.ToListAsync();
    }

    public async Task<T> GetByIdAsync(Guid id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(T entity)
    {
        _dbSet.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var entity = await GetByIdAsync(id);

        if (entity != null)
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}