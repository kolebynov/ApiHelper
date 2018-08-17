using System.Threading.Tasks;

namespace RestApi.EntityActions
{
    public class DefaultEntityActionsHandler<TEntity> : IEntityActionsHandler<TEntity>
    {
        public virtual Task OnCreatingAsync(TEntity entity) => Task.CompletedTask;

        public virtual Task OnCreatedAsync(TEntity entity) => Task.CompletedTask;

        public virtual Task OnUpdatingAsync(TEntity entity) => Task.CompletedTask;

        public virtual Task OnUpdatedAsync(TEntity entity) => Task.CompletedTask;

        public virtual Task OnDeletingAsync(TEntity entity) => Task.CompletedTask;

        public virtual Task OnDeletedAsync(TEntity entity) => Task.CompletedTask;
    }
}