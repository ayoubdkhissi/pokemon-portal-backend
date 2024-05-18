using Domain.Common;

namespace Domain.Interfaces;
public interface IRepositoryFactory
{
    IRepository<T> CreateRepository<T>() where T : BaseEntity;
}
