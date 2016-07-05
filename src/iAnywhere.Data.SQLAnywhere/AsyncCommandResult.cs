using System;
using System.Data;
using System.Threading;

namespace iAnywhere.Data.SQLAnywhere
{
    internal sealed class AsyncCommandResult : IAsyncResult
    {
        internal IntPtr InputParmValues = IntPtr.Zero;
        private ManualResetEvent _waitHandle;
        private object _state;
        private bool _isCompleted;
        private AsyncCallback _callback;
        private AsyncCommandType _cmdType;
        internal int ParmCount;
        internal IntPtr ParmsDM;
        internal int InputParmCount;
        internal CommandBehavior Behavior;

        public object AsyncState
        {
            get
            {
                return _state;
            }
        }

        public WaitHandle AsyncWaitHandle
        {
            get
            {
                lock (this)
                {
                    if (_waitHandle == null)
                        _waitHandle = new ManualResetEvent(_isCompleted);
                    return _waitHandle;
                }
            }
        }

        public bool CompletedSynchronously
        {
            get
            {
                return false;
            }
        }

        public bool IsCompleted
        {
            get
            {
                return _isCompleted;
            }
        }

        internal AsyncCommandResult(AsyncCallback callback, object state, AsyncCommandType type)
        {
            _waitHandle = null;
            _state = state;
            _isCompleted = false;
            _callback = callback;
            _cmdType = type;
        }

        ~AsyncCommandResult()
        {
            if (_waitHandle == null)
                return;
            _waitHandle.Dispose();
        }

        internal void Complete()
        {
            lock (this)
            {
                if (_waitHandle != null)
                    _waitHandle.Set();
                //if (_callback != null)
                //    new Thread(new ThreadStart(this.StartCallback)).Start();
                _isCompleted = true;
            }
        }

        private void StartCallback()
        {
            _callback(this);
        }

        internal void CheckCommandType(AsyncCommandType type)
        {
            if (_cmdType != type)
            {
                Exception e = new InvalidOperationException(SARes.GetString(14976, FindCommandName(type), FindCommandName(_cmdType)));
                throw e;
            }
        }

        private string FindCommandName(AsyncCommandType type)
        {
            switch (type)
            {
                case AsyncCommandType.ExecuteReader:
                    return "EndExecuteReader";
                case AsyncCommandType.ExecuteNonQuery:
                    return "EndExecuteNonQuery";
                default:
                    return null;
            }
        }
    }
}
