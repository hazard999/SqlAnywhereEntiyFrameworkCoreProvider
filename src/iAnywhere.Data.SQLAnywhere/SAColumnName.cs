using System;

namespace iAnywhere.Data.SQLAnywhere
{
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
