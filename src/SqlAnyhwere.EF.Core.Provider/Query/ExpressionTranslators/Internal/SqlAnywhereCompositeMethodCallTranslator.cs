using Microsoft.EntityFrameworkCore.Query.ExpressionTranslators;
using Microsoft.Extensions.Logging;

namespace EntityFrameworkCore.RelationalProviderStarter.Query.ExpressionTranslators.Internal
{
    public class SqlAnywhereCompositeMethodCallTranslator : RelationalCompositeMethodCallTranslator
    {
        public SqlAnywhereCompositeMethodCallTranslator(ILogger<SqlAnywhereCompositeMethodCallTranslator> logger)
            : base(logger)
        {
        }
    }
}