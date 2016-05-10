using System;
using System.Data.Common;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using iAnywhere.Data.SQLAnywhere;

namespace EntityFrameworkCore.RelationalProviderStarter.Storage
{
    public class SqlAnywhereRelationalConnection : RelationalConnection
    {
        private IDbContextOptions Options;

        public SqlAnywhereRelationalConnection(IDbContextOptions options, ILogger logger)
            : base(options, logger)
        {
            Options = options;
        }

        protected override DbConnection CreateDbConnection()
        {
            var myOptions = Options.FindExtension<Infrastructure.SqlAnywhereProviderOptionsExtension>();
            return new SAConnection(myOptions.ConnectionString);
        }
    }
}