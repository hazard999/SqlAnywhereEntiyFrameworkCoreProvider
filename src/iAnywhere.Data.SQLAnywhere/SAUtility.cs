using System;

namespace iAnywhere.Data.SQLAnywhere
{
    /// <summary>Summary description for SAUtility</summary>
    internal sealed class SAUtility
    {
        public static char[] GetCharArray(string str)
        {
            return (str + "\0").ToCharArray();
        }

        public static IntPtr GetUnmanagedString(string str)
        {
            if (str == null)
                return (IntPtr)0;
            char* chPtr = (char*)(void*)SAUnmanagedMemory.Alloc((str.Length + 1) * 2);
            char[] charArray = str.ToCharArray();
            for (int index = 0; index < charArray.Length; ++index)
                chPtr[index] = charArray[index];
            return (IntPtr)((void*)chPtr);
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
            {
                Exception e = new ArgumentException(SARes.GetString(25994, argName), argName);
                SATrace.Exception(e);
                throw e;
            }
        }

        public static void CheckArgumentNull(object argVal, string argName)
        {
            if (argVal == null)
            {
                ArgumentNullException argumentNullException = new ArgumentNullException(argName);
                SATrace.Exception(argumentNullException);
                throw argumentNullException;
            }
        }

        public static bool CompareString(string str1, string str2)
        {
            return string.Compare(str1, str2, StringComparison.OrdinalIgnoreCase) == 0;
        }
    }
}
