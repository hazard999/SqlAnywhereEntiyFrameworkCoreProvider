using System;
using System.Runtime.InteropServices;

namespace iAnywhere.Data.SQLAnywhere
{
    struct SAValue:IDisposable
    {
        public int Ordinal;
        public SADataItem Value;

        public SAValue(int ordinal, SADataItem value)
        {
            Ordinal = ordinal;
            Value = value;
        }

        public void Dispose()
        {
            Value.Dispose();
        }
    }

    static class SAValueExtensions
    {
        public static IntPtr ToIntPtr(this SAValue[] @params)
        {
            if (@params == null)
                return IntPtr.Zero;

            var recordSize = Marshal.SizeOf<SAValue>();
            var ptr = Marshal.AllocHGlobal(recordSize * @params.Length);
            var assignPrt = ptr;
            foreach (var param in @params)
            {
                Marshal.StructureToPtr(param, assignPrt, false);

                assignPrt = IntPtr.Add(assignPrt, recordSize);
            }

            return ptr;

        }
    }
}
