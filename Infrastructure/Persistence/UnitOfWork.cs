using Domain.Common;
using Domain.Interfaces;

namespace Infrastructure.Persistence;
public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    private Dictionary<Type, object> _repositories;
    private readonly IRepositoryFactory _repositoryFactory;
    private bool _disposed;

    public UnitOfWork(AppDbContext context, IRepositoryFactory repositoryFactory)
    {
        _context = context;
        _repositoryFactory = repositoryFactory;
        _repositories = [];
    }

    public IRepository<T> Repository<T>() where T : BaseEntity
    {
        if (_repositories.ContainsKey(typeof(T)))
            return (_repositories[typeof(T)] as IRepository<T>)!;

        var repo = _repositoryFactory.CreateRepository<T>();
        _repositories.Add(typeof(T), repo);
        return repo;
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync().ConfigureAwait(false);
    }
    
    protected virtual void Dispose(bool disposing)
    {
        if(!_disposed)
        {
            if(disposing)
            {
                _context.Dispose();
                _repositories.Clear();
            }
            _repositories = null!;
            _disposed = true;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
