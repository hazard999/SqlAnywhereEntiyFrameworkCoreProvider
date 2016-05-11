using System.Text;
using Microsoft.EntityFrameworkCore.Storage;

namespace EntityFrameworkCore.RelationalProviderStarter.Storage
{
    public class SqlAnywhereSqlGenerationHelper : RelationalSqlGenerationHelper
    {
        public override string GenerateParameterName(string name)
        {
            return base.GenerateParameterName(name);
        }

        public override void GenerateParameterName(StringBuilder builder, string name)
        {
            builder.Append(":" + name);
            //base.GenerateParameterName(builder, name);
        }
    }
}