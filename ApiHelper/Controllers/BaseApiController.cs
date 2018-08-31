using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
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
    public abstract class BaseApiController<TEntity, TGetModel, TGetSingleModel, TAddModel, TUpdateModel> : ControllerBase
        where TEntity : class, IIdentifiable
        where TGetSingleModel: class, IIdentifiable
        where TGetModel : class, IIdentifiable
    {
        protected readonly IRepository<TEntity> EntityRepository;
        protected readonly IApiHelper ApiHelper;
        protected readonly IEntityConverter<TEntity, TGetModel, TGetSingleModel, TAddModel, TUpdateModel> EntityConverter;

        [HttpGet]
        public virtual async Task<GetApiResult<IEnumerable<TGetModel>>> GetItems(GetOptions options)
        {
            return await ApiHelper.CreateApiResultFromQueryAsync(GetQueryForGetItems(), options);
        }

        [HttpGet("{id}")]
        public virtual Task<ApiResult<IEnumerable<TGetSingleModel>>> GetItem(Guid id)
        {
            return Task.FromResult(
                ApiResult.SuccessResult((IEnumerable<TGetSingleModel>) new[] { GetQueryForGetItem(id).First()}));
        }

        [HttpPost]
        public virtual async Task<ApiResult<IEnumerable<TGetSingleModel>>> AddItem([FromBody] TAddModel item)
        {
            TEntity newEntity = await AddInternalAsync(EntityConverter.ToEntity(item), new AddItemContext<TAddModel>(item));
            TGetSingleModel getModel = GetQueryForGetItem(newEntity.Id).First();

            Response.Headers[HeaderNames.Location] = Url.Action("GetItem",
                ControllerContext.ActionDescriptor.ControllerName, new {id = getModel.Id}, Request.Scheme,
                Request.Host.ToUriComponent());
            Response.StatusCode = StatusCodes.Status201Created;

            return ApiResult.SuccessResult((IEnumerable<TGetSingleModel>) new[] {getModel});
        }

        [HttpPut("{id}")]
        public virtual async Task<ApiResult<IEnumerable<TGetSingleModel>>> UpdateItem(Guid id, [FromBody] TUpdateModel item)
        {
            TEntity entity = EntityConverter.ToEntity(item, id);
            entity.Id = id;
            await UpdateInternalAsync(entity, new UpdateItemContext<TUpdateModel>(id, item));
            TGetSingleModel getModel = GetQueryForGetItem(entity.Id).First();

            return ApiResult.SuccessResult((IEnumerable<TGetSingleModel>)new[] {getModel});
        }

        [HttpDelete("{id}")]
        public virtual async Task<ApiResult> RemoveItem(Guid id)
        {
            await RemoveInternalAsync(id);
            return ApiResult.SuccessResult(new object[0]);
        }

        protected BaseApiController(IRepository<TEntity> repository, IApiHelper apiHelper, 
            IEntityConverter<TEntity, TGetModel, TGetSingleModel, TAddModel, TUpdateModel> entityConverter)
        {
            EntityRepository = repository ?? throw new ArgumentNullException(nameof(repository));
            ApiHelper = apiHelper ?? throw new ArgumentNullException(nameof(apiHelper));
            EntityConverter = entityConverter ?? throw new ArgumentNullException(nameof(entityConverter));
        }

        protected virtual IQueryable<TGetSingleModel> GetQueryForGetItem(Guid id) => 
            EntityRepository.Entities.Where(entity => entity.Id == id).Select(EntityConverter.GetEntityToGetSingleModelExpression());

        protected virtual IQueryable<TGetModel> GetQueryForGetItems() => 
            EntityRepository.Entities.Select(EntityConverter.GetEntityToGetModelExpression());

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

    public class BaseApiController<TEntity, TModel> : BaseApiController<TEntity, TModel, TModel, TModel, TModel>
        where TEntity : class, IIdentifiable
        where TModel : class, IIdentifiable
    {
        public BaseApiController(IRepository<TEntity> repository, IApiHelper apiHelper, IEntityConverter<TEntity, TModel, TModel, TModel, TModel> entityConverter) 
            : base(repository, apiHelper, entityConverter)
        {
        }
    }
}