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
                IQueryable<IIdentifiable> entities = (IQueryable<IIdentifiable>)entitiesProperty.GetValue(repository);
                if (!entities.Any(entity => entity.Id == id))
                {
                    context.Result = new NotFoundObjectResult(ApiResult.ErrorResult(new ApiError { Message = "Not found" }));
                }
            }
        }

        private Type GetEntityTypeFromContext(ActionExecutingContext context)
        {
            Type apiControllerType = GetApiControllerType(context.Controller.GetType());
            if (apiControllerType != null)
            {
                return apiControllerType.GenericTypeArguments[0];
            }

            return null;
        }

        private Type GetApiControllerType(Type controllerType)
        {
            Type baseType = controllerType.BaseType;
            while (baseType != null)
            {
                if (baseType.GetGenericTypeDefinition().IsAssignableFrom(typeof(BaseApiController<,,,>)))
                {
                    return baseType;
                }

                baseType = baseType.BaseType;
            }

            return null;
        }
    }
}
