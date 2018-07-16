using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using RestApi.ApiResults;
using RestApi.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using RestApi.Infrastructure;

namespace RestApi.Filters
{
    public class IdCheckActionFilterAttribute : ActionFilterAttribute
    {
        public Type EntityType { get; set; }

        private readonly IExpressionBuilder _expressionBuilder;

        private static readonly Dictionary<Type, Func<object, object>> EntitiesSelectorsCache = new Dictionary<Type, Func<object, object>>();

        public IdCheckActionFilterAttribute(IExpressionBuilder expressionBuilder)
        {
            _expressionBuilder = expressionBuilder ?? throw new ArgumentNullException(nameof(expressionBuilder));
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            Type entityType = EntityType ?? GetEntityTypeFromContext(context);
            string idStr = (string)context.RouteData.Values["id"];
            if (entityType != null && !string.IsNullOrEmpty(idStr))
            {
                Guid id = new Guid(idStr);
                Type repositoryType = typeof(IRepository<>).MakeGenericType(entityType);
                object repository =
                    context.HttpContext.RequestServices.GetService(repositoryType);
                IQueryable<IIdentifiable> entities = GetEntitiesFromRepository(repositoryType, repository);
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

        private IQueryable<IIdentifiable> GetEntitiesFromRepository(Type repositoryType, object repository)
        {
            if (!EntitiesSelectorsCache.TryGetValue(repositoryType, out var selector))
            {
                selector = _expressionBuilder.GetPropertyExpression(repositoryType, "Entities", false).Compile();
                EntitiesSelectorsCache[repositoryType] = selector;
            }

            return (IQueryable<IIdentifiable>) selector(repository);
        }
    }
}
