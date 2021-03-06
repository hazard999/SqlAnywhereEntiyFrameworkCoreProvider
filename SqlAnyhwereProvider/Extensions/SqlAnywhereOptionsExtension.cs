﻿using EntityFrameworkCore.RelationalProviderStarter.Infrastructure;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Microsoft.EntityFrameworkCore
{
    public static class DbContextOptionsBuilderExtension
    {
        public static DbContextOptionsBuilder UseSqlAnywhere(this DbContextOptionsBuilder optionsBuilder,
            string connectionString)
        {
            ((IDbContextOptionsBuilderInfrastructure)optionsBuilder).AddOrUpdateExtension(
                new SqlAnywhereOptionsExtension
                {
                    ConnectionString = connectionString
                });
            
            return optionsBuilder;
        }
    }
}