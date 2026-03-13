using EntityLayer.Entites.Common;
using RepositoryLayer.Pagination;
using System.Linq.Expressions;

namespace RepositoryLayer.Repositories.Abstraction;

public interface IRepository<T> where T : IEntity
{
    // Non-projected
    Task<IEnumerable<T>> GetAllAsync();
    Task<T> GetByIdAsync(Guid id);
    Task<T?> FirstOrDefault(IEnumerable<Expression<Func<T, bool>>> predicates);
    Task<IEnumerable<T>> GetByFiltersAsync(IEnumerable<Expression<Func<T, bool>>> predicates);
    Task<PagedDataListModel<T>> GetPagedDataListAsync(int pageNumber, int pageSize, IEnumerable<Expression<Func<T, bool>>>? predicates = null);

    // Projected
    Task<IEnumerable<TResult>> GetAllAsync<TResult>(Expression<Func<T, TResult>> selector);
    Task<TResult> GetByIdAsync<TResult>(Guid id, Expression<Func<T, TResult>> selector);
    Task<IEnumerable<TResult>> GetByFiltersAsync<TResult>(IEnumerable<Expression<Func<T, bool>>> predicates, Expression<Func<T, TResult>> selector);
    Task<PagedDataListModel<TResult>> GetPagedDataListAsync<TResult>(int pageNumber, int pageSize, Expression<Func<T, TResult>> selector, IEnumerable<Expression<Func<T, bool>>>? predicates = null);
    Task<TResult?> FirstOrDefault<TResult>(IEnumerable<Expression<Func<T, bool>>> predicates, Expression<Func<T, TResult>> selector);
    
    Task<int> CountAsync(IEnumerable<Expression<Func<T, bool>>>? predicates = null);
    Task<bool> ExistAsync(IEnumerable<Expression<Func<T, bool>>> predicates);

    Task<bool> AddAsync(T entity);
    Task<bool> UpdateAsync(T entity);
    Task<bool> DeleteAsync(T entity);
    Task<bool> DeleteRangeAsync(IEnumerable<Guid> deleteIds);
}
