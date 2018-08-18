using System;
using System.Linq.Expressions;

namespace RestApi.Converters
{
    public interface IEntityConverter<TEntity, TGetModel, in TAddModel, in TUpdateModel>
    {
        Expression<Func<TEntity, TGetModel>> GetEntityToGetModelExpression();
        TEntity ToEntity(TAddModel model);
        TEntity ToEntity(TUpdateModel model);
    }

    public interface IEntityConverter<TEntity, TModel> : IEntityConverter<TEntity, TModel, TModel, TModel>
    { }
}