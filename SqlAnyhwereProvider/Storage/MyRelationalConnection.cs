using System;
using System.Data.Common;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using iAnywhere.Data.SQLAnywhere;

namespace EntityFrameworkCore.RelationalProviderStarter.Storage
{
    public class MyRelationalConnection : RelationalConnection
    {
        private IDbContextOptions Options;

        public MyRelationalConnection(IDbContextOptions options, ILogger logger)
            : base(options, logger)
        {
            Options = options;
        }

        protected override DbConnection CreateDbConnection()
        {
            var myOptions = Options.FindExtension<Infrastructure.MyRelationalProviderOptionsExtension>();
            return new SAConnection(myOptions.ConnectionString);
        }
    }
}