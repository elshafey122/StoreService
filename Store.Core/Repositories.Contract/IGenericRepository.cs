using Store.Core.Entities;
using Store.Core.Specifications;

namespace Store.Core.Repositories.Contract
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<T?> GetByIdAsync(int id);
        Task<IReadOnlyList<T>> GetAllAsync();

        Task<T?> GetEntityWithSpecAsync(ISpecifications<T> specs);

        Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecifications<T> specs);

        Task <int> GetCountAsync(ISpecifications<T> specs);

        Task AddAsync(T entity);

        void Update(T entity);

        void Delete(T entity);

    }
}
 