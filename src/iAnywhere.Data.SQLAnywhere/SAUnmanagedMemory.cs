using System;

namespace iAnywhere.Data.SQLAnywhere
{
    internal class SAUnmanagedMemory
    {
        private const int LMEM_ZEROINIT = 64;

        public static IntPtr Alloc(int size)
        {
            return PInvokeMethods.LocalAlloc(64U, new UIntPtr(Convert.ToUInt32(size)));
        }

        public static void Free(IntPtr hMem)
        {
            if (!(hMem != IntPtr.Zero))
                return;
            PInvokeMethods.LocalFree(hMem);
        }
    }
}
