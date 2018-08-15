using AutoMapper;
using AutoMapper.QueryableExtensions;
using System;
using System.Linq.Expressions;
using RestApi.Infrastructure;

namespace RestApi.Converters
{
    public class DefaultEntityConverter<TEntity, TModel> : IEntityConverter<TEntity, TModel>
    {
        private readonly IMapper _mapper;

        public DefaultEntityConverter(MapperProvider mapperProvider)
        {
            _mapper = mapperProvider.Mapper;
        }

        public Expression<Func<TEntity, TModel>> GetEntityToModelExpression()
        {
            return _mapper.ConfigurationProvider.ExpressionBuilder.GetMapExpression<TEntity, TModel>();
        }

        public TModel ToModel(TEntity entity)
        {
            return _mapper.Map<TEntity, TModel>(entity);
        }

        public TEntity ToEntity(TModel model)
        {
            return _mapper.Map<TModel, TEntity>(model);
        }
    }
}