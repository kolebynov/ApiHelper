using System;
using System.Linq.Expressions;

namespace RestApi.Converters
{
    public interface IEntityConverter<TEntity, TModel>
    {
        Expression<Func<TModel, TEntity>> GetModelToEntityExpression();
        Expression<Func<TEntity, TModel>> GetEntityToModelExpression();
        TModel ToModel(TEntity entity);
        TEntity ToEntity(TModel model);
    }
}