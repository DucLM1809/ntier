using System.Linq.Expressions;
using Ntier.Shared.Models;

namespace Ntier.DataAccess.Repository.Interfaces;

public interface IGenericRepository<T> where T : BaseEntity
{
    IQueryable<T> GetAll();
    IQueryable<T> Find(Expression<Func<T, bool>> predicate);
    Task<List<T>> GetFilteredAsync(Expression<Func<T, bool>>? predicate, QueryParameters queryParams);
    Task<T> GetByIdAsync(Guid id);
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(Guid id);
}