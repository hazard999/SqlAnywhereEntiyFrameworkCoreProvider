using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Update;

namespace EntityFrameworkCore.ProviderStarter.Storage
{
    public class SqlAnywhereDatabase : Database
    {
        public SqlAnywhereDatabase(IQueryCompilationContextFactory queryCompilationContextFactory)
            : base(queryCompilationContextFactory)
        {
        }

        public override int SaveChanges(IReadOnlyList<IUpdateEntry> entries)
        {
            return 0;
        }

        public override Task<int> SaveChangesAsync(IReadOnlyList<IUpdateEntry> entries, CancellationToken cancellationToken = new CancellationToken())
        {
            return Task.FromResult(0);
        }
    }
}