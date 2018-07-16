using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using RestApi.Services.Api;

namespace RestApi.Filters
{
    public class ModelStateCheckActionFilterAttribute : ActionFilterAttribute
    {
        private readonly IApiHelper _apiHelper;

        public ModelStateCheckActionFilterAttribute(IApiHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            if (!context.ModelState.IsValid)
            {
                context.Result = new BadRequestObjectResult(_apiHelper.GetErrorResultFromModelState(context.ModelState));
            }
        }
    }
}