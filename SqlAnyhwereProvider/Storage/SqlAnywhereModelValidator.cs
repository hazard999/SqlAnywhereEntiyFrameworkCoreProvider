using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;

namespace SqlAnywhereProvider.Storage
{
    public class SqlAnywhereModelValidator : RelationalModelValidator
    {
        public SqlAnywhereModelValidator(ILogger<RelationalModelValidator> loggerFactory, IRelationalAnnotationProvider relationalExtensions, IRelationalTypeMapper typeMapper)
            : base(loggerFactory, relationalExtensions, typeMapper)
        {
        }
    }
}
