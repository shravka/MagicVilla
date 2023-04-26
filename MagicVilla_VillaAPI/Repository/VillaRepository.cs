using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Model;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MagicVilla_VillaAPI.Repository
{
    public class VillaRepository :IRepository
    {
        private readonly ApplicationDBContext _dbContext;
        public VillaRepository(ApplicationDBContext dbContext) {

            _dbContext = dbContext;
        }

        public async  Task<List<Villa>> GetAsyncAll(Expression<Func<Villa,bool>> filter=null)
        {
           IQueryable<Villa> query = _dbContext.Villas;
           if (filter!=null)
            {
                query = query.Where(filter);
            }
            return await query.ToListAsync() ;
        }

        public async Task<Villa> GetAsyncVilla(Expression<Func<Villa, bool>> filter, bool tracked=false)
        {
            IQueryable<Villa> query = _dbContext.Villas;
            if (tracked)
                query = query.AsNoTracking();

            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.FirstOrDefaultAsync();
        }
        public async Task CreateAsync(Villa villa)
        {
           await _dbContext.AddAsync(villa);
           await SaveAsync();
        }

        public async Task RemoveAsync(Villa villa)
        {
            _dbContext.Remove(villa);
            await SaveAsync();
        }

        public async Task UpdateAsync(Villa villa)
        {
            _dbContext.Update(villa);
            await SaveAsync();
        }

        public async Task SaveAsync()
        {
            await _dbContext.SaveChangesAsync();
        }       
    }
}
