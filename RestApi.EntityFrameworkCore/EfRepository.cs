using Microsoft.EntityFrameworkCore;
using RestApi.Extensions;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RestApi.EntityFrameworkCore
{
    public class EfRepository<TEntity> : IRepository<TEntity> 
        where TEntity : class, IIdentifiable
    {
        protected DbSet<TEntity> DbSet { get; }
        protected DbContext DbContext { get; }

        public IQueryable<TEntity> Entities => DbSet;

        public EfRepository(DbContext dbContext)
        {
            DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            DbSet = DbContext.Set<TEntity>();
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            entity.CheckArgumentNull(nameof(entity));

            if (entity.Id == Guid.Empty)
            {
                entity.Id = Guid.NewGuid();
            }

            TEntity addedEntity = DbSet.Add(entity).Entity;
            await DbContext.SaveChangesAsync();
            return addedEntity;
        }

        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            entity.CheckArgumentNull(nameof(entity));

            DbSet.Attach(entity).State = EntityState.Modified;
            await DbContext.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(Guid id)
        {
            TEntity entity = DbSet.Find(id);
            if (entity != null)
            {
                DbSet.Remove(entity);
                await DbContext.SaveChangesAsync();
            }
        }

        public async Task<TEntity> GetByIdAsync(Guid id) =>
            await DbSet.FindAsync(id);
    }
}