using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using RestApi.ApiResults;
using RestApi.Controllers;
using System;
using System.Linq;
using System.Reflection;

namespace RestApi.Filters
{
    public class IdCheckActionFilterAttribute : ActionFilterAttribute
    {
        public Type EntityType { get; set; }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            Type entityType = EntityType ?? GetEntityTypeFromContext(context);
            string idStr = (string)context.RouteData.Values["id"];
            if (entityType != null && !string.IsNullOrEmpty(idStr))
            {
                Guid id = new Guid(idStr);
                Type repositoryType = typeof(IRepository<>).MakeGenericType(entityType);
                PropertyInfo entitiesProperty = repositoryType.GetProperty("Entities");
                object repository =
                    context.HttpContext.RequestServices.GetService(repositoryType);
                IQueryable<BaseEntity> entities = (IQueryable<BaseEntity>)entitiesProperty.GetValue(repository);
                if (!entities.Any(entity => entity.Id == id))
                {
                    context.Result = new JsonResult(ApiResult.ErrorResult(new ApiError {Message = "Not found"}));
                    context.HttpContext.Response.StatusCode = 404;
                }
            }
        }

        private Type GetEntityTypeFromContext(ActionExecutingContext context)
        {
            Type controllerType = context.Controller.GetType();
            if (controllerType.BaseType.IsGenericType 
                && controllerType.BaseType.GetGenericTypeDefinition().IsAssignableFrom(typeof(BaseApiController<,,,>)))
            {
                return controllerType.BaseType.GenericTypeArguments[0];
            }

            return null;
        }
    }
}
