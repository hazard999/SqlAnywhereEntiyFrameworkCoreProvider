using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
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

        protected override IModel CreateModel(DbContext context, IConventionSetBuilder conventionSetBuilder, IModelValidator validator)
        {
            var model = base.CreateModel(context, conventionSetBuilder, validator);
            return model;
        }
    }
}