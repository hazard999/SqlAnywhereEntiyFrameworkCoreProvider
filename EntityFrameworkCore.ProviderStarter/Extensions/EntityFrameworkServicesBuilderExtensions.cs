using EntityFrameworkCore.ProviderStarter.Infrastructure;
using EntityFrameworkCore.ProviderStarter.Query;
using EntityFrameworkCore.ProviderStarter.Query.ExpressionVisitors;
using EntityFrameworkCore.ProviderStarter.Storage;
using EntityFrameworkCore.ProviderStarter.ValueGeneration;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class EntityFrameworkServicesBuilderExtensions
    {
        public static EntityFrameworkServicesBuilder AddMyProvider(this EntityFrameworkServicesBuilder builder)
        {
            var serviceCollection = builder.GetInfrastructure();

            serviceCollection.TryAddEnumerable(ServiceDescriptor
                .Singleton<IDatabaseProvider, DatabaseProvider<SqlAnywhereProviderServices, SqlAnyhwewProviderOptionsExtension>>());

            serviceCollection.TryAdd(new ServiceCollection()
                // singleton services
                .AddSingleton<MyModelSource>()
                .AddSingleton<MyValueGeneratorCache>()
                // scoped services
                .AddScoped<SqlAnywhereProviderServices>()
                .AddScoped<SqlAnyhereDatabaseCreator>()
                .AddScoped<SqlAnywhereDatabase>()
                .AddScoped<MyEntityQueryableExpressionVisitorFactory>()
                .AddScoped<MyEntityQueryModelVisitorFactory>()
                .AddScoped<MyQueryContextFactory>()
                .AddScoped<SqlAnywhereTransactionManager>());

            return builder;
        }

        public static IServiceCollection AddMyProvider(this IServiceCollection builder)
        {
            var serviceCollection = builder;

            serviceCollection.TryAddEnumerable(ServiceDescriptor
                .Singleton<IDatabaseProvider, DatabaseProvider<SqlAnywhereProviderServices, SqlAnyhwewProviderOptionsExtension>>());

            serviceCollection.TryAdd(new ServiceCollection()
                // singleton services
                .AddSingleton<MyModelSource>()
                .AddSingleton<MyValueGeneratorCache>()
                // scoped services
                .AddScoped<SqlAnywhereProviderServices>()
                .AddScoped<SqlAnyhereDatabaseCreator>()
                .AddScoped<SqlAnywhereDatabase>()
                .AddScoped<MyEntityQueryableExpressionVisitorFactory>()
                .AddScoped<MyEntityQueryModelVisitorFactory>()
                .AddScoped<MyQueryContextFactory>()
                .AddScoped<SqlAnywhereTransactionManager>());

            return builder;
        }
    }
}
