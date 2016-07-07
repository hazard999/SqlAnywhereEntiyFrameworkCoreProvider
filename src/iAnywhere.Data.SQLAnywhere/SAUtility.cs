using System;
using System.Runtime.InteropServices;

namespace iAnywhere.Data.SQLAnywhere
{
    sealed class SAUtility
    {
        public static char[] GetCharArray(string str)
        {
            return (str + "\0").ToCharArray();
        }

        public static IntPtr GetUnmanagedString(string str)
        {
            if (str == null)
                return IntPtr.Zero;

            var chPtr = SAUnmanagedMemory.Alloc((str.Length + 1) * 2);
            var charArray = str.ToCharArray();
            Marshal.Copy(charArray, 0, chPtr, charArray.Length);
            return chPtr;            
        }

        public static string EscapeQuotationMarks(char quotationMark, string str)
        {
            return str.Replace(new string(quotationMark, 1), new string(quotationMark, 2));
        }

        public static bool IsValidId(int id)
        {
            return id >= 1;
        }

        public static void CheckStringArgument(string argVal, string argName)
        {
            if (string.IsNullOrEmpty(argVal) || argVal.Trim().Length < 1)
                throw new ArgumentException(SARes.GetString(25994, argName), argName);
        }

        public static void CheckArgumentNull(object argVal, string argName)
        {
            if (argVal == null)
                throw new ArgumentNullException(argName);
        }

        public static bool CompareString(string str1, string str2)
        {
            return string.Compare(str1, str2, StringComparison.OrdinalIgnoreCase) == 0;
        }
    }
}
