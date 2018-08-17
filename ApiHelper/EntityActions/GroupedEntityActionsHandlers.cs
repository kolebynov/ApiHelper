using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestApi.EntityActions
{
    public class GroupedEntityActionsHandlers<TEnttiy> : IEntityActionsHandler<TEnttiy>
    {
        private readonly IEnumerable<IEntityActionsHandler<TEnttiy>> _actionsHandlers;

        public GroupedEntityActionsHandlers(IEnumerable<IEntityActionsHandler<TEnttiy>> actionsHandlers)
        {
            _actionsHandlers = actionsHandlers ?? throw new ArgumentNullException(nameof(actionsHandlers));
        }

        public async Task OnCreatingAsync(TEnttiy entity)
        {
            foreach (var actionsHandler in _actionsHandlers)
            {
                await actionsHandler.OnCreatingAsync(entity);
            }
        }

        public async Task OnCreatedAsync(TEnttiy entity)
        {
            foreach (var actionsHandler in _actionsHandlers)
            {
                await actionsHandler.OnCreatedAsync(entity);
            }
        }

        public async Task OnUpdatingAsync(TEnttiy entity)
        {
            foreach (var actionsHandler in _actionsHandlers)
            {
                await actionsHandler.OnUpdatingAsync(entity);
            }
        }

        public async Task OnUpdatedAsync(TEnttiy entity)
        {
            foreach (var actionsHandler in _actionsHandlers)
            {
                await actionsHandler.OnUpdatedAsync(entity);
            }
        }

        public async Task OnDeletingAsync(TEnttiy entity)
        {
            foreach (var actionsHandler in _actionsHandlers)
            {
                await actionsHandler.OnDeletingAsync(entity);
            }
        }

        public async Task OnDeletedAsync(TEnttiy entity)
        {
            foreach (var actionsHandler in _actionsHandlers)
            {
                await actionsHandler.OnDeletedAsync(entity);
            }
        }
    }
}
