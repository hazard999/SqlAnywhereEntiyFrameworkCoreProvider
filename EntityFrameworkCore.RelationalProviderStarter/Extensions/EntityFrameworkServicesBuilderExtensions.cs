using EntityFrameworkCore.RelationalProviderStarter.Infrastructure;
using EntityFrameworkCore.RelationalProviderStarter.Metadata;
using EntityFrameworkCore.RelationalProviderStarter.Migrations;
using EntityFrameworkCore.RelationalProviderStarter.Query.ExpressionTranslators.Internal;
using EntityFrameworkCore.RelationalProviderStarter.Query.Sql;
using EntityFrameworkCore.RelationalProviderStarter.Storage;
using EntityFrameworkCore.RelationalProviderStarter.Update;
using EntityFrameworkCore.RelationalProviderStarter.ValueGeneration;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class EntityFrameworkServicesBuilderExtensions
    {
        public static IServiceCollection AddSqlAnywhereProvider(this IServiceCollection builder)
        {
            var serviceCollection = builder;

            serviceCollection.TryAddEnumerable(ServiceDescriptor
                .Singleton
                <IDatabaseProvider,
                    DatabaseProvider<SqlAnyhwereDatabaseProviderServices, MyRelationalProviderOptionsExtension>>());

            serviceCollection.TryAdd(new ServiceCollection()
                // all singleton services
                
                .AddSingleton<MyRelationalAnnotationProvider>()
                .AddSingleton<MyRelationalCompositeMemberTranslator>()
                .AddSingleton<MyRelationalCompositeMethodCallTranslator>()
                .AddSingleton<MyRelationalSqlGenerationHelper>()
                .AddSingleton<MyModelSource>()
                .AddSingleton<MyValueGeneratorCache>()
                .AddSingleton<SqlAnyhwereValueGeneratorSelector>()
                // all scoped services
                .AddScoped<MyRelationalDatabaseCreator>()
                .AddScoped<SqlAnyhwereDatabaseProviderServices>()
                .AddScoped<MyHistoryRepository>()
                .AddScoped<MyModificationCommandBatchFactory>()
                .AddScoped<MyQuerySqlGeneratorFactory>()
                .AddScoped<MyRelationalConnection>()
                .AddScoped<MyRelationalDatabaseCreator>()
                .AddScoped<MyUpdateSqlGenerator>());

            return builder;
        }
    }
}