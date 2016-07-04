using System;
using System.Data.Common;
using System.Text;

namespace iAnywhere.Data.SQLAnywhere
{
    public class SAException : DbException
    {
        private SAErrorCollection _errors;

        public SAErrorCollection Errors
        {
            get
            {
                return _errors;
            }
        }

        public override string Message
        {
            get
            {
                StringBuilder stringBuilder = new StringBuilder();
                for (int index = 0; index < _errors.Count; ++index)
                {
                    stringBuilder.Append(_errors[index].Message);
                    if (index < _errors.Count - 1)
                        stringBuilder.Append("\r\n");
                }
                return stringBuilder.ToString();
            }
        }

        public override string Source
        {
            get
            {
                return _errors[0].Source;
            }
        }

        public int NativeError
        {
            get
            {
                return _errors[0].NativeError;
            }
        }

        private SAException()
        {
        }

        internal SAException(string message)
        {
            _errors = new SAErrorCollection();
            _errors.Add(new SAError(0, message, "00000"));
        }

        internal static bool IsException(int idEx)
        {
            return idEx >= 1;
        }

        internal static void CheckException(int idEx)
        {
            if (SAException.IsException(idEx))
            {
                SAException saException = new SAException();
                saException._errors = SAErrorCollection.GetErrors(idEx);
                throw saException;
            }
        }

        internal static SAException CreateInstance(int idEx)
        {
            return new SAException() { _errors = SAErrorCollection.GetErrors(idEx) };
        }

        internal static void FreeException(int idEx)
        {
            SAErrorCollection.FreeException(idEx);
        }
    }
}
