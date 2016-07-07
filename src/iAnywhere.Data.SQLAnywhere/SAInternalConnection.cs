using System;

namespace iAnywhere.Data.SQLAnywhere
{
    internal sealed class SAInternalConnection
    {
        private const int SERVER_VERSION_LEN = 16;
        private const int DATABASE_LEN = 64;
        private const int DATASOURCE_LEN = 64;
        private int _idConn;
        private bool _pooled;
        private bool _disposed;
        private bool _enlisted;
        private bool _busy;
        private string _connStr;
        private DateTime _creationTime;
        private WeakReference _dtcTran;
        private SAConnectionGroup _connGroup;
        private SATransaction _asaTran;
        private WeakReference _parent;

        public SATransaction Transaction
        {
            get
            {
                return _asaTran;
            }
        }

        public SAConnection Parent
        {
            get
            {
                if (_parent != null && _parent.IsAlive)
                    return (SAConnection)_parent.Target;
                return null;
            }
            set
            {
                _parent = new WeakReference(value);
            }
        }

        public bool Busy
        {
            get
            {
                return _busy;
            }
            set
            {
                _busy = value;
            }
        }

        public bool IsAlive
        {
            get
            {
                bool isAlive = false;
                if (SAUtility.IsValidId(_idConn))
                    SAException.FreeException(PInvokeMethods.AsaConnection_IsAlive(_idConn, ref isAlive));
                if (!isAlive)
                    FreeConnection(false);
                return isAlive;
            }
        }

        public bool Pooled
        {
            get
            {
                return _pooled;
            }
            set
            {
                _pooled = value;
            }
        }

        public bool Enlisted
        {
            get
            {
                return _enlisted;
            }
            set
            {
                _enlisted = value;
            }
        }

        public bool Disposed
        {
            get
            {
                return _disposed;
            }
            set
            {
                _disposed = true;
            }
        }

        public bool TransactionIsOver
        {
            get
            {
                return _dtcTran == null || _dtcTran.Target == null;
            }
        }

        public string ConnectionString
        {
            get
            {
                return _connStr;
            }
        }

        public SAConnectionGroup ConnectionGroup
        {
            get
            {
                return _connGroup;
            }
            set
            {
                _connGroup = value;
            }
        }

        public double LifeTime
        {
            get
            {
                return (DateTime.Now - _creationTime).TotalSeconds;
            }
        }

        public int ConnectionId
        {
            get
            {
                return _idConn;
            }
        }

        public SAInternalConnection(SAConnection parent, bool pooled, object dtcTran, string connectionString, SAConnectionGroup connectionGroup)
        {
            Init();
            _pooled = pooled;
            _connStr = connectionString;
            _connGroup = connectionGroup;
            _dtcTran = dtcTran == null ? null : new WeakReference(dtcTran);
            _parent = parent == null ? null : new WeakReference(parent);
            _idConn = 0;
            OpenConnection();
            _creationTime = DateTime.Now;
        }

        ~SAInternalConnection()
        {
            if (_connGroup != null)
                _connGroup.RemoveConnection(this);
            if (_enlisted)
                return;
            FreeConnection(true);
        }

        private void Init()
        {
            _busy = false;
            _disposed = false;
            _enlisted = false;
            _asaTran = null;
            _dtcTran = null;
            _parent = null;
        }

        private void OpenConnection()
        {
            CheckException(PInvokeMethods.AsaConnection_Open(ref _idConn, _connStr), true);
        }

        public void Close()
        {
            if (_connGroup != null)
                _connGroup.RemoveConnection(this);
            RollbackTransaction();
            FreeConnection(true);
            GC.SuppressFinalize(this);
        }

        private void RollbackTransaction()
        {
            if (_asaTran == null || !_asaTran.IsValid)
                return;
            _asaTran.Rollback();
            _asaTran = null;
        }

        public SATransaction BeginTransaction(SAIsolationLevel isolationLevel)
        {
            int idTrans = BeginNewTransaction(isolationLevel);
            _asaTran = new SATransaction(this, isolationLevel, idTrans);
            return _asaTran;
        }

        public int BeginNewTransaction(SAIsolationLevel isolationLevel)
        {
            int idTrans = 0;
            CheckException(PInvokeMethods.AsaConnection_BeginTransaction(_idConn, (int)isolationLevel, ref idTrans), false);
            return idTrans;
        }

        public void ReturnToPool()
        {
            RollbackTransaction();
            CheckException(PInvokeMethods.AsaConnection_CloseDataReaders(_idConn), true);
            Init();
        }

        public void CheckPooledConnection()
        {
            if (IsAlive)
                return;
            OpenConnection();
            _creationTime = DateTime.Now;
        }

        private void FreeConnection(bool throwEx)
        {
            Init();
            if (!SAUtility.IsValidId(_idConn))
                return;
            try
            {
                CheckException(PInvokeMethods.AsaConnection_Close(_idConn), false);
            }
            catch (SAException ex)
            {
                if (throwEx && ex.NativeError != -85 && ex.NativeError != -308)
                    throw ex;
            }
            catch (Exception ex)
            {
                if (throwEx)
                    throw ex;
            }
            finally
            {
                _idConn = 0;
            }
        }

        private void CheckException(int idEx, bool freeConn)
        {
            if (SAException.IsException(idEx))
            {
                if (freeConn && SAUtility.IsValidId(_idConn))
                {
                    SAException.FreeException(PInvokeMethods.AsaConnection_Close(_idConn));
                    _idConn = 0;
                }
                throw SAException.CreateInstance(idEx);
            }
        }

        public void SetMessageCallback(SAInfoMessageDelegate msgDelegate)
        {
            CheckException(PInvokeMethods.AsaConnection_SetMessageCallback(_idConn, msgDelegate), false);
        }

        public void CleanUpTransaction()
        {
            _asaTran = null;
        }
    }
}
