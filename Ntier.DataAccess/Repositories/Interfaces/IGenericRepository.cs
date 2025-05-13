using System.Linq.Expressions;
using Ntier.Shared.Models;

namespace Ntier.DataAccess.Repository.Interfaces;

public interface IGenericRepository<T> where T : BaseEntity
{
    IQueryable<T> GetAll(CancellationToken cancellationToken);
    IQueryable<T> Find(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken);
    Task<List<T>> GetFilteredAsync(Expression<Func<T, bool>>? predicate, QueryParameters queryParams, CancellationToken cancellationToken);
    Task<T> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task AddAsync(T entity, CancellationToken cancellationToken);
    Task UpdateAsync(T entity, CancellationToken cancellationToken);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken);
}