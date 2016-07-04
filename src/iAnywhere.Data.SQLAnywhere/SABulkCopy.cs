
// Type: iAnywhere.Data.SQLAnywhere.SABulkCopy
// Assembly: iAnywhere.Data.SQLAnywhere.v4.0, Version=11.0.1.27424, Culture=neutral, PublicKeyToken=f222fc4333e0d400
// MVID: CC4F9F8C-E618-49D1-9147-C06A9EF53D1F
// Assembly location: C:\Program Files\SQL Anywhere 11\Assembly\V4\iAnywhere.Data.SQLAnywhere.v4.0.dll

using System;
using System.Collections;
using System.Data;
using System.Text;

namespace iAnywhere.Data.SQLAnywhere
{
  /// <summary>
  ///     <para>Efficiently bulk load a SQL Anywhere table with data from another source.</para>
  /// </summary>
  /// <remarks>
  ///     <para><b>Restrictions:</b> The SABulkCopy class is not available in the .NET Compact Framework 2.0.</para>
  ///     <para><b>Implements: </b> <see cref="T:System.IDisposable" /></para>
  /// </remarks>
  public sealed class SABulkCopy : IDisposable
  {
    private int _bulkCopyTimeout = 30;
    private int _objectId = SABulkCopy.s_CurrentId++;
    private const int c_TimeoutDefaultValue = 30;
    private SABulkCopyColumnMappingCollection _mappings;
    private SAConnection _conn;
    private SATransaction _externalTransaction;
    private SABulkCopyOptions _options;
    private int _batchSize;
    private string _destinationTableName;
    private int _notifyAfter;
    private bool _ownConnection;
    private bool _closed;
    private bool _cantClose;
    private static int s_CurrentId;

    /// <summary>
    ///     <para>Gets or sets the number of rows in each batch. At the end of each batch, the rows in the batch are sent to the server.</para>
    /// </summary>
    /// <value> The number of rows in each batch. The default is 0.</value>
    /// <remarks>
    ///     <para>Setting this property to zero causes all the rows to be sent in one batch.</para>
    ///     <para>Setting this property to a value less than zero is an error.</para>
    ///     <para>If this value is changed while a batch is in progress, the current batch completes and any further batches use the new value.</para>
    /// </remarks>
    public int BatchSize
    {
      get
      {
        return _batchSize;
      }
      set
      {
        if (value < 0)
        {
          Exception e = new ArgumentOutOfRangeException("BatchSize", SARes.GetString(15008, "BatchSize"));
          SATrace.Exception(e);
          throw e;
        }
                _batchSize = value;
      }
    }

    /// <summary>
    ///     <para>Gets or sets the number of seconds for the operation to complete before it times out.</para>
    /// </summary>
    /// <value>
    /// <para>The default value is 30 seconds.</para>
    /// </value>
    /// <remarks>
    ///     <para>A value of zero indicates no limit. This should be avoided because it may cause an indefinite wait.</para>
    ///     <para>If the operation times out, then all rows in the current transaction are rolled back and an SAException is raised.</para>
    ///     <para>Setting this property to a value less than zero is an error.</para>
    /// </remarks>
    public int BulkCopyTimeout
    {
      get
      {
        return _bulkCopyTimeout;
      }
      set
      {
        if (value < 0)
        {
          Exception e = new ArgumentOutOfRangeException("BulkCopyTimeout", SARes.GetString(15008, "BulkCopyTimeout"));
          SATrace.Exception(e);
          throw e;
        }
                _bulkCopyTimeout = value;
      }
    }

    /// <summary>
    ///     <para>Returns a collection of SABulkCopyColumnMapping items. Column mappings define the relationships between columns in the data source and columns in the destination. </para>
    /// </summary>
    /// <value>By default, it is an empty collection.</value>
    /// <remarks>
    ///     <para>The property cannot be modified while WriteToServer is executing.</para>
    ///     <para>If ColumnMappings is empty when WriteToServer is executed, then the first column in the source is mapped to the first column in the destination, the second to the second, and so on. This takes place as long as the column types are convertible, there are at least as many destination columns as source columns, and any extra destination columns are nullable.</para>
    /// </remarks>
    public SABulkCopyColumnMappingCollection ColumnMappings
    {
      get
      {
        return _mappings;
      }
    }

    /// <summary>
    ///     <para>Gets or sets the name of the destination table on the server. </para>
    /// </summary>
    /// <value>The default value is a null reference. In Visual Basic it is Nothing.</value>
    /// <remarks>
    ///     <para>If the value is changed while WriteToServer is executing, the change has no effect.</para>
    ///     <para>If the value has not been set before a call to WriteToServer, an InvalidOperationException is raised.</para>
    ///     <para>It is an error to set the value to NULL or the empty string.</para>
    /// </remarks>
    public string DestinationTableName
    {
      get
      {
        return _destinationTableName;
      }
      set
      {
        if (value == null)
        {
          Exception e = new ArgumentNullException("DestinationTableName");
          SATrace.Exception(e);
          throw e;
        }
        if (value.Length == 0)
        {
          Exception e = new ArgumentOutOfRangeException("DestinationTableName");
          SATrace.Exception(e);
          throw e;
        }
                _destinationTableName = value;
      }
    }

    /// <summary>
    ///     <para>Gets or sets the number of rows to be processed before generating a notification event.</para>
    /// </summary>
    /// <value>Zero is returned if the property has not been set.</value>
    /// <remarks>
    ///     <para>Changes made to NotifyAfter, while executing WriteToServer, do not take effect until after the next notification.</para>
    ///     <para>Setting this property to a value less than zero is an error.</para>
    ///     <para>The values of NotifyAfter and BulkCopyTimeOut are mutually exclusive, so the event can fire even if no rows have been sent to the database or committed.</para>
    /// </remarks>
    /// <seealso cref="P:iAnywhere.Data.SQLAnywhere.SABulkCopy.BulkCopyTimeout" />
    public int NotifyAfter
    {
      get
      {
        return _notifyAfter;
      }
      set
      {
        if (value < 0)
        {
          Exception e = new ArgumentOutOfRangeException("NotifyAfter", SARes.GetString(15008, "NotifyAfter"));
          SATrace.Exception(e);
          throw e;
        }
                _notifyAfter = value;
      }
    }

    /// <summary>
    ///     <para>This event occurs every time the number of rows specified by the NotifyAfter property have been processed.</para>
    /// </summary>
    /// <remarks>
    ///     <para>The receipt of an SARowsCopied event does not imply that any rows have been sent to the database server or committed. You cannot call the Close method from this event.</para>
    /// </remarks>
    /// <seealso cref="P:iAnywhere.Data.SQLAnywhere.SABulkCopy.NotifyAfter" />
    public event SARowsCopiedEventHandler SARowsCopied;

    /// <summary>
    ///     <para>Initializes an SABulkCopy object.</para>
    /// </summary>
    /// <remarks>
    ///     <para><b>Restrictions:</b> The SABulkCopy class is not available in the .NET Compact Framework 2.0.</para>
    /// </remarks>
    /// <param name="connection">
    ///     The already open SAConnection that will be used to perform the bulk-copy operation. If the connection is not open, an exception is thrown in WriteToServer.
    /// </param>
    public SABulkCopy(SAConnection connection)
      : this(connection, SABulkCopyOptions.Default, null)
    {
    }

    /// <summary>
    ///     <para>Initializes an SABulkCopy object.</para>
    /// </summary>
    /// <remarks>
    ///     <para>This syntax opens a connection during WriteToServer using connectionString. The connection is closed at the end of WriteToServer.</para>
    ///     <para><b>Restrictions:</b> The SABulkCopy class is not available in the .NET Compact Framework 2.0.</para>
    /// </remarks>
    /// <param name="connectionString">
    ///     The string defining the connection that will be opened for use by the SABulkCopy instance. A connection string is a semicolon-separated list of keyword=value pairs.
    /// </param>
    public SABulkCopy(string connectionString)
      : this(connectionString, SABulkCopyOptions.Default)
    {
    }

    /// <summary>
    ///     <para>Initializes an SABulkCopy object.</para>
    /// </summary>
    /// <remarks>
    ///     <para>This syntax opens a connection during WriteToServer using connectionString. The connection is closed at the end of WriteToServer. The copyOptions parameter has the effects described above.</para>
    ///     <para><b>Restrictions:</b> The SABulkCopy class is not available in the .NET Compact Framework 2.0.</para>
    /// </remarks>
    /// <param name="connectionString">
    ///     The string defining the connection that will be opened for use by the SABulkCopy instance. A connection string is a semicolon-separated list of keyword=value pairs.
    /// </param>
    /// <param name="copyOptions">
    ///     A combination of values from the SABulkCopyOptions enumeration that determines which data source rows are copied to the destination table.
    /// </param>
    public SABulkCopy(string connectionString, SABulkCopyOptions copyOptions)
    {
      if (connectionString == null)
      {
        Exception e = new ArgumentNullException("connectionString");
        SATrace.Exception(e);
        throw e;
      }
            _conn = new SAConnection(connectionString);
            _ownConnection = true;
            _mappings = new SABulkCopyColumnMappingCollection();
            _externalTransaction = null;
            _options = copyOptions;
    }

    /// <summary>
    ///     <para>Initializes an SABulkCopy object.</para>
    /// </summary>
    /// <remarks>
    ///     <para><b>Restrictions:</b> The SABulkCopy class is not available in the .NET Compact Framework 2.0.</para>
    /// </remarks>
    /// <param name="connection">
    ///     The already open SAConnection that will be used to perform the bulk-copy operation. If the connection is not open, an exception is thrown in WriteToServer.
    /// </param>
    /// <param name="copyOptions">
    ///     A combination of values from the SABulkCopyOptions enumeration that determines which data source rows are copied to the destination table.
    /// </param>
    /// <param name="externalTransaction">
    ///     An existing SATransaction instance under which the bulk copy will occur. If externalTransaction is not NULL, then the bulk-copy operation is done within it. It is an error to specify both an external transaction and the UseInternalTransaction option.
    /// </param>
    public SABulkCopy(SAConnection connection, SABulkCopyOptions copyOptions, SATransaction externalTransaction)
    {
      if (connection == null)
      {
        Exception e = new ArgumentNullException("connection");
        SATrace.Exception(e);
        throw e;
      }
      if ((copyOptions & SABulkCopyOptions.UseInternalTransaction) != SABulkCopyOptions.Default && externalTransaction != null)
      {
        Exception e = new ArgumentException(SARes.GetString(15017), "externalTransaction");
        SATrace.Exception(e);
        throw e;
      }
            _mappings = new SABulkCopyColumnMappingCollection();
            _conn = connection;
            _externalTransaction = externalTransaction;
            _options = copyOptions;
    }

    /// <summary>
    ///     <para>Destructs an SABulkCopy object.</para>
    /// </summary>
    ~SABulkCopy()
    {
      try
      {
        SATrace.FunctionScopeEnter("<sa.SABulkCopy.Finalize|API>", _objectId, new string[0]);
                Dispose(false);
      }
      finally
      {
        SATrace.FunctionScopeLeave();
      }
    }

    /// <summary>
    ///     <para>Disposes of the SABulkCopy instance.</para>
    /// </summary>
    public void Dispose()
    {
      try
      {
        SATrace.FunctionScopeEnter("<sa.SABulkCopy.Dispose|API>", _objectId, new string[0]);
                Dispose(true);
        GC.SuppressFinalize(this);
      }
      finally
      {
        SATrace.FunctionScopeLeave();
      }
    }

    private void Dispose(bool disposing)
    {
      if (_closed)
        return;
      if (disposing && _ownConnection && _conn.GetConnectionState() == ConnectionState.Open)
                _conn.Close();
            _closed = true;
    }

    /// <summary>
    ///     <para>Closes the SABulkCopy instance.</para>
    /// </summary>
    public void Close()
    {
      if (_cantClose)
      {
        Exception e = new InvalidOperationException(SARes.GetString(15013));
        SATrace.Exception(e);
        throw e;
      }
            Dispose();
    }

    /// <summary>
    ///     <para>Copies all rows in the supplied array of <see cref="T:System.Data.DataRow" /> objects to a destination table specified by the DestinationTableName property of the SABulkCopy object.</para>
    /// </summary>
    /// <remarks>
    ///     <para><b>Restrictions:</b> The SABulkCopy class is not available in the .NET Compact Framework 2.0.</para>
    /// </remarks>
    /// <param name="rows">
    ///     An array of System.Data.DataRow objects that will be copied to the destination table.
    /// </param>
    /// <seealso cref="P:iAnywhere.Data.SQLAnywhere.SABulkCopy.DestinationTableName" />
    public void WriteToServer(DataRow[] rows)
    {
      try
      {
        SATrace.FunctionScopeEnter("<sa.SABulkCopy.WriteToServer|API>", _objectId, "rows");
                CheckSource(rows);
        this._WriteToServer(rows, false, (DataRowState) -1);
      }
      finally
      {
        SATrace.FunctionScopeLeave();
      }
    }

    /// <summary>
    ///     <para>Copies all rows in the supplied <see cref="T:System.Data.DataTable" /> to a destination table specified by the DestinationTableName property of the SABulkCopy object.</para>
    /// </summary>
    /// <remarks>
    ///     <para><b>Restrictions:</b> The SABulkCopy class is not available in the .NET Compact Framework 2.0.</para>
    /// </remarks>
    /// <param name="table">
    ///     A System.Data.DataTable whose rows will be copied to the destination table.
    /// </param>
    /// <seealso cref="P:iAnywhere.Data.SQLAnywhere.SABulkCopy.DestinationTableName" />
    public void WriteToServer(DataTable table)
    {
      try
      {
        SATrace.FunctionScopeEnter("<sa.SABulkCopy.WriteToServer|API>", _objectId, "table");
                CheckSource(table);
        this._WriteToServer(FindRowsFromTable(table), false, (DataRowState) -1);
      }
      finally
      {
        SATrace.FunctionScopeLeave();
      }
    }

    /// <summary>
    ///     <para>Copies all rows in the supplied <see cref="T:System.Data.IDataReader" /> to a destination table specified by the DestinationTableName property of the SABulkCopy object.</para>
    /// </summary>
    /// <remarks>
    ///     <para><b>Restrictions:</b> The SABulkCopy class is not available in the .NET Compact Framework 2.0.</para>
    /// </remarks>
    /// <param name="reader">
    ///     A System.Data.IDataReader whose rows will be copied to the destination table.
    /// </param>
    /// <seealso cref="P:iAnywhere.Data.SQLAnywhere.SABulkCopy.DestinationTableName" />
    public void WriteToServer(IDataReader reader)
    {
      try
      {
        SATrace.FunctionScopeEnter("<sa.SABulkCopy.WriteToServer|API>", _objectId, "reader");
                CheckSource(reader);
        this._WriteToServer(FindRowsFromReader(reader), false, (DataRowState) -1);
      }
      finally
      {
        SATrace.FunctionScopeLeave();
      }
    }

    /// <summary>
    ///     <para>Copies all rows in the supplied <see cref="T:System.Data.DataTable" /> with the specified row state to a destination table specified by the DestinationTableName property of the SABulkCopy object.</para>
    /// </summary>
    /// <remarks>
    ///     <para>Only those rows matching the row state are copied.</para>
    ///     <para><b>Restrictions:</b> The SABulkCopy class is not available in the .NET Compact Framework 2.0.</para>
    /// </remarks>
    /// <param name="table">
    ///     A System.Data.DataTable whose rows will be copied to the destination table.
    /// </param>
    /// <param name="rowState">
    ///     A value from the System.Data.DataRowState enumeration. Only rows matching the row state are copied to the destination.
    /// </param>
    /// <seealso cref="P:iAnywhere.Data.SQLAnywhere.SABulkCopy.DestinationTableName" />
    public void WriteToServer(DataTable table, DataRowState rowState)
    {
      try
      {
        SATrace.FunctionScopeEnter("<sa.SABulkCopy.WriteToServer|API>", _objectId, "table", "rowState");
                CheckSource(table);
                _WriteToServer(FindRowsFromTable(table), true, rowState);
      }
      finally
      {
        SATrace.FunctionScopeLeave();
      }
    }

    private void CheckSource(object source)
    {
      if (source == null)
      {
        Exception e = new ArgumentNullException("source");
        SATrace.Exception(e);
        throw e;
      }
    }

    private void _WriteToServer(DataRow[] source, bool filterByRowState, DataRowState rowState)
    {
      if (_closed)
      {
        Exception e = new InvalidOperationException(SARes.GetString(15012));
        SATrace.Exception(e);
        throw e;
      }
      if (_ownConnection && _conn.GetConnectionState() == ConnectionState.Closed)
                _conn.Open();
      else if (_conn.GetConnectionState() == ConnectionState.Closed)
      {
        Exception e = new InvalidOperationException(SARes.GetString(10993, "WriteToServer"));
        SATrace.Exception(e);
        throw e;
      }
      if (_destinationTableName == null)
      {
        Exception e = new InvalidOperationException(SARes.GetString(10987, "DestinationTableName", "WriteToServer"));
        SATrace.Exception(e);
        throw e;
      }
      if (source.Length == 0)
        return;
      SATransaction tran = null;
      bool flag1 = (_options & SABulkCopyOptions.DoNotFireTriggers) != SABulkCopyOptions.Default;
      bool flag2 = (_options & SABulkCopyOptions.TableLock) != SABulkCopyOptions.Default;
      try
      {
                _mappings.AllowChanges = false;
        int num = _batchSize;
        int notifyAfter = _notifyAfter;
        DateTime now = DateTime.Now;
        ArrayList sourceColumnOrdering = new ArrayList();
        string stmt = BuildInsertStmt(source[0], sourceColumnOrdering);
        if (flag1)
                    DisableTriggers();
        if (flag2 && _externalTransaction != null)
                    LockTable(_externalTransaction);
        SACommand command = CreateCommand(stmt);
        long rowCount = 0;
        if ((_options & SABulkCopyOptions.UseInternalTransaction) != SABulkCopyOptions.Default)
        {
          tran = _conn.BeginTransaction();
          command.Transaction = tran;
          if (flag2)
                        LockTable(tran);
        }
        for (int index1 = 0; index1 < source.Length; ++index1)
        {
                    CheckBulkCopyRunTime(now);
          DataRow row = source[index1];
          if (!filterByRowState || rowState == row.RowState)
          {
            for (int index2 = 0; index2 < sourceColumnOrdering.Count; ++index2)
              command.Parameters.Add(new SAParameter((source.Length * index1 + index2).ToString(), FindColumnVal(row, sourceColumnOrdering[index2])));
                        FireNotifyAfterEvent(rowCount, ref notifyAfter);
            if (index1 == source.Length - 1 || num != 0 && (rowCount + 1L) % num == 0L)
            {
              command.Prepare();
              command.ExecuteNonQuery();
              command.Parameters.Clear();
              num = _batchSize;
              if ((_options & SABulkCopyOptions.UseInternalTransaction) != SABulkCopyOptions.Default)
              {
                tran.Commit();
                tran = _conn.BeginTransaction();
                command.Transaction = tran;
                if (flag2)
                                    LockTable(tran);
              }
            }
            ++rowCount;
          }
        }
        if (tran == null)
          return;
        tran.Commit();
      }
      catch (Exception ex)
      {
        if (tran != null)
          tran.Rollback();
        throw;
      }
      finally
      {
        if (flag1)
                    ResetTriggers();
        if (_ownConnection)
                    _conn.Close();
                _mappings.AllowChanges = true;
      }
    }

    private void LockTable(SATransaction tran)
    {
      SACommand command = CreateCommand(string.Format("LOCK TABLE {0} IN SHARE MODE", _destinationTableName));
      command.Transaction = tran;
      command.ExecuteNonQuery();
    }

    private SACommand CreateCommand(string stmt)
    {
      SACommand saCommand = new SACommand(stmt, _conn);
      if (_externalTransaction != null)
        saCommand.Transaction = _externalTransaction;
      return saCommand;
    }

    private void DisableTriggers()
    {
            CreateCommand("SET TEMPORARY OPTION FIRE_TRIGGERS = OFF").ExecuteNonQuery();
    }

    private void ResetTriggers()
    {
            CreateCommand("SET TEMPORARY OPTION FIRE_TRIGGERS =").ExecuteNonQuery();
    }

    private void CheckBulkCopyRunTime(DateTime start)
    {
      if ((DateTime.Now - start).TotalSeconds > _bulkCopyTimeout)
      {
        Exception e = new SAException(SARes.GetString(15015));
        SATrace.Exception(e);
        throw e;
      }
    }

    private void FireNotifyAfterEvent(long rowCount, ref int notifyAfter)
    {
      if (notifyAfter == 0 || (rowCount + 1L) % notifyAfter != 0L)
        return;
      if (SARowsCopied != null)
      {
        SARowsCopiedEventArgs rowsCopiedEventArgs = new SARowsCopiedEventArgs(rowCount + 1L);
                _cantClose = true;
        try
        {
                    SARowsCopied(this, rowsCopiedEventArgs);
        }
        finally
        {
                    _cantClose = false;
        }
        if (rowsCopiedEventArgs.Abort)
        {
          Exception e = (Exception) new SystemException(SARes.GetString(17032));
          SATrace.Exception(e);
          throw e;
        }
      }
      notifyAfter = _notifyAfter;
    }

    private object FindColumnVal(DataRow row, object columnIndicator)
    {
      return !(columnIndicator is int) ? row[(string) columnIndicator] : row[columnIndicator];
    }

    private string BuildInsertStmt(DataRow sourceRow, ArrayList sourceColumnOrdering)
    {
      StringBuilder stringBuilder1 = new StringBuilder();
      stringBuilder1.Append("INSERT INTO ");
      stringBuilder1.Append(_destinationTableName);
      stringBuilder1.Append(" ");
      StringBuilder stringBuilder2 = new StringBuilder();
      stringBuilder2.Append(string.Format("SELECT * FROM \"{0}\"", _destinationTableName));
      DataTable schemaTable1;
      using (SADataReader saDataReader = CreateCommand(stringBuilder2.ToString()).ExecuteReader(CommandBehavior.SchemaOnly))
        schemaTable1 = saDataReader.GetSchemaTable();
      DataTable schemaTable2;
      using (DataTableReader dataReader = sourceRow.Table.CreateDataReader())
        schemaTable2 = dataReader.GetSchemaTable();
      if (_mappings == null || _mappings.Count == 0)
      {
                CheckThatSchemaMatch(schemaTable2, schemaTable1);
        stringBuilder1.Append("VALUES (");
        for (int index = 0; index < schemaTable1.Rows.Count; ++index)
        {
          if (index > 0)
            stringBuilder1.Append(", ");
          if (index < sourceRow.ItemArray.Length && (!this.IsIdentityColumn(schemaTable1.Rows[index]["ColumnName"], schemaTable1) || (_options & SABulkCopyOptions.KeepIdentity) != SABulkCopyOptions.Default))
          {
            stringBuilder1.Append("?");
            sourceColumnOrdering.Add(index);
          }
          else
            stringBuilder1.Append("DEFAULT");
        }
        stringBuilder1.Append(")");
      }
      else
      {
        stringBuilder1.Append("( ");
        for (int index = 0; index < _mappings.Count; ++index)
        {
                    CheckTypesMatch(_mappings[index], schemaTable2, schemaTable1);
          if (!IsIdentityColumn(_mappings[index].Destination, schemaTable1) || (_options & SABulkCopyOptions.KeepIdentity) != SABulkCopyOptions.Default)
          {
            if (sourceColumnOrdering.Count > 0)
              stringBuilder1.Append(", ");
            stringBuilder1.Append(ColumnName(_mappings[index], schemaTable1));
            sourceColumnOrdering.Add(_mappings[index].Source);
          }
        }
        stringBuilder1.Append(" ) VALUES ( ?");
        for (int index = 1; index < _mappings.Count; ++index)
          stringBuilder1.Append(", ?");
        stringBuilder1.Append(" )");
      }
      return stringBuilder1.ToString();
    }

    private DataRow FindSchemaRow(object columnMarker, DataTable schema)
    {
      string index = !(columnMarker is int) ? "ColumnName" : "ColumnOrdinal";
      foreach (DataRow row in (InternalDataCollectionBase) schema.Rows)
      {
        if (row[index].Equals(columnMarker))
          return row;
      }
      return (DataRow) null;
    }

    private void CheckTypesMatch(SABulkCopyColumnMapping mapping, DataTable sourceSchema, DataTable destinationSchema)
    {
      DataRow schemaRow1 = FindSchemaRow(mapping.Source, sourceSchema);
      if (schemaRow1 == null)
      {
        Exception e = new InvalidOperationException(SARes.GetString(15010, mapping.Source.ToString()));
        SATrace.Exception(e);
        throw e;
      }
      DataRow schemaRow2 = FindSchemaRow(mapping.Destination, destinationSchema);
      if (schemaRow2 == null)
      {
        Exception e = new InvalidOperationException(SARes.GetString(15009, mapping.Destination.ToString()));
        SATrace.Exception(e);
        throw e;
      }
            CheckTypesMatch(schemaRow1, schemaRow2);
    }

    private void CheckTypesMatch(DataRow sourceRow, DataRow destinationRow)
    {
      object obj1 = sourceRow["DataType"];
      object obj2 = destinationRow["DataType"];
      if (!obj1.Equals(obj2))
      {
        Exception e = new InvalidOperationException(SARes.GetString(15018, obj1.ToString(), obj2.ToString()));
        SATrace.Exception(e);
        throw e;
      }
    }

    private void CheckThatSchemaMatch(DataTable sourceSchema, DataTable destinationSchema)
    {
      if (sourceSchema.Rows.Count > destinationSchema.Rows.Count)
      {
        Exception e = new InvalidOperationException(SARes.GetString(15016));
        SATrace.Exception(e);
        throw e;
      }
      for (int index = 0; index < sourceSchema.Rows.Count; ++index)
        this.CheckTypesMatch(sourceSchema.Rows[index], destinationSchema.Rows[index]);
    }

    private bool IsIdentityColumn(object destination, DataTable schemaTable)
    {
      foreach (DataRow row in (InternalDataCollectionBase) schemaTable.Rows)
      {
        if (destination is int && (int) row["ColumnOrdinal"] == destination || destination is string && (string) row["ColumnName"] == (string) destination)
          return (bool) row["IsIdentity"] || (bool) row["IsAutoIncrement"];
      }
      return false;
    }

    private string ColumnName(int destinationOrdinal, DataTable schema)
    {
      foreach (DataRow row in (InternalDataCollectionBase) schema.Rows)
      {
        if ((int) row["ColumnOrdinal"] == destinationOrdinal)
          return (string) row["ColumnName"];
      }
      return null;
    }

    private string ColumnName(SABulkCopyColumnMapping mapping, DataTable schema)
    {
      if (mapping.DestinationColumn != null)
        return mapping.DestinationColumn;
      return ColumnName(mapping.DestinationOrdinal, schema);
    }

    private DataRow[] FindRowsFromTable(DataTable source)
    {
      DataRow[] array = new DataRow[source.Rows.Count];
      source.Rows.CopyTo(array, 0);
      return array;
    }

    private DataRow[] FindRowsFromReader(IDataReader source)
    {
      DataTable source1 = new DataTable();
      source1.Load(source);
      return FindRowsFromTable(source1);
    }
  }
}
