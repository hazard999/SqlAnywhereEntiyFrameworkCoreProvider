using EntityFrameworkCore.ProviderStarter.Infrastructure;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Microsoft.EntityFrameworkCore
{
    public static class SqlAnyhwereProviderDbContextOptionsExtensions
    {
        public static DbContextOptionsBuilder UseSqlAnywhere(this DbContextOptionsBuilder optionsBuilder,
            string connectionString)
        {
            ((IDbContextOptionsBuilderInfrastructure) optionsBuilder).AddOrUpdateExtension(
                new SqlAnyhwewProviderOptionsExtension
                {
                    ConnectionString = connectionString
                });

            return optionsBuilder;
        }
    }
}