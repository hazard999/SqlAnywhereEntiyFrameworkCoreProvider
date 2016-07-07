using System;

namespace iAnywhere.Data.SQLAnywhere
{
    sealed class SAUnmanagedDll
    {
        static IntPtr s_hModule = IntPtr.Zero;
        static SAUnmanagedDll s_instance = null;
        static string s_dllPath;

        public static SAUnmanagedDll Instance
        {
            get
            {
                lock (typeof(SAUnmanagedDll))
                {
                    if (s_instance == null)
                        s_instance = new SAUnmanagedDll();
                    return s_instance;
                }
            }
        }

        SAUnmanagedDll()
        {
            LoadDll();
            PInvokeMethods.Unmanaged_Init();
        }

        ~SAUnmanagedDll()
        {
            if (s_instance == null)
                return;
            PInvokeMethods.Unmanaged_Fini();
        }

        static void UnloadDll()
        {
            if (!(s_hModule != IntPtr.Zero))
                return;
            PInvokeMethods.FreeLibrary(s_hModule);
        }

        static void LoadDll()
        {
            s_dllPath = @"C:\Users\admin\Source\Repos\SqlAnywhereEntiyFrameworkCoreProvider\src\iAnywhere.Data.SQLAnywhere\dbdata11.dll";

            s_hModule = PInvokeMethods.LoadLibrary(s_dllPath);
        }
    }
}
