using Microsoft.AspNetCore.Mvc;
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
    [IdCheckActionFilter]
    [ServiceFilter(typeof(ApiExceptionActionFilterAttribute))]
    public abstract class BaseApiController<TEntity, TGetModel, TAddModel, TUpdateModel> : ControllerBase
        where TEntity : BaseEntity
        where TGetModel : IIdentifiable
    {
        protected readonly IApiQuery ApiQuery;
        protected readonly IRepository<TEntity> EntityRepository;
        protected readonly IApiHelper ApiHelper;
        protected readonly IEntityConverter<TEntity, TGetModel> EntityToGetModelConverter;
        protected readonly IEntityConverter<TEntity, TAddModel> EntityToAddModelConverter;
        protected readonly IEntityConverter<TEntity, TUpdateModel> EntityToUpdateModelConverter;

        [HttpGet("{id?}")]
        public virtual async Task<GetApiResult<IEnumerable<TGetModel>>> GetItems(Guid id, GetOptions options)
        {
            return await ApiHelper.CreateApiResultFromQueryAsync(GetQueryForGetModel(), id, options);
        }

        [HttpPost]
        public virtual async Task<IActionResult> AddItem([FromBody] TAddModel item)
        {
            TEntity newEntity = await EntityRepository.AddAsync(EntityToAddModelConverter.ToEntity(item));
            TGetModel getModel = (await ApiQuery.GetItemsFromQueryAsync(GetQueryForGetModel(), newEntity.Id, null)).First();
            return CreatedAtAction("GetItems", new { id = getModel.Id },
                ApiResult.SuccessResult(new[] { getModel }));
        }

        [HttpPut("{id}")]
        [IdCheckActionFilter]
        public virtual async Task<IActionResult> UpdateItem(Guid id, [FromBody] TUpdateModel item)
        {
            await EntityRepository.UpdateAsync(EntityToUpdateModelConverter.ToEntity(item));
            TGetModel getModel = (await ApiQuery.GetItemsFromQueryAsync(GetQueryForGetModel(), id, null)).First();
            return CreatedAtAction("GetItems", new { id }, ApiResult.SuccessResult(new[] { getModel }));
        }

        [HttpDelete("{id}")]
        [IdCheckActionFilter]
        public virtual async Task<ApiResult> RemoveItem(Guid id)
        {
            await EntityRepository.DeleteAsync(id);
            return new ApiResult { Success = true };
        }

        protected BaseApiController(IRepository<TEntity> repository,
            IApiQuery apiQuery, IApiHelper apiHelper, IEntityConverter<TEntity, TGetModel> entityToGetModelConverter, 
            IEntityConverter<TEntity, TAddModel> entityToAddModelConverter, IEntityConverter<TEntity, TUpdateModel> entityToUpdateModelConverter)
        {
            EntityRepository = repository ?? throw new ArgumentNullException(nameof(repository));
            ApiQuery = apiQuery ?? throw new ArgumentNullException(nameof(apiQuery));
            ApiHelper = apiHelper ?? throw new ArgumentNullException(nameof(apiHelper));
            EntityToGetModelConverter = entityToGetModelConverter ?? throw new ArgumentNullException(nameof(entityToGetModelConverter));
            EntityToAddModelConverter = entityToAddModelConverter ?? throw new ArgumentNullException(nameof(entityToAddModelConverter));
            EntityToUpdateModelConverter = entityToUpdateModelConverter ?? throw new ArgumentNullException(nameof(entityToUpdateModelConverter));
        }

        private IQueryable<TGetModel> GetQueryForGetModel() =>
            EntityRepository.Entities.Select(EntityToGetModelConverter.GetEntityToModelExpression());
    }

    public class BaseApiController<TEntity, TModel> : BaseApiController<TEntity, TModel, TModel, TModel>
        where TEntity : BaseEntity
        where TModel : IIdentifiable
    {
        public BaseApiController(IRepository<TEntity> repository, IApiQuery apiQuery, IApiHelper apiHelper, IEntityConverter<TEntity, TModel> entityToGetModelConverter, 
            IEntityConverter<TEntity, TModel> entityToAddModelConverter, IEntityConverter<TEntity, TModel> entityToUpdateModelConverter) 
            : base(repository, apiQuery, apiHelper, entityToGetModelConverter, entityToAddModelConverter, entityToUpdateModelConverter)
        {
        }
    }
}