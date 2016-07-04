using System;
using EntityFrameworkCore.RelationalProviderStarter.Infrastructure;
using EntityFrameworkCore.RelationalProviderStarter.Metadata;
using EntityFrameworkCore.RelationalProviderStarter.Migrations;
using EntityFrameworkCore.RelationalProviderStarter.Query.ExpressionTranslators.Internal;
using EntityFrameworkCore.RelationalProviderStarter.Query.Sql;
using EntityFrameworkCore.RelationalProviderStarter.Update;
using EntityFrameworkCore.RelationalProviderStarter.ValueGeneration;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Query.ExpressionTranslators;
using Microsoft.EntityFrameworkCore.Query.Sql;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Update;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using System.Reflection;
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
            => GetType().GetTypeInfo().Assembly.GetName().Name;

        public override IRelationalAnnotationProvider AnnotationProvider => GetService<SqlAnywhereAnnotationProvider>();

        public override IMemberTranslator CompositeMemberTranslator => GetService<SqlAnywhereCompositeMemberTranslator>();

        public override IMethodCallTranslator CompositeMethodCallTranslator => GetService<SqlAnywhereCompositeMethodCallTranslator>();

        public override IDatabaseCreator Creator => GetService<SqlAnywhereDatabaseCreator>();

        public override IHistoryRepository HistoryRepository => GetService<SqlAnywhereHistoryRepository>();

        public override IModelSource ModelSource => GetService<SqlAnywhereModelSource>();

        public override IModificationCommandBatchFactory ModificationCommandBatchFactory => GetService<SqlAnywhereModificationCommandBatchFactory>();

        public override IQuerySqlGeneratorFactory QuerySqlGeneratorFactory => GetService<SqlAnywhereQuerySqlGeneratorFactory>();

        public override IRelationalConnection RelationalConnection => GetService<SqlAnywhereConnection>();

        public override IRelationalDatabaseCreator RelationalDatabaseCreator => GetService<SqlAnywhereDatabaseCreator>();

        public override ISqlGenerationHelper SqlGenerationHelper => GetService<SqlAnywhereSqlGenerationHelper>();

        public override IUpdateSqlGenerator UpdateSqlGenerator => GetService<SqlAnyhwereUpdateSqlGenerator>();

        public override IValueGeneratorCache ValueGeneratorCache => GetService<SqlAnywhereValueGeneratorCache>();

        public override IValueGeneratorSelector ValueGeneratorSelector => GetService<SqlAnywhereValueGeneratorSelector>();

        public override IRelationalTypeMapper TypeMapper => GetService<SqlAnywhereTypeMapper>();

        //public override IDatabase Database => GetService<SqlAnywhereDatabase>();

        //public override IModelValidator ModelValidator
        //    => GetService<SqlAnywhereModelValidator>();
    }
}