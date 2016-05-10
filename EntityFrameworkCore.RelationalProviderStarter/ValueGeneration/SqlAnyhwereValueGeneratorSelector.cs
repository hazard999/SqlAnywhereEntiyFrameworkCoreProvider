using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace EntityFrameworkCore.RelationalProviderStarter.Storage
{
    internal class SqlAnyhwereValueGeneratorSelector : RelationalValueGeneratorSelector
    {
        public SqlAnyhwereValueGeneratorSelector(IValueGeneratorCache cache, IRelationalAnnotationProvider relationalExtensions) : base(cache, relationalExtensions)
        {
        }
    }
}