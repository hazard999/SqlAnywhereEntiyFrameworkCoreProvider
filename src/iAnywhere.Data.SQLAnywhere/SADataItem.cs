using System;
using System.Runtime.InteropServices;

namespace iAnywhere.Data.SQLAnywhere
{    
    struct SADataItem : IDisposable
    {
        public int SADataType;
        public int Length;
        public int IsNull;
        public int IsDefault;
        public IntPtr Value;
        public int BufferLength;

        public SADataItem(int saDataType, int length, int isNull, int isDefault, IntPtr val, int bufferLength)
        {
            SADataType = saDataType;
            Length = length;
            IsNull = isNull;
            IsDefault = isDefault;
            Value = val;
            BufferLength = bufferLength;
        }

        public void Dispose()
        {
            SAUnmanagedMemory.Free(Value);
        }
    }
}
