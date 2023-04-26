using MagicVilla_VillaAPI.Model;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MagicVilla_VillaAPI.Repository
{
    public interface IRepository 
    {
        Task<List<Villa>> GetAsyncAll(Expression<Func<Villa,bool>> filter=null);

        Task<Villa> GetAsyncVilla(Expression<Func<Villa,bool>> filter = null,bool tracked=false);

        Task CreateAsync(Villa villa);

        Task RemoveAsync(Villa villa);

        Task UpdateAsync(Villa villa);

        Task SaveAsync();

       
    }
}
