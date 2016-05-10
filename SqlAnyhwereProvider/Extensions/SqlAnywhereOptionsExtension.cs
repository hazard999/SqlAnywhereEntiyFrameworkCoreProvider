using EntityFrameworkCore.RelationalProviderStarter.Infrastructure;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Microsoft.EntityFrameworkCore
{
    public static class SqlAnywhereOptionsExtension
    {
        public static DbContextOptionsBuilder UseSqlAnywhere(this DbContextOptionsBuilder optionsBuilder,
            string connectionString)
        {
            ((IDbContextOptionsBuilderInfrastructure)optionsBuilder).AddOrUpdateExtension(
                new global::EntityFrameworkCore.RelationalProviderStarter.Infrastructure.SqlAnywhereOptionsExtension
                {
                    ConnectionString = connectionString
                });

            return optionsBuilder;
        }
    }
}