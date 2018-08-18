using Microsoft.Extensions.DependencyInjection;
using RestApi.Configuration;
using RestApi.Converters;
using RestApi.Filters;
using RestApi.Infrastructure;
using RestApi.Services.Api;
using RestApi.Validating;
using System;

namespace RestApi.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRestApi(this IServiceCollection serviceCollection,
            Action<RestApiOptions> setupAction)
        {
            serviceCollection.CheckArgumentNull(nameof(serviceCollection));

            serviceCollection.AddOptions<RestApiOptions>()
                .Configure(options =>
                {
                    options.ApiException = new ApiExceptionOptions
                    {
                        ShowFullErrorInfo = false
                    };

                    setupAction?.Invoke(options);
                });

            serviceCollection.AddSingleton<IApiQuery, ApiQuery>();
            serviceCollection.AddSingleton(typeof(IEntityConverter<,,,>), typeof(DefaultEntityConverter<,,,>));
            serviceCollection.AddSingleton(typeof(IEntityConverter<,>), typeof(DefaultEntityConverter<,>));
            serviceCollection.AddSingleton<IExpressionBuilder, ExpressionBuilder>();
            serviceCollection.AddSingleton<MapperProvider, MapperProvider>();
            serviceCollection.AddSingleton(typeof(IEntityValidator<>), typeof(DataAnnotationsValidator<>));
            serviceCollection.AddScoped<IApiHelper, ApiHelper>();
            serviceCollection.AddScoped<ModelStateCheckActionFilterAttribute, ModelStateCheckActionFilterAttribute>();
            serviceCollection.AddScoped<ApiExceptionActionFilterAttribute, ApiExceptionActionFilterAttribute>();
            serviceCollection.AddScoped<IdCheckActionFilterAttribute, IdCheckActionFilterAttribute>();
            return serviceCollection;
        }

        public static IServiceCollection AddRestApi(this IServiceCollection serviceCollection) =>
            serviceCollection.AddRestApi(null);
    }
}
