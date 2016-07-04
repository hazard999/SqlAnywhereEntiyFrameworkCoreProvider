using System;
using System.Collections;
using System.Data;
using System.Data.Common;
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
        private SADataReader.SAColumnMetaData[] _columns;
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
                SATrace.PropertyCall("<sa.SADataReader.get_HasRows|API>", _objectId);
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
                SATrace.PropertyCall("<sa.SADataReader.get_IsClosed|API>", _objectId);
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
                SATrace.PropertyCall("<sa.SADataReader.get_this[int]|API>", _objectId);
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
                SATrace.PropertyCall("<sa.SADataReader.get_this[string]|API>", _objectId);
                CheckInvalidRead();
                if (name == null)
                {
                    Exception e = new ArgumentNullException("name");
                    SATrace.Exception(e);
                    throw e;
                }
                int ordinal = FindOrdinal(name);
                if (ordinal < 0)
                {
                    Exception e = new IndexOutOfRangeException(name);
                    SATrace.Exception(e);
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
                SATrace.PropertyCall("<sa.SADataReader.get_RecordsAffected|API>", _objectId);
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

        /// <summary>
        ///     <para>Destructs an SADataReader object.</para>
        /// </summary>
        ~SADataReader()
        {
            Dispose(false);
        }

        /// <summary>
        ///     <para>Frees the resources associated with the object.</para>
        /// </summary>
        public void myDispose()
        {
            try
            {
                SATrace.FunctionScopeEnter("<sa.SADataReader.Dispose|API>", _objectId, new string[0]);
                myDispose(true);
                GC.SuppressFinalize(this);
            }
            finally
            {
                SATrace.FunctionScopeLeave();
            }
        }

        private void myDispose(bool disposing)
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

        private unsafe void GetColumnMetaData()
        {
            IntPtr columnNames = IntPtr.Zero;
            _hasBlobColumn = false;
            SAException.CheckException(PInvokeMethods.AsaDataReader_GetColumnNames(_idReader, ref _fieldCount, ref columnNames));
            _columns = new SADataReader.SAColumnMetaData[_fieldCount];
            _columnTypes = new DotNetType[_fieldCount];
            SAColumnName* saColumnNamePtr = (SAColumnName*)(void*)columnNames;
            for (int index = 0; index < _fieldCount; ++index)
            {
                SADataReader.SAColumnMetaData saColumnMetaData = new SADataReader.SAColumnMetaData(saColumnNamePtr->Ordinal, saColumnNamePtr->SADataType, SADataConvert.GetSADataTypeName((SADbType)saColumnNamePtr->SADataType), new string((char*)(void*)saColumnNamePtr->ColumnName));
                string str = saColumnMetaData.ColumnName;
                if (str.Length > 1 && str[0] == 34 && str[str.Length - 1] == 34)
                    saColumnMetaData.ColumnName = str.Substring(1, str.Length - 2);
                _columns[index] = saColumnMetaData;
                _columnTypes[index] = SADataConvert.MapToDotNetType(saColumnMetaData.SADataType);
                if (SADataConvert.IsLong((int)saColumnMetaData.SADataType))
                    _hasBlobColumn = true;
                ++saColumnNamePtr;
            }
            SAException.CheckException(PInvokeMethods.AsaDataReader_FreeColumnNames(_idReader, _fieldCount, columnNames));
            _rowBufferLength = _hasBlobColumn ? 1 : 20;
            _cachedRows = new object[_rowBufferLength, _fieldCount];
            _cachedRowCount = 0;
            _cachedRowCurrent = -1;
        }

        private void CheckClosed(string methodName)
        {
            if (_isClosed)
            {
                Exception e = new InvalidOperationException(SARes.GetString(11005, methodName));
                throw e;
            }
        }

        private void CheckClosed()
        {
            if (_isClosed)
            {
                Exception e = new InvalidOperationException(SARes.GetString(11004));
                throw e;
            }
        }

        private void CheckInvalidRead()
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

        private void CheckIndex(int index)
        {
            if (index < 0 || index >= _fieldCount)
            {
                Exception e = new IndexOutOfRangeException();
                throw e;
            }
        }

        private void CheckDBNull(int index)
        {
            if (IsDBNull(index))
            {
                Exception e = new SAException(SARes.GetString(11017));
                throw e;
            }
        }

        private void CheckBuffer(Array buffer, int bufferIndex, int length)
        {
            if (bufferIndex < 0 || bufferIndex + length > buffer.Length)
            {
                Exception e = new ArgumentOutOfRangeException("bufferIndex");
                throw e;
            }
        }

        private unsafe object FetchValue(int ordinal, DotNetType dotNetType, bool throwExIfDBNull, bool isGetValue)
        {
            CheckInvalidRead();
            CheckIndex(ordinal);
            object val1;
            if (_cachedRowCurrent >= 0)
            {
                val1 = _cachedRows[_cachedRowCurrent, ordinal];
                if (throwExIfDBNull && DBNull.Value.Equals(val1))
                {
                    Exception e = new SAException(SARes.GetString(11017));
                    throw e;
                }
            }
            else
            {
                if (IsDBNull(ordinal))
                {
                    if (throwExIfDBNull)
                    {
                        Exception e = new SAException(SARes.GetString(11017));
                        throw e;
                    }
                    return DBNull.Value;
                }
                IntPtr val2 = IntPtr.Zero;
                SAException.CheckException(PInvokeMethods.AsaDataReader_GetValue(_idReader, ordinal, ref val2));
                val1 = SADataConvert.SAToDotNet(((SAValue*)(void*)val2)->Value, _columnTypes[ordinal]);
                SAException.CheckException(PInvokeMethods.AsaDataReader_FreeValue(_idReader, ordinal, val2));
            }
            if (isGetValue || _columnTypes[ordinal] == dotNetType)
                return val1;
            return ConvertValue(_columnTypes[ordinal], dotNetType, val1);
        }

        private object ConvertValue(DotNetType fromType, DotNetType toType, object val)
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

        private unsafe object FetchValue(int ordinal, long index, int length, DotNetType dotNetType, bool throwExIfDBNull, bool isGetValue)
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
                obj1 = SADataConvert.SAToDotNet(((SAValue*)(void*)val)->Value, dotNetType);
                SAException.CheckException(PInvokeMethods.AsaDataReader_FreeValue(_idReader, ordinal, val));
            }
            return obj1;
        }

        public override IEnumerator GetEnumerator()
        {
            return new SADataReader.DREnumerator(this);
        }

        IList IListSource.GetList()
        {
            try
            {
                if (_isClosed || !_hasResult)
                    return null;
                DataTable table = new DataTable("Table");
                for (int ordinal = 0; ordinal < FieldCount; ++ordinal)
                    table.Columns.Add(GetName(ordinal)).DataType = GetFieldType(ordinal);
                object[] values = new object[FieldCount];
                while (Read())
                {
                    GetValues(values);
                    table.Rows.Add(values);
                }
                return (IList)new DataView(table);
            }
            finally
            {
                SATrace.FunctionScopeLeave();
            }
        }

        /// <summary>
        ///     <para>Returns the value of the specified column as a Boolean.</para>
        /// </summary>
        /// <remarks>
        ///     <para>No conversions are performed, so the data retrieved must already be a Boolean.</para>
        /// </remarks>
        /// <param name="ordinal">
        ///     An ordinal number indicating the column from which the value is obtained. The numbering is zero-based.
        /// </param>
        /// <returns>
        /// <para>The value of the column.</para>
        ///    </returns>
        /// <seealso cref="M:iAnywhere.Data.SQLAnywhere.SADataReader.GetOrdinal(System.String)" />
        /// <seealso cref="M:iAnywhere.Data.SQLAnywhere.SADataReader.GetFieldType(System.Int32)" />
        public override bool GetBoolean(int ordinal)
        {
            try
            {
                SATrace.FunctionScopeEnter("<sa.SADataReader.GetBoolean|API>", _objectId, "ordinal");
                return (bool)FetchValue(ordinal, DotNetType.Boolean, true, false);
            }
            finally
            {
                SATrace.FunctionScopeLeave();
            }
        }

        /// <summary>
        ///     <para>Returns the value of the specified column as a Byte.</para>
        /// </summary>
        /// <remarks>
        ///     <para>No conversions are performed, so the data retrieved must already be a byte.</para>
        /// </remarks>
        /// <param name="ordinal">
        ///     An ordinal number indicating the column from which the value is obtained. The numbering is zero-based.
        /// </param>
        /// <returns>
        /// <para>The value of the column.</para>
        ///    </returns>
        public override byte GetByte(int ordinal)
        {
            try
            {
                SATrace.FunctionScopeEnter("<sa.SADataReader.GetByte|API>", _objectId, "ordinal");
                return (byte)FetchValue(ordinal, DotNetType.Byte, true, false);
            }
            finally
            {
                SATrace.FunctionScopeLeave();
            }
        }

        public override unsafe long GetBytes(int ordinal, long dataIndex, byte[] buffer, int bufferIndex, int length)
        {
            try
            {
                SATrace.FunctionScopeEnter("<sa.SADataReader.GetBytes|API>", _objectId, "ordinal", "dataIndex", "buffer", "bufferIndex", "length");
                long actualSize = 0;
                int bytesRead = 0;
                CheckInvalidRead();
                CheckIndex(ordinal);
                CheckDBNull(ordinal);
                SADbType saDbType = _columns[ordinal].SADataType;
                if (!SADataConvert.IsBinary((int)saDbType))
                {
                    Exception e = new InvalidCastException();
                    SATrace.Exception(e);
                    throw e;
                }
                if (buffer == null)
                {
                    if (SADataConvert.IsLong((int)saDbType))
                    {
                        SAException.CheckException(PInvokeMethods.AsaDataReader_ReadBytes(_idReader, ordinal, 0L, (byte*)null, 0, 1, ref bytesRead, ref actualSize));
                    }
                    else
                    {
                        byte[] numArray = (byte[])GetValue(ordinal);
                        actualSize = numArray != null ? numArray.GetLength(0) : 0L;
                    }
                    return actualSize;
                }
                CheckBuffer(buffer, bufferIndex, length);
                if (saDbType == SADbType.Binary || saDbType == SADbType.VarBinary)
                {
                    byte[] numArray = (byte[])GetValue(ordinal);
                    int num = buffer.Length - bufferIndex > length ? length : buffer.Length - bufferIndex;
                    bytesRead = numArray.Length - (int)dataIndex > num ? num : numArray.Length - (int)dataIndex;
                    Array.Copy(numArray, (int)dataIndex, buffer, bufferIndex, bytesRead);
                }
                else
                {
                    if (saDbType != SADbType.LongBinary)
                    {
                        if (saDbType != SADbType.Image)
                            goto label_14;
                    }
                    int idEx;
                    fixed (byte* buffer1 = buffer)
                      idEx = PInvokeMethods.AsaDataReader_ReadBytes(_idReader, ordinal, dataIndex, buffer1, bufferIndex, length, ref bytesRead, ref actualSize);
                    SAException.CheckException(idEx);
                }
                label_14:
                return bytesRead;
            }
            finally
            {
                SATrace.FunctionScopeLeave();
            }
        }

        /// <summary>
        ///     <para>Returns the value of the specified column as a character.</para>
        /// </summary>
        /// <remarks>
        ///     <para>No conversions are performed, so the data retrieved must already be a character.</para>
        ///     <para>Call the SADataReader.IsDBNull method to check for null values before calling this method.</para>
        /// </remarks>
        /// <param name="ordinal">
        ///     An ordinal number indicating the column from which the value is obtained. The numbering is zero-based.
        /// </param>
        /// <returns>
        /// <para>The value of the column.</para>
        ///    </returns>
        /// <seealso cref="M:iAnywhere.Data.SQLAnywhere.SADataReader.IsDBNull(System.Int32)" />
        /// <seealso cref="M:iAnywhere.Data.SQLAnywhere.SADataReader.IsDBNull(System.Int32)" />
        public override char GetChar(int ordinal)
        {
            try
            {
                SATrace.FunctionScopeEnter("<sa.SADataReader.GetChar|API>", _objectId, "ordinal");
                Exception e = new NotSupportedException();
                SATrace.Exception(e);
                throw e;
            }
            finally
            {
                SATrace.FunctionScopeLeave();
            }
        }

        /// <summary>
        ///     <para>Reads a stream of characters from the specified column offset into the buffer as an array starting at the given buffer offset.</para>
        /// </summary>
        /// <remarks>
        ///     <para>GetChars returns the number of available characters in the field. In most cases this is the exact length of the field. However, the number returned may be less than the true length of the field if GetChars has already been used to obtain characters from the field. This may be the case, for example, when the SADataReader is reading a large data structure into a buffer.</para>
        ///     <para>If you pass a buffer that is a null reference (Nothing in Visual Basic), GetChars returns the length of the field in characters.</para>
        ///     <para>No conversions are performed, so the data retrieved must already be a character array.</para>
        ///     <para>For information about handling BLOBs, see @olink targetdoc="programming" targetptr="handling-blobs-adodotnet"@Handling BLOBs@/olink@.</para>
        /// </remarks>
        /// <param name="ordinal">The zero-based column ordinal.</param>
        /// <param name="dataIndex">
        ///     The index within the row from which to begin the read operation.
        /// </param>
        /// <param name="buffer">The buffer into which to copy data.</param>
        /// <param name="bufferIndex">
        ///     The index for buffer to begin the read operation.
        /// </param>
        /// <param name="length">The number of characters to read.</param>
        /// <returns>
        /// <para>The actual number of characters read.</para>
        ///    </returns>
        public override unsafe long GetChars(int ordinal, long dataIndex, char[] buffer, int bufferIndex, int length)
        {
            try
            {
                SATrace.FunctionScopeEnter("<sa.SADataReader.GetChars|API>", _objectId, "ordinal", "dataIndex", "buffer", "bufferIndex", "length");
                long actualSize = 0;
                int charsRead = 0;
                CheckInvalidRead();
                CheckIndex(ordinal);
                CheckDBNull(ordinal);
                if (_columns[ordinal].SADataType != SADbType.LongVarbit && _columns[ordinal].SADataType != SADbType.LongVarchar && (_columns[ordinal].SADataType != SADbType.LongNVarchar && _columns[ordinal].SADataType != SADbType.NText) && (_columns[ordinal].SADataType != SADbType.Text && _columns[ordinal].SADataType != SADbType.Xml))
                {
                    Exception e = new InvalidCastException();
                    SATrace.Exception(e);
                    throw e;
                }
                if (buffer == null)
                    return ((string)GetValue(ordinal)).Length;
                CheckBuffer(buffer, bufferIndex, length);
                int idEx;
                fixed (char* buffer1 = buffer)
                  idEx = PInvokeMethods.AsaDataReader_ReadChars(_idReader, ordinal, dataIndex, buffer1, bufferIndex, length, ref charsRead, ref actualSize);
                SAException.CheckException(idEx);
                return charsRead;
            }
            finally
            {
                SATrace.FunctionScopeLeave();
            }
        }

        /// <summary>
        ///     <para>Returns the name of the source data type.</para>
        /// </summary>
        /// <param name="index">The zero-based column ordinal.</param>
        /// <returns>
        /// <para>The name of the back-end data type.</para>
        ///    </returns>
        public override string GetDataTypeName(int index)
        {
            try
            {
                SATrace.FunctionScopeEnter("<sa.SADataReader.GetDataTypeName|API>", _objectId, "index");
                CheckClosed();
                CheckIndex(index);
                return _columns[index].SADataTypeName;
            }
            finally
            {
                SATrace.FunctionScopeLeave();
            }
        }

        /// <summary>
        ///     <para>Returns the value of the specified column as a DateTime object.</para>
        /// </summary>
        /// <remarks>
        ///     <para>No conversions are performed, so the data retrieved must already be a DateTime object.</para>
        ///     <para>Call the SADataReader.IsDBNull method to check for null values before calling this method.</para>
        /// </remarks>
        /// <param name="ordinal">The zero-based column ordinal.</param>
        /// <returns>
        /// <para>The value of the specified column.</para>
        ///    </returns>
        /// <seealso cref="M:iAnywhere.Data.SQLAnywhere.SADataReader.IsDBNull(System.Int32)" />
        public override DateTime GetDateTime(int ordinal)
        {
            try
            {
                SATrace.FunctionScopeEnter("<sa.SADataReader.GetDateTime|API>", _objectId, "ordinal");
                return (DateTime)FetchValue(ordinal, DotNetType.DateTime, true, false);
            }
            finally
            {
                SATrace.FunctionScopeLeave();
            }
        }

        /// <summary>
        ///     <para>Returns the value of the specified column as a TimeSpan object.</para>
        /// </summary>
        /// <remarks>
        ///     <para>The column must be a SQL Anywhere TIME data type. The data is converted to TimeSpan. The Days property of TimeSpan is always set to 0.</para>
        ///     <para>Call SADataReader.IsDBNull method to check for NULL values before calling this method.</para>
        ///     <para>For more information, see @olink targetdoc="programming" targetptr="adodotnet-development-s-4163101"@Obtaining time values@/olink@.</para>
        /// </remarks>
        /// <param name="ordinal">
        ///     An ordinal number indicating the column from which the value is obtained. The numbering is zero-based.
        /// </param>
        /// <returns>
        /// <para>The value of the specified column.</para>
        ///    </returns>
        /// <seealso cref="M:iAnywhere.Data.SQLAnywhere.SADataReader.IsDBNull(System.Int32)" />
        public TimeSpan GetTimeSpan(int ordinal)
        {
            try
            {
                SATrace.FunctionScopeEnter("<sa.SADataReader.GetTimeSpan|API>", _objectId, "ordinal");
                return (TimeSpan)FetchValue(ordinal, DotNetType.TimeSpan, true, false);
            }
            finally
            {
                SATrace.FunctionScopeLeave();
            }
        }

        /// <summary>
        ///     <para>Returns the value of the specified column as a Decimal object.</para>
        /// </summary>
        /// <remarks>
        ///     <para>No conversions are performed, so the data retrieved must already be a Decimal object.</para>
        ///     <para>Call the SADataReader.IsDBNull method to check for null values before calling this method.</para>
        /// </remarks>
        /// <param name="ordinal">
        ///     An ordinal number indicating the column from which the value is obtained. The numbering is zero-based.
        /// </param>
        /// <returns>
        /// <para>The value of the specified column.</para>
        ///    </returns>
        /// <seealso cref="M:iAnywhere.Data.SQLAnywhere.SADataReader.IsDBNull(System.Int32)" />
        public override Decimal GetDecimal(int ordinal)
        {
            try
            {
                SATrace.FunctionScopeEnter("<sa.SADataReader.GetDecimal|API>", _objectId, "ordinal");
                return (Decimal)FetchValue(ordinal, DotNetType.Decimal, true, false);
            }
            finally
            {
                SATrace.FunctionScopeLeave();
            }
        }

        /// <summary>
        ///     <para>Returns the value of the specified column as a double-precision floating point number.</para>
        /// </summary>
        /// <remarks>
        ///     <para>No conversions are performed, so the data retrieved must already be a double-precision floating point number.</para>
        ///     <para>Call the SADataReader.IsDBNull method to check for null values before calling this method.</para>
        /// </remarks>
        /// <param name="ordinal">
        ///     An ordinal number indicating the column from which the value is obtained. The numbering is zero-based.
        /// </param>
        /// <returns>
        /// <para>The value of the specified column.</para>
        ///    </returns>
        /// <seealso cref="M:iAnywhere.Data.SQLAnywhere.SADataReader.IsDBNull(System.Int32)" />
        public override double GetDouble(int ordinal)
        {
            try
            {
                SATrace.FunctionScopeEnter("<sa.SADataReader.GetDouble|API>", _objectId, "ordinal");
                return (double)FetchValue(ordinal, DotNetType.Double, true, false);
            }
            finally
            {
                SATrace.FunctionScopeLeave();
            }
        }

        /// <summary>Returns the Type that is the data type of the object.</summary>
        /// <param name="index">The zero-based column ordinal.</param>
        /// <returns>
        /// <para>The type that is the data type of the object.</para>
        ///    </returns>
        public override Type GetFieldType(int index)
        {
            try
            {
                SATrace.FunctionScopeEnter("<sa.SADataReader.GetFieldType|API>", _objectId, "index");
                CheckClosed();
                CheckIndex(index);
                switch (_columnTypes[index])
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
            finally
            {
                SATrace.FunctionScopeLeave();
            }
        }

        /// <summary>
        ///     Returns the value of the specified column as a single-precision floating point number.
        /// </summary>
        /// <remarks>
        ///     <para>No conversions are performed, so the data retrieved must already be a single-precision floating point number.</para>
        ///     <para>Call the SADataReader.IsDBNull method to check for null values before calling this method.</para>
        /// </remarks>
        /// <param name="ordinal">
        ///     An ordinal number indicating the column from which the value is obtained. The numbering is zero-based.
        /// </param>
        /// <returns>
        /// <para>The value of the specified column.</para>
        ///    </returns>
        /// <seealso cref="M:iAnywhere.Data.SQLAnywhere.SADataReader.IsDBNull(System.Int32)" />
        public override float GetFloat(int ordinal)
        {
            try
            {
                SATrace.FunctionScopeEnter("<sa.SADataReader.GetFloat|API>", _objectId, "ordinal");
                return (float)FetchValue(ordinal, DotNetType.Single, true, false);
            }
            finally
            {
                SATrace.FunctionScopeLeave();
            }
        }

        /// <summary>
        ///     <para>Returns the value of the specified column as a global unique identifier (GUID).</para>
        /// </summary>
        /// <remarks>
        ///     <para>The data retrieved must already be a globally-unique identifier or binary(16).</para>
        ///     <para>Call the SADataReader.IsDBNull method to check for null values before calling this method.</para>
        /// </remarks>
        /// <param name="ordinal">
        ///     An ordinal number indicating the column from which the value is obtained. The numbering is zero-based.
        /// </param>
        /// <returns>
        /// <para>The value of the specified column.</para>
        ///    </returns>
        /// <seealso cref="M:iAnywhere.Data.SQLAnywhere.SADataReader.IsDBNull(System.Int32)" />
        public override Guid GetGuid(int ordinal)
        {
            try
            {
                SATrace.FunctionScopeEnter("<sa.SADataReader.GetGuid|API>", _objectId, "ordinal");
                return (Guid)FetchValue(ordinal, DotNetType.Guid, true, false);
            }
            finally
            {
                SATrace.FunctionScopeLeave();
            }
        }

        /// <summary>
        ///     <para>Returns the value of the specified column as a 16-bit signed integer.</para>
        /// </summary>
        /// <remarks>
        ///     <para>No conversions are performed, so the data retrieved must already be a 16-bit signed integer.</para>
        /// </remarks>
        /// <param name="ordinal">
        ///     An ordinal number indicating the column from which the value is obtained. The numbering is zero-based.
        /// </param>
        /// <returns>
        /// <para>The value of the specified column.</para>
        ///    </returns>
        public override short GetInt16(int ordinal)
        {
            try
            {
                SATrace.FunctionScopeEnter("<sa.SADataReader.GetInt16|API>", _objectId, "ordinal");
                return (short)FetchValue(ordinal, DotNetType.Int16, true, false);
            }
            finally
            {
                SATrace.FunctionScopeLeave();
            }
        }

        /// <summary>
        ///     <para>Returns the value of the specified column as a 32-bit signed integer.</para>
        /// </summary>
        /// <remarks>
        ///     <para>No conversions are performed, so the data retrieved must already be a 32-bit signed integer.</para>
        /// </remarks>
        /// <param name="ordinal">
        ///     An ordinal number indicating the column from which the value is obtained. The numbering is zero-based.
        /// </param>
        /// <returns>
        /// <para>The value of the specified column.</para>
        ///    </returns>
        public override int GetInt32(int ordinal)
        {
            try
            {
                SATrace.FunctionScopeEnter("<sa.SADataReader.GetInt32|API>", _objectId, "ordinal");
                return (int)FetchValue(ordinal, DotNetType.Int32, true, false);
            }
            finally
            {
                SATrace.FunctionScopeLeave();
            }
        }

        /// <summary>
        ///     <para>Returns the value of the specified column as a 64-bit signed integer.</para>
        /// </summary>
        /// <remarks>
        ///     <para>No conversions are performed, so the data retrieved must already be a 64-bit signed integer.</para>
        /// </remarks>
        /// <param name="ordinal">
        ///     An ordinal number indicating the column from which the value is obtained. The numbering is zero-based.
        /// </param>
        /// <returns>
        /// <para>The value of the specified column.</para>
        ///    </returns>
        public override long GetInt64(int ordinal)
        {
            try
            {
                SATrace.FunctionScopeEnter("<sa.SADataReader.GetInt64|API>", _objectId, "ordinal");
                return (long)FetchValue(ordinal, DotNetType.Int64, true, false);
            }
            finally
            {
                SATrace.FunctionScopeLeave();
            }
        }

        /// <summary>
        ///     <para>Returns the name of the specified column.</para>
        /// </summary>
        /// <param name="index">The zero-based index of the column.</param>
        /// <returns>
        /// <para>The name of the specified column.</para>
        ///    </returns>
        public override string GetName(int index)
        {
            try
            {
                SATrace.FunctionScopeEnter("<sa.SADataReader.GetName|API>", _objectId, "ordinal");
                CheckClosed();
                CheckIndex(index);
                return _columns[index].ColumnName;
            }
            finally
            {
                SATrace.FunctionScopeLeave();
            }
        }

        /// <summary>
        ///     <para>Returns the column ordinal, given the column name.</para>
        /// </summary>
        /// <remarks>
        ///     <para>GetOrdinal performs a case-sensitive lookup first. If it fails, a second case-insensitive search is made.</para>
        ///     <para>GetOrdinal is Japanese kana-width insensitive.</para>
        ///     <para>Because ordinal-based lookups are more efficient than named lookups, it is inefficient to call GetOrdinal within a loop. You can save time by calling GetOrdinal once and assigning the results to an integer variable for use within the loop.</para>
        /// </remarks>
        /// <param name="name">The column name.</param>
        /// <returns>
        /// <para>The zero-based column ordinal.</para>
        ///    </returns>
        public override int GetOrdinal(string name)
        {
            try
            {
                SATrace.FunctionScopeEnter("<sa.SADataReader.GetOrdinal|API>", _objectId, "name");
                CheckClosed();
                if (name == null)
                {
                    Exception e = new ArgumentNullException("name");
                    SATrace.Exception(e);
                    throw e;
                }
                int ordinal = FindOrdinal(name);
                if (ordinal < 0)
                {
                    Exception e = new IndexOutOfRangeException(name);
                    SATrace.Exception(e);
                    throw e;
                }
                return ordinal;
            }
            finally
            {
                SATrace.FunctionScopeLeave();
            }
        }

        private int FindOrdinal(string name)
        {
            try
            {
                SATrace.FunctionScopeEnter("<sa.SADataReader.FindOrdinal|API>", _objectId, "name");
                bool[] flagArray = new bool[2] { false, true };
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
            finally
            {
                SATrace.FunctionScopeLeave();
            }
        }

        private void SetKeyUniqueColumns(DataTable schemaTable)
        {
            ArrayList indexCols = new ArrayList();
            ArrayList arrayList1 = new ArrayList();
            ArrayList arrayList2 = new ArrayList();
            ArrayList arrayList3 = new ArrayList();
            ArrayList keyColRows = new ArrayList();
            ArrayList arrayList4 = new ArrayList();
            foreach (DataRow row in (InternalDataCollectionBase)schemaTable.Rows)
            {
                object obj = row["BaseTableName"];
                if (!DBNull.Value.Equals(obj))
                {
                    string str = (string)obj;
                    if (!arrayList1.Contains(str))
                        arrayList1.Add(str);
                }
            }
            if (arrayList1.Count < 1)
                return;
            for (int index = 0; index < arrayList1.Count; ++index)
            {
                arrayList2.Add(new ArrayList());
                arrayList3.Add(new ArrayList());
            }
            StringBuilder stringBuilder = new StringBuilder();
            for (int index = 0; index < arrayList1.Count; ++index)
            {
                if (index > 0)
                    stringBuilder.Append(" OR ");
                stringBuilder.Append(string.Format(" ( systable.table_name = '{0}' )", SAUtility.EscapeQuotationMarks('\'', arrayList1[index] as string)));
            }
            string string1 = stringBuilder.ToString();
            SACommand command1 = _conn.CreateCommand();
            command1.CommandText = string.Format("SELECT systable.table_name, sysindex.index_id, sysindex.[unique], syscolumn.column_name FROM sys.systable JOIN sys.sysindex JOIN sys.sysixcol JOIN sys.syscolumn WHERE ( ( sysindex.[unique] = 'U' ) OR ( sysindex.[unique] = 'Y' ) ) AND ( {0} ) ORDER BY systable.table_name, sysindex.index_id, sysindex.[unique]", string1);
            SADataReader saDataReader1 = command1.ExecuteReader();
            while (saDataReader1.Read())
                indexCols.Add(new SADataReader.IndexColumnInfo(saDataReader1.GetString(0), saDataReader1.GetUInt32(1), saDataReader1.GetString(2), saDataReader1.GetString(3)));
            saDataReader1.Close();
            command1.Dispose();
            foreach (SADataReader.IndexColumnInfo indexColumnInfo in indexCols)
            {
                if (string.Compare(indexColumnInfo.Unique, "U", true) == 0)
                {
                    foreach (DataRow row in (InternalDataCollectionBase)schemaTable.Rows)
                    {
                        if (!DBNull.Value.Equals(row["BaseTableName"]) && !DBNull.Value.Equals(row["ColumnName"]) && (string.Compare(indexColumnInfo.TableName, (string)row["BaseTableName"], true) == 0 && string.Compare(indexColumnInfo.ColumnName, (string)row["ColumnName"], true) == 0))
                            row["IsUnique"] = true;
                    }
                }
            }
            SACommand command2 = _conn.CreateCommand();
            command2.CommandText = string.Format("SELECT systable.table_name, syscolumn.column_name FROM   sys.systable JOIN sys.syscolumn ON sys.systable.table_id = sys.syscolumn.table_id WHERE  ( syscolumn.pkey = 'Y' ) AND ( {0} )", string1);
            SADataReader saDataReader2 = command2.ExecuteReader();
            while (saDataReader2.Read())
            {
                string string2 = saDataReader2.GetString(0);
                string string3 = saDataReader2.GetString(1);
                ((ArrayList)arrayList2[arrayList1.IndexOf(string2)]).Add(string3);
            }
            saDataReader2.Close();
            command2.Dispose();
            bool flag = true;
            for (int index1 = 0; index1 < arrayList1.Count; ++index1)
            {
                string baseTable = (string)arrayList1[index1];
                ArrayList keyCols = (ArrayList)arrayList2[index1];
                if (FindPKFromKeyColumns(baseTable, schemaTable, keyCols, keyColRows) || FindPKFromUniqueColumns(baseTable, schemaTable, keyColRows) || FindPKFromUIColumns(baseTable, schemaTable, indexCols, keyColRows))
                {
                    for (int index2 = 0; index2 < keyColRows.Count; ++index2)
                        arrayList4.Add(keyColRows[index2]);
                }
                else
                    return;
            }
            if (!flag || arrayList4 == null)
                return;
            foreach (DataRow dataRow in arrayList4)
                dataRow["IsKey"] = true;
        }

        private bool FindPKFromKeyColumns(string baseTable, DataTable schemaTable, ArrayList keyCols, ArrayList keyColRows)
        {
            if (keyCols != null && keyCols.Count > 0)
            {
                keyColRows.Clear();
                for (int index = 0; index < keyCols.Count; ++index)
                {
                    string strA = (string)keyCols[index];
                    foreach (DataRow row in (InternalDataCollectionBase)schemaTable.Rows)
                    {
                        if (!DBNull.Value.Equals(row["BaseTableName"]) && !DBNull.Value.Equals(row["ColumnName"]) && (string.Compare(baseTable, (string)row["BaseTableName"], true) == 0 && string.Compare(strA, (string)row["ColumnName"], true) == 0))
                        {
                            bool flag = false;
                            foreach (DataRow keyColRow in keyColRows)
                            {
                                if (string.Compare(strA, (string)keyColRow["ColumnName"], true) == 0)
                                {
                                    flag = true;
                                    break;
                                }
                            }
                            if (!flag)
                                keyColRows.Add((object)row);
                        }
                    }
                }
                if (keyColRows.Count == keyCols.Count)
                {
                    if (keyColRows.Count == 1)
                        ((DataRow)keyColRows[0])["IsUnique"] = true;
                    return true;
                }
            }
            return false;
        }

        private bool FindPKFromUniqueColumns(string baseTable, DataTable schemaTable, ArrayList keyColRows)
        {
            keyColRows.Clear();
            foreach (DataRow row in (InternalDataCollectionBase)schemaTable.Rows)
            {
                if (!DBNull.Value.Equals(row["BaseTableName"]) && !DBNull.Value.Equals(row["ColumnName"]) && (!DBNull.Value.Equals(row["AllowDBNull"]) && !DBNull.Value.Equals(row["IsUnique"])) && (string.Compare(baseTable, (string)row["BaseTableName"], true) == 0 && (bool)row["IsUnique"] && !(bool)row["AllowDBNull"]))
                {
                    bool flag = false;
                    string strA = (string)row["ColumnName"];
                    foreach (DataRow keyColRow in keyColRows)
                    {
                        if (string.Compare(strA, (string)keyColRow["ColumnName"], true) == 0)
                        {
                            flag = true;
                            break;
                        }
                    }
                    if (!flag)
                        keyColRows.Add((object)row);
                }
            }
            return keyColRows.Count > 0;
        }

        private bool FindPKFromUIColumns(string baseTable, DataTable schemaTable, ArrayList indexCols, ArrayList keyColRows)
        {
            int index1 = 0;
            ArrayList keyCols = new ArrayList();
            while (index1 < indexCols.Count)
            {
                SADataReader.IndexColumnInfo indexColumnInfo1 = (SADataReader.IndexColumnInfo)indexCols[index1];
                if (string.Compare(baseTable, indexColumnInfo1.TableName, true) == 0 && string.Compare("Y", indexColumnInfo1.Unique, true) == 0)
                {
                    int num1;
                    int num2 = num1 = index1;
                    for (; index1 < indexCols.Count; ++index1)
                    {
                        SADataReader.IndexColumnInfo indexColumnInfo2 = (SADataReader.IndexColumnInfo)indexCols[index1];
                        if ((int)indexColumnInfo1.IndexId == (int)indexColumnInfo2.IndexId && string.Compare("Y", indexColumnInfo2.Unique, true) == 0 && string.Compare(baseTable, indexColumnInfo2.TableName, true) == 0)
                            num1 = index1;
                        else
                            break;
                    }
                    keyCols.Clear();
                    for (int index2 = num2; index2 <= num1; ++index2)
                        keyCols.Add(((SADataReader.IndexColumnInfo)indexCols[index2]).ColumnName);
                    if (FindPKFromKeyColumns(baseTable, schemaTable, keyCols, keyColRows))
                        return true;
                }
                else
                    ++index1;
            }
            return false;
        }

        private object GetSchemaBoolValue(int value)
        {
            if (value == 1)
                return true;
            if (value == 0)
                return false;
            return DBNull.Value;
        }

        private unsafe object GetSchemaStringValue(IntPtr value)
        {
            if (value != IntPtr.Zero)
                return new string((char*)(void*)value);
            return DBNull.Value;
        }

        /// <summary>
        ///     <para>Returns the value of the specified column as a string.</para>
        /// </summary>
        /// <remarks>
        ///     <para>No conversions are performed, so the data retrieved must already be a string.</para>
        ///     <para>Call the SADataReader.IsDBNull method to check for NULL values before calling this method.</para>
        /// </remarks>
        /// <param name="ordinal">
        ///     An ordinal number indicating the column from which the value is obtained. The numbering is zero-based.
        /// </param>
        /// <returns>
        /// <para>The value of the specified column.</para>
        ///    </returns>
        /// <seealso cref="M:iAnywhere.Data.SQLAnywhere.SADataReader.IsDBNull(System.Int32)" />
        public override string GetString(int ordinal)
        {
            try
            {
                SATrace.FunctionScopeEnter("<sa.SADataReader.GetString|API>", _objectId, "ordinal");
                return (string)FetchValue(ordinal, DotNetType.String, true, false);
            }
            finally
            {
                SATrace.FunctionScopeLeave();
            }
        }

        /// <summary>
        ///     <para>Returns the value of the specified column as a 16-bit unsigned integer.</para>
        /// </summary>
        /// <remarks>
        ///     <para>No conversions are performed, so the data retrieved must already be a 16-bit unsigned integer.</para>
        /// </remarks>
        /// <param name="ordinal">
        ///     An ordinal number indicating the column from which the value is obtained. The numbering is zero-based.
        /// </param>
        /// <returns>
        /// <para>The value of the specified column.</para>
        ///    </returns>
        [CLSCompliant(false)]
        public ushort GetUInt16(int ordinal)
        {
            try
            {
                SATrace.FunctionScopeEnter("<sa.SADataReader.GetUInt16|API>", _objectId, "ordinal");
                return (ushort)FetchValue(ordinal, DotNetType.UInt16, true, false);
            }
            finally
            {
                SATrace.FunctionScopeLeave();
            }
        }

        /// <summary>
        ///     <para>Returns the value of the specified column as a 32-bit unsigned integer.</para>
        /// </summary>
        /// <remarks>
        ///     <para>No conversions are performed, so the data retrieved must already be a 32-bit unsigned integer.</para>
        /// </remarks>
        /// <param name="ordinal">
        ///     An ordinal number indicating the column from which the value is obtained. The numbering is zero-based.
        /// </param>
        /// <returns>
        /// <para>The value of the specified column.</para>
        ///    </returns>
        [CLSCompliant(false)]
        public uint GetUInt32(int ordinal)
        {
            try
            {
                SATrace.FunctionScopeEnter("<sa.SADataReader.GetUInt32|API>", _objectId, "ordinal");
                return (uint)FetchValue(ordinal, DotNetType.UInt32, true, false);
            }
            finally
            {
                SATrace.FunctionScopeLeave();
            }
        }

        /// <summary>
        ///     <para>Returns the value of the specified column as a 64-bit unsigned integer.</para>
        /// </summary>
        /// <remarks>
        ///     <para>No conversions are performed, so the data retrieved must already be a 64-bit unsigned integer.</para>
        /// </remarks>
        /// <param name="ordinal">
        ///     An ordinal number indicating the column from which the value is obtained. The numbering is zero-based.
        /// </param>
        /// <returns>
        /// <para>The value of the specified column.</para>
        ///    </returns>
        [CLSCompliant(false)]
        public ulong GetUInt64(int ordinal)
        {
            try
            {
                SATrace.FunctionScopeEnter("<sa.SADataReader.GetUInt64|API>", _objectId, "ordinal");
                return (ulong)FetchValue(ordinal, DotNetType.UInt64, true, false);
            }
            finally
            {
                SATrace.FunctionScopeLeave();
            }
        }

        /// <summary>
        ///     <para>Returns the value of the specified column as an Object.</para>
        /// </summary>
        /// <remarks>
        ///     <para>This method returns DBNull for NULL database columns.</para>
        /// </remarks>
        /// <param name="ordinal">
        ///     An ordinal number indicating the column from which the value is obtained. The numbering is zero-based.
        /// </param>
        /// <returns>
        /// <para>The value of the specified column as an object.</para>
        ///    </returns>
        public override object GetValue(int ordinal)
        {
            try
            {
                SATrace.FunctionScopeEnter("<sa.SADataReader.GetValue|API>", _objectId, "ordinal");
                return FetchValue(ordinal, _columnTypes[ordinal], false, true);
            }
            finally
            {
                SATrace.FunctionScopeLeave();
            }
        }

        /// <summary>
        ///     <para>Returns a substring of the value of the specified column as an Object.</para>
        /// </summary>
        /// <remarks>
        ///     <para>This method returns DBNull for NULL database columns.</para>
        /// </remarks>
        /// <param name="ordinal">
        ///     An ordinal number indicating the column from which the value is obtained. The numbering is zero-based.
        /// </param>
        /// <param name="index">
        ///     A zero-based index of the substring of the value to be obtained.
        /// </param>
        /// <param name="length">
        ///     The length of the substring of the value to be obtained.
        /// </param>
        /// <returns>
        /// <para>The substring value is returned as an object.</para>
        ///    </returns>
        public object GetValue(int ordinal, long index, int length)
        {
            try
            {
                SATrace.FunctionScopeEnter("<sa.SADataReader.GetValue|API>", _objectId, "ordinal");
                return FetchValue(ordinal, index, length, _columnTypes[ordinal], false, true);
            }
            finally
            {
                SATrace.FunctionScopeLeave();
            }
        }

        /// <summary>
        ///     <para>Gets all the columns in the current row.</para>
        /// </summary>
        /// <remarks>
        ///     <para>For most applications, the GetValues method provides an efficient means for retrieving all columns, rather than retrieving each column individually.</para>
        ///     <para>You can pass an Object array that contains fewer than the number of columns contained in the resulting row. Only the amount of data the Object array holds is copied to the array. You can also pass an Object array whose length is more than the number of columns contained in the resulting row.</para>
        ///     <para>This method returns DBNull for NULL database columns.</para>
        /// </remarks>
        /// <param name="values">
        ///     An array of objects that holds an entire row of the result set.
        /// </param>
        /// <returns>
        /// <para>The number of objects in the array.</para>
        ///    </returns>
        public override int GetValues(object[] values)
        {
            try
            {
                SATrace.FunctionScopeEnter("<sa.SADataReader.GetValues|API>", _objectId, "values");
                CheckInvalidRead();
                if (values == null || values.Length < 1)
                {
                    Exception e = new ArgumentNullException("values");
                    SATrace.Exception(e);
                    throw e;
                }
                int num = values.Length;
                if (num > _fieldCount)
                    num = _fieldCount;
                if (num < 1)
                    return 0;
                for (int index = 0; index < num; ++index)
                    values[index] = _cachedRows[_cachedRowCurrent, index];
                return num;
            }
            finally
            {
                SATrace.FunctionScopeLeave();
            }
        }

        /// <summary>
        ///     <para>Returns a value indicating whether the column contains NULL values.</para>
        /// </summary>
        /// <remarks>
        ///     <para>Call this method to check for NULL column values before calling the typed get methods (for example, GetByte, GetChar, and so on) to avoid raising an exception.</para>
        /// </remarks>
        /// <param name="ordinal">The zero-based column ordinal.</param>
        /// <returns>
        /// <para>Returns true if the specified column value is equivalent to DBNull. Otherwise, it returns false.</para>
        ///    </returns>
        public override bool IsDBNull(int ordinal)
        {
            try
            {
                SATrace.FunctionScopeEnter("<sa.SADataReader.IsDBNull|API>", _objectId, "ordinal");
                CheckInvalidRead();
                CheckIndex(ordinal);
                bool isDBNull = false;
                if (_cachedRowCurrent >= 0)
                    isDBNull = _cachedRows[_cachedRowCurrent, ordinal].Equals(DBNull.Value);
                else
                    SAException.CheckException(PInvokeMethods.AsaDataReader_IsDBNull(_idReader, ordinal, ref isDBNull));
                return isDBNull;
            }
            finally
            {
                SATrace.FunctionScopeLeave();
            }
        }

        /// <summary>
        ///     <para>Advances the SADataReader to the next result, when reading the results of batch SQL statements.</para>
        /// </summary>
        /// <remarks>
        ///     <para>Used to process multiple results, which can be generated by executing batch SQL statements.</para>
        ///     <para>By default, the data reader is positioned on the first result.</para>
        /// </remarks>
        /// <returns>
        /// <para>Returns true if there are more result sets. Otherwise, it returns false.</para>
        ///    </returns>
        public override bool NextResult()
        {
            try
            {
                SATrace.FunctionScopeEnter("<sa.SADataReader.NextResult|API>", _objectId, new string[0]);
                CheckClosed("NextResult");
                if ((_cmdBehavior & CommandBehavior.SingleResult) > CommandBehavior.Default)
                    return false;
                SAException.CheckException(PInvokeMethods.AsaDataReader_NextResult(_idReader, ref _hasResult));
                Init(true);
                if (_hasResult)
                    _hasRows = Read();
                return _hasResult;
            }
            finally
            {
                SATrace.FunctionScopeLeave();
            }
        }

        /// <summary>
        ///     <para>Reads the next row of the result set and moves the SADataReader to that row.</para>
        /// </summary>
        /// <remarks>
        ///     <para>The default position of the SADataReader is prior to the first record. Therefore, you must call Read to begin accessing any data.</para>
        /// </remarks>
        /// <returns>
        /// <para>Returns true if there are more rows. Otherwise, it returns false.</para>
        ///    </returns>
        /// <example>
        ///             <para>The following code fills a listbox with the values in a single column of results.</para>
        ///             <code>while( reader.Read() )
        /// {
        ///     listResults.Items.Add(
        ///         reader.GetValue( 0 ).ToString() );
        /// }
        /// listResults.EndUpdate();
        /// reader.Close();</code>
        /// 
        ///         </example>
        public override unsafe bool Read()
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
                        SAValue* saValuePtr = (SAValue*)(void*)values;
                        for (int index1 = 0; index1 < _cachedRowCount; ++index1)
                        {
                            for (int index2 = 0; index2 < _fieldCount; ++index2)
                            {
                                _cachedRows[index1, index2] = SADataConvert.SAToDotNet(saValuePtr->Value, _columnTypes[index2]);
                                ++saValuePtr;
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
        private struct SAColumnMetaData
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
        private struct IndexColumnInfo
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
            private SADataReader _dataReader;

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
                Exception e = new NotSupportedException();
                throw e;
            }

            public bool MoveNext()
            {
                return _dataReader.Read();
            }
        }
    }
}
