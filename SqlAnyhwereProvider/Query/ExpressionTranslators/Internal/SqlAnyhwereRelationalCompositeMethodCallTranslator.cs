using Microsoft.EntityFrameworkCore.Query.ExpressionTranslators;
using Microsoft.Extensions.Logging;

namespace EntityFrameworkCore.RelationalProviderStarter.Query.ExpressionTranslators.Internal
{
    public class SqlAnyhwereRelationalCompositeMethodCallTranslator : RelationalCompositeMethodCallTranslator
    {
        public SqlAnyhwereRelationalCompositeMethodCallTranslator(ILogger logger)
            : base(logger)
        {
        }
    }
}