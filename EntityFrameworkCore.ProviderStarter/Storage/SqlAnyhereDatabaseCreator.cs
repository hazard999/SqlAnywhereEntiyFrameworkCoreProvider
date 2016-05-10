using System;
using System.Threading;
using System.Threading.Tasks;
using EntityFrameworkCore.ProviderStarter.Infrastructure;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace EntityFrameworkCore.ProviderStarter.Storage
{
    public class SqlAnyhereDatabaseCreator : IDatabaseCreator
    {
        private SqlAnyhwewProviderOptionsExtension _myOptions;

        public SqlAnyhereDatabaseCreator(IDbContextOptions options)
        {
            _myOptions = options.FindExtension<SqlAnyhwewProviderOptionsExtension>();
        }

        public bool EnsureCreated()
        {
            throw new NotImplementedException();
        }

        public Task<bool> EnsureCreatedAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public bool EnsureDeleted()
        {
            throw new NotImplementedException();
        }

        public Task<bool> EnsureDeletedAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }
    }
}