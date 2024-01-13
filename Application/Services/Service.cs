using Application.Services.Interfaces;
using Domain.Common;
using Domain.Interfaces;
using System.Linq.Expressions;

namespace Application.Services;
public class Service<T> : IService<T> where T : BaseEntity
{
    private readonly IRepository<T> _repository;

    public Service(IRepository<T> repository)
    {
        _repository = repository;
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task<T?> GetByIdWithIncludesAsync(int id, params Expression<Func<T, object>>[] includes)
    {
        return await _repository.GetByIdWithIncludesAsync(id, includes);
    }
    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<IEnumerable<T>> GetAllWithIncludesAsync(params Expression<Func<T, object>>[] includes)
    {
        return await _repository.GetAllWithIncludesAsync(includes);
    }

    public async Task<IEnumerable<T>> GetByConditionAsync(Expression<Func<T, bool>> predicate)
    {
        return await _repository.GetByConditionAsync(predicate);
    }

    public Task<IEnumerable<T>> GetByConditionWithIncludesAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
    {
        return _repository.GetByConditionWithIncludesAsync(predicate, includes);
    }

    public async Task<T> AddAsync(T entity)
    {
        var addedEntity = _repository.Add(entity);
        await _repository.SaveChangesAsync();
        return addedEntity;
    }

    public async Task AddRangeAsync(IEnumerable<T> entities)
    {
        _repository.AddRange(entities);
        await _repository.SaveChangesAsync();
    }

    public async Task DeleteAsync(T entity)
    {
        _repository.Delete(entity);
        await _repository.SaveChangesAsync();
    }

    public async Task DeleteRangeAsync(IEnumerable<T> entities)
    {
        _repository.DeleteRange(entities);
        await _repository.SaveChangesAsync();
    }

    public async Task UpdateAsync(T entity)
    {
        _repository.Update(entity);
        await _repository.SaveChangesAsync();
    }

    public async Task UpdateRangeAsync(IEnumerable<T> entities)
    {
        _repository.UpdateRange(entities);
        await _repository.SaveChangesAsync();
    }
}
