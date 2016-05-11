using System.Data.Common;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using iAnywhere.Data.SQLAnywhere;

namespace EntityFrameworkCore.RelationalProviderStarter.Storage
{
    public class SqlAnywhereConnection : RelationalConnection
    {
        private IDbContextOptions Options;

        public SqlAnywhereConnection(IDbContextOptions options, ILogger<SqlAnywhereConnection> logger)
            : base(options, logger)
        {
            Options = options;
        }

        protected override DbConnection CreateDbConnection()
        {
            var myOptions = Options.FindExtension<Infrastructure.SqlAnywhereOptionsExtension>();
            return new SAConnection(myOptions.ConnectionString);
        }
    }
}