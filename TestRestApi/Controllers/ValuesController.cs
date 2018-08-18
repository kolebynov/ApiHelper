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
            IEntityConverter<TestEntity, TestModel> entityConverter) 
            : base(repository, apiQuery, apiHelper, entityConverter)
        {
        }
    }
}
