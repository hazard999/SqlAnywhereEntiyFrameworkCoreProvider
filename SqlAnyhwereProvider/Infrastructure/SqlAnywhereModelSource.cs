using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Internal;

namespace EntityFrameworkCore.RelationalProviderStarter.Infrastructure
{
    public class SqlAnywhereModelSource : ModelSource
    {
        public SqlAnywhereModelSource(IDbSetFinder setFinder,
            ICoreConventionSetBuilder coreConventionSetBuilder,
            IModelCustomizer modelCustomizer,
            IModelCacheKeyFactory modelCacheKeyFactory)
            : base(setFinder,
                coreConventionSetBuilder,
                modelCustomizer,
                modelCacheKeyFactory)
        {
        }
    }
}