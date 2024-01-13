using Domain.Common;
using System.Linq.Expressions;

namespace Application.Services.Interfaces;
public interface IService<T> where T : BaseEntity
{
    Task<T?> GetByIdAsync(int id);
    Task<T?> GetByIdWithIncludesAsync(int id, params Expression<Func<T, object>>[] includes);
    Task<IEnumerable<T>> GetAllAsync();
    Task<IEnumerable<T>> GetAllWithIncludesAsync(params Expression<Func<T, object>>[] includes);
    Task<IEnumerable<T>> GetByConditionAsync(Expression<Func<T, bool>> predicate);
    Task<IEnumerable<T>> GetByConditionWithIncludesAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);
    Task<T> AddAsync(T entity);
    Task AddRangeAsync(IEnumerable<T> entities);
    Task UpdateAsync(T entity);
    Task UpdateRangeAsync(IEnumerable<T> entities);
    Task DeleteAsync(T entity);
    Task DeleteRangeAsync(IEnumerable<T> entities);
}
