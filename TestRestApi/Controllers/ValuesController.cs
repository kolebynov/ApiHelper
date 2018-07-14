using Microsoft.AspNetCore.Mvc;
using RestApi;
using RestApi.Controllers;
using RestApi.Converters;
using RestApi.Services.Api;

namespace TestRestApi.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : BaseApiController<TestEntity, TestModel>
    {
        public ValuesController(IRepository<TestEntity> repository, IApiQuery apiQuery, IApiHelper apiHelper, 
            IEntityConverter<TestEntity, TestModel> entityToGetModelConverter, IEntityConverter<TestEntity, TestModel> entityToAddModelConverter, 
            IEntityConverter<TestEntity, TestModel> entityToUpdateModelConverter) 
            : base(repository, apiQuery, apiHelper, entityToGetModelConverter, entityToAddModelConverter, entityToUpdateModelConverter)
        {
        }
    }
}
