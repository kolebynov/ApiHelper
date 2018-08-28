﻿using Microsoft.AspNetCore.Mvc;
using RestApi.ApiResults;
using RestApi.Common;
using RestApi.Converters;
using RestApi.Filters;
using RestApi.Services.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestApi.Controllers
{
    [Produces("application/json")]
    [ServiceFilter(typeof(ModelStateCheckActionFilterAttribute))]
    [ServiceFilter(typeof(IdCheckActionFilterAttribute))]
    [ServiceFilter(typeof(ApiExceptionActionFilterAttribute))]
    public abstract class BaseApiController<TEntity, TGetModel, TAddModel, TUpdateModel> : ControllerBase
        where TEntity : class, IIdentifiable
        where TGetModel : class, IIdentifiable
    {
        protected readonly IApiQuery ApiQuery;
        protected readonly IRepository<TEntity> EntityRepository;
        protected readonly IApiHelper ApiHelper;
        protected readonly IEntityConverter<TEntity, TGetModel, TAddModel, TUpdateModel> EntityConverter;

        [HttpGet("{id?}")]
        public virtual async Task<GetApiResult<IEnumerable<TGetModel>>> GetItems(Guid id, GetOptions options)
        {
            return await ApiHelper.CreateApiResultFromQueryAsync(GetQueryForGetItems(), id, options);
        }

        [HttpPost]
        public virtual async Task<IActionResult> AddItem([FromBody] TAddModel item)
        {
            TEntity newEntity = await AddInternalAsync(EntityConverter.ToEntity(item), new AddItemContext<TAddModel>(item));
            TGetModel getModel = (await ApiQuery.GetItemsFromQueryAsync(GetQueryForGetModel(), newEntity.Id, null)).First();
            return CreatedAtAction("GetItems", new { id = getModel.Id },
                ApiResult.SuccessResult(new[] { getModel }));
        }

        [HttpPut("{id}")]
        public virtual async Task<ApiResult<TGetModel[]>> UpdateItem(Guid id, [FromBody] TUpdateModel item)
        {
            TEntity entity = EntityConverter.ToEntity(item, id);
            entity.Id = id;
            await UpdateInternalAsync(entity, new UpdateItemContext<TUpdateModel>(id, item));
            TGetModel getModel = (await ApiQuery.GetItemsFromQueryAsync(GetQueryForGetModel(), id, null)).First();
            return ApiResult.SuccessResult(new[] {getModel});
        }

        [HttpDelete("{id}")]
        public virtual async Task<ApiResult> RemoveItem(Guid id)
        {
            await RemoveInternalAsync(id);
            return ApiResult.SuccessResult(new object[0]);
        }

        protected BaseApiController(IRepository<TEntity> repository,
            IApiQuery apiQuery, IApiHelper apiHelper, IEntityConverter<TEntity, TGetModel, TAddModel, TUpdateModel> entityConverter)
        {
            EntityRepository = repository ?? throw new ArgumentNullException(nameof(repository));
            ApiQuery = apiQuery ?? throw new ArgumentNullException(nameof(apiQuery));
            ApiHelper = apiHelper ?? throw new ArgumentNullException(nameof(apiHelper));
            EntityConverter = entityConverter ?? throw new ArgumentNullException(nameof(entityConverter));
        }

        protected virtual IQueryable<TGetModel> GetQueryForGetModel() =>
            EntityRepository.Entities.Select(EntityConverter.GetEntityToGetModelExpression());

        protected virtual IQueryable<TGetModel> GetQueryForGetItems() => GetQueryForGetModel();

        protected virtual async Task<TEntity> AddInternalAsync(TEntity entity, AddItemContext<TAddModel> context) =>
            await EntityRepository.AddAsync(entity);

        protected virtual async Task<TEntity> UpdateInternalAsync(TEntity entity, UpdateItemContext<TUpdateModel> context) =>
            await EntityRepository.UpdateAsync(entity);

        protected virtual async Task RemoveInternalAsync(Guid id) =>
            await EntityRepository.DeleteAsync(id);

        public class AddItemContext<TModel>
        {
            public TModel Model { get; }

            public AddItemContext(TModel model)
            {
                Model = model;
            }
        }

        public class UpdateItemContext<TModel>
        {
            public TModel Model { get; }
            public Guid Id { get; }

            public UpdateItemContext(Guid id, TModel model)
            {
                Model = model;
                Id = id;
            }
        }
    }

    public class BaseApiController<TEntity, TModel> : BaseApiController<TEntity, TModel, TModel, TModel>
        where TEntity : class, IIdentifiable
        where TModel : class, IIdentifiable
    {
        public BaseApiController(IRepository<TEntity> repository, IApiQuery apiQuery, IApiHelper apiHelper, IEntityConverter<TEntity, TModel> entityConverter) 
            : base(repository, apiQuery, apiHelper, entityConverter)
        {
        }
    }
}