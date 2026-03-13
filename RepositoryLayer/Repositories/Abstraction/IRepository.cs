using EntityLayer.Entites.Common;
using System.Linq.Expressions;

namespace RepositoryLayer.Repositories.Abstraction;

public interface IRepository<T> where T : BaseEntity
{
    // Non-projected
    Task<IEnumerable<T>> GetAllAsync();
    Task<T> GetByIdAsync(Guid id);
    Task<IEnumerable<T>> GetByFiltersAsync(IEnumerable<Expression<Func<T, bool>>> predicates);

    // Projected
    Task<IEnumerable<TResult>> GetAllAsync<TResult>(Expression<Func<T, TResult>> selector);
    Task<TResult> GetByIdAsync<TResult>(Guid id, Expression<Func<T, TResult>> selector);
    Task<IEnumerable<TResult>> GetByFiltersAsync<TResult>(IEnumerable<Expression<Func<T, bool>>> predicates, Expression<Func<T, TResult>> selector);

}
