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
