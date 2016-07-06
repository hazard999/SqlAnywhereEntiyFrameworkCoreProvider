namespace iAnywhere.Data.SQLAnywhere
{
    public sealed class SAError
    {
        int _nativeError;
        string _message;
        string _sqlState;

        public int NativeError
        {
            get
            {
                return _nativeError;
            }
        }

        public string Message
        {
            get
            {
                return _message;
            }
        }

        public string SqlState
        {
            get
            {
                return _sqlState;
            }
        }

        public string Source
        {
            get
            {
                return "SQL Anywhere .NET Data Provider";
            }
        }

        internal SAError(int nativeError, string message, string sqlState)
        {
            _nativeError = nativeError;
            _message = message;
            _sqlState = sqlState;
        }

        public override string ToString()
        {
            return GetType().Name + ":" + _message;
        }
    }
}
