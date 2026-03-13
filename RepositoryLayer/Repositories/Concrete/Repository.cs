using EntityLayer.Entites.Common;
using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Context;
using RepositoryLayer.Pagination;
using RepositoryLayer.Repositories.Abstraction;
using System.Linq.Expressions;

namespace RepositoryLayer.Repositories.Concrete;

public class Repository<T> : IRepository<T> where T : class, IEntity
{
    private readonly SmartWarehouseManagementSystemDbContext _context;
    private readonly DbSet<T> _dbSet;

    public Repository(SmartWarehouseManagementSystemDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }

    private static IQueryable<T> ApplyPredicates(IQueryable<T> query, IEnumerable<Expression<Func<T, bool>>>? predicates)
    {
        if (predicates is null)
        {
            return query;
        }

        foreach (var predicate in predicates)
        {
            query = query.Where(predicate);
        }

        return query;
    }

    public async Task<bool> AddAsync(T entity)
    {
        var entityEntry = await _dbSet.AddAsync(entity);
        return entityEntry.State == EntityState.Added;
    }

    public async Task<int> CountAsync(IEnumerable<Expression<Func<T, bool>>>? predicates = null)
    {
        var query = ApplyPredicates(_dbSet.AsQueryable(), predicates);
        return await query.CountAsync();
    }

    public async Task<bool> DeleteAsync(T entity)
    {
        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;
        entity.UpdatedAt = DateTime.UtcNow;
        var entityEntry = _dbSet.Update(entity);

        return entityEntry.State == EntityState.Modified;
    }

    public async Task DeleteRangeAsync(IEnumerable<Guid> deleteIds)
    {
        var entities = await _dbSet.Where(x => deleteIds.Contains(x.Id)).ToListAsync();
        if (entities.Count == 0)
            return;

        var now = DateTime.UtcNow;
        foreach (var entity in entities)
        {
            entity.IsDeleted = true;
            entity.DeletedAt = now;
            entity.UpdatedAt = now;
        }

        _dbSet.UpdateRange(entities);
    }

    public async Task<bool> ExistAsync(IEnumerable<Expression<Func<T, bool>>> predicates)
    {
        var query = ApplyPredicates(_dbSet.AsQueryable(), predicates);
        return await query.AnyAsync();
    }

    public async Task<T?> FirstOrDefault(IEnumerable<Expression<Func<T, bool>>> predicates)
    {
        var query = ApplyPredicates(_dbSet.AsQueryable(), predicates);
        return await query.FirstOrDefaultAsync();
    }

    public async Task<TResult?> FirstOrDefault<TResult>(IEnumerable<Expression<Func<T, bool>>> predicates, Expression<Func<T, TResult>> selector)
    {
        var query = ApplyPredicates(_dbSet.AsQueryable(), predicates);
        return await query.Select(selector).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task<IEnumerable<TResult>> GetAllAsync<TResult>(Expression<Func<T, TResult>> selector)
    {
        return await _dbSet.Select(selector).ToListAsync();
    }

    public async Task<IEnumerable<T>> GetByFiltersAsync(IEnumerable<Expression<Func<T, bool>>> predicates)
    {
        var query = ApplyPredicates(_dbSet.AsQueryable(), predicates);
        return await query.ToListAsync();
    }

    public async Task<IEnumerable<TResult>> GetByFiltersAsync<TResult>(IEnumerable<Expression<Func<T, bool>>> predicates, Expression<Func<T, TResult>> selector)
    {
        var query = ApplyPredicates(_dbSet.AsQueryable(), predicates);
        return await query.Select(selector).ToListAsync();
    }

    public async Task<T> GetByIdAsync(Guid id)
    {
        var entity = await _dbSet.FirstOrDefaultAsync(x => x.Id == id);
        if (entity is null)
        {
            throw new KeyNotFoundException($"{typeof(T).Name} with id '{id}' was not found.");
        }

        return entity;
    }

    public async Task<TResult> GetByIdAsync<TResult>(Guid id, Expression<Func<T, TResult>> selector)
    {
        var projected = await _dbSet.Where(x => x.Id == id).Select(selector).FirstOrDefaultAsync();
        if (projected is null)
        {
            throw new KeyNotFoundException($"{typeof(T).Name} with id '{id}' was not found.");
        }

        return projected;
    }

    public async Task<PagedDataListModel<T>> GetPagedDataListAsync(int pageNumber, int pageSize, IEnumerable<Expression<Func<T, bool>>>? predicates = null)
    {
        var query = ApplyPredicates(_dbSet.AsQueryable(), predicates);

        var totalCount = await query.CountAsync();
        var datas = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedDataListModel<T>(totalCount, pageNumber, pageSize, datas);
    }

    public async Task<PagedDataListModel<TResult>> GetPagedDataListAsync<TResult>(int pageNumber, int pageSize, Expression<Func<T, TResult>> selector, IEnumerable<Expression<Func<T, bool>>>? predicates = null)
    {
        var query = ApplyPredicates(_dbSet.AsQueryable(), predicates);

        var totalCount = await query.CountAsync();
        var datas = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(selector)
            .ToListAsync();

        return new PagedDataListModel<TResult>(totalCount, pageNumber, pageSize, datas);
    }

    public async Task<bool> UpdateAsync(T entity)
    {
        entity.UpdatedAt = DateTime.UtcNow;
        var entityEntry = _dbSet.Update(entity);
        return entityEntry.State == EntityState.Modified;
    }
}
