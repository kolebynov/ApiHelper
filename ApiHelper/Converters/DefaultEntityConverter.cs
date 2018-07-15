using AutoMapper;
using AutoMapper.QueryableExtensions;
using System;
using System.Linq.Expressions;

namespace RestApi.Converters
{
    public class DefaultEntityConverter<TEntity, TModel> : IEntityConverter<TEntity, TModel>
    {
        public Expression<Func<TEntity, TModel>> GetEntityToModelExpression()
        {
            return Mapper.Configuration.ExpressionBuilder.GetMapExpression<TEntity, TModel>();
        }

        public TModel ToModel(TEntity entity)
        {
            return Mapper.Map<TEntity, TModel>(entity);
        }

        public TEntity ToEntity(TModel model)
        {
            return Mapper.Map<TModel, TEntity>(model);
        }
    }
}