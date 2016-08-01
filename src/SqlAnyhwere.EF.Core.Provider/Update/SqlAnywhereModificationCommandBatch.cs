using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Update;

namespace SqlAnywhereProvider.Update
{
    public class SqlAnywhereModificationCommandBatch : AffectedCountModificationCommandBatch
    {
        public SqlAnywhereModificationCommandBatch(IRelationalCommandBuilderFactory commandBuilderFactory, ISqlGenerationHelper sqlGenerationHelper, IUpdateSqlGenerator updateSqlGenerator, IRelationalValueBufferFactoryFactory valueBufferFactoryFactory) 
            : base(commandBuilderFactory, sqlGenerationHelper, updateSqlGenerator, valueBufferFactoryFactory)
        {
        }

        protected override bool CanAddCommand(ModificationCommand modificationCommand)
        {
            return true;
        }

        protected override bool IsCommandTextValid()
        {
            return true;
        }
    }
}
