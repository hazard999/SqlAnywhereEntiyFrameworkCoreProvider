using System;
using System.Collections;
using System.Text;

namespace iAnywhere.Data.SQLAnywhere
{
    public sealed class SAErrorCollection : ICollection, IEnumerable
    {
        private const int MESSAGE_LEN = 128;
        private const int SQL_STATE_LEN = 8;
        private ArrayList _errors;

        /// <summary>
        ///     <para>Returns the number of errors in the collection.</para>
        /// </summary>
        public int Count
        {
            get
            {
                return _errors.Count;
            }
        }

        /// <summary>
        ///     <para>Returns the error at the specified index.</para>
        /// </summary>
        /// <value>An SAError object that contains the error at the specified index.</value>
        /// <param name="index">The zero-based index of the error to retrieve.</param>
        /// <seealso cref="T:iAnywhere.Data.SQLAnywhere.SAError" />
        public SAError this[int index]
        {
            get
            {
                return (SAError)_errors[index];
            }
        }

        bool ICollection.IsSynchronized
        {
            get
            {
                return false;
            }
        }

        object ICollection.SyncRoot
        {
            get
            {
                return this;
            }
        }

        internal SAErrorCollection()
        {
            _errors = new ArrayList();
        }

        internal static SAErrorCollection GetErrors(int idEx)
        {
            int count = 0;
            int nativeError = 0;
            string msg = null;
            string sqlState = null;
            SAErrorCollection.FreeException(PInvokeMethods.AsaException_GetErrorCount(idEx, ref count));
            SAErrorCollection saErrorCollection = new SAErrorCollection();
            for (int errorIndex = 0; errorIndex < count; ++errorIndex)
            {
                SAErrorCollection.GetErrorInfo(idEx, errorIndex, ref nativeError, ref sqlState, ref msg);
                SAError saError = new SAError(nativeError, msg, sqlState);
                saErrorCollection._errors.Add(saError);
            }
            SAErrorCollection.FreeException(idEx);
            return saErrorCollection;
        }

        internal static void FreeException(int idEx)
        {
            if (!SAException.IsException(idEx))
                return;
            SAErrorCollection.FreeException(PInvokeMethods.AsaException_Fini(idEx));
        }

        internal static string GetErrorMessage(int idEx)
        {
            int count = 0;
            int nativeError = 0;
            string msg = null;
            string sqlState = null;
            SAErrorCollection.FreeException(PInvokeMethods.AsaException_GetErrorCount(idEx, ref count));
            StringBuilder stringBuilder = new StringBuilder("");
            for (int errorIndex = 0; errorIndex < count; ++errorIndex)
            {
                SAErrorCollection.GetErrorInfo(idEx, errorIndex, ref nativeError, ref sqlState, ref msg);
                if (stringBuilder.Length > 0)
                    stringBuilder.Append(", ");
                stringBuilder.Append(msg);
            }
            SAErrorCollection.FreeException(idEx);
            return stringBuilder.ToString();
        }

        private static void GetErrorInfo(int idEx, int errorIndex, ref int nativeError, ref string sqlState, ref string msg)
        {
            int lenSqlState = 0;
            int bufLenSqlState = 8;
            char[] chArray1 = new char[bufLenSqlState];
            int lenMsg = 0;
            int bufLenMsg = 128;
            char[] chArray2 = new char[bufLenMsg];

            SAErrorCollection.FreeException(PInvokeMethods.AsaException_GetErrorInfo(idEx, errorIndex, ref nativeError, chArray1, bufLenSqlState, ref lenSqlState, chArray2, bufLenMsg, ref lenMsg));
            bool flag = false;
            if (bufLenSqlState < lenSqlState)
            {
                bufLenSqlState = lenSqlState + 1;
                chArray1 = new char[bufLenSqlState];
                flag = true;
            }
            if (bufLenMsg < lenMsg)
            {
                bufLenMsg = lenMsg + 1;
                chArray2 = new char[bufLenMsg];
                flag = true;
            }
            if (flag)
            {
                SAErrorCollection.FreeException(PInvokeMethods.AsaException_GetErrorInfo(idEx, errorIndex, ref nativeError, chArray1, bufLenSqlState, ref lenSqlState, chArray2, bufLenMsg, ref lenMsg));
            }
            sqlState = new string(chArray1, 0, lenSqlState);
            msg = new string(chArray2, 0, lenMsg);
        }

        /// <summary>
        ///     <para>Copies the elements of the SAErrorCollection into an array, starting at the given index within the array.</para>
        /// </summary>
        /// <param name="array">The array into which to copy the elements.</param>
        /// <param name="index">The starting index of the array.</param>
        public void CopyTo(Array array, int index)
        {
            _errors.CopyTo(array, index);
        }

        /// <summary>
        ///     <para>Returns an enumerator that iterates through the SAErrorCollection.</para>
        /// </summary>
        /// <returns>
        /// <para>An <see cref="T:System.Collections.IEnumerator" /> for the SAErrorCollection. </para>
        ///    </returns>
        public IEnumerator GetEnumerator()
        {
            return _errors.GetEnumerator();
        }

        internal void Add(SAError asaError)
        {
            _errors.Add(asaError);
        }
    }
}
