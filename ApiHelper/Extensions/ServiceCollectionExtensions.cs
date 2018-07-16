using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using RestApi.Converters;
using RestApi.Filters;
using RestApi.Infrastructure;
using RestApi.Services.Api;

namespace RestApi.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRestApi(this IServiceCollection serviceCollection)
        {
            serviceCollection.CheckArgumentNull(nameof(serviceCollection));

            serviceCollection.AddSingleton<IApiQuery, ApiQuery>();
            serviceCollection.AddSingleton<IApiHelper, ApiHelper>();
            serviceCollection.AddSingleton(typeof(IEntityConverter<,>), typeof(DefaultEntityConverter<,>));
            serviceCollection.AddSingleton<IExpressionBuilder, ExpressionBuilder>();
            serviceCollection.AddScoped<ModelStateCheckActionFilterAttribute, ModelStateCheckActionFilterAttribute>();
            serviceCollection.AddScoped<ApiExceptionActionFilterAttribute, ApiExceptionActionFilterAttribute>();
            serviceCollection.AddScoped<IdCheckActionFilterAttribute, IdCheckActionFilterAttribute>();
            return serviceCollection;
        }
    }
}
