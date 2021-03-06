﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RestApi.EntityActions;
using RestApi.EntityFrameworkCore.Resources;
using RestApi.Extensions;
using RestApi.Validating;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RestApi.EntityFrameworkCore
{
    public class EfRepository<TEntity> : IRepository<TEntity> 
        where TEntity : class, IIdentifiable
    {
        public IQueryable<TEntity> Entities => DbSet;

        protected DbSet<TEntity> DbSet { get; }
        protected DbContext DbContext { get; }

        protected IEntityValidator<TEntity> EntityValidator => _entityValidatorLazy.Value;

        protected IEntityActionsHandler<TEntity> ActionsHandler => _actionsHandlerLazy.Value;

        private readonly Lazy<IEntityValidator<TEntity>> _entityValidatorLazy;
        private readonly Lazy<IEntityActionsHandler<TEntity>> _actionsHandlerLazy;

        public EfRepository(DbContext dbContext, IServiceProvider serviceProvider)
        {
            DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            DbSet = DbContext.Set<TEntity>();

            _entityValidatorLazy = new Lazy<IEntityValidator<TEntity>>(() =>
                new GroupedEntityValidators<TEntity>(serviceProvider.GetServices<IEntityValidator<TEntity>>()));
            _actionsHandlerLazy = new Lazy<IEntityActionsHandler<TEntity>>(() =>
                new GroupedEntityActionsHandlers<TEntity>(serviceProvider.GetServices<IEntityActionsHandler<TEntity>>()));
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            entity.CheckArgumentNull(nameof(entity));

            if (entity.Id == Guid.Empty)
            {
                entity.Id = Guid.NewGuid();
            }

            await ValidateEntity(entity);
            await ActionsHandler.OnCreatingAsync(entity);

            TEntity addedEntity = DbSet.Add(entity).Entity;
            await DbContext.SaveChangesAsync();

            await ActionsHandler.OnCreatedAsync(entity);

            return addedEntity;
        }

        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            entity.CheckArgumentNull(nameof(entity));

            await ValidateEntity(entity);
            await ActionsHandler.OnUpdatingAsync(entity);

            DbSet.Attach(entity).State = EntityState.Modified;
            await DbContext.SaveChangesAsync();

            await ActionsHandler.OnUpdatedAsync(entity);

            return entity;
        }

        public async Task DeleteAsync(Guid id)
        {
            TEntity entity = DbSet.Find(id);
            if (entity != null)
            {
                await ActionsHandler.OnDeletingAsync(entity);

                DbSet.Remove(entity);
                await DbContext.SaveChangesAsync();

                await ActionsHandler.OnDeletedAsync(entity);
            }
        }

        public async Task<TEntity> GetByIdAsync(Guid id)
        {
            TEntity entity = await DbSet.FindAsync(id);
            if (entity == null)
            {
                throw new ArgumentException(string.Format(RepositoryResources.EntityNotFound,
                    typeof(TEntity).GetDisplayName(), id));
            }

            return entity;
        }

        private async Task ValidateEntity(TEntity entity)
        {
            ValidationResult validationResult = await EntityValidator.ValidateAsync(entity);

            if (!validationResult.Success)
            {
                throw new ValidationException(validationResult);
            }
        }
    }
}