using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;

namespace EntityFrameworkCore.RelationalProviderStarter.Storage
{

    public class SqlAnywhereRelationalCommand : RelationalCommand
    {
        public SqlAnywhereRelationalCommand(ISensitiveDataLogger logger, DiagnosticSource diagnosticSource, string commandText, IReadOnlyList<IRelationalParameter> parameters)
            : base(logger, diagnosticSource, commandText, parameters)
        {
        }
    }

    
}