using System;
using System.Runtime.InteropServices;

namespace iAnywhere.Data.SQLAnywhere
{
    struct SAParameterDM : IDisposable
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

        public void Dispose()
        {
            SAUnmanagedMemory.Free(Name);
            Value.Dispose();
        }
    }

    static class SAParameterDMExtensions
    {
        public static IntPtr ToIntPtr(this SAParameterDM[] @params)
        {
            if (@params == null)
                return IntPtr.Zero;

            var recordSize = Marshal.SizeOf<SAParameterDM>();
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
