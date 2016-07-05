using System;
using System.Data;
using System.Data.Common;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace iAnywhere.Data.SQLAnywhere
{
    public sealed class SACommand : DbCommand
    {
        private string[] _allParmNames = new string[0];
        private string[] _inParmNames = new string[0];
        private string[] _outParmNames = new string[0];
        private bool _getOutputParms = true;
        private int _objectId = s_CurrentId++;
        private int _timeout;
        private bool _designTimeVisible;
        private string _cmdText;
        private SAConnection _conn;
        private SATransaction _asaTran;
        private SAParameterCollection _parms;
        private SAParameterCollection _parmsOld;
        private bool _namedParms;
        private CommandType _cmdType;
        private UpdateRowSource _updatedRowSrc;
        private bool _isExecuting;
        private bool _isPrepared;
        private int _idCmd;
        private string _exeMethodName;
        private int _recordsAffected;
        private bool _disposed;
        private WeakReference _wrReader;
        private SACommand.AsyncCommandCallback _asyncCallback;
        private AsyncCommandResult _currentAsyncResult;
        private AutoResetEvent _asyncController;
        private static int s_CurrentId;

        internal int RecordsAffected
        {
            get
            {
                return _recordsAffected;
            }
        }

        public override string CommandText
        {
            get
            {
                return _cmdText;
            }
            set
            {
                if (!_cmdText.Equals(value))
                    Unprepare();
                _cmdText = value == null ? "" : value;
                if (!SACommand.IsCreateTableStmt(_cmdText))
                    return;
                _cmdText = ModifyCreateTableStmt(_cmdText);
            }
        }

        public override int CommandTimeout
        {
            get
            {
                return _timeout;
            }
            set
            {
                if (value < 0)
                {
                    Exception e = new ArgumentException(SARes.GetString(10995, value.ToString()), "value");
                    throw e;
                }
                _timeout = value;
            }
        }

        public override CommandType CommandType
        {
            get
            {
                return _cmdType;
            }
            set
            {
                if (value == CommandType.TableDirect)
                {
                    Exception e = new ArgumentException(SARes.GetString(10996), "value");
                    throw e;
                }
                if (_cmdType != value)
                    Unprepare();
                _cmdType = value;
            }
        }

        protected override DbConnection DbConnection
        {
            get
            {
                return _conn;
            }
            set
            {
                if (_conn != value)
                    Unprepare();
                _conn = (SAConnection)value;
            }
        }

        public SAConnection Connection
        {
            get
            {
                return (SAConnection)DbConnection;
            }
            set
            {
                DbConnection = value;
            }
        }

        public override bool DesignTimeVisible
        {
            get
            {
                return _designTimeVisible;
            }
            set
            {
                _designTimeVisible = value;
            }
        }

        protected override DbParameterCollection DbParameterCollection
        {
            get
            {
                return _parms;
            }
        }

        public SAParameterCollection Parameters
        {
            get
            {
                return (SAParameterCollection)DbParameterCollection;
            }
        }

        protected override DbTransaction DbTransaction
        {
            get
            {
                return _asaTran;
            }
            set
            {
                _asaTran = (SATransaction)value;
            }
        }

        public SATransaction Transaction
        {
            get
            {
                return (SATransaction)DbTransaction;
            }
            set
            {
                DbTransaction = value;
            }
        }

        public override UpdateRowSource UpdatedRowSource
        {
            get
            {
                return _updatedRowSrc;
            }
            set
            {
                _updatedRowSrc = value;
            }
        }

        public SACommand()
        {
            Init();
        }

        public SACommand(string cmdText)
        {
            Init();
            CommandText = cmdText;
        }

        public SACommand(string cmdText, SAConnection connection)
        {
            Init();
            CommandText = cmdText;
            Connection = connection;
        }

        public SACommand(string cmdText, SAConnection connection, SATransaction transaction)
        {
            Init();
            CommandText = cmdText;
            Connection = connection;
            Transaction = transaction;
        }

        internal SACommand(SACommand other)
          : this(other, false)
        {
        }

        internal SACommand(SACommand other, bool copyParms)
        {
            Init();
            _timeout = other._timeout;
            _conn = other._conn;
            _asaTran = other._asaTran;
            _cmdType = other._cmdType;
            _updatedRowSrc = other._updatedRowSrc;
            _isExecuting = other._isExecuting;
            _idCmd = other._idCmd;
            _parms = new SAParameterCollection();
            CommandText = other._cmdText;
            if (!copyParms)
                return;
            foreach (SAParameter parm in (DbParameterCollection)other._parms)
                _parms.Add(new SAParameter(parm));
        }

        ~SACommand()
        {
            Dispose(false);
        }

        protected override void Dispose(bool disposing)
        {
            ResetAsyncCommand();
            if (_disposed)
                return;
            try
            {
                if (_asyncController != null)
                    _asyncController.Dispose();
                if (disposing)
                {
                    FreeCommand(true);
                    Fini();
                }
                _disposed = true;
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        void FreeCommand(bool checkException)
        {
            if (!SAUtility.IsValidId(_idCmd))
                return;
            int idEx = PInvokeMethods.AsaCommand_Fini(_idCmd);
            if (checkException)
                SAException.CheckException(idEx);
            else
                SAException.FreeException(idEx);
            _idCmd = 0;
        }

        bool IsCommandCanceled(SAException ex)
        {
            return ex.Errors[0].NativeError == -299;
        }

        internal void DataReaderClosed()
        {
            _wrReader = null;
        }

        void CheckExistingDataReader()
        {
            if (_wrReader != null && (SADataReader)_wrReader.Target != null)
                throw new InvalidOperationException(SARes.GetString(17931));
        }

        static bool IsCreateTableStmt(string sql)
        {
            if (!string.IsNullOrEmpty(sql))
            {
                sql = sql.TrimStart();
                if (sql.StartsWith("CREATE ", StringComparison.OrdinalIgnoreCase) && sql.Substring(7).TrimStart().StartsWith("TABLE ", StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;
        }

        static string ModifyCreateTableStmt(string sql)
        {
            var format = "({0}(\\s)*\\((\\s)*-(\\s)*1(\\s)*\\))";
            string[] strArray1 = { "nvarchar", "varbinary", "varchar" };
            string[] strArray2 = { "long nvarchar", "long binary", "long varchar" };
            for (int index = 0; index < strArray1.Length; ++index)
                sql = Regex.Replace(sql, string.Format(format, strArray1[index]), strArray2[index], RegexOptions.IgnoreCase);
            return sql;
        }

        public void ResetCommandTimeout()
        {
            _timeout = 30;
        }

        protected override DbParameter CreateDbParameter()
        {
            return new SAParameter();
        }

        public SAParameter CreateParameter()
        {
            return (SAParameter)CreateDbParameter();
        }

        protected override DbDataReader ExecuteDbDataReader(CommandBehavior behavior)
        {
            return _ExecuteReader(behavior, false, false);
        }

        public SADataReader ExecuteReader()
        {
            return _ExecuteReader(CommandBehavior.Default, false, false);
        }

        public SADataReader ExecuteReader(CommandBehavior behavior)
        {
            return (SADataReader)ExecuteDbDataReader(behavior);
        }

        public IAsyncResult BeginExecuteReader()
        {
            return BeginExecuteReader(null, null, CommandBehavior.Default);
        }

        public IAsyncResult BeginExecuteReader(CommandBehavior behavior)
        {
            return BeginExecuteReader(null, null, behavior);
        }

        public IAsyncResult BeginExecuteReader(AsyncCallback callback, object stateObject)
        {
            return BeginExecuteReader(callback, stateObject, CommandBehavior.Default);
        }

        public IAsyncResult BeginExecuteReader(AsyncCallback callback, object stateObject, CommandBehavior behavior)
        {
            CheckAsyncNotExecuting();
            CheckNoExistingAsyncCmd();
            if (_asyncController == null)
                _asyncController = new AutoResetEvent(false);
            _currentAsyncResult = new AsyncCommandResult(callback, stateObject, AsyncCommandType.ExecuteReader);
            _conn.AsyncCommand = this;
            try
            {
                _ExecuteReader(behavior, false, true);
            }
            catch (Exception ex)
            {
                ResetAsyncCommand();
                throw;
            }
            return _currentAsyncResult;
        }

        public SADataReader EndExecuteReader(IAsyncResult asyncResult)
        {
            try
            {
                CheckAsyncResult(asyncResult, AsyncCommandType.ExecuteReader);
                _asyncController.WaitOne();
                int outputParmValueCount = 0;
                IntPtr outputParmValues = IntPtr.Zero;
                int idReader = 0;
                int rowCount = -1;
                try
                {
                    int idEx = PInvokeMethods.AsaCommand_EndExecuteReader(_conn.InternalConnection.ConnectionId, ref outputParmValueCount, ref outputParmValues, ref idReader, ref rowCount);
                    if (SAException.IsException(idEx))
                    {
                        SAException instance = SAException.CreateInstance(idEx);
                        throw instance;
                    }
                    if (!_isExecuting)
                        return null;
                    GetParameterValues(outputParmValueCount, outputParmValues);
                    return new SADataReader(_conn, _currentAsyncResult.Behavior, idReader, _recordsAffected, this);
                }
                finally
                {
                    Marshal.FreeHGlobal(_currentAsyncResult.ParmsDM);

                    //_parms.FreeParameterInfo(_currentAsyncResult.ParmCount, (SAParameterDM*).ToPointer());
                    FreeCommand(true);
                    _isExecuting = false;
                    _currentAsyncResult = null;
                }
            }
            finally
            {
                ResetAsyncCommand();
            }
        }

        void ResetAsyncCommand()
        {
            if (_conn == null || _conn.AsyncCommand != this)
                return;
            _conn.AsyncCommand = null;
        }

        public override void Prepare()
        {
            _exeMethodName = "Prepare";
            CheckAlreadyExecuting();
            CheckExistingDataReader();
            Validate();
            _Prepare();
            _isExecuting = false;
        }

        private void _Prepare()
        {
            int count = 0;
            string sqlCommand = GetSQLCommand();
            char[] arParmNames1 = new char[2048];
            char[] arParmNames2 = new char[2048];
            char[] arParmNames3 = new char[2048];
            SAParameterDM[] pParmsDM = null;
            try
            {
                _isPrepared = false;
                FreeCommand(true);
                _parms.GetParameterInfo(out count, ref pParmsDM, false, false, null);

                SAException.CheckException(PInvokeMethods.AsaCommand_Prepare(ref _idCmd, _conn.InternalConnection.ConnectionId, sqlCommand, count, pParmsDM.ToIntPtr(), ref _namedParms, arParmNames1.ToIntPr(), arParmNames2.ToIntPr(), arParmNames3.ToIntPr()));

                if (_namedParms)
                {
                    _allParmNames = GetParameterNames(arParmNames1);
                    _inParmNames = GetParameterNames(arParmNames2);
                    _outParmNames = GetParameterNames(arParmNames3);
                }
                SaveParameters();
                _isPrepared = true;
            }
            catch (Exception ex)
            {
                FreeCommand(false);
                throw ex;
            }
            finally
            {
                _parms.FreeParameterInfo(count, pParmsDM);
            }
        }

        void Unprepare()
        {
            _namedParms = false;
            _allParmNames = new string[0];
            _inParmNames = new string[0];
            _outParmNames = new string[0];
            FreeCommand(true);
            _isPrepared = false;
        }

        string[] GetParameterNames(char[] arParmNames)
        {
            string str1 = new string(arParmNames);
            string str2 = str1.Substring(0, str1.IndexOf(char.MinValue));
            if (str2.Length <= 0)
                return new string[0];
            return str2.Split('\t');
        }

        private SADataReader _ExecuteReader(CommandBehavior commandBehavior, bool isExecuteScalar, bool isBeginExecuteReader)
        {
            int idEx = 0;
            int idReader = 0;
            int count1 = 0;
            SAParameterDM[] pParmsDM = null;
            int count2 = 0;
            SAValue[] pValues = null;
            int outputParmCount = 0;
            IntPtr outputParmValues = IntPtr.Zero;
            _exeMethodName = !isBeginExecuteReader ? (!isExecuteScalar ? "ExecuteReader" : "ExecuteScalar") : "BeginExecuteReader";
            CheckAlreadyExecuting();
            CheckExistingDataReader();
            Validate();
            VerifyParameterType();
            try
            {
                if (isBeginExecuteReader)
                {
                    _parms.GetParameterInfo(out count1, ref pParmsDM, true, _namedParms, _allParmNames);
                    _currentAsyncResult.ParmCount = count1;
                    var intprt = new IntPtr();
                    Marshal.StructureToPtr(pParmsDM, intprt, true);
                    _currentAsyncResult.ParmsDM = intprt;
                    _currentAsyncResult.Behavior = commandBehavior;
                    Unprepare();
                    idEx = PInvokeMethods.AsaCommand_BeginExecuteReaderDirect(ref _idCmd, _conn.InternalConnection.ConnectionId, GetSQLCommand(), count1, pParmsDM.ToIntPtr(), Marshal.GetFunctionPointerForDelegate(_asyncCallback));
                }
                else
                {
                    if (!_isPrepared || ParameterChanged())
                        _Prepare();
                    _parms.GetInputParameterValues(out count2, ref pValues, _allParmNames, _inParmNames, _namedParms);
                    bool flag = true;
                    while (flag)
                    {

                        idEx = PInvokeMethods.AsaCommand_ExecuteReader(_idCmd, count2, pValues.ToIntPtr(), ref outputParmCount, ref outputParmValues, ref idReader, ref _recordsAffected);
                        if (idEx == -1)
                            _Prepare();
                        else
                            flag = false;
                    }
                }
                if (SAException.IsException(idEx))
                {
                    SAException instance = SAException.CreateInstance(idEx);
                    throw instance;
                }
                if (!_isExecuting || isBeginExecuteReader)
                    return null;
                GetParameterValues(outputParmCount, outputParmValues);
                if (!SAUtility.IsValidId(idReader))
                    return null;
                SADataReader saDataReader = new SADataReader(_conn, commandBehavior, idReader, _recordsAffected, this);
                _wrReader = new WeakReference(saDataReader);
                
                return saDataReader;
            }
            catch (Exception ex)
            {
                if (isBeginExecuteReader)
                    _parms.FreeParameterInfo(count1, pParmsDM);
                _isPrepared = false;
                FreeCommand(false);
                throw ex;
            }
            finally
            {
                if (!isBeginExecuteReader)
                {
                    _parms.FreeParameterValues(count2, pValues);
                    _isExecuting = false;
                }
            }
        }

        private void SaveParameters()
        {
            _parmsOld.Clear();
            foreach (SAParameter parm in (DbParameterCollection)_parms)
            {
                SAParameter saParameter = new SAParameter(parm.ParameterName, parm.SADbType, parm.Size, parm.Direction, parm.IsNullable, parm.Precision, parm.Scale, parm.SourceColumn, null);
                saParameter.Size = parm.Size;
                _parmsOld.Add(saParameter);
            }
        }

        private bool ParameterChanged()
        {
            if (_parms.Count != _parmsOld.Count)
                return true;
            for (int index = 0; index < _parms.Count; ++index)
            {
                if (_parms[index].Direction != _parmsOld[index].Direction || _parms[index].IsNullable != _parmsOld[index].IsNullable || (_parms[index].Size != _parmsOld[index].Size || _parms[index].Precision != _parmsOld[index].Precision) || _parms[index].Scale != _parmsOld[index].Scale)
                    return true;
            }
            return false;
        }

        public override int ExecuteNonQuery()
        {
            int idEx = 0;
            int count1 = 0;
            SAValue[] pValues = null;
            int outputParmCount = 0;
            IntPtr outputParmValues = IntPtr.Zero;
            bool flag1 = _currentAsyncResult != null;
            int count2 = 0;
            SAParameterDM[] pParmsDM = null;
            _exeMethodName = !flag1 ? "ExecuteNonQuery" : "BeginExecuteNonQuery";
            CheckAlreadyExecuting();
            CheckExistingDataReader();
            Validate();
            VerifyParameterType();
            try
            {
                if (flag1)
                {
                    _parms.GetParameterInfo(out count2, ref pParmsDM, true, _namedParms, _allParmNames);
                    _currentAsyncResult.ParmCount = count2;
                    _currentAsyncResult.ParmsDM = pParmsDM.ToIntPtr();

                    Unprepare();
                    idEx = PInvokeMethods.AsaCommand_BeginExecuteNonQueryDirect(ref _idCmd, _conn.InternalConnection.ConnectionId, GetSQLCommand(), count2, pParmsDM.ToIntPtr(), Marshal.GetFunctionPointerForDelegate(_asyncCallback));
                }
                else
                {
                    if (!_isPrepared || ParameterChanged())
                        _Prepare();
                    _parms.GetInputParameterValues(out count1, ref pValues, _allParmNames, _inParmNames, _namedParms);
                    bool flag2 = true;
                    while (flag2)
                    {
                        idEx = PInvokeMethods.AsaCommand_ExecuteNonQuery(_idCmd, count1, pValues.ToIntPtr(), ref outputParmCount, ref outputParmValues, ref _recordsAffected);
                        if (idEx == -1)
                            _Prepare();
                        else
                            flag2 = false;
                    }
                }
                if (SAException.IsException(idEx))
                {
                    SAException instance = SAException.CreateInstance(idEx);
                    throw instance;
                }
                if (!_isExecuting || flag1)
                    return -1;
                GetParameterValues(outputParmCount, outputParmValues);
            }
            catch (Exception ex)
            {
                if (flag1)
                    _parms.FreeParameterInfo(count2, pParmsDM);
                _isPrepared = false;
                FreeCommand(false);
                throw ex;
            }
            finally
            {
                if (!flag1)
                {
                    _parms.FreeParameterValues(count1, pValues);
                    _isExecuting = false;
                }
            }
            return _recordsAffected;

        }

        public IAsyncResult BeginExecuteNonQuery()
        {
            return BeginExecuteNonQuery(null, null);
        }

        public IAsyncResult BeginExecuteNonQuery(AsyncCallback callback, object stateObject)
        {
            CheckAsyncNotExecuting();
            CheckNoExistingAsyncCmd();
            if (_asyncController == null)
                _asyncController = new AutoResetEvent(false);
            _currentAsyncResult = new AsyncCommandResult(callback, stateObject, AsyncCommandType.ExecuteNonQuery);
            _conn.AsyncCommand = this;
            try
            {
                ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                ResetAsyncCommand();
                throw;
            }
            return _currentAsyncResult;
        }

        public int EndExecuteNonQuery(IAsyncResult asyncResult)
        {
            try
            {
                AsyncCommandResult asyncCommandResult = (AsyncCommandResult)asyncResult;
                CheckAsyncResult(asyncCommandResult, AsyncCommandType.ExecuteNonQuery);
                _asyncController.WaitOne();
                int outputParmCount = 0;
                IntPtr outputParmValues = IntPtr.Zero;
                _exeMethodName = "EndExecuteNonQuery";
                Validate();
                try
                {
                    int idEx = PInvokeMethods.AsaCommand_EndExecuteNonQuery(_conn.InternalConnection.ConnectionId, ref outputParmCount, ref outputParmValues, ref _recordsAffected);
                    if (SAException.IsException(idEx))
                    {
                        SAException instance = SAException.CreateInstance(idEx);
                        throw instance;
                    }
                    if (!_isExecuting)
                        return -1;
                    GetParameterValues(outputParmCount, outputParmValues);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    //_parms.FreeParameterValues(asyncCommandResult.InputParmCount, (SAValue*)asyncCommandResult.InputParmValues.ToPointer());
                    //_parms.FreeParameterInfo(asyncCommandResult.ParmCount, (SAParameterDM*)asyncCommandResult.ParmsDM.ToPointer());
                    _isExecuting = false;
                    _currentAsyncResult = null;
                }
                return _recordsAffected;
            }
            finally
            {
                ResetAsyncCommand();
            }
        }

        public override object ExecuteScalar()
        {
            try
            {
                object obj = null;
                SADataReader saDataReader = _ExecuteReader(CommandBehavior.Default, true, false);
                if (saDataReader != null)
                {
                    if (saDataReader.Read())
                        obj = saDataReader.GetValue(0);
                }
                return obj;
            }
            finally
            {
            }
        }

        public override void Cancel()
        {
            try
            {
                if (_isExecuting)
                {
                    _isExecuting = false;
                    if (_idCmd < 0)
                        return;
                    SAException.CheckException(PInvokeMethods.AsaCommand_Cancel(_idCmd));
                }                
            }
            finally
            {
                ResetAsyncCommand();
            }
        }

        private string GetSQLCommand()
        {
            string str = null;
            if (_cmdType == CommandType.Text)
                str = _cmdText;
            else if (_cmdType == CommandType.StoredProcedure)
            {
                if (_parms.Count == 0)
                {
                    str = "CALL " + _cmdText;
                }
                else
                {
                    StringBuilder stringBuilder = new StringBuilder(512);
                    bool flag = false;
                    for (int index = 0; index < _parms.Count; ++index)
                    {
                        if (_parms[index].Direction == ParameterDirection.ReturnValue)
                        {
                            flag = true;
                            break;
                        }
                    }
                    if (flag)
                        stringBuilder.Append("? = CALL ");
                    else
                        stringBuilder.Append("CALL ");
                    stringBuilder.Append(_cmdText);
                    stringBuilder.Append("( ");
                    int num = flag ? _parms.Count - 1 : _parms.Count;
                    for (int index = 0; index < num; ++index)
                    {
                        if (index > 0)
                            stringBuilder.Append(", ");
                        stringBuilder.Append("?");
                    }
                    stringBuilder.Append(" )");
                    str = stringBuilder.ToString();
                }
            }
            else if (_cmdType == CommandType.TableDirect)
                str = string.Format("SELECT * FROM \"{0}\"", _cmdText);
            return str;
        }

        private void CheckAlreadyExecuting()
        {
            if (_isExecuting)
            {
                Exception e = new InvalidOperationException(SARes.GetString(10994));
                throw e;
            }
            _isExecuting = true;
        }

        private void Validate()
        {
            Exception e = null;
            if (_conn == null)
            {
                e = new InvalidOperationException(SARes.GetString(10986, "Connection"));
            }
            else if (_conn.GetConnectionState() != ConnectionState.Open)
            {
                e = new InvalidOperationException(SARes.GetString(10993, _exeMethodName));
            }
            else
            {
                if (_asaTran == null || !_asaTran.IsValid)
                {
                    if (_conn.Transaction != null)
                    {
                        e = new InvalidOperationException(SARes.GetString(11001));
                        goto label_16;
                    }
                }
                else if (_conn != _asaTran.Connection)
                {
                    e = new InvalidOperationException(SARes.GetString(10992));
                    goto label_16;
                }
                if (_cmdText == null || _cmdText.Trim().Length < 1)
                {
                    e = new InvalidOperationException(SARes.GetString(10986, _exeMethodName + ": CommandText"));
                }
                else
                {
                    for (int index = 0; index < Parameters.Count; ++index)
                    {
                        SAParameter saParameter = Parameters[index];
                        if (saParameter.Size == 0 && (saParameter.Direction == ParameterDirection.Output || saParameter.Direction == ParameterDirection.InputOutput) && (saParameter.DbType == DbType.AnsiString || saParameter.DbType == DbType.AnsiStringFixedLength || (saParameter.DbType == DbType.String || saParameter.DbType == DbType.StringFixedLength) || (saParameter.DbType == DbType.Binary || saParameter.DbType == DbType.Xml)))
                        {
                            e = new InvalidOperationException(SARes.GetString(17421, index.ToString()));
                            break;
                        }
                    }
                }
            }
            label_16:
            if (e != null)
            {
                _isExecuting = false;
                throw e;
            }
        }

        private void VerifyParameterType()
        {
            if (_conn.ServerVersion == null || _conn.ServerVersion.IndexOf("9.") != 0)
                return;
            foreach (SAParameter parm in (DbParameterCollection)_parms)
            {
                if (parm.SADbType == SADbType.LongNVarchar)
                    parm.SADbType = SADbType.LongVarchar;
                else if (parm.SADbType == SADbType.NChar)
                    parm.SADbType = SADbType.Char;
                else if (parm.SADbType == SADbType.NText)
                    parm.SADbType = SADbType.Text;
                else if (parm.SADbType == SADbType.NVarChar)
                    parm.SADbType = SADbType.VarChar;
            }
        }

        private void GetParameterValues(int count, IntPtr pValues)
        {
            if (count <= 0 || _parms.GetOutputParameterCount() <= 0)
                return;
            if (_getOutputParms)
            {
                int index1 = 0;

                var saValuePtr = Marshal.PtrToStructure<SAValue[]>(pValues);
                if (_namedParms)
                {
                    for (int index2 = 0; index2 < _outParmNames.GetLength(0); ++index2)
                    {
                        for (int index3 = 0; index3 < _parms.Count; ++index3)
                        {
                            var saParameter = _parms[index3];
                            if (string.Compare(_outParmNames[index2], saParameter.ParameterName, true) == 0 && saParameter.IsOutputParameter())
                            {
                                saParameter.Value = SADataConvert.SAToDotNet(saValuePtr[index2].Value, SADataConvert.MapToDotNetType(saParameter.SADbType), saParameter.Size, saParameter.Offset);

                                break;
                            }
                        }
                    }
                }
                else
                {
                    for (int index2 = 0; index2 < count; ++index2)
                    {
                        while (_parms[index1].Direction == ParameterDirection.Input)
                            ++index1;
                        SAParameter saParameter = _parms[index1];
                        saParameter.Value = SADataConvert.SAToDotNet(saValuePtr[index2].Value, SADataConvert.MapToDotNetType(saParameter.SADbType), saParameter.Size, saParameter.Offset);

                        ++index1;
                    }
                }
            }
            SAException.CheckException(PInvokeMethods.AsaCommand_FreeOutputParameterValues(count, pValues));
        }

        private void Init()
        {
            Fini();
            _parms = new SAParameterCollection();
            _parmsOld = new SAParameterCollection();
            _asyncCallback = new SACommand.AsyncCommandCallback(AsyncCallBack);
        }

        private void Fini()
        {
            _timeout = 30;
            _designTimeVisible = true;
            _cmdText = "";
            _conn = null;
            _asaTran = null;
            _cmdType = CommandType.Text;
            _updatedRowSrc = UpdateRowSource.OutputParameters;
            _isExecuting = false;
            _idCmd = 0;
            _parms = null;
            _parmsOld = null;
            _asyncCallback = null;
        }

        private void AsyncCallBack()
        {
            _currentAsyncResult.Complete();
            _asyncController.Set();
        }

        private void CheckNoExistingAsyncCmd()
        {
            if (_conn.AsyncCommand != null)
            {
                Exception e = new InvalidOperationException(SARes.GetString(18531));
                throw e;
            }
        }

        private void CheckAsyncNotExecuting()
        {
            if (_currentAsyncResult != null)
            {
                Exception e = new InvalidOperationException(SARes.GetString(14973));
                throw e;
            }
        }

        private void CheckAsyncResult(IAsyncResult asyncResult, AsyncCommandType type)
        {
            if (asyncResult == null)
            {
                Exception e = new ArgumentNullException("asyncResult");
                throw e;
            }
            if (_currentAsyncResult == null)
            {
                Exception e = new InvalidOperationException(SARes.GetString(14974));
                throw e;
            }
            if (_currentAsyncResult != asyncResult)
            {
                Exception e = new ArgumentException(SARes.GetString(14975), "asyncResult");
                throw e;
            }
            _currentAsyncResult.CheckCommandType(type);
        }

        private delegate void AsyncCommandCallback();
    }

    static class CharExtensions
    {
        public static IntPtr ToIntPr(this char[] charArray)
        {
            var bytes = Encoding.UTF8.GetBytes(charArray);
            var ptr = Marshal.AllocHGlobal(charArray.Length);
            Marshal.Copy(bytes, 0, ptr, bytes.Length);
            return ptr;
        }
    }
}
