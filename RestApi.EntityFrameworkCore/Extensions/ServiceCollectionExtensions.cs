using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RestApi.Configuration;
using RestApi.Extensions;

namespace RestApi.EntityFrameworkCore.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRestApiWithEntityFramework<TContext>(
            this IServiceCollection serviceCollection, Action<RestApiOptions> setupAction,
            ServiceLifetime contextLifetime = ServiceLifetime.Scoped)
            where TContext : DbContext
        {
            serviceCollection.AddRestApi(setupAction);
            serviceCollection.Add(new ServiceDescriptor(typeof(IRepository<>), typeof(EfRepository<>),
                contextLifetime));
            serviceCollection.Add(new ServiceDescriptor(typeof(DbContext), sp => sp.GetRequiredService<TContext>(), contextLifetime));

            return serviceCollection;
        }

        public static IServiceCollection AddRestApiWithEntityFramework<TContext>(
            this IServiceCollection serviceCollection, ServiceLifetime contextLifetime = ServiceLifetime.Scoped)
            where TContext : DbContext =>
            serviceCollection.AddRestApiWithEntityFramework<TContext>(null, contextLifetime);
    }
}
