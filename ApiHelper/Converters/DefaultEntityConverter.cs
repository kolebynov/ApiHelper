using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using AutoMapper.QueryableExtensions;

namespace RestApi.Converters
{
    public class DefaultEntityConverter<TEntity, TModel> : IEntityConverter<TEntity, TModel>
    {
        public Expression<Func<TModel, TEntity>> GetModelToEntityExpression()
        {
            return Mapper.Configuration.ExpressionBuilder.GetMapExpression<TModel, TEntity>();
        }

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