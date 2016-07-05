using System;
using System.Runtime.InteropServices;

namespace iAnywhere.Data.SQLAnywhere
{
    struct SAValue
    {
        public int Ordinal;
        public SADataItem Value;

        public SAValue(int ordinal, SADataItem value)
        {
            Ordinal = ordinal;
            Value = value;
        }
    }

    static class SAValueExtensions
    {
        public static IntPtr ToIntPtr(this SAValue[] @params)
        {
            if (@params == null)
                return IntPtr.Zero;

            var ptr = new IntPtr();

            Marshal.StructureToPtr(@params, ptr, true);

            return ptr;

        }
    }
}
