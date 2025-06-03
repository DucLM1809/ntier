using System.Linq.Expressions;
using Ntier.Shared.Models;

namespace Ntier.DataAccess.Repository.Interfaces;

public interface IGenericRepository<T> where T : BaseEntity
{
    Task<IQueryable<T>> GetAsync(
        Expression<Func<T, bool>>? predicate = null,
        QueryParameters? queryParams = null,
        List<Expression<Func<T, object>>>? includes = null,
        bool disableTracking = false);

    IQueryable<T> Get(
        Expression<Func<T, bool>>? predicate = null,
        QueryParameters? queryParams = null,
        List<Expression<Func<T, object>>>? includes = null,
        bool disableTracking = false);

    Task<T?> GetByIdAsync(object id);
    Task<T> AddAsync(T entity);
    Task AddRange(IEnumerable<T> entities);
    Task UpdateAsync(T entity);
    Task DeleteAsync(object id);
    Task DeleteAsync(T entity);
    Task DeleteRange(IEnumerable<T> entities);
}