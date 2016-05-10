using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Metadata;

namespace SqlAnywhereProvider.Storage
{
    class SqlAnywhereTypeMapper : RelationalTypeMapper
    {
        protected override string GetColumnType(IProperty property)
        {
            throw new NotImplementedException();
        }

        protected override IReadOnlyDictionary<Type, RelationalTypeMapping> GetSimpleMappings()
        {
            throw new NotImplementedException();
        }

        protected override IReadOnlyDictionary<string, RelationalTypeMapping> GetSimpleNameMappings()
        {
            throw new NotImplementedException();
        }
    }
}
