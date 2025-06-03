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

    public IQueryable<T> Get(
        Expression<Func<T, bool>>? predicate = null,
        QueryParameters? queryParams = null,
        List<Expression<Func<T, object>>>? includes = null,
        bool disableTracking = false)
    {
        IQueryable<T> query = _dbSet;

        // Apply Filtering (if predicate exists)
        if (predicate is not null) query = query.Where(predicate);

        // Apply Sorting and Pagination (if queryParams exists)
        if (queryParams is not null && queryParams.SortBy is not null)
            query = query
                .ApplySorting(queryParams.SortBy, queryParams.SortOrder)
                .ApplyPagination(queryParams.Page, queryParams.PageSize);

        // Apply Includes (if any)
        if (includes is not null) query = includes.Aggregate(query, (current, include) => current.Include(include));

        if (disableTracking) query = query.AsNoTracking();

        return query.AsQueryable();
    }

    public Task<IQueryable<T>> GetAsync(
        Expression<Func<T, bool>>? predicate = null,
        QueryParameters? queryParams = null,
        List<Expression<Func<T, object>>>? includes = null,
        bool disableTracking = false)
    {
        return Task.FromResult(Get(predicate, queryParams, includes, disableTracking));
    }

    /*
     * The T? in Task<T?> GetByIdAsync(object id); indicates that the method may return a value of type T or null.
     * This is useful for reference types or nullable value types, allowing the method to signal that an entity with the given id might not exist in the data store.
     * It improves null-safety and makes the contract of the method explicit.
     */
    public async Task<T?> GetByIdAsync(object id)
    {
        return await _dbSet.FindAsync(id);
    }

    /*
     * Using await _dbSet.AddAsync(entity); await _context.SaveChangesAsync(); inside the repository method immediately persists the entity, which can lead to:
     * Less efficient database usage (multiple small transactions)
     * Harder unit testing and transaction management
     * Less flexibility for the service layer to control save timing
     */
    public async Task<T> AddAsync(T entity)
    {
        if (_context.Entry(entity).State == EntityState.Detached) _dbSet.Attach(entity);
        await _dbSet.AddAsync(entity);
        return entity;
    }

    public async Task AddRange(IEnumerable<T> entities)
    {
        var listEntities = entities.ToList();

        listEntities.ForEach(entity =>
        {
            if (_context.Entry(entity).State == EntityState.Detached) _dbSet.Attach(entity);
        });

        await _dbSet.AddRangeAsync(listEntities);
    }

    public Task UpdateAsync(T entity)
    {
        if (_context.Entry(entity).State == EntityState.Detached) _dbSet.Attach(entity);

        _dbSet.Entry(entity).State = EntityState.Modified;

        _context.Set<T>().Update(entity);
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(object id)
    {
        var entity = await GetByIdAsync(id);

        if (entity != null)
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }

    public Task DeleteAsync(T entity)
    {
        if (_context.Entry(entity).State == EntityState.Detached) _dbSet.Attach(entity);

        _dbSet.Remove(entity);

        return Task.CompletedTask;
    }

    public Task DeleteRange(IEnumerable<T> entities)
    {
        var listEntities = entities.ToList();
        listEntities.ForEach(entity =>
        {
            if (_context.Entry(entity).State == EntityState.Detached) _dbSet.Attach(entity);
        });

        _dbSet.RemoveRange(listEntities);

        return Task.CompletedTask;
    }
}