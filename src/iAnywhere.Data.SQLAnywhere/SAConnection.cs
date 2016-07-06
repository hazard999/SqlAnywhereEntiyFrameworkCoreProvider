using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Text.RegularExpressions;

namespace iAnywhere.Data.SQLAnywhere
{
    public sealed class SAConnection : DbConnection
    {
        bool _showConnFormOnFail = true;
        static SAUnmanagedDll s_unmanagedDll = SAUnmanagedDll.Instance;
        static int s_currentId = 0;
        static int s_openCount = 0;
        static string[] s_keyValuePatterns = { "((;|^)(\\s)*{0}(\\s)*=(\\s)*{{(\\s|\\S)*}}(\\s)*([^;])*)", "((;|^)(\\s)*{0}(\\s)*=(\\s)*'(('')|[^'])*'(\\s)*([^;])*)", "((;|^)(\\s)*{0}(\\s)*=(\\s)*\"((\"\")|[^\"])*\"(\\s)*([^;])*)", "((;|^)(\\s)*{0}(\\s)*=([^;])*([^;])*)" };
        static SAConnectionPoolManager s_poolManager = new SAConnectionPoolManager();
        static Hashtable s_connStrCacheTable = new Hashtable();
        static Hashtable s_asaConnStrCached = new Hashtable();
        bool _isPooled;
        string _connStr;
        string _asaConnStr;
        string _initString;
        string _svrVer;
        Hashtable _connOpts;
        SAInternalConnection _conn;
        SACommand _asyncCmd;
        ConnectionState _state;
        int _objectId;
        bool _isServerSideConnection;

        private bool IsServerSideConnection
        {
            get
            {
                return _isServerSideConnection;
            }
            set
            {
                _isServerSideConnection = value;
            }
        }

        internal SACommand AsyncCommand
        {
            get
            {
                return _asyncCmd;
            }
            set
            {
                _asyncCmd = value;
            }
        }

        internal SATransaction Transaction
        {
            get
            {
                if (_conn == null)
                    return null;
                return _conn.Transaction;
            }
        }

        internal string SAConnectionString
        {
            get
            {
                return _asaConnStr;
            }
        }

        public string InitString
        {
            get
            {
                return _initString;
            }
            set
            {
                _initString = value;
            }
        }

        public override string ConnectionString
        {
            get
            {
                if (_connStr == null)
                    return "";

                if (SAConnectionOptions.GetAdoDotNetTesting(_connOpts))
                {
                    string asaConnStr;
                    Hashtable removedOptions;
                    AdaptToSAConnectionString(_connStr, out asaConnStr, out removedOptions);
                    StringBuilder stringBuilder = new StringBuilder(asaConnStr);
                    stringBuilder.Append("[");
                    int num = 0;
                    foreach (string connectionOption in SAConnectionOptions.s_nonSAConnectionOptions)
                    {
                        if (removedOptions.ContainsKey(connectionOption))
                        {
                            if (num > 0)
                                stringBuilder.Append(";");
                            stringBuilder.Append(connectionOption);
                            stringBuilder.Append("=");
                            stringBuilder.Append(removedOptions[connectionOption]);
                            ++num;
                        }
                    }
                    stringBuilder.Append("]");
                    return stringBuilder.ToString();
                }
                if (SAConnectionOptions.GetPersistSecurityInfo(_connOpts))
                    return _connStr;
                string connStrOut;
                string removedKey;
                string removedValue;
                RemoveKeyValuesFromString(_connStr, SAConnectionOptions.s_passwordKeys, false, out connStrOut, out removedKey, out removedValue);
                return connStrOut;
            }
            set
            {
                if (_state == ConnectionState.Open)
                    throw new InvalidOperationException(SARes.GetString(10981, "ConnectionString"));

                if (value == null || value.Trim().Length < 1)
                {
                    _connStr = "";
                    _connOpts = null;
                }
                else
                {
                    if (s_connStrCacheTable.Contains(value))
                    {
                        _connOpts = (Hashtable)s_connStrCacheTable[value];
                        _asaConnStr = (string)s_asaConnStrCached[value];
                    }
                    else
                    {
                        _connOpts = ParseConnectionString(value);
                        lock (s_connStrCacheTable.SyncRoot)
                        {
                            s_connStrCacheTable[value] = _connOpts;
                            s_asaConnStrCached[value] = _asaConnStr;
                        }
                    }
                    _connStr = value;
                }
            }
        }

        bool ShowConnectionFormOnFail
        {
            get
            {
                return _showConnFormOnFail;
            }
            set
            {
                _showConnFormOnFail = value;
            }
        }

        public override int ConnectionTimeout
        {
            get
            {
                return SAConnectionOptions.GetConnectionTimeout(_connOpts);
            }
        }

        public override string Database
        {
            get
            {
                if (_state != ConnectionState.Open)
                    return SAConnectionOptions.GetDatabase(_connOpts);
                SACommand saCommand = new SACommand("SELECT CURRENT DATABASE", this, Transaction);
                string str = (string)saCommand.ExecuteScalar();
                saCommand.Dispose();
                return str;
            }
        }

        public override string DataSource
        {
            get
            {
                if (_state != ConnectionState.Open)
                    return SAConnectionOptions.GetDataSource(_connOpts);
                SACommand saCommand = new SACommand("SELECT @@servername", this, Transaction);
                string str = (string)saCommand.ExecuteScalar();
                saCommand.Dispose();
                return str;
            }
        }

        public override string ServerVersion
        {
            get
            {
                if (_state == ConnectionState.Closed)
                {
                    throw new InvalidOperationException(SARes.GetString(10982));
                }
                if (_svrVer != null)
                    return GetNormalizedVersion(_svrVer);
                return _svrVer;
            }
        }

        internal static string GetNormalizedVersion(string verStr)
        {
            char[] chArray = { '.', '/' };
            string[] strArray = Regex.Match(verStr, "([0-9]{1,2})(.[0-9]{1,2})(.[0-9]{1,2})(.[0-9]{1,4})").Value.Split(chArray);
            return string.Format("{0}.{1}.{2}.{3}", GetNormalizedVersion(strArray[0], 2), GetNormalizedVersion(strArray[1], 2), GetNormalizedVersion(strArray[2], 2), GetNormalizedVersion(strArray[3], 4));
        }

        internal static string GetNormalizedVersion(string ver, int len)
        {
            return new string('0', len - ver.Length) + ver;
        }


        public override ConnectionState State
        {
            get
            {
                if (_state == ConnectionState.Open && (_conn == null || !_conn.IsAlive))
                    Close();
                return _state;
            }
        }

        bool Enlisted
        {
            get
            {
                if (ConnectionState.Open != _state)
                    return false;
                return _conn.Enlisted;
            }
        }

        internal SAInternalConnection InternalConnection
        {
            get
            {
                return _conn;
            }
        }

        public SAConnection()
        {
            Init();
        }

        public SAConnection(string connectionString)
        {
            Init();
            try
            {
                ConnectionString = connectionString;
            }
            catch (ArgumentException ex)
            {
                int length = ex.Message.IndexOf("\r\n");
                if (length >= 0)
                    throw new ArgumentException(ex.Message.Substring(0, length), "connectionString");
                throw new ArgumentException(ex.Message, "connectionString");
            }
        }

        static int GetPoolCount()
        {
            return s_poolManager.GetPoolCount();
        }

        static int GetPooledConnectionCount()
        {
            return s_poolManager.GetConnectionCount();
        }

        public override void Close()
        {
            Dispose(true);
        }

        /// <summary>
        ///     <para>Frees the resources associated with the object.</para>
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (_state != ConnectionState.Open)
                return;
            if (_isServerSideConnection)
            {
                throw new InvalidOperationException(SARes.GetString(17447));
            }
            _conn.SetMessageCallback(null);
            if (!disposing)
                return;
            if (_conn.Enlisted)
                _conn.Disposed = true;
            else if (_conn.Pooled)
                s_poolManager.ReturnConnection(_conn);
            else
                _conn.Close();
            FireStateChange(ConnectionState.Open, ConnectionState.Closed);
            _state = ConnectionState.Closed;
            _conn = null;
        }

        void Init()
        {
            _initString = null;
            _connStr = null;
            _connOpts = null;
            _conn = null;
            _objectId = s_currentId++;
            _state = ConnectionState.Closed;
        }

        internal ConnectionState GetConnectionState()
        {
            return _state;
        }

        public new SATransaction BeginTransaction(System.Data.IsolationLevel isolationLevel)
        {
            return BeginTransaction(SATransaction.ConvertToSAIsolationLevel(isolationLevel));
        }

        protected override DbTransaction BeginDbTransaction(IsolationLevel isolationLevel)
        {
            return BeginTransaction(isolationLevel);
        }

        public SATransaction BeginTransaction(SAIsolationLevel isolationLevel)
        {
            return _BeginTransaction(isolationLevel);
        }

        SATransaction _BeginTransaction(SAIsolationLevel isolationLevel)
        {
            if (_state != ConnectionState.Open)
            {
                Exception e = new InvalidOperationException(SARes.GetString(10982));
                throw e;
            }
            if (_conn.Transaction != null)
            {
                Exception e = new InvalidOperationException(SARes.GetString(10998));
                throw e;
            }
            if (isolationLevel == SAIsolationLevel.Chaos || isolationLevel == SAIsolationLevel.Unspecified)
            {
                Exception e = new ArgumentException(SARes.GetString(17016), "isolationLevel");
                throw e;
            }
            if (_conn.Enlisted)
            {
                Exception e = new InvalidOperationException(SARes.GetString(17886));
                throw e;
            }
            SATransaction saTransaction = _conn.BeginTransaction(isolationLevel);
            saTransaction.SetConnection(this);
            return saTransaction;
        }

        public override void ChangeDatabase(string databaseName)
        {
            if (_state != ConnectionState.Open)
            {
                Exception e = new InvalidOperationException(SARes.GetString(10983));
                throw e;
            }
            if (databaseName == null || databaseName.Trim().Length < 1)
            {
                Exception e = new ArgumentException(SARes.GetString(10984), nameof(databaseName));
                throw e;
            }
            Close();
            string connStrOut;
            string removedKey;
            string removedValue;
            RemoveKeyValuesFromString(_connStr, SAConnectionOptions.s_databaseKeys, false, out connStrOut, out removedKey, out removedValue);
            StringBuilder stringBuilder = new StringBuilder(connStrOut);
            if (databaseName.IndexOf('\\') >= 0)
                stringBuilder.Append(";dbf=");
            else
                stringBuilder.Append(";dbn=");
            stringBuilder.Append('\'');
            stringBuilder.Append(databaseName.Trim().Replace("'", "''"));
            stringBuilder.Append('\'');
            ConnectionString = stringBuilder.ToString();
            Open();
        }

        public override void Open()
        {
            ++s_openCount;

            if (_state == ConnectionState.Open)
            {
                throw new InvalidOperationException(SARes.GetString(10985));
            }

            if (_connStr == null || _connStr.Trim().Length < 1)
            {
                throw new InvalidOperationException(SARes.GetString(10986, "ConnectionString"));
            }

            object dtcTran = null;
            _isPooled = SAConnectionOptions.GetPooling(_connOpts);
            try
            {
                _conn = !_isPooled ? new SAInternalConnection(this, false, dtcTran, _asaConnStr, null) : SAConnection.s_poolManager.AllocateConnection(this, dtcTran, _asaConnStr, _connOpts);
            }
            catch (Exception ex)
            {
                if (ex is SAException && ((SAException)ex).Errors[0].NativeError == -102)
                {
                    GC.Collect();
                    _conn = !_isPooled ? new SAInternalConnection(this, false, dtcTran, _asaConnStr, null) : SAConnection.s_poolManager.AllocateConnection(this, dtcTran, _asaConnStr, _connOpts);
                }
                else
                {
                    if (!(ex is SAException) || ((SAException)ex).Errors[0].NativeError != -103)
                        throw ex;
                }
            }
            _state = ConnectionState.Open;
            /* TODO: InfoMessage
            if (InfoMessage != null)
            {
                _msgDelegate = new SAInfoMessageDelegate(this.MessageHandler);
                _conn.SetMessageCallback(_msgDelegate);
            }
            */
            var saCommand1 = new SACommand("SELECT CURRENT DATABASE, @@version", this);
            string strB;
            using (var saDataReader = saCommand1.ExecuteReader())
            {
                saDataReader.Read();
                strB = saDataReader[0] as string;
                _svrVer = saDataReader[1] as string;
            }

            if (string.Compare("utility_db", strB, true) != 0)
            {
                saCommand1.CommandText = "SET TEMPORARY OPTION QUOTED_IDENTIFIER = ON";
                saCommand1.ExecuteNonQuery();
            }
            saCommand1.Dispose();
            string initString = GetInitString();
            if (initString != null && initString.Length > 0)
            {
                SACommand saCommand2 = new SACommand(initString, this);
                saCommand2.ExecuteNonQuery();
                saCommand2.Dispose();
            }
            //TODO: Fix _EnlistTransaction
            //if (SAConnectionOptions.GetEnlist(_connOpts))
            //    this._EnlistTransaction(System.Transactions.Transaction.Current);

            FireStateChange(ConnectionState.Closed, ConnectionState.Open);

        }

        static int GetOpenCount()
        {
            return s_openCount;
        }

        static void ResetOpenCount()
        {
            s_openCount = 0;
        }

        string GetInitString()
        {
            string str;
            if (_initString != null)
            {
                str = _initString.Trim();
            }
            else
            {
                str = SAConnectionOptions.GetInitString(_connOpts);
                if (str != null)
                    str = str.Trim();
            }
            if (str != null && str.Length >= 2 && (str[0] == 123 && str[str.Length - 1] == 125 || str[0] == 34 && str[str.Length - 1] == 34 || str[0] == 39 && str[str.Length - 1] == 39))
                str = str.Substring(1, str.Length - 2).Trim();
            return str;
        }

        Hashtable ParseConnectionString(string connStr)
        {
            int idParser = 0;
            int count = 0;
            int indicator = 0;
            int keyLength = 0;
            int valueLength = 0;
            Hashtable removedOptions = null;
            SAConnection.AdaptToSAConnectionString(connStr, out _asaConnStr, out removedOptions);
            SAConnection.CheckConnectionOptions(removedOptions);
            if (_asaConnStr.Length < 1)
                return removedOptions;
            int idEx = PInvokeMethods.AsaConnectionStringParser_Init(ref idParser);
            if (!SAException.IsException(idEx))
            {
                idEx = PInvokeMethods.AsaConnectionStringParser_ParseConnectionString(idParser, _asaConnStr, ref indicator);
                if (!SAException.IsException(idEx))
                {
                    idEx = PInvokeMethods.AsaConnectionStringParser_GetParameterCount(idParser, ref count);
                    if (!SAException.IsException(idEx))
                    {
                        char[] chArray1 = new char[32];
                        char[] chArray2 = new char[64];
                        for (int index = 0; index < count; ++index)
                        {
                            idEx = PInvokeMethods.AsaConnectionStringParser_GetParameter(idParser, index, chArray1, 32, ref keyLength, chArray2, 64, ref valueLength);
                            if (!SAException.IsException(idEx))
                            {
                                string str1;
                                string str2;
                                if (keyLength <= 32 && valueLength <= 64)
                                {
                                    str1 = new string(chArray1, 0, keyLength);
                                    str2 = new string(chArray2, 0, valueLength);
                                }
                                else
                                {
                                    char[] chArray3 = new char[keyLength];
                                    char[] chArray4 = new char[valueLength];

                                    idEx = PInvokeMethods.AsaConnectionStringParser_GetParameter(idParser, index, chArray3, keyLength, ref keyLength, chArray4, valueLength, ref valueLength);
                                    if (!SAException.IsException(idEx))
                                    {
                                        str1 = new string(chArray3, 0, keyLength);
                                        str2 = new string(chArray4, 0, valueLength);
                                    }
                                    else
                                        break;

                                }
                                if (str1.Length > 0 && str2.Length > 0)
                                    removedOptions[str1.Trim().ToLower()] = str2.Trim();
                            }
                            else
                                break;
                        }
                    }
                }
            }
            if (SAUtility.IsValidId(idParser))
                PInvokeMethods.AsaConnectionStringParser_Fini(idParser);
            if (SAException.IsException(idEx))
            {
                Exception e = new ArgumentException(SARes.GetString(10977, SAErrorCollection.GetErrorMessage(idEx)), "value");
                throw e;
            }
            return removedOptions;
        }

        public static void ClearAllPools()
        {
            s_poolManager.ClearAllPools();
        }

        /// <summary>
        ///     <para>Empties the connection pool associated with the specified connection.</para>
        /// </summary>
        /// <param name="connection">
        ///     The SAConnection object to be cleared from the pool.
        /// </param>
        /// <seealso cref="T:iAnywhere.Data.SQLAnywhere.SAConnection" />
        public static void ClearPool(SAConnection connection)
        {
            s_poolManager.ClearPool(connection);
        }

        private static void CheckConnectionOptions(Hashtable connOptions)
        {
            CheckBoolConnectionOption(connOptions, "pooling");
            CheckBoolConnectionOption(connOptions, "enlist");
            CheckBoolConnectionOption(connOptions, "connection reset");
            CheckBoolConnectionOption(connOptions, "persist security info");
            CheckIntConnectionOption(connOptions, "connect timeout");
            CheckIntConnectionOption(connOptions, "connection timeout");
            CheckIntConnectionOption(connOptions, "connection lifetime");
            CheckIntConnectionOption(connOptions, "min pool size");
            string str1 = "max pool size";
            CheckIntConnectionOption(connOptions, str1);
            if (connOptions.Contains(str1) && Convert.ToInt32(connOptions[str1]) == 0)
            {
                Exception e = new ArgumentException(SARes.GetString(10978, str1, (string)connOptions[str1]), "value");
                throw e;
            }
            string str2 = "max pool size";
            string str3 = "min pool size";
            if ((connOptions.Contains(str2) || connOptions.Contains(str2)) && Convert.ToInt32((string)connOptions[str2]) < Convert.ToInt32((string)connOptions[str3]))
            {
                Exception e = new ArgumentException(SARes.GetString(10980), "value");
                throw e;
            }
        }

        static void CheckBoolConnectionOption(Hashtable connOptions, string key)
        {
            if (!connOptions.Contains(key))
                return;
            string parm2 = (string)connOptions[key];
            try
            {
                Convert.ToBoolean(parm2);
            }
            catch
            {
                throw new ArgumentException(SARes.GetString(10978, key, parm2), "value");
            }
        }

        static void CheckIntConnectionOption(Hashtable connOptions, string key)
        {
            if (!connOptions.Contains(key))
                return;
            string parm2 = (string)connOptions[key];
            int int32;
            try
            {
                int32 = Convert.ToInt32(parm2);
            }
            catch
            {
                throw new ArgumentException(SARes.GetString(10978, key, parm2), "value");
            }

            if (int32 < 0)
            {
                throw new ArgumentException(SARes.GetString(10978, key, parm2), "value");
            }
        }

        private static bool RemoveKeyValuesFromString(string connStrIn, string keysString, bool removeQuotesFromValue, out string connStrOut, out string removedKey, out string removedValue)
        {
            bool flag = false;
            connStrOut = null;
            removedKey = null;
            removedValue = null;
            if (connStrIn == null || connStrIn.Trim().Length < 1)
            {
                connStrOut = "";
                return flag;
            }
            ArrayList keys = SAConnectionOptions.GetKeys(keysString);
            StringBuilder stringBuilder = new StringBuilder(connStrIn);
            foreach (string key in keys)
            {
                MatchCollection matchCollection = Regex.Matches(stringBuilder.ToString(), SAConnection.GetRegExPattern(key), RegexOptions.IgnoreCase);
                if (matchCollection.Count > 0)
                    flag = true;
                for (int index = matchCollection.Count - 1; index >= 0; --index)
                {
                    Match match = matchCollection[index];
                    removedKey = key;
                    removedValue = SAConnection.GetValueFromKeyValuePair(match.Value, removeQuotesFromValue);
                    if (match.Index == 0 && match.Length < stringBuilder.Length && stringBuilder[match.Length] == 59)
                        stringBuilder.Remove(match.Index, match.Length + 1);
                    else
                        stringBuilder.Remove(match.Index, match.Length);
                }
            }
            connStrOut = stringBuilder.ToString();
            return flag;
        }

        private static void AdaptToSAConnectionString(string adoConnStr, out string asaConnStr, out Hashtable removedOptions)
        {
            string removedKey = null;
            string removedValue = null;
            asaConnStr = adoConnStr;
            removedOptions = new Hashtable();
            foreach (string connectionOption in SAConnectionOptions.s_nonSAConnectionOptions)
            {
                if (SAConnection.RemoveKeyValuesFromString(asaConnStr, connectionOption, true, out asaConnStr, out removedKey, out removedValue) && removedValue != null && removedValue.Length > 0)
                    removedOptions[removedKey] = removedValue;
            }
            int index = 0;
            while (index < SAConnectionOptions.s_adoConnectionOptions.Length)
            {
                if (SAConnection.RemoveKeyValuesFromString(asaConnStr, SAConnectionOptions.s_adoConnectionOptions[index], false, out asaConnStr, out removedKey, out removedValue))
                    asaConnStr = asaConnStr.Length <= 0 || asaConnStr[asaConnStr.Length - 1] == 59 ? string.Format("{0}{1}={2}", asaConnStr, SAConnectionOptions.s_adoConnectionOptions[index + 1], removedValue) : string.Format("{0};{1}={2}", asaConnStr, SAConnectionOptions.s_adoConnectionOptions[index + 1], removedValue);
                index += 2;
            }
        }

        private static string GetRegExPattern(string key)
        {
            StringBuilder stringBuilder = new StringBuilder(256);
            for (int index = 0; index < SAConnection.s_keyValuePatterns.Length; ++index)
            {
                if (index > 0)
                    stringBuilder.Append("|");
                stringBuilder.Append(string.Format(SAConnection.s_keyValuePatterns[index], key));
            }
            return stringBuilder.ToString();
        }

        private static string GetValueFromKeyValuePair(string pair, bool removeQuotesFromValue)
        {
            string str = "";
            int num = pair.IndexOf('=', 0);
            if (num >= 0)
            {
                str = pair.Substring(num + 1).Trim();
                if (removeQuotesFromValue)
                {
                    if (str.Length >= 2 && str[0] == 39 && str[str.Length - 1] == 39)
                        str = str.Substring(1, str.Length - 2);
                    else if (str.Length >= 2 && str[0] == 34 && str[str.Length - 1] == 34)
                        str = str.Substring(1, str.Length - 2);
                }
            }
            return str;
        }

        void FireStateChange(ConnectionState originalState, ConnectionState currentState)
        {
            OnStateChange(new StateChangeEventArgs(originalState, currentState));
        }

        internal void UnPool()
        {
            if (!_isPooled)
                return;
            _isPooled = false;
        }

        protected override DbCommand CreateDbCommand()
        {
            return new SACommand("", this);
        }
    }
}
