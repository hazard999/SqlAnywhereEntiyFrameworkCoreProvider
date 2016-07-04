using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace EntityFrameworkCore.RelationalProviderStarter.Storage
{
    public class SqlAnywhereValueGeneratorSelector : RelationalValueGeneratorSelector
    {
        public SqlAnywhereValueGeneratorSelector(IValueGeneratorCache cache, IRelationalAnnotationProvider relationalExtensions) : base(cache, relationalExtensions)
        {
        }
    }
}