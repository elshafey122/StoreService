using System.Collections;
using Store.Core.Entities;
using Store.Core.Repositories.Contract;
using Store.Repository.Data;

namespace Store.Repository;

public class UnitOfWork : IUnitOfWork
{
    private readonly StoreContext _storeContext;
    private Hashtable _repositories;  

    public UnitOfWork( StoreContext storeContext) 
    {
        _storeContext = storeContext;
        _repositories = new Hashtable();
    }

    public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity  
    {
        var key = typeof(TEntity).Name;
        if(!_repositories.ContainsKey(key))
        {
            var repository = new GenericRepository<TEntity>(_storeContext) ;
            _repositories.Add(key, repository);
        }
        return _repositories[key] as IGenericRepository<TEntity>;
    }

    public async Task<int> CompleteAsync()
        => await _storeContext.SaveChangesAsync();

    public async ValueTask DisposeAsync()
    => await _storeContext.DisposeAsync();



}
