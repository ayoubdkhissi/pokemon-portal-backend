using Domain.Common;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Persistence.Repositories;
using Infrastructure.Repositories;

namespace Infrastructure.Persistence;
public class RepositoryFactory : IRepositoryFactory
{
    private readonly AppDbContext _context;

    public RepositoryFactory(AppDbContext context)
    {
        _context = context;
    }

    public IRepository<T> CreateRepository<T>() where T : BaseEntity
    {
        if (typeof(T) == typeof(Pokemon))
        {
            return (IRepository<T>)new PokemonRepository(_context);
        }
        return new Repository<T>(_context);
    }
}
