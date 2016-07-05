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
        private bool _showConnFormOnFail = true;
        private static SAUnmanagedDll s_unmanagedDll = SAUnmanagedDll.Instance;
        private static int s_currentId = 0;
        private static int s_openCount = 0;
        private static string[] s_keyValuePatterns = { "((;|^)(\\s)*{0}(\\s)*=(\\s)*{{(\\s|\\S)*}}(\\s)*([^;])*)", "((;|^)(\\s)*{0}(\\s)*=(\\s)*'(('')|[^'])*'(\\s)*([^;])*)", "((;|^)(\\s)*{0}(\\s)*=(\\s)*\"((\"\")|[^\"])*\"(\\s)*([^;])*)", "((;|^)(\\s)*{0}(\\s)*=([^;])*([^;])*)" };
        private static SAConnectionPoolManager s_poolManager = new SAConnectionPoolManager();
        private static Hashtable s_connStrCacheTable = new Hashtable();
        private static Hashtable s_asaConnStrCached = new Hashtable();
        private bool _isPooled;
        private string _connStr;
        private string _asaConnStr;
        private string _initString;
        private string _svrVer;
        private Hashtable _connOpts;
        private SAInternalConnection _conn;
        private SACommand _asyncCmd;
        private ConnectionState _state;
        private int _objectId;
        private bool _isServerSideConnection;
        private SAInfoMessageDelegate _msgDelegate;

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

        /// <summary>
        ///     <para>A command that is executed immediately after the connection is established.</para>
        /// </summary>
        /// <remarks>
        ///     <para>The InitString will be executed immediately after the connection is opened.</para>
        /// </remarks>
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

        /// <summary>
        ///     <para>Provides the database connection string.</para>
        /// </summary>
        /// <remarks>
        ///     <para>The ConnectionString is designed to match the SQL Anywhere connection string format as closely as possible with the following exception: when the Persist Security Info value is set to false (the default), the connection string that is returned is the same as the user-set ConnectionString minus security information. The SQL Anywhere SQL Anywhere .NET Data Provider does not persist the password in a returned connection string unless you set Persist Security Info to true.</para>
        ///     <para>You can use the ConnectionString property to connect to a variety of data sources.</para>
        ///     <para>You can set the ConnectionString property only when the connection is closed. Many of the connection string values have corresponding read-only properties. When the connection string is set, all of these properties are updated, unless an error is detected. If an error is detected, none of the properties are updated. SAConnection properties return only those settings contained in the ConnectionString.</para>
        ///     <para>If you reset the ConnectionString on a closed connection, all connection string values and related properties are reset, including the password.</para>
        ///     <para>When the property is set, a preliminary validation of the connection string is performed. When an application calls the Open method, the connection string is fully validated. A runtime exception is generated if the connection string contains invalid or unsupported properties.</para>
        ///     <para>Values can be delimited by single or double quotes. Either single or double quotes may be used within a connection string by using the other delimiter, for example, name="value's" or name= 'value"s', but not name='value's' or name= ""value"". Blank characters are ignored unless they are placed within a value or within quotes. keyword=value pairs must be separated by a semicolon. If a semicolon is part of a value, it must also be delimited by quotes. Escape sequences are not supported, and the value type is irrelevant. Names are not case sensitive. If a property name occurs more than once in the connection string, the value associated with the last occurrence is used.</para>
        ///     <para>You should use caution when constructing a connection string based on user input, such as when retrieving a user ID and password from a window, and appending it to the connection string. The application should not allow a user to embed extra connection string parameters in these values.</para>
        ///     <para>The default value of connection pooling is true (pooling=true).</para>
        /// </remarks>
        /// <example>
        ///             <para>The following statements set a connection string for an ODBC data source named SQL Anywhere 11 Demo and open the connection.</para>
        ///             <code>SAConnection conn = new SAConnection();
        /// conn.ConnectionString = "DSN=SQL Anywhere 11 Demo";
        /// conn.Open();</code>
        /// 
        ///         </example>
        /// <seealso cref="T:iAnywhere.Data.SQLAnywhere.SAConnection" />
        /// <seealso cref="M:iAnywhere.Data.SQLAnywhere.SAConnection.Open" />
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
                    SAConnection.AdaptToSAConnectionString(_connStr, out asaConnStr, out removedOptions);
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
                SAConnection.RemoveKeyValuesFromString(_connStr, SAConnectionOptions.s_passwordKeys, false, out connStrOut, out removedKey, out removedValue);
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
                    if (SAConnection.s_connStrCacheTable.Contains(value))
                    {
                        _connOpts = (Hashtable)s_connStrCacheTable[value];
                        _asaConnStr = (string)s_asaConnStrCached[value];
                    }
                    else
                    {
                        _connOpts = ParseConnectionString(value);
                        lock (SAConnection.s_connStrCacheTable.SyncRoot)
                        {
                            SAConnection.s_connStrCacheTable[value] = _connOpts;
                            SAConnection.s_asaConnStrCached[value] = _asaConnStr;
                        }
                    }
                    _connStr = value;
                }
            }
        }

        private bool ShowConnectionFormOnFail
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
            char[] chArray = new char[2] { '.', '/' };
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

        private bool Enlisted
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

        public event SAInfoMessageEventHandler InfoMessage;
        public override event StateChangeEventHandler StateChange;

        /// <summary>
        ///     <para>Initializes an SAConnection object. The connection must be opened before you can perform any operations against the database.</para>
        /// </summary>
        public SAConnection()
        {
            Init();
        }

        /// <summary>
        ///     <para>Initializes an SAConnection object. The connection must then be opened before you can perform any operations against the database.</para>
        /// </summary>
        /// <param name="connectionString">
        ///     A SQL Anywhere connection string. A connection string is a semicolon-separated list of keyword=value pairs.
        ///     <para>For a list of connection parameters, see @olink targetdoc="dbadmin" targetptr="conmean"@Connection parameters@/olink@.</para>
        /// </param>
        /// <example>
        ///             <para>The following statement initializes an SAConnection object for a connection to a database named policies running on a SQL Anywhere database server named hr. The connection uses the user ID admin and the password money.</para>
        ///             <code>SAConnection conn = new SAConnection(
        /// "UID=admin;PWD=money;ENG=hr;DBN=policies" );
        /// conn.Open();</code>
        /// 
        ///         </example>
        /// <seealso cref="T:iAnywhere.Data.SQLAnywhere.SAConnection" />
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

        /// <summary>
        ///     <para>Destructs an SAConnection object.</para>
        /// </summary>
        private static int GetPoolCount()
        {
            return SAConnection.s_poolManager.GetPoolCount();
        }

        private static int GetPooledConnectionCount()
        {
            return SAConnection.s_poolManager.GetConnectionCount();
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
                SAConnection.s_poolManager.ReturnConnection(_conn);
            else
                _conn.Close();
            FireStateChange(ConnectionState.Open, ConnectionState.Closed);
            _state = ConnectionState.Closed;
            _conn = null;
        }

        private void Init()
        {
            _initString = null;
            _connStr = null;
            _connOpts = null;
            _conn = null;
            _objectId = SAConnection.s_currentId++;
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

        /// <summary>
        ///     <para>Changes the current database for an open SAConnection.</para>
        /// </summary>
        /// <param name="database">
        ///     The name of the database to use instead of the current database.
        /// </param>
        public override void ChangeDatabase(string database)
        {
            if (_state != ConnectionState.Open)
            {
                Exception e = new InvalidOperationException(SARes.GetString(10983));
                throw e;
            }
            if (database == null || database.Trim().Length < 1)
            {
                Exception e = new ArgumentException(SARes.GetString(10984), "database");
                throw e;
            }
            Close();
            string connStrOut;
            string removedKey;
            string removedValue;
            SAConnection.RemoveKeyValuesFromString(_connStr, SAConnectionOptions.s_databaseKeys, false, out connStrOut, out removedKey, out removedValue);
            StringBuilder stringBuilder = new StringBuilder(connStrOut);
            if (database.IndexOf('\\') >= 0)
                stringBuilder.Append(";dbf=");
            else
                stringBuilder.Append(";dbn=");
            stringBuilder.Append('\'');
            stringBuilder.Append(database.Trim().Replace("'", "''"));
            stringBuilder.Append('\'');
            ConnectionString = stringBuilder.ToString();
            Open();
        }

        /// <summary>
        ///     <para>Creates and returns a <see cref="T:System.Data.Common.DbCommand" /> object associated with the current connection.</para>
        /// </summary>
        /// <returns>
        /// <para>A <see cref="T:System.Data.Common.DbCommand" /> object.</para>
        /// </returns>
        protected override DbCommand CreateDbCommand()
        {
            return CreateCommand();
        }

        /// <summary>
        ///     <para>Initializes an SACommand object.</para>
        /// </summary>
        /// <remarks>
        ///     <para>The command object is associated with the SAConnection object.</para>
        /// </remarks>
        /// <returns>
        /// <para>An SACommand object.</para>
        /// </returns>
        /// <seealso cref="T:iAnywhere.Data.SQLAnywhere.SACommand" />
        /// <seealso cref="T:iAnywhere.Data.SQLAnywhere.SAConnection" />
        public SACommand CreateCommand()
        {
            return new SACommand("", this, Transaction);
        }

        /// <summary>
        ///     <para>Opens a database connection with the property settings specified by the SAConnection.ConnectionString.</para>
        /// </summary>
        /// <seealso cref="P:iAnywhere.Data.SQLAnywhere.SAConnection.ConnectionString" />
        public override void Open()
        {
            ++SAConnection.s_openCount;

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

        private static int GetOpenCount()
        {
            return SAConnection.s_openCount;
        }

        private static void ResetOpenCount()
        {
            SAConnection.s_openCount = 0;
        }

        private string GetInitString()
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

        private Hashtable ParseConnectionString(string connStr)
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

        /// <summary>
        ///     <para>Empties all connection pools.</para>
        /// </summary>
        public static void ClearAllPools()
        {
            SAConnection.s_poolManager.ClearAllPools();
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
            SAConnection.s_poolManager.ClearPool(connection);
        }

        private static void CheckConnectionOptions(Hashtable connOptions)
        {
            CheckBoolConnectionOption(connOptions, "pooling");
            CheckBoolConnectionOption(connOptions, "enlist");
            CheckBoolConnectionOption(connOptions, "connection reset");
            CheckBoolConnectionOption(connOptions, "persist security info");
            SAConnection.CheckIntConnectionOption(connOptions, "connect timeout");
            SAConnection.CheckIntConnectionOption(connOptions, "connection timeout");
            SAConnection.CheckIntConnectionOption(connOptions, "connection lifetime");
            SAConnection.CheckIntConnectionOption(connOptions, "min pool size");
            string str1 = "max pool size";
            SAConnection.CheckIntConnectionOption(connOptions, str1);
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

        private static void CheckBoolConnectionOption(Hashtable connOptions, string key)
        {
            if (!connOptions.Contains(key))
                return;
            string parm2 = (string)connOptions[key];
            try
            {
                Convert.ToBoolean(parm2);
            }
            catch (Exception ex)
            {
                Exception e = new ArgumentException(SARes.GetString(10978, key, parm2), "value");
                throw e;
            }
        }

        private static void CheckIntConnectionOption(Hashtable connOptions, string key)
        {
            if (!connOptions.Contains(key))
                return;
            string parm2 = (string)connOptions[key];
            int int32;
            try
            {
                int32 = Convert.ToInt32(parm2);
            }
            catch (Exception ex)
            {
                Exception e = new ArgumentException(SARes.GetString(10978, key, parm2), "value");
                throw e;
            }
            if (int32 < 0)
            {
                Exception e = new ArgumentException(SARes.GetString(10978, key, parm2), "value");
                throw e;
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

        private void FireStateChange(ConnectionState originalState, ConnectionState currentState)
        {
            if (StateChange == null)
                return;
            StateChange(this, new StateChangeEventArgs(originalState, currentState));
        }

        internal void UnPool()
        {
            if (!_isPooled)
                return;
            _isPooled = false;
        }
    }
}
