using MagicVilla_VillaAPI.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MagicVilla_VillaAPI.Repository
{
    public class Repository<T>  : IRepository<T> where T :class
    {
        private readonly ApplicationDBContext _dbContext;

        internal DbSet<T> _dbSet;
        public Repository(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
          //  _dbContext.VillaNumber.Include(u => u.villa).ToList();
            this._dbSet = _dbContext.Set<T>();
        }
  
        public async Task<List<T>> GetAsyncAll(Expression<Func<T, bool>> filter = null, string? includeProperties=null)
        {
            IQueryable<T> query = _dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includeProperties != null)
            {
                foreach (var property in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query=query.Include(property);
                }
            }

            return await query.ToListAsync();
        }

        public async Task<T> GetAsyncVilla(Expression<Func<T, bool>> filter, bool tracked = false, string? includeProperties = null)
        {
            IQueryable<T> query = _dbSet;
            if (tracked)
                query = query.AsNoTracking();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            //if (includeProperties != null)
            //{
            //    foreach (var property in includeProperties.Split(',', StringSplitOptions.RemoveEmptyEntries))
            //    {
            //        query.Include<T>(property);
            //    }
            //}
            return await query.FirstOrDefaultAsync();
        }
        public async Task CreateAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await SaveAsync();
        }

        public async Task RemoveAsync(T entity)
        {
            _dbSet.Remove(entity);
            await SaveAsync();
        }

        public async Task SaveAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
