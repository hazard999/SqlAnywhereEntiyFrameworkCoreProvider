using System;
using System.Runtime.InteropServices;

namespace iAnywhere.Data.SQLAnywhere
{
    internal class SAUnmanagedMemory
    {
        private const int LMEM_ZEROINIT = 64;

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
