using Microsoft.AspNetCore.Mvc;
using RestApi;
using RestApi.Controllers;
using RestApi.Converters;
using RestApi.Services.Api;

namespace TestRestApi.Controllers
{
    [Route("api/[controller]")]
    public class Values2Controller : BaseApiController<TestEntity, TestModel2>
    {
        public Values2Controller(IRepository<TestEntity> repository, IApiHelper apiHelper, 
            IEntityConverter<TestEntity, TestModel2, TestModel2, TestModel2, TestModel2> entityConverter) 
            : base(repository, apiHelper, entityConverter)
        {
        }
    }
}
