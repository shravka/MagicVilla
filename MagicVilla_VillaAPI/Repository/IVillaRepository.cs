using MagicVilla_VillaAPI.Model;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MagicVilla_VillaAPI.Repository
{
    public interface IVillaRepository : IRepository<Villa>
    {
        Task<Villa> UpdateAsync(Villa villa);
    }
}
