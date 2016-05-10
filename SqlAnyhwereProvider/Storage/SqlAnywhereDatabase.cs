using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Update;

namespace EntityFrameworkCore.RelationalProviderStarter.Storage
{
    public class SqlAnywhereDatabase : RelationalDatabase
    {
        public SqlAnywhereDatabase(IQueryCompilationContextFactory queryCompilationContextFactory,
            ICommandBatchPreparer batchPreparer,
            IBatchExecutor batchExecutor,
            IRelationalConnection connection)
            : base(queryCompilationContextFactory,
                batchPreparer,
                batchExecutor,
                connection)
        {
        }
    }
}