using System;

namespace iAnywhere.Data.SQLAnywhere
{
    public sealed class SAInfoMessageEventArgs : EventArgs
    {
        private int _objectId = s_CurrentId++;
        private int _nativeError;
        private string _msg;
        private SAMessageType _msgType;
        private static int s_CurrentId;

        public SAMessageType MessageType
        {
            get
            {
                return _msgType;
            }
        }

        public SAErrorCollection Errors
        {
            get
            {
                return null;
            }
        }

        public string Message
        {
            get
            {
                return _msg;
            }
        }

        public string Source
        {
            get
            {
                return "SQL Anywhere .NET Data Provider";
            }
        }

        public int NativeError
        {
            get
            {
                return _nativeError;
            }
        }

        internal SAInfoMessageEventArgs(SAMessageType msgType, int nativeError, string msg)
        {
            _msgType = msgType;
            _nativeError = nativeError;
            _msg = msg;
        }

        public override string ToString()
        {
            return Message;
        }
    }
}
