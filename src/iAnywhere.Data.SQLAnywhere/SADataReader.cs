using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Runtime.InteropServices;
using System.Text;

namespace iAnywhere.Data.SQLAnywhere
{
    public sealed class SADataReader : DbDataReader
    {
        private int _rowBufferLength = -1;
        private int _objectId = SADataReader.s_CurrentId++;
        private const int c_rowBufferLength = 20;
        private const int c_unknown = -100;
        internal const string SC_ColumnName = "ColumnName";
        internal const string SC_ColumnOrdinal = "ColumnOrdinal";
        internal const string SC_ColumnSize = "ColumnSize";
        internal const string SC_NumericPrecision = "NumericPrecision";
        internal const string SC_NumericScale = "NumericScale";
        internal const string SC_IsUnique = "IsUnique";
        internal const string SC_IsKey = "IsKey";
        internal const string SC_BaseServerName = "BaseServerName";
        internal const string SC_BaseCatalogName = "BaseCatalogName";
        internal const string SC_BaseColumnName = "BaseColumnName";
        internal const string SC_BaseSchemaName = "BaseSchemaName";
        internal const string SC_BaseTableName = "BaseTableName";
        internal const string SC_DataType = "DataType";
        internal const string SC_AllowDBNull = "AllowDBNull";
        internal const string SC_ProviderType = "ProviderType";
        internal const string SC_IsAliased = "IsAliased";
        internal const string SC_IsExpression = "IsExpression";
        internal const string SC_IsIdentity = "IsIdentity";
        internal const string SC_IsAutoIncrement = "IsAutoIncrement";
        internal const string SC_IsRowVersion = "IsRowVersion";
        internal const string SC_IsHidden = "IsHidden";
        internal const string SC_IsLong = "IsLong";
        internal const string SC_IsReadOnly = "IsReadOnly";
        private int _idReader;
        private int _fieldCount;
        private int _depth;
        private int _recordsAffected;
        private int _numOfReadCalls;
        private bool _isClosed;
        private bool _hasData;
        private bool _hasResult;
        private bool _hasBlobColumn;
        private bool _schemaOnly;
        private bool _singleRow;
        private bool _hasRows;
        private SAConnection _conn;
        private CommandBehavior _cmdBehavior;
        private DotNetType[] _columnTypes;
        private SAColumnMetaData[] _columns;
        private object[,] _cachedRows;
        private int _cachedRowCount;
        private int _cachedRowCurrent;
        private WeakReference _wrCmd;
        private static int s_CurrentId;

        /// <summary>
        ///     <para>Gets a value indicating the depth of nesting for the current row. The outermost table has a depth of zero.</para>
        /// </summary>
        /// <value> The depth of nesting for the current row. </value>
        public override int Depth
        {
            get
            {
                CheckClosed("Depth");
                return _depth;
            }
        }

        /// <summary>
        ///     <para>Gets the number of columns in the result set.</para>
        /// </summary>
        /// <value>The number of columns in the current record.</value>
        public override int FieldCount
        {
            get
            {
                CheckClosed("FieldCount");
                return _fieldCount;
            }
        }

        /// <summary>
        ///     <para>Gets a value that indicates whether the SADataReader contains one or more rows.</para>
        /// </summary>
        /// <value>True if the SADataReader contains one or more rows; otherwise, false.</value>
        public override bool HasRows
        {
            get
            {
                CheckClosed("HasRows");
                return _hasRows;
            }
        }

        /// <summary>
        ///     <para>Gets a values that indicates whether the SADataReader is closed.</para>
        /// </summary>
        /// <value>True if the SADataReader is closed; otherwise, false.</value>
        /// <remarks>
        ///     <para>IsClosed and RecordsAffected are the only properties that you can call after the SADataReader is closed.</para>
        /// </remarks>
        public override bool IsClosed
        {
            get
            {
                return _isClosed;
            }
        }

        /// <summary>
        ///     <para>Returns the value of a column in its native format. In C#, this property is the indexer for the SADataReader class.</para>
        /// </summary>
        /// <param name="index">The column ordinal.</param>
        public override object this[int index]
        {
            get
            {
                CheckInvalidRead();
                CheckIndex(index);
                return GetValue(index);
            }
        }

        /// <summary>
        ///     <para>Returns the value of a column in its native format. In C#, this property is the indexer for the SADataReader class.</para>
        /// </summary>
        /// <param name="name">The column name.</param>
        public override object this[string name]
        {
            get
            {
                CheckInvalidRead();
                if (name == null)
                {
                    Exception e = new ArgumentNullException("name");
                    throw e;
                }
                int ordinal = FindOrdinal(name);
                if (ordinal < 0)
                {
                    Exception e = new IndexOutOfRangeException(name);
                    throw e;
                }
                return GetValue(ordinal);
            }
        }

        /// <summary>
        ///     The number of rows changed, inserted, or deleted by execution of the SQL statement.
        /// </summary>
        /// <value>The number of rows changed, inserted, or deleted. This is 0 if no rows were affected or the statement failed, or -1 for SELECT statements.</value>
        /// <remarks>
        ///     <para>The number of rows changed, inserted, or deleted. The value is 0 if no rows were affected or the statement failed, and -1 for SELECT statements.</para>
        ///     <para>The value of this property is cumulative. For example, if two records are inserted in batch mode, the value of RecordsAffected will be two.</para>
        ///     <para>IsClosed and RecordsAffected are the only properties that you can call after the SADataReader is closed.</para>
        /// </remarks>
        public override int RecordsAffected
        {
            get
            {
                return _recordsAffected;
            }
        }

        internal SADataReader(SAConnection conn, CommandBehavior cmdBehavior, int idReader, int recordsAffected, SACommand cmd)
        {
            _conn = conn;
            _cmdBehavior = cmdBehavior;
            _idReader = idReader;
            _isClosed = false;
            _recordsAffected = recordsAffected;
            _hasResult = true;
            _schemaOnly = (_cmdBehavior & CommandBehavior.SchemaOnly) > CommandBehavior.Default;
            _singleRow = (_cmdBehavior & CommandBehavior.SingleRow) > CommandBehavior.Default;
            try
            {
                Init(true);
                _wrCmd = new WeakReference(cmd);
                _hasRows = Read();
            }
            catch (Exception ex)
            {
                SAException.FreeException(PInvokeMethods.AsaDataReader_Close(_idReader));
                throw ex;
            }
        }

        ~SADataReader()
        {
            Dispose(false);
        }

        protected override void Dispose(bool disposing)
        {
            if (_isClosed)
                return;
            SAException.CheckException(PInvokeMethods.AsaDataReader_Close(_idReader));
            SACommand saCommand = (SACommand)_wrCmd.Target;
            if (saCommand != null)
                saCommand.DataReaderClosed();
            if (disposing)
            {
                if ((_cmdBehavior & CommandBehavior.CloseConnection) > CommandBehavior.Default)
                    _conn.Close();
                Init(false);
            }
            _isClosed = true;

            base.Dispose(disposing);
        }

        private void Init(bool getColumnMetaData)
        {
            _fieldCount = 0;
            _depth = 0;
            _hasData = false;
            _numOfReadCalls = 0;
            if (!getColumnMetaData || !_hasResult || _isClosed)
                return;
            GetColumnMetaData();
        }

        private void GetColumnMetaData()
        {
            IntPtr columnNames = IntPtr.Zero;
            _hasBlobColumn = false;
            SAException.CheckException(PInvokeMethods.AsaDataReader_GetColumnNames(_idReader, ref _fieldCount, ref columnNames));
            _columns = new SADataReader.SAColumnMetaData[_fieldCount];
            _columnTypes = new DotNetType[_fieldCount];
            var saColumnNamePtrs = Marshal.PtrToStructure<SAColumnName>(columnNames);
            for (int index = 0; index < _fieldCount; ++index)
            {
                var saColumnNamePtr = saColumnNamePtrs;
                var columnMarshaled = Marshal.PtrToStringUni(saColumnNamePtr.ColumnName);
                var saColumnMetaData = new SAColumnMetaData(saColumnNamePtr.Ordinal, saColumnNamePtr.SADataType, SADataConvert.GetSADataTypeName((SADbType)saColumnNamePtr.SADataType), columnMarshaled);
                string str = saColumnMetaData.ColumnName;
                if (str.Length > 1 && str[0] == 34 && str[str.Length - 1] == 34)
                    saColumnMetaData.ColumnName = str.Substring(1, str.Length - 2);
                _columns[index] = saColumnMetaData;
                _columnTypes[index] = SADataConvert.MapToDotNetType(saColumnMetaData.SADataType);
                if (SADataConvert.IsLong((int)saColumnMetaData.SADataType))
                    _hasBlobColumn = true;

                columnNames = IntPtr.Add(columnNames, Marshal.SizeOf<SAColumnName>());
                saColumnNamePtrs = Marshal.PtrToStructure<SAColumnName>(columnNames);
            }

            //SAException.CheckException(PInvokeMethods.AsaDataReader_FreeColumnNames(_idReader, _fieldCount, columnNames));
            _rowBufferLength = _hasBlobColumn ? 1 : 20;
            _cachedRows = new object[_rowBufferLength, _fieldCount];
            _cachedRowCount = 0;
            _cachedRowCurrent = -1;
        }

        void CheckClosed(string methodName)
        {
            if (_isClosed)
            {
                Exception e = new InvalidOperationException(SARes.GetString(11005, methodName));
                throw e;
            }
        }

        void CheckClosed()
        {
            if (_isClosed)
            {
                Exception e = new InvalidOperationException(SARes.GetString(11004));
                throw e;
            }
        }

        void CheckInvalidRead()
        {
            if (_isClosed)
            {
                Exception e = new InvalidOperationException(SARes.GetString(11004));
                throw e;
            }
            if (!_hasResult || _numOfReadCalls < 2 || !_hasData)
            {
                Exception e = new InvalidOperationException(SARes.GetString(11003));
                throw e;
            }
        }

        void CheckIndex(int index)
        {
            if (index < 0 || index >= _fieldCount)
            {
                Exception e = new IndexOutOfRangeException();
                throw e;
            }
        }

        void CheckDBNull(int index)
        {
            if (IsDBNull(index))
            {
                Exception e = new SAException(SARes.GetString(11017));
                throw e;
            }
        }

        void CheckBuffer(Array buffer, int bufferIndex, int length)
        {
            if (bufferIndex < 0 || bufferIndex + length > buffer.Length)
            {
                Exception e = new ArgumentOutOfRangeException("bufferIndex");
                throw e;
            }
        }

        object FetchValue(int ordinal, DotNetType dotNetType, bool isGetValue)
        {
            CheckInvalidRead();
            CheckIndex(ordinal);
            object val1;
            if (_cachedRowCurrent >= 0)
            {
                val1 = _cachedRows[_cachedRowCurrent, ordinal];
            }
            else
            {
                if (IsDBNull(ordinal))
                {
                    return DBNull.Value;
                }
                IntPtr val2 = IntPtr.Zero;
                SAException.CheckException(PInvokeMethods.AsaDataReader_GetValue(_idReader, ordinal, ref val2));
                val1 = SADataConvert.SAToDotNet(Marshal.PtrToStructure<SAValue>(val2).Value, _columnTypes[ordinal]);
                SAException.CheckException(PInvokeMethods.AsaDataReader_FreeValue(_idReader, ordinal, val2));
            }
            if (isGetValue || _columnTypes[ordinal] == dotNetType)
                return val1;
            return ConvertValue(_columnTypes[ordinal], dotNetType, val1);
        }

        object ConvertValue(DotNetType fromType, DotNetType toType, object val)
        {
            if (fromType == DotNetType.TimeSpan && toType == DotNetType.DateTime)
            {
                DateTime now = DateTime.Now;
                TimeSpan timeSpan = (TimeSpan)val;
                return new DateTime(now.Year, now.Month, now.Day, timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds, timeSpan.Milliseconds);
            }
            switch (toType)
            {
                case DotNetType.Boolean:
                    return Convert.ToBoolean(val);
                case DotNetType.Byte:
                    return Convert.ToByte(val);
                case DotNetType.Char:
                    return Convert.ToChar(val);
                case DotNetType.DateTime:
                    return Convert.ToDateTime(val);
                case DotNetType.Decimal:
                    return Convert.ToDecimal(val);
                case DotNetType.Double:
                    return Convert.ToDouble(val);
                case DotNetType.Single:
                    return Convert.ToSingle(val);
                case DotNetType.Guid:
                    return new Guid(val.ToString());
                case DotNetType.Int16:
                    return Convert.ToInt16(val);
                case DotNetType.Int32:
                    return Convert.ToInt32(val);
                case DotNetType.Int64:
                    return Convert.ToInt64(val);
                case DotNetType.String:
                    return Convert.ToString(val);
                case DotNetType.UInt16:
                    return Convert.ToUInt16(val);
                case DotNetType.UInt32:
                    return Convert.ToUInt32(val);
                case DotNetType.UInt64:
                    return Convert.ToUInt64(val);
                default:
                    Exception e = new InvalidCastException();
                    throw e;
            }
        }

        object FetchValue(int ordinal, long index, int length, DotNetType dotNetType, bool throwExIfDBNull, bool isGetValue)
        {
            CheckInvalidRead();
            CheckIndex(ordinal);
            if (isGetValue)
                dotNetType = _columnTypes[ordinal];
            else if (_columnTypes[ordinal] != dotNetType)
            {
                Exception e = new InvalidCastException();
                throw e;
            }
            object obj1;
            if (_cachedRowCurrent >= 0)
            {
                if (dotNetType == DotNetType.String)
                {
                    object obj2 = _cachedRows[_cachedRowCurrent, ordinal];
                    obj1 = ((string)obj2).Substring((int)index, index + (long)length > (long)((string)obj2).Length ? (int)((long)((string)obj2).Length - index) : length);
                }
                else
                    obj1 = _cachedRows[_cachedRowCurrent, ordinal];
                if (throwExIfDBNull && DBNull.Value.Equals(obj1))
                {
                    Exception e = new SAException(SARes.GetString(11017));
                    throw e;
                }
            }
            else if (IsDBNull(ordinal))
            {
                if (throwExIfDBNull)
                {
                    Exception e = new SAException(SARes.GetString(11017));
                    throw e;
                }
                obj1 = DBNull.Value;
            }
            else
            {
                IntPtr val = IntPtr.Zero;
                SAException.CheckException(PInvokeMethods.AsaDataReader_GetValueL(_idReader, ordinal, index, length, ref val));
                var saValue = Marshal.PtrToStructure<SAValue>(val);
                obj1 = SADataConvert.SAToDotNet(saValue.Value, dotNetType);
                SAException.CheckException(PInvokeMethods.AsaDataReader_FreeValue(_idReader, ordinal, val));
            }
            return obj1;
        }

        public override IEnumerator GetEnumerator()
        {
            return new DREnumerator(this);
        }

        public override bool GetBoolean(int ordinal)
        {
            return FetchValue(ordinal, DotNetType.Boolean, false).ToDefaultIfDBNull<bool>();
        }

        public override byte GetByte(int ordinal)
        {
            return FetchValue(ordinal, DotNetType.Byte, false).ToDefaultIfDBNull<byte>();
        }

        public override long GetBytes(int ordinal, long dataOffset, byte[] buffer, int bufferOffset, int length)
        {
            long actualSize = 0;
            int bytesRead = 0;
            CheckInvalidRead();
            CheckIndex(ordinal);
            CheckDBNull(ordinal);
            SADbType saDbType = _columns[ordinal].SADataType;
            if (!SADataConvert.IsBinary((int)saDbType))
            {
                Exception e = new InvalidCastException();
                throw e;
            }
            if (buffer == null)
            {
                if (SADataConvert.IsLong((int)saDbType))
                {
                    SAException.CheckException(PInvokeMethods.AsaDataReader_ReadBytes(_idReader, ordinal, 0L, null, 0, 1, ref bytesRead, ref actualSize));
                }
                else
                {
                    byte[] numArray = (byte[])GetValue(ordinal);
                    actualSize = numArray != null ? numArray.GetLength(0) : 0L;
                }
                return actualSize;
            }
            CheckBuffer(buffer, bufferOffset, length);
            if (saDbType == SADbType.Binary || saDbType == SADbType.VarBinary)
            {
                byte[] numArray = (byte[])GetValue(ordinal);
                int num = buffer.Length - bufferOffset > length ? length : buffer.Length - bufferOffset;
                bytesRead = numArray.Length - (int)dataOffset > num ? num : numArray.Length - (int)dataOffset;
                Array.Copy(numArray, (int)dataOffset, buffer, bufferOffset, bytesRead);
            }
            else
            {
                if (saDbType != SADbType.LongBinary)
                {
                    if (saDbType != SADbType.Image)
                        goto label_14;
                }
                int idEx;

                idEx = PInvokeMethods.AsaDataReader_ReadBytes(_idReader, ordinal, dataOffset, buffer, bufferOffset, length, ref bytesRead, ref actualSize);
                SAException.CheckException(idEx);
            }
            label_14:
            return bytesRead;
        }


        public override char GetChar(int ordinal)
        {
            Exception e = new NotSupportedException();

            throw e;
        }

        public override long GetChars(int ordinal, long dataOffset, char[] buffer, int bufferOffset, int length)
        {
            long actualSize = 0;
            int charsRead = 0;
            CheckInvalidRead();
            CheckIndex(ordinal);
            CheckDBNull(ordinal);
            if (_columns[ordinal].SADataType != SADbType.LongVarbit && _columns[ordinal].SADataType != SADbType.LongVarchar && (_columns[ordinal].SADataType != SADbType.LongNVarchar && _columns[ordinal].SADataType != SADbType.NText) && (_columns[ordinal].SADataType != SADbType.Text && _columns[ordinal].SADataType != SADbType.Xml))
            {
                Exception e = new InvalidCastException();

                throw e;
            }
            if (buffer == null)
                return ((string)GetValue(ordinal)).Length;
            CheckBuffer(buffer, bufferOffset, length);
            int idEx;

            idEx = PInvokeMethods.AsaDataReader_ReadChars(_idReader, ordinal, dataOffset, buffer, bufferOffset, length, ref charsRead, ref actualSize);
            SAException.CheckException(idEx);
            return charsRead;
        }

        public override string GetDataTypeName(int ordinal)
        {
            CheckClosed();
            CheckIndex(ordinal);
            return _columns[ordinal].SADataTypeName;
        }

        public override DateTime GetDateTime(int ordinal)
        {
            return FetchValue(ordinal, DotNetType.DateTime, false).ToDefaultIfDBNull<DateTime>();
        }

        public TimeSpan GetTimeSpan(int ordinal)
        {
            return FetchValue(ordinal, DotNetType.TimeSpan, false).ToDefaultIfDBNull<TimeSpan>();
        }

        public override decimal GetDecimal(int ordinal)
        {
            return FetchValue(ordinal, DotNetType.Decimal, false).ToDefaultIfDBNull<decimal>();
        }

        public override double GetDouble(int ordinal)
        {
            return FetchValue(ordinal, DotNetType.Double, false).ToDefaultIfDBNull<double>();
        }

        public override Type GetFieldType(int ordinal)
        {
            CheckClosed();
            CheckIndex(ordinal);
            switch (_columnTypes[ordinal])
            {
                case DotNetType.Boolean:
                    return typeof(bool);
                case DotNetType.Byte:
                    return typeof(byte);
                case DotNetType.Bytes:
                    return typeof(byte[]);
                case DotNetType.Char:
                    return typeof(char);
                case DotNetType.Chars:
                    return typeof(string);
                case DotNetType.DateTime:
                    return typeof(DateTime);
                case DotNetType.Decimal:
                    return typeof(Decimal);
                case DotNetType.Double:
                    return typeof(double);
                case DotNetType.Single:
                    return typeof(float);
                case DotNetType.Guid:
                    return typeof(Guid);
                case DotNetType.Int16:
                    return typeof(short);
                case DotNetType.Int32:
                    return typeof(int);
                case DotNetType.Int64:
                    return typeof(long);
                case DotNetType.String:
                    return typeof(string);
                case DotNetType.TimeSpan:
                    return typeof(TimeSpan);
                case DotNetType.UInt16:
                    return typeof(ushort);
                case DotNetType.UInt32:
                    return typeof(uint);
                case DotNetType.UInt64:
                    return typeof(ulong);
                default:
                    return null;
            }
        }

        public override float GetFloat(int ordinal)
        {
            return FetchValue(ordinal, DotNetType.Single, false).ToDefaultIfDBNull<float>();
        }

        public override Guid GetGuid(int ordinal)
        {
            return FetchValue(ordinal, DotNetType.Guid, false).ToDefaultIfDBNull<Guid>();
        }

        public override short GetInt16(int ordinal)
        {
            return FetchValue(ordinal, DotNetType.Int16, false).ToDefaultIfDBNull<short>();
        }

        public override int GetInt32(int ordinal)
        {
            return FetchValue(ordinal, DotNetType.Int32, false).ToDefaultIfDBNull<int>();
        }

        public override long GetInt64(int ordinal)
        {
            return FetchValue(ordinal, DotNetType.Int64, false).ToDefaultIfDBNull<long>();
        }

        public override string GetName(int ordinal)
        {
            CheckClosed();
            CheckIndex(ordinal);
            return _columns[ordinal].ColumnName;
        }

        public override int GetOrdinal(string name)
        {
            CheckClosed();
            if (name == null)
            {
                Exception e = new ArgumentNullException("name");

                throw e;
            }
            int ordinal = FindOrdinal(name);
            if (ordinal < 0)
            {
                Exception e = new IndexOutOfRangeException(name);

                throw e;
            }
            return ordinal;
        }

        private int FindOrdinal(string name)
        {
            bool[] flagArray = { false, true };
            foreach (bool ignoreCase in flagArray)
            {
                for (int index = 0; index < _columns.Length; ++index)
                {
                    if (string.Compare(_columns[index].ColumnName, name, ignoreCase) == 0)
                        return index;
                }
            }

            return -1;
        }

        public override string GetString(int ordinal)
        {
            var value = FetchValue(ordinal, DotNetType.String, false);
            if (DBNull.Value == value)
                return default(string);

            return (string)value;
        }

        [CLSCompliant(false)]
        public ushort GetUInt16(int ordinal)
        {
            return (ushort)FetchValue(ordinal, DotNetType.UInt16, false);
        }

        [CLSCompliant(false)]
        public uint GetUInt32(int ordinal)
        {
            return (uint)FetchValue(ordinal, DotNetType.UInt32, false);
        }

        [CLSCompliant(false)]
        public ulong GetUInt64(int ordinal)
        {
            return (ulong)FetchValue(ordinal, DotNetType.UInt64, false);
        }

        public override object GetValue(int ordinal)
        {
            return FetchValue(ordinal, _columnTypes[ordinal], true);
        }

        public object GetValue(int ordinal, long index, int length)
        {
            return FetchValue(ordinal, index, length, _columnTypes[ordinal], false, true);
        }

        public override int GetValues(object[] values)
        {
            CheckInvalidRead();

            if (values == null || values.Length < 1)
                throw new ArgumentNullException(nameof(values));

            int num = values.Length;
            if (num > _fieldCount)
                num = _fieldCount;
            if (num < 1)
                return 0;
            for (int index = 0; index < num; ++index)
                values[index] = _cachedRows[_cachedRowCurrent, index];
            return num;
        }

        public override bool IsDBNull(int ordinal)
        {
            CheckInvalidRead();
            CheckIndex(ordinal);
            bool isDBNull = false;
            if (_cachedRowCurrent >= 0)
                isDBNull = _cachedRows[_cachedRowCurrent, ordinal].Equals(DBNull.Value);
            else
                SAException.CheckException(PInvokeMethods.AsaDataReader_IsDBNull(_idReader, ordinal, ref isDBNull));
            return isDBNull;
        }

        public override bool NextResult()
        {
            CheckClosed("NextResult");
            if ((_cmdBehavior & CommandBehavior.SingleResult) > CommandBehavior.Default)
                return false;
            SAException.CheckException(PInvokeMethods.AsaDataReader_NextResult(_idReader, ref _hasResult));
            Init(true);
            if (_hasResult)
                _hasRows = Read();
            return _hasResult;
        }

        public override bool Read()
        {
            CheckClosed("Read");
            if (!_hasResult || _schemaOnly)
                return false;
            if (_numOfReadCalls != 1)
            {
                if (_singleRow && _numOfReadCalls >= 2)
                    return false;
                _hasData = false;
                if (_cachedRowCount > 0 && _cachedRowCurrent < _cachedRowCount - 1)
                {
                    _hasData = true;
                    ++_cachedRowCurrent;
                }
                else if (_cachedRowCount <= 0 || _cachedRowCount >= _rowBufferLength || _cachedRowCurrent != _cachedRowCount - 1)
                {
                    IntPtr values = IntPtr.Zero;
                    SAException.CheckException(PInvokeMethods.AsaDataReader_FetchRows(_idReader, ref _cachedRowCount, ref values));
                    if (_cachedRowCount > 0)
                    {
                        var saValuePtrs = Marshal.PtrToStructure<SAValue>(values);
                        for (int index1 = 0; index1 < _cachedRowCount; ++index1)
                        {
                            for (int index2 = 0; index2 < _fieldCount; ++index2)
                            {
                                var saValuePtr = saValuePtrs;
                                _cachedRows[index1, index2] = SADataConvert.SAToDotNet(saValuePtr.Value, _columnTypes[index2]);

                                values = IntPtr.Add(values, Marshal.SizeOf<SAValue>());
                                saValuePtrs = Marshal.PtrToStructure<SAValue>(values);
                            }
                        }
                        _hasData = true;
                        _cachedRowCurrent = 0;
                    }
                }
            }
            ++_numOfReadCalls;
            return _hasData;
        }

        /// <summary>Summary description for SAColumnMetaData</summary>
        struct SAColumnMetaData
        {
            public int Ordinal;
            public SADbType SADataType;
            public string SADataTypeName;
            public string ColumnName;

            public SAColumnMetaData(int ordinal, int saDataType, string saDataTypeName, string columnName)
            {
                Ordinal = ordinal;
                SADataType = (SADbType)saDataType;
                SADataTypeName = saDataTypeName;
                ColumnName = columnName;
            }
        }

        /// <summary>Summary description for IndexColumnInfo</summary>
        struct IndexColumnInfo
        {
            public string TableName;
            public uint IndexId;
            public string Unique;
            public string ColumnName;

            public IndexColumnInfo(string tableName, uint indexId, string unique, string columnName)
            {
                TableName = tableName;
                IndexId = indexId;
                Unique = unique;
                ColumnName = columnName;
            }
        }

        internal sealed class DREnumerator : IEnumerator
        {
            SADataReader _dataReader;

            public object Current
            {
                get
                {
                    return _dataReader;
                }
            }

            public DREnumerator(SADataReader dataReader)
            {
                _dataReader = dataReader;
            }

            public void Reset()
            {
                throw new NotSupportedException();
            }

            public bool MoveNext()
            {
                return _dataReader.Read();
            }
        }
    }

    static class DBNullExtension
    {
        public static T ToDefaultIfDBNull<T>(this object dbValue)
        {
            if (dbValue == DBNull.Value)
                return default(T);

            return (T)dbValue;
        }
    }
}
