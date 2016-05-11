using EntityFrameworkCore.RelationalProviderStarter.Infrastructure;
using EntityFrameworkCore.RelationalProviderStarter.Metadata;
using EntityFrameworkCore.RelationalProviderStarter.Migrations;
using EntityFrameworkCore.RelationalProviderStarter.Storage;
using EntityFrameworkCore.RelationalProviderStarter.Update;
using EntityFrameworkCore.RelationalProviderStarter.ValueGeneration;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SqlAnywhereProvider.Storage;
using Microsoft.EntityFrameworkCore.Infrastructure;
using SqlAnywhereProvider.Migrations;
using Microsoft.EntityFrameworkCore.Update;
using EntityFrameworkCore.RelationalProviderStarter.Query.ExpressionTranslators.Internal;
using EntityFrameworkCore.RelationalProviderStarter.Query.Sql;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class EntityFrameworkServicesBuilderExtensions
    {
        public static IServiceCollection AddEntityFrameworkSqlAnywhere(this IServiceCollection services)
        {
            services.AddRelational();

            services.TryAddEnumerable(ServiceDescriptor
                .Singleton<IDatabaseProvider, DatabaseProvider<SqlAnywhereProviderServices, SqlAnywhereOptionsExtension>>());

            var serviceCollection = new ServiceCollection();
            serviceCollection
                //.AddSingleton<SqlAnywhereDatabase>()
                .AddSingleton<SqlAnywhereValueGeneratorCache>()
                .AddSingleton<SqlAnywhereTypeMapper>()
                .AddSingleton<SqlAnywhereSqlGenerationHelper>()
                .AddSingleton<SqlAnywhereModelSource>()
                .AddSingleton<SqlAnywhereAnnotationProvider>()
                .AddSingleton<SqlAnywhereMigrationsAnnotationProvider>();

            serviceCollection
                .AddScoped<SqlAnywhereConventionSetBuilder>()
                .AddScoped<SqlAnyhwereUpdateSqlGenerator>();

            serviceCollection
                //.AddScoped<SqlAnyhwereCompositeMethodCallTranslator>()
                //.AddScoped<SqlAnywhereSequenceValueGeneratorFactory>()
                .AddScoped<SqlAnywhereModificationCommandBatchFactory>()
                .AddScoped<SqlAnywhereValueGeneratorSelector>()
                .AddScoped<SqlAnywhereProviderServices>()
                .AddScoped<SqlAnywhereConnection>()
                //.AddScoped<SqlAnywhereMigrationsSqlGenerator>()
                .AddScoped<SqlAnywhereDatabaseCreator>()
                .AddScoped<SqlAnywhereHistoryRepository>()
            //.AddScoped<SqlAnywhereQueryModelVisitorFactory>()
            //.AddScoped<SqlAnywhereCompiledQueryCacheKeyGenerator>()
                .AddQuery();

            services.TryAdd(serviceCollection);
            return services;
        }

        private static IServiceCollection AddQuery(this IServiceCollection serviceCollection)
        => serviceCollection
            //.AddScoped<SqlAnywhereQueryCompilationContextFactory>()
            .AddScoped<SqlAnywhereCompositeMemberTranslator>()
            .AddScoped<SqlAnywhereCompositeMethodCallTranslator>()
            .AddScoped<SqlAnywhereQuerySqlGeneratorFactory>();
    }
}