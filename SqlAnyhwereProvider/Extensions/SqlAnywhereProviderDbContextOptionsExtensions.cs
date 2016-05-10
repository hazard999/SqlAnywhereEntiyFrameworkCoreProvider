using EntityFrameworkCore.RelationalProviderStarter.Infrastructure;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Microsoft.EntityFrameworkCore
{
    public static class SqlAnywhereProviderDbContextOptionsExtensions
    {
        public static DbContextOptionsBuilder UseSqlAnywhere(this DbContextOptionsBuilder optionsBuilder,
            string connectionString)
        {
            ((IDbContextOptionsBuilderInfrastructure)optionsBuilder).AddOrUpdateExtension(
                new SqlAnywhereProviderOptionsExtension
                {
                    ConnectionString = connectionString
                });

            return optionsBuilder;
        }
    }
}