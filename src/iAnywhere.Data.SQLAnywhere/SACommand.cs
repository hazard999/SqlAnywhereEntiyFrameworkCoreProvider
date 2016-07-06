using System;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;

namespace iAnywhere.Data.SQLAnywhere
{
    public sealed class SACommand : DbCommand
    {
        string[] _allParmNames = new string[0];
        string[] _inParmNames = new string[0];
        string[] _outParmNames = new string[0];
        bool _getOutputParms = true;
        int _objectId = s_CurrentId++;
        int _timeout;
        bool _designTimeVisible;
        string _cmdText;
        SAConnection _conn;
        SATransaction _asaTran;
        SAParameterCollection _parms;
        SAParameterCollection _parmsOld;
        bool _namedParms;
        CommandType _cmdType;
        UpdateRowSource _updatedRowSrc;
        bool _isExecuting;
        bool _isPrepared;
        int _idCmd;
        string _exeMethodName;
        int _recordsAffected;
        bool _disposed;
        WeakReference _wrReader;
        static int s_CurrentId;

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

                if (!IsCreateTableStmt(_cmdText))
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
                    throw new ArgumentException(SARes.GetString(10995, value.ToString()), nameof(value));

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
                    throw new ArgumentException(SARes.GetString(10996), nameof(value));


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
            if (_disposed)
                return;

            try
            {
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

        protected override DbDataReader ExecuteDbDataReader(CommandBehavior behavior)
        {
            return _ExecuteReader(behavior, false);
        }

        public override void Prepare()
        {
            _exeMethodName = "Prepare";
            try
            {
                CheckAlreadyExecuting();
                CheckExistingDataReader();
                Validate();
                _Prepare();
            }
            finally
            {
                _isExecuting = false;
            }
        }

        void _Prepare()
        {
            int count = 0;
            string sqlCommand = GetSQLCommand();

            SAParameterDM[] pParmsDM = null;
            try
            {
                _isPrepared = false;
                FreeCommand(true);
                _parms.GetParameterInfo(out count, ref pParmsDM, false, false, null);

                var paramsPtr = pParmsDM.ToIntPtr();
                var allParamsPtr = Marshal.AllocHGlobal(2048);
                var inParamsPtr = Marshal.AllocHGlobal(2048);
                var outParamsPtr = Marshal.AllocHGlobal(2048);

                SAException.CheckException(
                    PInvokeMethods.AsaCommand_Prepare(
                        ref _idCmd,
                        _conn.InternalConnection.ConnectionId,
                        sqlCommand,
                        count,
                        paramsPtr,
                        ref _namedParms,
                         allParamsPtr,
                         inParamsPtr,
                         outParamsPtr));

                if (_namedParms)
                {
                    _allParmNames = GetParameterNames(allParamsPtr);
                    _inParmNames = GetParameterNames(inParamsPtr);
                    _outParmNames = GetParameterNames(outParamsPtr);
                }

                Marshal.FreeHGlobal(allParamsPtr);
                Marshal.FreeHGlobal(inParamsPtr);
                Marshal.FreeHGlobal(outParamsPtr);

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
                _parms.FreeParameterInfo(pParmsDM);
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

        string[] GetParameterNames(IntPtr arParmNames)
        {
            string str1 = Marshal.PtrToStringUni(arParmNames);
            string str2 = str1.Substring(0, str1.IndexOf(char.MinValue));
            if (str2.Length <= 0)
                return new string[0];
            return str2.Split('\t');
        }

        SADataReader _ExecuteReader(CommandBehavior commandBehavior, bool isExecuteScalar)
        {
            int idEx = 0;
            int idReader = 0;
            int count2 = 0;
            SAValue[] pValues = null;
            int outputParmCount = 0;
            IntPtr outputParmValues = IntPtr.Zero;
            _exeMethodName = !isExecuteScalar ? "ExecuteReader" : "ExecuteScalar";

            CheckAlreadyExecuting();
            CheckExistingDataReader();
            Validate();
            VerifyParameterType();

            try
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

                if (SAException.IsException(idEx))
                    throw SAException.CreateInstance(idEx);

                if (!_isExecuting)
                    return null;

                GetParameterValues(outputParmCount, outputParmValues);

                if (!SAUtility.IsValidId(idReader))
                    return null;

                SADataReader saDataReader = new SADataReader(_conn, commandBehavior, idReader, _recordsAffected, this);
                _wrReader = new WeakReference(saDataReader);

                return saDataReader;
            }
            catch
            {
                _isPrepared = false;
                FreeCommand(false);
                throw;
            }
            finally
            {
                _parms.FreeParameterValues(count2, pValues);
                _isExecuting = false;
            }
        }

        void SaveParameters()
        {
            _parmsOld.Clear();
            foreach (var parm in _parms.OfType<SAParameter>())
            {
                var saParameter = new SAParameter(parm.ParameterName, parm.SADbType, parm.Size, parm.Direction, parm.IsNullable, parm.Precision, parm.Scale, parm.SourceColumn, null);
                saParameter.Size = parm.Size;
                _parmsOld.Add(saParameter);
            }
        }

        bool ParameterChanged()
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
            _exeMethodName = "ExecuteNonQuery";

            CheckAlreadyExecuting();
            CheckExistingDataReader();
            Validate();
            VerifyParameterType();

            try
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

                if (SAException.IsException(idEx))
                {
                    SAException instance = SAException.CreateInstance(idEx);
                    throw instance;
                }
                if (!_isExecuting)
                    return -1;
                GetParameterValues(outputParmCount, outputParmValues);
            }
            catch
            {

                _isPrepared = false;
                FreeCommand(false);
                throw;
            }
            finally
            {
                _parms.FreeParameterValues(count1, pValues);
                _isExecuting = false;
            }

            return _recordsAffected;
        }

        public override object ExecuteScalar()
        {
            var saDataReader = _ExecuteReader(CommandBehavior.Default, true);

            if (saDataReader != null)
            {
                if (saDataReader.Read())
                    return saDataReader.GetValue(0);
            }

            return null;
        }

        public override void Cancel()
        {
            if (_isExecuting)
            {
                _isExecuting = false;
                if (_idCmd < 0)
                    return;
                SAException.CheckException(PInvokeMethods.AsaCommand_Cancel(_idCmd));
            }
        }

        string GetSQLCommand()
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

        void CheckAlreadyExecuting()
        {
            if (_isExecuting)
                throw new InvalidOperationException(SARes.GetString(10994));

            _isExecuting = true;
        }

        void Validate()
        {
            if (_conn == null)
                throw new InvalidOperationException(SARes.GetString(10986, "Connection"));

            if (_conn.GetConnectionState() != ConnectionState.Open)
                throw new InvalidOperationException(SARes.GetString(10993, _exeMethodName));

            if (_asaTran == null || !_asaTran.IsValid)
            {
                if (_conn.Transaction != null)
                    throw new InvalidOperationException(SARes.GetString(11001));
            }
            else if (_conn != _asaTran.Connection)
            {
                throw new InvalidOperationException(SARes.GetString(10992));
            }

            if (_cmdText == null || _cmdText.Trim().Length < 1)
                throw new InvalidOperationException(SARes.GetString(10986, _exeMethodName + ": CommandText"));

            for (var index = 0; index < Parameters.Count; ++index)
            {
                var saParameter = (SAParameter)Parameters[index];
                if (saParameter.Size == 0 && (saParameter.Direction == ParameterDirection.Output || saParameter.Direction == ParameterDirection.InputOutput) && (saParameter.DbType == DbType.AnsiString || saParameter.DbType == DbType.AnsiStringFixedLength || (saParameter.DbType == DbType.String || saParameter.DbType == DbType.StringFixedLength) || (saParameter.DbType == DbType.Binary || saParameter.DbType == DbType.Xml)))
                    throw new InvalidOperationException(SARes.GetString(17421, index.ToString()));
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

        void Init()
        {
            Fini();
            _parms = new SAParameterCollection();
            _parmsOld = new SAParameterCollection();
        }

        void Fini()
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
        }
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
