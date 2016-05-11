using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Data;

namespace SqlAnywhereProvider.Storage
{
    class SqlAnywhereTypeMapper : RelationalTypeMapper
    {
        private readonly Dictionary<string, RelationalTypeMapping> _simpleNameMappings;
        private readonly Dictionary<Type, RelationalTypeMapping> _simpleMappings;

        private readonly RelationalTypeMapping _int = new RelationalTypeMapping("integer", typeof(int), DbType.Int32);
        private readonly RelationalTypeMapping _string = new RelationalTypeMapping("varchar", typeof(string), DbType.String);

        public SqlAnywhereTypeMapper()
        {
            _simpleNameMappings
                = new Dictionary<string, RelationalTypeMapping>(StringComparer.OrdinalIgnoreCase)
                {
                    { "integer", _int },
                    { "varchar", _string },
                };

            _simpleMappings = new Dictionary<Type, RelationalTypeMapping>
                {
                    { typeof(int), _int },
                    { typeof(string), _string},
                };
        }

        protected override string GetColumnType(IProperty property)
        {
            if (property.ClrType == typeof(int))
                return "integer";

            if (property.ClrType == typeof(string))
                return "varchar";

            return string.Empty;
        }

        protected override IReadOnlyDictionary<Type, RelationalTypeMapping> GetSimpleMappings()
        {
            return _simpleMappings;
        }

        protected override IReadOnlyDictionary<string, RelationalTypeMapping> GetSimpleNameMappings()
        {
            return _simpleNameMappings;
        }
    }
}
