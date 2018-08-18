﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using RestApi.Extensions;
using RestApi.Infrastructure;

namespace RestApi.Converters
{
    public class DefaultEntityConverter<TEntity, TGetModel, TAddModel, TUpdateModel> : IEntityConverter<TEntity, TGetModel, TAddModel, TUpdateModel>
        where TEntity : IIdentifiable
    {
        private readonly IMapper _mapper;
        private readonly IQueryable<TEntity> _entities;

        public DefaultEntityConverter(MapperProvider mapperProvider, IQueryable<TEntity> entities)
        {
            _entities = entities ?? throw new ArgumentNullException(nameof(entities));
            mapperProvider.CheckArgumentNull(nameof(mapperProvider));
            _mapper = mapperProvider.Mapper;
        }

        public virtual Expression<Func<TEntity, TGetModel>> GetEntityToGetModelExpression() =>
            _mapper.ConfigurationProvider.ExpressionBuilder.GetMapExpression<TEntity, TGetModel>();

        public virtual TEntity ToEntity(TAddModel model) =>
            _mapper.Map<TAddModel, TEntity>(model);

        public virtual TEntity ToEntity(TUpdateModel model, Guid id) =>
            _mapper.Map(model, _entities.First(x => x.Id == id));

        public TGetModel ToGetModel(TEntity entity) =>
            _mapper.Map<TEntity, TGetModel>(entity);
    }

    public class DefaultEntityConverter<TEntity, TModel> : DefaultEntityConverter<TEntity, TModel, TModel, TModel>, IEntityConverter<TEntity, TModel>
        where TEntity : IIdentifiable
    {
        public DefaultEntityConverter(MapperProvider mapperProvider, IQueryable<TEntity> entities) : base(mapperProvider, entities)
        {
        }
    }
}