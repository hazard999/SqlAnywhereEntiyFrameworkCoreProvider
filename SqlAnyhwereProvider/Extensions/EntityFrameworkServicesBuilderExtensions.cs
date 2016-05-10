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
                    DatabaseProvider<SqlAnyhwereDatabaseProviderServices, SqlAnywhereProviderOptionsExtension>>());

            serviceCollection.TryAdd(new ServiceCollection()
                // all singleton services
                
                .AddSingleton<SqlAnywhereRelationalAnnotationProvider>()
                .AddSingleton<SqlAnyhwereRelationalCompositeMemberTranslator>()
                .AddSingleton<SqlAnyhwereRelationalCompositeMethodCallTranslator>()
                .AddSingleton<SqlAnywhereRelationalSqlGenerationHelper>()
                .AddSingleton<SqlAnywhereModelSource>()
                .AddSingleton<SqlAnyhwereValueGeneratorCache>()
                .AddSingleton<SqlAnyhwereValueGeneratorSelector>()
                // all scoped services
                .AddScoped<SqlAnywhereRelationalDatabaseCreator>()
                .AddScoped<SqlAnyhwereDatabaseProviderServices>()
                .AddScoped<SqlAnywhereHistoryRepository>()
                .AddScoped<SqlAnyhwereModificationCommandBatchFactory>()
                .AddScoped<SqlAnyhwereQuerySqlGeneratorFactory>()
                .AddScoped<SqlAnywhereRelationalConnection>()
                .AddScoped<SqlAnywhereRelationalDatabaseCreator>()
                .AddScoped<SqlAnyhwereUpdateSqlGenerator>());

            return builder;
        }
    }
}