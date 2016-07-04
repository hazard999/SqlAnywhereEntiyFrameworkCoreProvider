using System.Data.Common;
using Microsoft.EntityFrameworkCore.Storage;

namespace EntityFrameworkCore.RelationalProviderStarter.Storage
{

    public class SqlAnywhereRelationalDataReader : RelationalDataReader
    {
        public SqlAnywhereRelationalDataReader(IRelationalConnection connection, DbCommand command, DbDataReader reader) : base(connection, command, reader)
        {
        }
    }
}