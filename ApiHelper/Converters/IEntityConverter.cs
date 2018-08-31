using System;
using System.Linq.Expressions;

namespace RestApi.Converters
{
    public interface IEntityConverter<TEntity, TGetModel, TGetSingleModel, in TAddModel, in TUpdateModel>
        where TEntity : IIdentifiable
    {
        Expression<Func<TEntity, TGetModel>> GetEntityToGetModelExpression();
        Expression<Func<TEntity, TGetSingleModel>> GetEntityToGetSingleModelExpression();
        TEntity ToEntity(TAddModel model);
        TEntity ToEntity(TUpdateModel model, Guid id);
        TGetModel ToGetModel(TEntity entity);
    }
}