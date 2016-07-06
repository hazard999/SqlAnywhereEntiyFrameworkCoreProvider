using System;
using System.Runtime.InteropServices;

namespace iAnywhere.Data.SQLAnywhere
{
    class SAUnmanagedMemory
    {
        public static IntPtr Alloc(int size)
        {
            return Marshal.AllocHGlobal(size);
        }

        public static void Free(IntPtr hMem)
        {
            if (!(hMem != IntPtr.Zero))
                return;

            Marshal.FreeHGlobal(hMem);
        }
    }
}
