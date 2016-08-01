using Microsoft.EntityFrameworkCore.Metadata.Conventions.Internal;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Storage;

namespace SqlAnywhereProvider.Migrations
{
    public class SqlAnywhereConventionSetBuilder : RelationalConventionSetBuilder
    {
        public SqlAnywhereConventionSetBuilder(IRelationalTypeMapper typeMapper, ICurrentDbContext currentContext, IDbSetFinder setFinder)
            : base(typeMapper, currentContext, setFinder)
        {
        }
    }
}
