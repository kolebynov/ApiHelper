using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using RestApi.Exceptions;
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
            context.Result = new JsonResult(_apiHelper.GetErrorResultFromException(context.Exception));
            context.HttpContext.Response.StatusCode = context.Exception is ApiException ? 400 : 500;
        }
    }
}