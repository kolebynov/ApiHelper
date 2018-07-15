using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RestApi.Extensions;

namespace RestApi.EntityFrameworkCore.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRestApiWithEntityFramework<TContext>(this IServiceCollection serviceCollection, ServiceLifetime contextLifetime = ServiceLifetime.Scoped)
            where TContext : DbContext
        {
            serviceCollection.AddRestApi();
            serviceCollection.Add(new ServiceDescriptor(typeof(IRepository<>), typeof(EfRepository<>), contextLifetime));
            serviceCollection.Add(new ServiceDescriptor(typeof(DbContext), typeof(TContext), contextLifetime));

            return serviceCollection;
        }
    }
}
