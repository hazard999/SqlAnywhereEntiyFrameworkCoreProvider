
// Type: iAnywhere.SQLAnywhere.Server.SAServerSideConnection
// Assembly: iAnywhere.Data.SQLAnywhere.v4.0, Version=11.0.1.27424, Culture=neutral, PublicKeyToken=f222fc4333e0d400
// MVID: CC4F9F8C-E618-49D1-9147-C06A9EF53D1F
// Assembly location: C:\Program Files\SQL Anywhere 11\Assembly\V4\iAnywhere.Data.SQLAnywhere.v4.0.dll

using iAnywhere.Data.SQLAnywhere;
using System;

namespace iAnywhere.SQLAnywhere.Server
{
    public sealed class SAServerSideConnection
    {
        private static SAConnection _conn;

        public static SAConnection Connection
        {
            get
            {
                if (SAServerSideConnection._conn == null)
                    throw new InvalidOperationException(SARes.GetString(17448));
                return SAServerSideConnection._conn;
            }
        }

        private static void SetConnection(SAConnection conn)
        {
            SAServerSideConnection._conn = conn;
        }
    }
}
