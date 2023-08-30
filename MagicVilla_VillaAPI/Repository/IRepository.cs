using MagicVilla_VillaAPI.Model;
using System.Linq.Expressions;
using System.Collections.Generic;

namespace MagicVilla_VillaAPI.Repository
{
    public interface IRepository<T> where T : class
    {
        Task<List<T>> GetAsyncAll(Expression<Func<T,bool>> filter = null,string? includeProperties=null);

        Task<T> GetAsyncVilla(Expression<Func<T, bool>> filter = null, bool tracked = false, string? includeProperties = null);

        Task CreateAsync(T entity);

        Task RemoveAsync(T entity);

        //Task<T> UpdateAsync(T entity);

        Task SaveAsync();
    }
}
