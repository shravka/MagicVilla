using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Model;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MagicVilla_VillaAPI.Repository
{
    public class VillaRepository : Repository<Villa>, IVillaRepository
    {
        private readonly ApplicationDBContext _dbContext;
        public VillaRepository(ApplicationDBContext dbContext) : base(dbContext)
        {

            _dbContext = dbContext;
        }
        public async Task<Villa> UpdateAsync(Villa villa)
        {
            villa.CreatedDate = DateTime.Now;
            _dbContext.Update(villa);
            await _dbContext.SaveChangesAsync();
            return villa;
        }

    }
}
