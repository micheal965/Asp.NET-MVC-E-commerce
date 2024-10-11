using System.Linq.Expressions;
namespace Franshy.DataAccess.Repository.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? Predicate = null, IEnumerable<string>? Includes = null);

        Task<T> GetByIdAsync(Expression<Func<T, bool>>? Predicate = null, IEnumerable<string>? Includes = null);
        Task AddAsync(T entity);
        Task Remove(T entity);
        Task RemoveRange(IEnumerable<T> entity);
    }
}
