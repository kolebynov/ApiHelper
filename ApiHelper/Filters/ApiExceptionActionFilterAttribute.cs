using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using RestApi.Services.Api;

namespace RestApi.Filters
{
    public class ApiExceptionActionFilterAttribute : ExceptionFilterAttribute
    {
        private readonly IApiHelper _apiHelper;

        public ApiExceptionActionFilterAttribute(IApiHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

        public override void OnException(ExceptionContext context)
        {
            context.Result = new BadRequestObjectResult(_apiHelper.GetErrorResultFromException(context.Exception));
        }
    }
}