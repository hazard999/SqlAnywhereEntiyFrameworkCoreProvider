
// Type: iAnywhere.Data.SQLAnywhere.SAConnectionFactory
// Assembly: iAnywhere.Data.SQLAnywhere.v4.0, Version=11.0.1.27424, Culture=neutral, PublicKeyToken=f222fc4333e0d400
// MVID: CC4F9F8C-E618-49D1-9147-C06A9EF53D1F
// Assembly location: C:\Program Files\SQL Anywhere 11\Assembly\V4\iAnywhere.Data.SQLAnywhere.v4.0.dll

using System;
using System.Data.Common;

namespace iAnywhere.Data.SQLAnywhere
{
    public sealed class SAConnectionFactory
    {
        public DbConnection CreateConnection(string nameOrConnectionString)
        {
            SAUtility.CheckStringArgument(nameOrConnectionString, "nameOrConnectionString");
            string str = "name=";
            string connectionString = nameOrConnectionString;
            if (connectionString.IndexOf(str, StringComparison.OrdinalIgnoreCase) == 0)
                connectionString = connectionString.Substring(str.Length);
            if (connectionString.IndexOf('=') >= 0)
                return new SAConnection(connectionString);
            return new SAConnection("INT=Yes");
        }
    }
}
