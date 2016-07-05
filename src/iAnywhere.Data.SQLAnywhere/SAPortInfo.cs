using System;

namespace iAnywhere.Data.SQLAnywhere
{
    struct SAPortInfo
    {
        public IntPtr Type;
        public IntPtr Options;

        public SAPortInfo(IntPtr type, IntPtr options)
        {
            Type = type;
            Options = options;
        }
    }
}
