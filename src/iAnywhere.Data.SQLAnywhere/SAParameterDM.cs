
// Type: iAnywhere.Data.SQLAnywhere.SAParameterDM
// Assembly: iAnywhere.Data.SQLAnywhere.v4.0, Version=11.0.1.27424, Culture=neutral, PublicKeyToken=f222fc4333e0d400
// MVID: CC4F9F8C-E618-49D1-9147-C06A9EF53D1F
// Assembly location: C:\Program Files\SQL Anywhere 11\Assembly\V4\iAnywhere.Data.SQLAnywhere.v4.0.dll

using System;

namespace iAnywhere.Data.SQLAnywhere
{
  /// <summary>Summary description for SAParameterDM</summary>
  internal struct SAParameterDM
  {
    public int Ordinal;
    public IntPtr Name;
    public int Direction;
    public int IsNullable;
    public int Size;
    public int Precision;
    public int Scale;
    public SADataItem Value;

    public SAParameterDM(int ordinal, IntPtr name, int direction, int isNullable, int size, int precision, int scale, SADataItem value)
    {
            Ordinal = ordinal;
            Name = name;
            Direction = direction;
            IsNullable = isNullable;
            Size = size;
            Precision = precision;
            Scale = scale;
            Value = value;
    }
  }
}
