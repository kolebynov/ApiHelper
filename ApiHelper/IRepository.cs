using System;
using System.Linq;
using System.Threading.Tasks;

namespace RestApi
{
    public interface IRepository<TEntity> where TEntity : class, IIdentifiable
    {
        IQueryable<TEntity> Entities { get; }

        Task<TEntity> AddAsync(TEntity entity);
        Task<TEntity> UpdateAsync(TEntity entity);
        Task DeleteAsync(Guid id);
        Task<TEntity> GetByIdAsync(Guid id);
    }
}