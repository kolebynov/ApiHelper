using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using RestApi.ApiResults;
using RestApi.Controllers;
using RestApi.Infrastructure;
using System;
using System.Collections.Concurrent;
using System.Linq;

namespace RestApi.Filters
{
    public class IdCheckActionFilterAttribute : ActionFilterAttribute
    {
        public Type EntityType { get; set; }

        private readonly IExpressionBuilder _expressionBuilder;

        private static readonly ConcurrentDictionary<Type, Func<object, IQueryable<IIdentifiable>>> EntitiesSelectorsCache =
            new ConcurrentDictionary<Type, Func<object, IQueryable<IIdentifiable>>>();

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
            var selector = EntitiesSelectorsCache.GetOrAdd(repositoryType, type =>
                (Func<object, IQueryable<IIdentifiable>>) _expressionBuilder
                    .GetPropertyExpression(type, "Entities", typeof(object),
                        typeof(IQueryable<IIdentifiable>), false).Compile());

            return selector(repository);
        }
    }
}
