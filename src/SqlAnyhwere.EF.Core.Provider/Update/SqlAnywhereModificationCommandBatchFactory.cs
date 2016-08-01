using Microsoft.EntityFrameworkCore.Update;
using SqlAnywhereProvider.Update;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace EntityFrameworkCore.RelationalProviderStarter.Update
{
    public class SqlAnywhereModificationCommandBatchFactory : IModificationCommandBatchFactory
    {
        private IRelationalCommandBuilderFactory _commandBuilderFactory;
        private IDbContextOptions _options;
        private ISqlGenerationHelper _sqlGenerationHelper;
        private IUpdateSqlGenerator _updateSqlGenerator;
        private IRelationalValueBufferFactoryFactory _valueBufferFactoryFactory;

        public SqlAnywhereModificationCommandBatchFactory(
            IRelationalCommandBuilderFactory commandBuilderFactory,
            ISqlGenerationHelper sqlGenerationHelper,
            IUpdateSqlGenerator updateSqlGenerator,
            IRelationalValueBufferFactoryFactory valueBufferFactoryFactory,
            IDbContextOptions options)
        {

            _commandBuilderFactory = commandBuilderFactory;
            _sqlGenerationHelper = sqlGenerationHelper;
            _updateSqlGenerator = updateSqlGenerator;
            _valueBufferFactoryFactory = valueBufferFactoryFactory;
            _options = options;
        }

        public ModificationCommandBatch Create()
        {
            return new SqlAnywhereModificationCommandBatch(_commandBuilderFactory, _sqlGenerationHelper, _updateSqlGenerator, _valueBufferFactoryFactory);
        }
    }
}