﻿using Microsoft.AspNetCore.Mvc;
using RestApi;
using RestApi.Controllers;
using RestApi.Converters;
using RestApi.Services.Api;

namespace TestRestApi.Controllers
{
    [Route("api/[controller]")]
    public class Values2Controller : BaseApiController<TestEntity, TestModel2>
    {
        public Values2Controller(IRepository<TestEntity> repository, IApiQuery apiQuery, IApiHelper apiHelper, 
            IEntityConverter<TestEntity, TestModel2> entityToGetModelConverter, IEntityConverter<TestEntity, TestModel2> entityToAddModelConverter, 
            IEntityConverter<TestEntity, TestModel2> entityToUpdateModelConverter) 
            : base(repository, apiQuery, apiHelper, entityToGetModelConverter, entityToAddModelConverter, entityToUpdateModelConverter)
        {
        }
    }
}