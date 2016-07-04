
// Type: iAnywhere.Data.SQLAnywhere.SAColumnName
// Assembly: iAnywhere.Data.SQLAnywhere.v4.0, Version=11.0.1.27424, Culture=neutral, PublicKeyToken=f222fc4333e0d400
// MVID: CC4F9F8C-E618-49D1-9147-C06A9EF53D1F
// Assembly location: C:\Program Files\SQL Anywhere 11\Assembly\V4\iAnywhere.Data.SQLAnywhere.v4.0.dll

using System;

namespace iAnywhere.Data.SQLAnywhere
{
    /// <summary>Summary description for SAColumnName</summary>
    internal struct SAColumnName
    {
        public int Ordinal;
        public int SADataType;
        public IntPtr ColumnName;

        public SAColumnName(int ordinal, int saDataType, IntPtr columnName)
        {
            Ordinal = ordinal;
            SADataType = saDataType;
            ColumnName = columnName;
        }
    }
}
