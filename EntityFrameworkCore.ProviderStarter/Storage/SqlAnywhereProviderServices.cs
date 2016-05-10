using System;
using EntityFrameworkCore.ProviderStarter.Infrastructure;
using EntityFrameworkCore.ProviderStarter.Query;
using EntityFrameworkCore.ProviderStarter.Query.ExpressionVisitors;
using EntityFrameworkCore.ProviderStarter.ValueGeneration;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.ExpressionVisitors;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace EntityFrameworkCore.ProviderStarter.Storage
{
    public class SqlAnywhereProviderServices : DatabaseProviderServices
    {
        public SqlAnywhereProviderServices(IServiceProvider services)
            : base(services)
        {
        }

        public override string InvariantName
            => "SqlAnywhereProvider";

        public override IDatabaseCreator Creator
            => GetService<SqlAnyhereDatabaseCreator>();

        public override IDatabase Database
            => GetService<SqlAnywhereDatabase>();

        public override IEntityQueryableExpressionVisitorFactory EntityQueryableExpressionVisitorFactory
            => GetService<MyEntityQueryableExpressionVisitorFactory>();

        public override IEntityQueryModelVisitorFactory EntityQueryModelVisitorFactory
            => GetService<MyEntityQueryModelVisitorFactory>();

        public override IModelSource ModelSource
            => GetService<MyModelSource>();

        public override IQueryContextFactory QueryContextFactory
            => GetService<MyQueryContextFactory>();

        public override IDbContextTransactionManager TransactionManager
            => GetService<SqlAnywhereTransactionManager>();

        public override IValueGeneratorCache ValueGeneratorCache
            => GetService<MyValueGeneratorCache>();
    }
}