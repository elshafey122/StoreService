using Microsoft.EntityFrameworkCore;
using Store.Core.Repositories.Contract;
using Store.Repository.Data;
using Store.Core.Entities;
using Store.Core.Specifications;

namespace Store.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly StoreContext _dbContext;

        public GenericRepository(StoreContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            return await _dbContext.Set<T>().ToListAsync();
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }

        public async Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecifications<T> specs)
        {
            return await ApplySpecifications(specs).ToListAsync();

        }

        public async Task<T?> GetEntityWithSpecAsync(ISpecifications<T> specs)
        {
            return await ApplySpecifications(specs).FirstOrDefaultAsync();
        }

        public async Task<int> GetCountAsync(ISpecifications<T> specs)
        {
            return await ApplySpecifications(specs).CountAsync();
        }

        private IQueryable<T> ApplySpecifications(ISpecifications<T> specs)
        {
            return SpecificationsEvaluator<T>.GetQuery(_dbContext.Set<T>(), specs);
        }

        public async Task AddAsync(T entity)
       => await _dbContext.Set<T>().AddAsync(entity);

        public void Update(T entity)
        => _dbContext.Set<T>().Update(entity);

        public void Delete(T entity)
        => _dbContext.Set<T>().Remove(entity);
    }
}
