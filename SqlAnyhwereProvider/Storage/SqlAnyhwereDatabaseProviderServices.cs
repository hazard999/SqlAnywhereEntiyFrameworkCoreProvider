using System;
using EntityFrameworkCore.RelationalProviderStarter.Infrastructure;
using EntityFrameworkCore.RelationalProviderStarter.Metadata;
using EntityFrameworkCore.RelationalProviderStarter.Migrations;
using EntityFrameworkCore.RelationalProviderStarter.Query.ExpressionTranslators.Internal;
using EntityFrameworkCore.RelationalProviderStarter.Query.Sql;
using EntityFrameworkCore.RelationalProviderStarter.Update;
using EntityFrameworkCore.RelationalProviderStarter.ValueGeneration;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Query.ExpressionTranslators;
using Microsoft.EntityFrameworkCore.Query.Sql;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Update;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using SqlAnywhereProvider.Storage;

namespace EntityFrameworkCore.RelationalProviderStarter.Storage
{
    public class SqlAnywhereProviderServices : RelationalDatabaseProviderServices
    {
        public SqlAnywhereProviderServices(IServiceProvider services)
            : base(services)
        {
        }

        public override string InvariantName
            => "SqlAnywhereProvider";

        public override IRelationalAnnotationProvider AnnotationProvider
            => GetService<SqlAnywhereAnnotationProvider>();

        public override IMemberTranslator CompositeMemberTranslator
            => GetService<SqlAnyhwereRelationalCompositeMemberTranslator>();

        public override IMethodCallTranslator CompositeMethodCallTranslator
            => GetService<SqlAnyhwereRelationalCompositeMethodCallTranslator>();

        public override IDatabaseCreator Creator
            => GetService<SqlAnywhereDatabaseCreator>();

        public override IHistoryRepository HistoryRepository
            => GetService<SqlAnywhereHistoryRepository>();

        public override IModelSource ModelSource
            => GetService<SqlAnywhereModelSource>();

        public override IModificationCommandBatchFactory ModificationCommandBatchFactory
            => GetService<SqlAnyhwereModificationCommandBatchFactory>();

        public override IQuerySqlGeneratorFactory QuerySqlGeneratorFactory
            => GetService<SqlAnyhwereQuerySqlGeneratorFactory>();

        public override IRelationalConnection RelationalConnection
            => GetService<SqlAnywhereConnection>();

        public override IRelationalDatabaseCreator RelationalDatabaseCreator
            => GetService<SqlAnywhereDatabaseCreator>();

        public override ISqlGenerationHelper SqlGenerationHelper
            => GetService<SqlAnywhereSqlGenerationHelper>();

        public override IUpdateSqlGenerator UpdateSqlGenerator
            => GetService<SqlAnyhwereUpdateSqlGenerator>();

        public override IValueGeneratorCache ValueGeneratorCache
            => GetService<SqlAnywhereValueGeneratorCache>();

        public override IValueGeneratorSelector ValueGeneratorSelector 
            => GetService<SqlAnywhereValueGeneratorSelector>();

        public override IModelValidator ModelValidator
            => GetService<SqlAnywhereModelValidator>();
    }
}