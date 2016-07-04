using System.Data.Common;

namespace iAnywhere.Data.SQLAnywhere
{
    public sealed class SAFactory : DbProviderFactory
    {
        public static readonly SAFactory Instance = new SAFactory();

        private SAFactory()
        {
        }

        public override DbCommand CreateCommand()
        {
            return new SACommand();
        }

        public override DbConnection CreateConnection()
        {
            return new SAConnection();
        }

        public override DbParameter CreateParameter()
        {
            return new SAParameter();
        }

        public override DbConnectionStringBuilder CreateConnectionStringBuilder()
        {
            return new SAConnectionStringBuilder();
        }
    }
}
