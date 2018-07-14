using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RestApi;

namespace TestRestApi
{
    public class TestRepository<TEntity> : IRepository<TEntity>
        where TEntity : BaseEntity
    {
        private readonly List<TEntity> _entities = new List<TEntity>();

        public IQueryable<TEntity> Entities => _entities.AsQueryable();

        public Task<TEntity> AddAsync(TEntity entity)
        {
            entity.Id = Guid.NewGuid();
            _entities.Add(entity);
            return Task.FromResult(entity);
        }

        public Task<TEntity> UpdateAsync(TEntity entity)
        {
            int index = _entities.FindIndex(e => e.Id == entity.Id);
            if (index > -1)
            {
                _entities[index] = entity;
                return Task.FromResult(entity);
            }

            throw new ArgumentException();
        }

        public Task DeleteAsync(Guid id)
        {
            _entities.RemoveAll(e => e.Id == id);
            return Task.CompletedTask;
        }

        public Task<TEntity> GetByIdAsync(Guid id)
        {
            return Task.FromResult(_entities.Find(e => e.Id == id));
        }
    }
}
