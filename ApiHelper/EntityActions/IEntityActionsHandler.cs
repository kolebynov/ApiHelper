using System.Threading.Tasks;

namespace RestApi.EntityActions
{
    public interface IEntityActionsHandler<in TEntity>
    {
        Task OnCreatingAsync(TEntity entity);
        Task OnCreatedAsync(TEntity entity);
        Task OnUpdatingAsync(TEntity entity);
        Task OnUpdatedAsync(TEntity entity);
        Task OnDeletingAsync(TEntity entity);
        Task OnDeletedAsync(TEntity entity);
    }
}