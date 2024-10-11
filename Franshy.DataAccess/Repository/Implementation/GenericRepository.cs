using Franshy.DataAccess.Data;
using Franshy.DataAccess.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Franshy.DataAccess.Repository.Implementation
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        private DbSet<T> _dbSet;
        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }
        public async Task AddAsync(T entity) => await _dbSet.AddAsync(entity);

        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? Predicate, IEnumerable<string>? Includes)
        {
            IQueryable<T> query = _dbSet;
            if (Predicate != null)
            {
                query = query.Where(Predicate);
            }
            if (Includes != null)
            {
                foreach (var include in Includes)
                {
                    query = query.Include(include);
                }
            }
            return await query.ToListAsync();

        }
        public async Task<T> GetByIdAsync(Expression<Func<T, bool>>? Predicate, IEnumerable<string>? Includes)
        {
            IQueryable<T> query = _dbSet;
            if (Predicate != null)
            {
                query = query.Where(Predicate);
            }
            if (Includes != null)
            {
                foreach (var include in Includes)
                {
                    query = query.Include(include);
                }
            }
            return await query.SingleOrDefaultAsync();


        }
        public async Task Remove(T entity) => _dbSet.Remove(entity);

        public async Task RemoveRange(IEnumerable<T> entity)
        {
            _dbSet.RemoveRange(entity);
        }
    }
}
