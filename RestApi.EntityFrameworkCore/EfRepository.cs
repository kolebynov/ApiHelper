using Microsoft.EntityFrameworkCore;
using RestApi.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RestApi.EntityActions;
using RestApi.Validating;

namespace RestApi.EntityFrameworkCore
{
    public class EfRepository<TEntity> : IRepository<TEntity> 
        where TEntity : class, IIdentifiable
    {
        public IQueryable<TEntity> Entities => DbSet;

        protected DbSet<TEntity> DbSet { get; }
        protected DbContext DbContext { get; }

        private readonly IEntityValidator<TEntity> _validator;
        private readonly IEntityActionsHandler<TEntity> _actionsHandler;

        public EfRepository(DbContext dbContext, IEnumerable<IEntityValidator<TEntity>> validators, IEnumerable<IEntityActionsHandler<TEntity>> actionsHandlers)
        {
            DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            DbSet = DbContext.Set<TEntity>();
            _validator = new GroupedEntityValidators<TEntity>(validators ?? new IEntityValidator<TEntity>[0]);
            _actionsHandler = new GroupedEntityActionsHandlers<TEntity>(actionsHandlers ?? new IEntityActionsHandler<TEntity>[0]);
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            entity.CheckArgumentNull(nameof(entity));

            if (entity.Id == Guid.Empty)
            {
                entity.Id = Guid.NewGuid();
            }

            await ValidateEntity(entity);
            await _actionsHandler.OnCreatingAsync(entity);

            TEntity addedEntity = DbSet.Add(entity).Entity;
            await DbContext.SaveChangesAsync();

            await _actionsHandler.OnCreatedAsync(entity);

            return addedEntity;
        }

        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            entity.CheckArgumentNull(nameof(entity));

            await ValidateEntity(entity);
            await _actionsHandler.OnUpdatingAsync(entity);

            DbSet.Attach(entity).State = EntityState.Modified;
            await DbContext.SaveChangesAsync();

            await _actionsHandler.OnUpdatedAsync(entity);

            return entity;
        }

        public async Task DeleteAsync(Guid id)
        {
            TEntity entity = DbSet.Find(id);
            if (entity != null)
            {
                await _actionsHandler.OnDeletingAsync(entity);

                DbSet.Remove(entity);
                await DbContext.SaveChangesAsync();

                await _actionsHandler.OnDeletedAsync(entity);
            }
        }

        public async Task<TEntity> GetByIdAsync(Guid id) =>
            await DbSet.FindAsync(id);

        private async Task ValidateEntity(TEntity entity)
        {
            ValidationResult validationResult = await _validator.ValidateAsync(entity);
            if (!validationResult.Success)
            {
                throw new EntityValidationException(validationResult);
            }
        }
    }
}