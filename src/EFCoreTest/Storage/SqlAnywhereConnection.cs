using System.Data.Common;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using iAnywhere.Data.SQLAnywhere;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using JetBrains.Annotations;
using System.Diagnostics;

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