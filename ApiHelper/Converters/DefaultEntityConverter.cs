using AutoMapper;
using AutoMapper.QueryableExtensions;
using System;
using System.Linq.Expressions;
using RestApi.Infrastructure;

namespace RestApi.Converters
{
    public class DefaultEntityConverter<TEntity, TGetModel, TAddModel, TUpdateModel> : IEntityConverter<TEntity, TGetModel, TAddModel, TUpdateModel>
    {
        private readonly IMapper _mapper;

        public DefaultEntityConverter(MapperProvider mapperProvider)
        {
            _mapper = mapperProvider.Mapper;
        }

        public virtual Expression<Func<TEntity, TGetModel>> GetEntityToGetModelExpression() =>
            _mapper.ConfigurationProvider.ExpressionBuilder.GetMapExpression<TEntity, TGetModel>();

        public virtual TEntity ToEntity(TAddModel model) =>
            _mapper.Map<TAddModel, TEntity>(model);

        public virtual TEntity ToEntity(TUpdateModel model) =>
            _mapper.Map<TUpdateModel, TEntity>(model);
    }

    public class DefaultEntityConverter<TEntity, TModel> : DefaultEntityConverter<TEntity, TModel, TModel, TModel>, IEntityConverter<TEntity, TModel>
    {
        public DefaultEntityConverter(MapperProvider mapperProvider) : base(mapperProvider)
        {
        }
    }
}