using Application.Services.Interfaces;
using Domain.Common;
using Domain.Interfaces;
using System.Linq.Expressions;

namespace Application.Services;
public class Service<T> : IService<T> where T : BaseEntity
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRepository<T> _repository;

    public Service(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _repository = _unitOfWork.Repository<T>();
    }

    public virtual async Task<T?> GetByIdAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public virtual async Task<T?> GetByIdWithIncludesAsync(int id, params Expression<Func<T, object>>[] includes)
    {
        return await _repository.GetByIdWithIncludesAsync(id, includes);
    }
    public virtual async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }

    public virtual async Task<IEnumerable<T>> GetAllWithIncludesAsync(params Expression<Func<T, object>>[] includes)
    {
        return await _repository.GetAllWithIncludesAsync(includes);
    }

    public virtual async Task<IEnumerable<T>> GetByConditionAsync(Expression<Func<T, bool>> predicate)
    {
        return await _repository.GetByConditionAsync(predicate);
    }

    public virtual Task<IEnumerable<T>> GetByConditionWithIncludesAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
    {
        return _repository.GetByConditionWithIncludesAsync(predicate, includes);
    }

    public virtual async Task<T> AddAsync(T entity)
    {
        var addedEntity = _repository.Add(entity);
        await _unitOfWork.SaveChangesAsync();
        return addedEntity;
    }

    public virtual async Task AddRangeAsync(IEnumerable<T> entities)
    {
        _repository.AddRange(entities);
        await _unitOfWork.SaveChangesAsync();
    }

    public virtual async Task DeleteAsync(T entity)
    {
        _repository.Delete(entity);
        await _unitOfWork.SaveChangesAsync();
    }

    public virtual async Task DeleteRangeAsync(IEnumerable<T> entities)
    {
        _repository.DeleteRange(entities);
        await _unitOfWork.SaveChangesAsync();
    }

    public virtual async Task UpdateAsync(T entity)
    {
        _repository.Update(entity);
        await _unitOfWork.SaveChangesAsync();
    }

    public virtual async Task UpdateRangeAsync(IEnumerable<T> entities)
    {
        _repository.UpdateRange(entities);
        await _unitOfWork.SaveChangesAsync();
    }
}
