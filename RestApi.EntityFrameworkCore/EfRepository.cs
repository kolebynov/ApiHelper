using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using RestApi.Extensions;

namespace RestApi.EntityFrameworkCore
{
    public class EfRepository<TEntity> : IRepository<TEntity> 
        where TEntity : BaseEntity
    {
        private static readonly string ENTITY_NOT_FOUND = "Entity with id {0} not found.";

        protected DbSet<TEntity> DbSet => DbContext.Set<TEntity>();
        protected DbContext DbContext { get; }

        public IQueryable<TEntity> Entities => DbSet;

        public EfRepository(DbContext dbContext)
        {
            DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
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