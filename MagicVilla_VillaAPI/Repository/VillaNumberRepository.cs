using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Model;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MagicVilla_VillaAPI.Repository
{
    public class VillaNumberRepository : Repository<VillaNumber>,IVillaNumberRepository
    {
        private readonly ApplicationDBContext _dbContext;
        public VillaNumberRepository(ApplicationDBContext dbContext) : base(dbContext)
        {

            _dbContext = dbContext;
        }

        public async Task<VillaNumber> UpdateAsync(VillaNumber villa)
        {
            villa.CreatedDate = DateTime.Now;
            _dbContext.Update(villa);
            await _dbContext.SaveChangesAsync();
            return villa;
        }

    }
}
