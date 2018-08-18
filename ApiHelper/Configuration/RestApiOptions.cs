using System;
using AutoMapper;

namespace RestApi.Configuration
{
    public class RestApiOptions
    {
        public ApiExceptionOptions ApiException { get; set; }
        public Action<IMapperConfigurationExpression> MapperConfiguration { get; set; }
    }
}
