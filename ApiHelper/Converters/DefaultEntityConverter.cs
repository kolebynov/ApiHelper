﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using RestApi.Extensions;
using RestApi.Infrastructure;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace RestApi.Converters
{
    public class DefaultEntityConverter<TEntity, TGetModel, TGetSingleModel, TAddModel, TUpdateModel> 
        : IEntityConverter<TEntity, TGetModel, TGetSingleModel, TAddModel, TUpdateModel>
        where TEntity : IIdentifiable
    {
        private readonly IMapper _mapper;
        private readonly IQueryable<TEntity> _entities;
        private Func<TEntity, TGetModel> _entityToGetModelFunc;

        private Func<TEntity, TGetModel> EntityToGetModelFunc =>
            _entityToGetModelFunc ?? (_entityToGetModelFunc = GetEntityToGetModelExpression().Compile());

        public DefaultEntityConverter(MapperProvider mapperProvider, IQueryable<TEntity> entities)
        {
            _entities = entities ?? throw new ArgumentNullException(nameof(entities));
            mapperProvider.CheckArgumentNull(nameof(mapperProvider));
            _mapper = mapperProvider.Mapper;
        }

        public virtual Expression<Func<TEntity, TGetModel>> GetEntityToGetModelExpression() =>
            _mapper.ConfigurationProvider.ExpressionBuilder.GetMapExpression<TEntity, TGetModel>();

        public virtual Expression<Func<TEntity, TGetSingleModel>> GetEntityToGetSingleModelExpression() =>
            _mapper.ConfigurationProvider.ExpressionBuilder.GetMapExpression<TEntity, TGetSingleModel>();

        public virtual TEntity ToEntity(TAddModel model) =>
            _mapper.Map<TAddModel, TEntity>(model);

        public virtual TEntity ToEntity(TUpdateModel model, Guid id) =>
            _mapper.Map(model, _entities.First(x => x.Id == id));

        public TGetModel ToGetModel(TEntity entity) =>
            EntityToGetModelFunc(entity);
    }
}