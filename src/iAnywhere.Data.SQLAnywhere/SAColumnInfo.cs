
// Type: iAnywhere.Data.SQLAnywhere.SAColumnInfo
// Assembly: iAnywhere.Data.SQLAnywhere.v4.0, Version=11.0.1.27424, Culture=neutral, PublicKeyToken=f222fc4333e0d400
// MVID: CC4F9F8C-E618-49D1-9147-C06A9EF53D1F
// Assembly location: C:\Program Files\SQL Anywhere 11\Assembly\V4\iAnywhere.Data.SQLAnywhere.v4.0.dll

using System;

namespace iAnywhere.Data.SQLAnywhere
{
  /// <summary>Summary description for SAColumnInfo</summary>
  internal struct SAColumnInfo
  {
    public IntPtr ColumnName;
    public int ColumnOrdinal;
    public int ColumnSize;
    public int NumericPrecision;
    public int NumericScale;
    public int IsUnique;
    public int IsKey;
    public IntPtr BaseServerName;
    public IntPtr BaseCatalogName;
    public IntPtr BaseColumnName;
    public IntPtr BaseSchemaName;
    public IntPtr BaseTableName;
    public int AllowDBNull;
    public int SADataType;
    public int IsAliased;
    public int IsExpression;
    public int IsIdentity;
    public int IsAutoIncrement;
    public int IsRowVersion;
    public int IsHidden;
    public int IsLong;
    public int IsReadOnly;

    public SAColumnInfo(IntPtr columnName, int columnOrdinal, int columnSize, int numericPrecision, int numericScale, int isUnique, int isKey, IntPtr baseServerName, IntPtr baseCatalogName, IntPtr baseColumnName, IntPtr baseSchemaName, IntPtr baseTableName, int allowDBNull, int saDataType, int isAliased, int isExpression, int isIdentity, int isAutoIncrement, int isRowVersion, int isHidden, int isLong, int isReadOnly)
    {
            ColumnName = columnName;
            ColumnOrdinal = columnOrdinal;
            ColumnSize = columnSize;
            NumericPrecision = numericPrecision;
            NumericScale = numericScale;
            IsUnique = isUnique;
            IsKey = isKey;
            BaseServerName = baseServerName;
            BaseCatalogName = baseCatalogName;
            BaseColumnName = baseColumnName;
            BaseSchemaName = baseSchemaName;
            BaseTableName = baseTableName;
            AllowDBNull = allowDBNull;
            SADataType = saDataType;
            IsAliased = isAliased;
            IsExpression = isExpression;
            IsIdentity = isIdentity;
            IsAutoIncrement = isAutoIncrement;
            IsRowVersion = isRowVersion;
            IsHidden = isHidden;
            IsLong = isLong;
            IsReadOnly = isReadOnly;
    }
  }
}
