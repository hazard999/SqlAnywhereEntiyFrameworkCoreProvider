
// Type: iAnywhere.Data.SQLAnywhere.SADataAdapter
// Assembly: iAnywhere.Data.SQLAnywhere.v4.0, Version=11.0.1.27424, Culture=neutral, PublicKeyToken=f222fc4333e0d400
// MVID: CC4F9F8C-E618-49D1-9147-C06A9EF53D1F
// Assembly location: C:\Program Files\SQL Anywhere 11\Assembly\V4\iAnywhere.Data.SQLAnywhere.v4.0.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Drawing.Design;

namespace iAnywhere.Data.SQLAnywhere
{
  /// <summary>
  ///     <para>Represents a set of commands and a database connection used to fill a <see cref="T:System.Data.DataSet" /> and to update a database.</para>
  /// </summary>
  /// <remarks>
  ///     <para>The <see cref="T:System.Data.DataSet" /> provides a way to work with data offline. The SADataAdapter provides methods to associate a DataSet with a set of SQL statements.</para>
  ///     <para><b>Implements:</b> <see cref="T:System.Data.IDbDataAdapter" />, <see cref="T:System.Data.IDataAdapter" />, <see cref="T:System.ICloneable" /></para>
  ///     <para>For more information, see @olink targetdoc="programming" targetptr="using-adapter-access-manipulate"@Using the SADataAdapter object to access and manipulate data@/olink@ and @olink targetdoc="programming" targetptr="accessing-adodotnet-dev"@Accessing and manipulating data@/olink@.</para>
  /// </remarks>
  [ToolboxBitmap(typeof (SADataAdapter), "DataAdapter.bmp")]
  [ToolboxItem("iAnywhere.VSIntegration.SQLAnywhere.DataAdapterToolboxItem, iAnywhere.VSIntegration.SQLAnywhere, Culture=neutral, PublicKeyToken=f222fc4333e0d400, Version=11.0.1.27424")]
  [DefaultEvent("RowUpdated")]
  [Designer("iAnywhere.VSIntegration.SQLAnywhere.DataAdapterDesigner, iAnywhere.VSIntegration.SQLAnywhere, Culture=neutral, PublicKeyToken=f222fc4333e0d400, Version=11.0.1.27424", typeof (IDesigner))]
  public sealed class SADataAdapter : DbDataAdapter, ICloneable
  {
    private List<SADataAdapter.CommandRowPair> _batchInsertCommands = new List<SADataAdapter.CommandRowPair>();
    private int _objectId = SADataAdapter.s_CurrentId++;
    private new const string DefaultSourceTableName = "Table";
    private SACommandBuilder _cmdBuilder;
    private int _batchSize;
    private int _currentBatchCommand;
    private SACommand _batchCommand;
    private string _methodName;
    private static int s_CurrentId;
    private bool _disposed;
    private bool _canceled;

    internal SACommandBuilder CommandBuilder
    {
      get
      {
        return _cmdBuilder;
      }
      set
      {
                _cmdBuilder = value;
      }
    }

    /// <summary>
    ///     <para>Gets or sets the number of rows that are processed in each round-trip to the server.</para>
    /// </summary>
    /// <remarks>
    ///     <para>The default value is 1.</para>
    ///     <para>Setting the value to something greater than 1 causes SADataAdapter.Update to execute all the insert statements in batches. The deletions and updates are executed sequentially as before, but insertions are executed afterward in batches of size equal to the value of UpdateBatchSize. Setting the value to 0 causes Update to send the insert statements in a single batch.</para>
    ///     <para>Setting the value to something greater than 1 causes SADataAdapter.Fill to execute all the insert statements in batches. The deletions and updates are executed sequentially as before, but insertions are executed afterward in batches of size equal to the value of UpdateBatchSize.</para>
    ///     <para>Setting the value to 0 causes Fill to send the insert statements in a single batch.</para>
    ///     <para>Setting it less than 0 is an error.</para>
    ///     <para>If UpdateBatchSize is set to something other than one, and the InsertCommand property is set to something that is not an INSERT statement, then an exception is thrown when calling Fill.</para>
    ///     <para>This behavior is different from SqlDataAdapter. It batches all types of commands.</para>
    /// </remarks>
    public override int UpdateBatchSize
    {
      get
      {
        SATrace.PropertyCall("<sa.SADataAdapter.get_UpdateBatchSize|API>", _objectId);
        return _batchSize;
      }
      set
      {
        SATrace.PropertyCall("<sa.SADataAdapter.set_UpdateBatchSize|API>", _objectId);
        int num = 100;
        if (value > num || value <= 0)
                    _batchSize = num;
        else
                    _batchSize = value;
      }
    }

    /// <summary>
    ///     <para>Specifies a collection that provides the master mapping between a source table and a DataTable.</para>
    /// </summary>
    /// <remarks>
    ///     <para>The default value is an empty collection.</para>
    ///     <para>When reconciling changes, the SADataAdapter uses the DataTableMappingCollection collection to associate the column names used by the data source with the column names used by the DataSet.</para>
    ///     <para><b>Restrictions:</b> The TableMappings property is not available in the .NET Compact Framework 2.0.</para>
    /// </remarks>
    [Description("How to map source table to DataSet table.")]
    [Editor("iAnywhere.VSIntegration.SQLAnywhere.TableMappingsEditor, iAnywhere.VSIntegration.SQLAnywhere, Culture=neutral, PublicKeyToken=f222fc4333e0d400, Version=11.0.1.27424", typeof (UITypeEditor))]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    [Category("Mapping")]
    public new DataTableMappingCollection TableMappings
    {
      get
      {
        return base.TableMappings;
      }
    }

    /// <summary>
    ///     <para>Specifies an SACommand object that is executed against the database when the Update method is called to delete rows in the database that correspond to deleted rows in the DataSet.</para>
    /// </summary>
    /// <remarks>
    ///     <para>If this property is not set and primary key information is present in the DataSet during Update, DeleteCommand can be generated automatically by setting SelectCommand and using the SACommandBuilder. In that case, the SACommandBuilder generates any additional commands that you do not set. This generation logic requires key column information to be present in the SelectCommand.</para>
    ///     <para>When DeleteCommand is assigned to an existing SACommand object, the SACommand object is not cloned. The DeleteCommand maintains a reference to the existing SACommand.</para>
    /// </remarks>
    /// <seealso cref="P:iAnywhere.Data.SQLAnywhere.SADataAdapter.SelectCommand" />
    [Description("Used during update for deleted rows in DataSet.")]
    [Editor("iAnywhere.VSIntegration.SQLAnywhere.CommandEditor, iAnywhere.VSIntegration.SQLAnywhere, Culture=neutral, PublicKeyToken=f222fc4333e0d400, Version=11.0.1.27424", typeof (UITypeEditor))]
    [Category("Update")]
    [DefaultValue(null)]
    public SACommand DeleteCommand
    {
      get
      {
        SATrace.PropertyCall("<sa.SADataAdapter.get_DeleteCommand|API>", _objectId);
        return (SACommand) base.DeleteCommand;
      }
      set
      {
        SATrace.PropertyCall("<sa.SADataAdapter.set_DeleteCommand|API>", _objectId);
                DeleteCommand = (DbCommand) value;
      }
    }

    /// <summary>
    ///     <para>Specifies an SACommand that is executed against the database when the Update method is called that adds rows to the database to correspond to rows that were inserted in the DataSet.</para>
    /// </summary>
    /// <remarks>
    ///     <para>The SACommandBuilder does not require key columns to generate InsertCommand.</para>
    ///     <para>When InsertCommand is assigned to an existing SACommand object, the SACommand is not cloned. The InsertCommand maintains a reference to the existing SACommand.</para>
    ///     <para>If this command returns rows, the rows may be added to the DataSet depending on how you set the UpdatedRowSource property of the SACommand object.</para>
    /// </remarks>
    [DefaultValue(null)]
    [Description("Used during update for new rows in DataSet.")]
    [Editor("iAnywhere.VSIntegration.SQLAnywhere.CommandEditor, iAnywhere.VSIntegration.SQLAnywhere, Culture=neutral, PublicKeyToken=f222fc4333e0d400, Version=11.0.1.27424", typeof (UITypeEditor))]
    [Category("Update")]
    public SACommand InsertCommand
    {
      get
      {
        SATrace.PropertyCall("<sa.SADataAdapter.get_InsertCommand|API>", _objectId);
        return (SACommand) base.InsertCommand;
      }
      set
      {
        SATrace.PropertyCall("<sa.SADataAdapter.set_InsertCommand|API>", _objectId);
                InsertCommand = (DbCommand) value;
      }
    }

    /// <summary>
    ///     <para>Specifies an SACommand that is used during Fill or FillSchema to obtain a result set from the database for copying into a DataSet.</para>
    /// </summary>
    /// <remarks>
    ///     <para>When SelectCommand is assigned to a previously-created SACommand, the SACommand is not cloned. The SelectCommand maintains a reference to the previously-created SACommand object.</para>
    ///     <para>If the SelectCommand does not return any rows, no tables are added to the DataSet, and no exception is raised.</para>
    ///     <para>The SELECT statement can also be specified in the SADataAdapter constructor.</para>
    /// </remarks>
    [Editor("iAnywhere.VSIntegration.SQLAnywhere.CommandEditor, iAnywhere.VSIntegration.SQLAnywhere, Culture=neutral, PublicKeyToken=f222fc4333e0d400, Version=11.0.1.27424", typeof (UITypeEditor))]
    [DefaultValue(null)]
    [Category("Fill")]
    [Description("Used during Fill/FillSchema.")]
    public SACommand SelectCommand
    {
      get
      {
        SATrace.PropertyCall("<sa.SADataAdapter.get_SelectCommand|API>", _objectId);
        return (SACommand) base.SelectCommand;
      }
      set
      {
        SATrace.PropertyCall("<sa.SADataAdapter.set_SelectCommand|API>", _objectId);
                SelectCommand = (DbCommand) value;
      }
    }

    /// <summary>
    ///     <para>Specifies an SACommand that is executed against the database when the Update method is called to update rows in the database that correspond to updated rows in the DataSet.</para>
    /// </summary>
    /// <remarks>
    ///     <para>During Update, if this property is not set and primary key information is present in the SelectCommand, the UpdateCommand can be generated automatically if you set the SelectCommand property and use the SACommandBuilder. Then, any additional commands that you do not set are generated by the SACommandBuilder. This generation logic requires key column information to be present in the SelectCommand.</para>
    ///     <para>When UpdateCommand is assigned to a previously-created SACommand, the SACommand is not cloned. The UpdateCommand maintains a reference to the previously-created SACommand object.</para>
    ///     <para>If execution of this command returns rows, these rows can be merged with the DataSet depending on how you set the UpdatedRowSource property of the SACommand object.</para>
    /// </remarks>
    [DefaultValue(null)]
    [Description("Used during update for modified rows in DataSet.")]
    [Category("Update")]
    [Editor("iAnywhere.VSIntegration.SQLAnywhere.CommandEditor, iAnywhere.VSIntegration.SQLAnywhere, Culture=neutral, PublicKeyToken=f222fc4333e0d400, Version=11.0.1.27424", typeof (UITypeEditor))]
    public SACommand UpdateCommand
    {
      get
      {
        SATrace.PropertyCall("<sa.SADataAdapter.get_UpdateCommand|API>", _objectId);
        return (SACommand) base.UpdateCommand;
      }
      set
      {
        SATrace.PropertyCall("<sa.SADataAdapter.set_UpdateCommand|API>", _objectId);
                UpdateCommand = (DbCommand) value;
      }
    }

    /// <summary>
    ///     <para>Occurs during an update after a command is executed against the data source. When an attempt to update is made, the event fires.</para>
    /// </summary>
    /// <remarks>
    ///     <para>The event handler receives an argument of type SARowUpdatedEventArgs containing data related to this event.</para>
    ///     <para>For more information, see the .NET Framework documentation for OleDbDataAdapter.RowUpdated Event.</para>
    /// </remarks>
    [Description("Event triggered after every DataRow is updated.")]
    [Category("Update")]
    public event SARowUpdatedEventHandler RowUpdated;

    /// <summary>
    ///     <para>Occurs during an update before a command is executed against the data source. When an attempt to update is made, the event fires.</para>
    /// </summary>
    /// <remarks>
    ///     <para>The event handler receives an argument of type SARowUpdatingEventArgs containing data related to this event.</para>
    ///     <para>For more information, see the .NET Framework documentation for OleDbDataAdapter.RowUpdating Event.</para>
    /// </remarks>
    [Category("Update")]
    [Description("Event triggered before every DataRow is updated.")]
    public event SARowUpdatingEventHandler RowUpdating;

    /// <summary>
    ///     <para>Initializes an SADataAdapter object.</para>
    /// </summary>
    /// <seealso cref="M:iAnywhere.Data.SQLAnywhere.SADataAdapter.#ctor(iAnywhere.Data.SQLAnywhere.SACommand)" />
    /// <seealso cref="M:iAnywhere.Data.SQLAnywhere.SADataAdapter.#ctor(System.String,iAnywhere.Data.SQLAnywhere.SAConnection)" />
    /// <seealso cref="M:iAnywhere.Data.SQLAnywhere.SADataAdapter.#ctor(System.String,System.String)" />
    public SADataAdapter()
    {
            Init();
    }

    /// <summary>
    ///     <para>Initializes an SADataAdapter object with the specified SELECT statement.</para>
    /// </summary>
    /// <param name="selectCommand">
    ///     An SACommand object that is used during <see cref="M:System.Data.Common.DbDataAdapter.Fill(System.Data.DataSet)" /> to select records from the data source for placement in the <see cref="T:System.Data.DataSet" />.
    /// </param>
    /// <seealso cref="M:iAnywhere.Data.SQLAnywhere.SADataAdapter.#ctor" />
    /// <seealso cref="M:iAnywhere.Data.SQLAnywhere.SADataAdapter.#ctor(System.String,iAnywhere.Data.SQLAnywhere.SAConnection)" />
    /// <seealso cref="M:iAnywhere.Data.SQLAnywhere.SADataAdapter.#ctor(System.String,System.String)" />
    public SADataAdapter(SACommand selectCommand)
    {
            Init();
            SelectCommand = selectCommand;
    }

    /// <summary>
    ///     <para>Initializes an SADataAdapter object with the specified SELECT statement and connection.</para>
    /// </summary>
    /// <param name="selectCommandText">
    ///     A SELECT statement to be used to set the SADataAdapter.SelectCommand property of the SADataAdapter object.
    /// </param>
    /// <param name="selectConnection">
    ///     An SAConnection object that defines a connection to a database.
    /// </param>
    /// <seealso cref="M:iAnywhere.Data.SQLAnywhere.SADataAdapter.#ctor" />
    /// <seealso cref="M:iAnywhere.Data.SQLAnywhere.SADataAdapter.#ctor(iAnywhere.Data.SQLAnywhere.SACommand)" />
    /// <seealso cref="M:iAnywhere.Data.SQLAnywhere.SADataAdapter.#ctor(System.String,System.String)" />
    /// <seealso cref="P:iAnywhere.Data.SQLAnywhere.SADataAdapter.SelectCommand" />
    /// <seealso cref="T:iAnywhere.Data.SQLAnywhere.SAConnection" />
    public SADataAdapter(string selectCommandText, SAConnection selectConnection)
    {
            Init();
            SelectCommand = new SACommand(selectCommandText);
            SelectCommand.Connection = selectConnection;
    }

    /// <summary>
    ///     <para>Initializes an SADataAdapter object with the specified SELECT statement and connection string.</para>
    /// </summary>
    /// <param name="selectCommandText">
    ///     A SELECT statement to be used to set the SADataAdapter.SelectCommand property of the SADataAdapter object.
    /// </param>
    /// <param name="selectConnectionString">
    ///     A connection string for a SQL Anywhere database.
    /// </param>
    /// <seealso cref="M:iAnywhere.Data.SQLAnywhere.SADataAdapter.#ctor" />
    /// <seealso cref="M:iAnywhere.Data.SQLAnywhere.SADataAdapter.#ctor(iAnywhere.Data.SQLAnywhere.SACommand)" />
    /// <seealso cref="M:iAnywhere.Data.SQLAnywhere.SADataAdapter.#ctor(System.String,iAnywhere.Data.SQLAnywhere.SAConnection)" />
    /// <seealso cref="P:iAnywhere.Data.SQLAnywhere.SADataAdapter.SelectCommand" />
    public SADataAdapter(string selectCommandText, string selectConnectionString)
    {
            Init();
            SelectCommand = new SACommand(selectCommandText);
            SelectCommand.Connection = new SAConnection(selectConnectionString);
    }

    private SADataAdapter(SADataAdapter other)
      : base((DbDataAdapter) other)
    {
            _batchSize = 1;
    }

    object ICloneable.Clone()
    {
      return new SADataAdapter(this);
    }

    /// <summary>
    ///     <para>Releases the unmanaged resources used by the SADataAdapter object and optionally releases the managed resources.</para>
    /// </summary>
    /// <param name="disposing">
    ///     True releases both managed and unmanaged resources; false releases only unmanaged resources.
    /// </param>
    /// <seealso cref="T:iAnywhere.Data.SQLAnywhere.SADataAdapter" />
    protected override void Dispose(bool disposing)
    {
      try
      {
        SATrace.FunctionScopeEnter("<sa.SADataAdapter.Dispose|API>", _objectId, "disposing");
        if (_disposed)
          return;
        try
        {
          if (disposing)
                        Init();
                    _disposed = true;
        }
        finally
        {
          base.Dispose(disposing);
        }
      }
      finally
      {
        SATrace.FunctionScopeLeave();
      }
    }

    /// <summary>
    ///     <para>Raises the RowUpdated event of a .NET Framework data provider.</para>
    /// </summary>
    /// <param name="value">
    ///     A <see cref="T:System.Data.Common.RowUpdatedEventArgs" /> that contains the event data.
    /// </param>
    protected override void OnRowUpdated(RowUpdatedEventArgs value)
    {
      try
      {
        SATrace.FunctionScopeEnter("<sa.SADataAdapter.OnRowUpdated|API>", _objectId, "value");
        base.OnRowUpdated(value);
        if (RowUpdated == null)
          return;
                RowUpdated(this, (SARowUpdatedEventArgs) value);
      }
      finally
      {
        SATrace.FunctionScopeLeave();
      }
    }

    /// <summary>
    ///     <para>Raises the RowUpdating event of a .NET Framework data provider.</para>
    /// </summary>
    /// <param name="value">
    ///     A <see cref="T:System.Data.Common.RowUpdatingEventArgs" /> that contains the event data.
    /// </param>
    protected override void OnRowUpdating(RowUpdatingEventArgs value)
    {
      try
      {
        SATrace.FunctionScopeEnter("<sa.SADataAdapter.Dispose|API>", _objectId, "value");
        base.OnRowUpdating(value);
        if (RowUpdating == null)
          return;
                RowUpdating(this, (SARowUpdatingEventArgs) value);
      }
      finally
      {
        SATrace.FunctionScopeLeave();
      }
    }

    private void Init()
    {
            _cmdBuilder = null;
            _batchSize = 1;
            _methodName = null;
    }

    private void AddToBatch(SACommand cmd, DataRow row)
    {
      if (cmd.UpdatedRowSource != UpdateRowSource.None)
      {
        Exception e = new SAException(SARes.GetString(14970));
        SATrace.Exception(e);
        throw e;
      }
      if (_batchCommand == null)
                _batchCommand = new SACommand(cmd);
            _batchInsertCommands.Add(new SADataAdapter.CommandRowPair(new SACommand(cmd, true), row));
    }

    /// <summary>
    ///     <para>Removes all SACommand objects from the batch.</para>
    /// </summary>
    /// <seealso cref="T:iAnywhere.Data.SQLAnywhere.SACommand" />
    protected override void ClearBatch()
    {
      try
      {
        SATrace.FunctionScopeEnter("<sa.SADataAdapter.ClearBatch|API>", _objectId, new string[0]);
                _batchInsertCommands.Clear();
                _currentBatchCommand = 0;
                _batchCommand = null;
      }
      finally
      {
        SATrace.FunctionScopeLeave();
      }
    }

    private int ExecuteBatch(Hashtable srcColMapToDataCol, out DataRow lastRow)
    {
      int num = 0;
      lastRow = (DataRow) null;
      if ((InsertCommand != null || _cmdBuilder != null) && _batchInsertCommands.Count > 0)
        num += ExecuteBatch(srcColMapToDataCol, _batchInsertCommands, out lastRow);
      return num;
    }

    private void CheckBatchCommand(SACommand cmd)
    {
      if (!cmd.CommandText.Trim().StartsWith("INSERT ", StringComparison.OrdinalIgnoreCase))
      {
        Exception e = new SAException(SARes.GetString(14971));
        SATrace.Exception(e);
        throw e;
      }
    }

    private int ExecuteBatch(Hashtable srcColMapToDataCol, List<SADataAdapter.CommandRowPair> batchCommands, out DataRow lastRow)
    {
      int num1 = 0;
            CheckBatchCommand(_batchCommand);
            _batchCommand.Parameters.Clear();
      for (int index = 0; (index < UpdateBatchSize || UpdateBatchSize == 0) && index + _currentBatchCommand < batchCommands.Count; ++index)
      {
        foreach (DbParameter parameter in batchCommands[index + _currentBatchCommand].Cmd.Parameters)
        {
          parameter.ParameterName = num1.ToString();
                    _batchCommand.Parameters.Add(parameter);
          ++num1;
        }
      }
            _batchCommand.Prepare();
      int num2 = _batchCommand.ExecuteNonQuery();
      for (int index = 0; (index < UpdateBatchSize || UpdateBatchSize == 0) && index + _currentBatchCommand < batchCommands.Count; ++index)
      {
        SADataAdapter.CommandRowPair commandRowPair = batchCommands[index + _currentBatchCommand];
        if (commandRowPair.Cmd.UpdatedRowSource == UpdateRowSource.OutputParameters)
                    CopyOutputParmsToDataRow((SACommand) commandRowPair.Cmd, commandRowPair.Row, srcColMapToDataCol);
      }
            _currentBatchCommand += UpdateBatchSize;
      if (_currentBatchCommand > batchCommands.Count || UpdateBatchSize == 0)
                _currentBatchCommand = batchCommands.Count;
      lastRow = batchCommands[_currentBatchCommand - 1].Row;
      return num2;
    }

    /// <summary>
    ///     <para>Initializes batching for the SADataAdapter object.</para>
    /// </summary>
    /// <seealso cref="T:iAnywhere.Data.SQLAnywhere.SADataAdapter" />
    protected override void InitializeBatching()
    {
      try
      {
        SATrace.FunctionScopeEnter("<sa.SADataAdapter.InitializeBatching|API>", _objectId, new string[0]);
                ClearBatch();
      }
      finally
      {
        SATrace.FunctionScopeLeave();
      }
    }

    /// <summary>
    ///     <para>Ends batching for the SADataAdapter object.</para>
    /// </summary>
    /// <seealso cref="T:iAnywhere.Data.SQLAnywhere.SADataAdapter" />
    protected override void TerminateBatching()
    {
      try
      {
        SATrace.FunctionScopeEnter("<sa.SADataAdapter.TerminateBatching|API>", _objectId, new string[0]);
                ClearBatch();
      }
      finally
      {
        SATrace.FunctionScopeLeave();
      }
    }

    private void CheckDataSet(DataSet dataSet, string parmName)
    {
      if (dataSet == null)
      {
        Exception e = new ArgumentNullException(parmName);
        SATrace.Exception(e);
        throw e;
      }
    }

    private void CheckDataTable(DataTable dataTable, string parmName)
    {
      if (dataTable == null)
      {
        Exception e = new ArgumentNullException(parmName);
        SATrace.Exception(e);
        throw e;
      }
    }

    private void CheckSourceTableName(string srcTable, string parmName)
    {
      if (srcTable == null || srcTable.Length < 1)
      {
        Exception e = new ArgumentException(SARes.GetString(11018, _methodName), parmName);
        SATrace.Exception(e);
        throw e;
      }
    }

    private void CheckCommand(SACommand cmd, string cmdName, StatementType stmtType)
    {
      if (cmd == null)
      {
        if (stmtType == StatementType.Select)
        {
          Exception e = new InvalidOperationException(SARes.GetString(10987, cmdName, _methodName));
          SATrace.Exception(e);
          throw e;
        }
        if (stmtType == StatementType.Insert)
        {
          Exception e = new InvalidOperationException(SARes.GetString(11027));
          SATrace.Exception(e);
          throw e;
        }
        if (stmtType == StatementType.Delete)
        {
          Exception e = new InvalidOperationException(SARes.GetString(11029));
          SATrace.Exception(e);
          throw e;
        }
        if (stmtType == StatementType.Update)
        {
          Exception e = new InvalidOperationException(SARes.GetString(11028));
          SATrace.Exception(e);
          throw e;
        }
      }
      if (cmd.Connection == null)
      {
        Exception e = new InvalidOperationException(SARes.GetString(10987, cmdName + ".Connection", _methodName));
        SATrace.Exception(e);
        throw e;
      }
    }

    private void FindDataTable(DataSet dataSet, string srcTableName, out DataTable dataTable, out DataTableMapping tm, out bool newTable)
    {
      string index = null;
      tm = (DataTableMapping) null;
      dataTable = null;
      newTable = false;
      if (TableMappings.Contains(srcTableName))
      {
        tm = TableMappings[srcTableName];
        index = TableMappings[srcTableName].DataSetTable;
      }
      else
      {
        if (this.MissingMappingAction == MissingMappingAction.Error)
        {
          Exception e = new InvalidOperationException(SARes.GetString(11023, srcTableName));
          SATrace.Exception(e);
          throw e;
        }
        if (this.MissingMappingAction == MissingMappingAction.Ignore)
          return;
        if (this.MissingMappingAction == MissingMappingAction.Passthrough)
          index = srcTableName;
      }
      if (dataSet.Tables.Contains(index))
        dataTable = dataSet.Tables[index];
      else if (this.MissingSchemaAction == MissingSchemaAction.Add || this.MissingSchemaAction == MissingSchemaAction.AddWithKey)
      {
        dataTable = new DataTable(index);
        newTable = true;
      }
      else
      {
        if (this.MissingSchemaAction == MissingSchemaAction.Error)
        {
          Exception e = new InvalidOperationException(SARes.GetString(11021, index, srcTableName));
          SATrace.Exception(e);
          throw e;
        }
        int num = (int) this.MissingSchemaAction;
      }
    }

    private void FindDataColumn(DataTable dataTable, string srcColName, DataTableMapping tm, Type dataType, out DataColumn dataCol, out bool newCol)
    {
      dataCol = (DataColumn) null;
      newCol = false;
      if (tm == null || !tm.ColumnMappings.Contains(srcColName))
      {
        if (this.MissingMappingAction == MissingMappingAction.Error)
        {
          Exception e = (Exception) new InvalidOperationException(SARes.GetString(11025, srcColName));
          SATrace.Exception(e);
          throw e;
        }
        if (this.MissingMappingAction == MissingMappingAction.Ignore || this.MissingMappingAction != MissingMappingAction.Passthrough)
          return;
        if (dataTable.Columns.Contains(srcColName))
          dataCol = dataTable.Columns[srcColName];
        else if (this.MissingSchemaAction == MissingSchemaAction.Add || this.MissingSchemaAction == MissingSchemaAction.AddWithKey)
        {
          dataCol = dataTable.Columns.Add(srcColName, dataType);
          newCol = true;
        }
        else
        {
          if (this.MissingSchemaAction == MissingSchemaAction.Error)
          {
            Exception e = (Exception) new InvalidOperationException(SARes.GetString(11022, srcColName, dataTable.TableName, srcColName));
            SATrace.Exception(e);
            throw e;
          }
          if (this.MissingSchemaAction == MissingSchemaAction.Ignore)
            ;
        }
      }
      else
      {
        string dataSetColumn = tm.ColumnMappings[srcColName].DataSetColumn;
        if (dataTable.Columns.Contains(dataSetColumn))
          dataCol = dataTable.Columns[dataSetColumn];
        else if (this.MissingSchemaAction == MissingSchemaAction.Add || this.MissingSchemaAction == MissingSchemaAction.AddWithKey)
        {
          dataCol = dataTable.Columns.Add(dataSetColumn, dataType);
          newCol = true;
        }
        else
        {
          if (this.MissingSchemaAction == MissingSchemaAction.Error)
          {
            Exception e = (Exception) new InvalidOperationException(SARes.GetString(11022, dataSetColumn, dataTable.TableName, srcColName));
            SATrace.Exception(e);
            throw e;
          }
          int num = (int) this.MissingSchemaAction;
        }
      }
    }

    internal void CancelFill()
    {
            _canceled = true;
    }

    /// <summary>
    ///     <para>Adds or refreshes rows in a <see cref="T:System.Data.DataSet" /> or <see cref="T:System.Data.DataTable" /> object with data from the database.</para>
    /// </summary>
    /// <remarks>
    ///     <para>Even if you use the startRecord argument to limit the number of records that are copied to the DataSet, all records in the SADataAdapter query are fetched from the database to the client. For large result sets, this can have a significant performance impact.</para>
    ///     <para>An alternative is to use an SADataReader when a read-only, forward-only result set is sufficient, perhaps with SQL statements (ExecuteNonQuery) to undertake modifications. Another alternative is to write a stored procedure that returns only the result you need.</para>
    ///     <para>If SelectCommand does not return any rows, no tables are added to the DataSet and no exception is raised.</para>
    ///     <para>For more information, see @olink targetdoc="programming" targetptr="getting-data-adapter"@Getting data using the SADataAdapter object@/olink@.</para>
    /// </remarks>
    /// <param name="dataSet">
    ///     A <see cref="T:System.Data.DataSet" /> to fill with records and optionally, schema.
    /// </param>
    /// <param name="startRecord">
    ///     The zero-based record number with which to start.
    /// </param>
    /// <param name="maxRecords">
    ///     The maximum number of records to be read into the <see cref="T:System.Data.DataSet" />.
    /// </param>
    /// <param name="srcTable">
    ///     The name of the source table to use for table mapping.
    /// </param>
    /// <param name="command">
    ///     The SQL SELECT statement used to retrieve rows from the data source.
    /// </param>
    /// <param name="behaviour">One of the CommandBehavior values.</param>
    /// <returns>
    /// <para>The number of rows successfully added or refreshed in the <see cref="T:System.Data.DataSet" />.</para>
    /// </returns>
    /// <seealso cref="T:System.Data.CommandBehavior" />
    protected override int Fill(DataSet dataSet, int startRecord, int maxRecords, string srcTable, IDbCommand command, CommandBehavior behaviour)
    {
      try
      {
        SATrace.FunctionScopeEnter("<sa.SADataAdapter.Fill|API>", _objectId, "dataSet", "startRecord", "maxRecords", "srcTable", "command", "behaviour");
        SACommand cmd = (SACommand) command;
                _methodName = "Fill";
                CheckDataSet(dataSet, "dataSet");
        if (startRecord < 0)
        {
          Exception e = new ArgumentException(SARes.GetString(11019, startRecord.ToString()), "startRecord");
          SATrace.Exception(e);
          throw e;
        }
        if (maxRecords < 0)
        {
          Exception e = new ArgumentException(SARes.GetString(11020, maxRecords.ToString()), "maxRecords");
          SATrace.Exception(e);
          throw e;
        }
                CheckSourceTableName(srcTable, "srcTable");
        this.CheckCommand(cmd, "SelectCommand", StatementType.Select);
        int num = 0;
        int tableIndex = 0;
        bool newTable = false;
        DataTable dataTable = null;
        Exception e1 = null;
        bool closeConnection;
        SADataReader dataReader = OpenDataReader(false, out closeConnection);
        if (dataReader == null)
        {
          if (closeConnection)
            cmd.Connection.Close();
          return 0;
        }
                _canceled = false;
        cmd.DataAdapter = this;
        do
        {
          try
          {
            DataTableMapping tm;
                        FindDataTable(dataSet, GetSourceTableName(srcTable, tableIndex), out dataTable, out tm, out newTable);
            if (dataTable != null)
            {
              num += _Fill(dataTable, startRecord, maxRecords, dataReader, tm);
              if (!_canceled)
              {
                if (newTable && dataTable.Columns.Count > 0)
                  dataSet.Tables.Add(dataTable);
              }
              else
                break;
            }
            ++tableIndex;
          }
          catch (Exception ex)
          {
            e1 = ex;
            break;
          }
        }
        while (dataReader.NextResult());
        dataReader.Close();
        if (closeConnection)
          cmd.Connection.Close();
        cmd.DataAdapter = null;
        if (e1 != null)
        {
          SATrace.Exception(e1);
          throw e1;
        }
        return num;
      }
      finally
      {
        SATrace.FunctionScopeLeave();
      }
    }

    /// <summary>
    ///     <para>Adds or refreshes rows in a specified range in the <see cref="T:System.Data.DataSet" /> to match those in the data source using the System.Data.DataSet and <see cref="T:System.Data.DataTable" /> names.</para>
    /// </summary>
    /// <remarks>
    ///     <para>Even if you use the startRecord argument to limit the number of records that are copied to the DataSet, all records in the SADataAdapter query are fetched from the database to the client. For large result sets, this can have a significant performance impact.</para>
    ///     <para>An alternative is to use an SADataReader when a read-only, forward-only result set is sufficient, perhaps with SQL statements (ExecuteNonQuery) to undertake modifications. Another alternative is to write a stored procedure that returns only the result you need.</para>
    ///     <para>If SelectCommand does not return any rows, no tables are added to the DataSet and no exception is raised.</para>
    ///     <para>For more information, see @olink targetdoc="programming" targetptr="getting-data-adapter"@Getting data using the SADataAdapter object@/olink@.</para>
    /// </remarks>
    /// <param name="dataTables">
    ///     The <see cref="T:System.Data.DataTable" /> objects to fill from the data source.
    /// </param>
    /// <param name="startRecord">
    ///     The zero-based record number to start with.
    /// </param>
    /// <param name="maxRecords">The maximum number of records to retrieve.</param>
    /// <param name="command">
    ///     The System.Data.IDbCommand executed to fill the System.Data.DataTable objects.
    /// </param>
    /// <param name="behavior">
    ///     One of the System.Data.CommandBehavior values.
    /// </param>
    /// <returns>
    /// <para>The number of rows added to or refreshed in the data tables.</para>
    /// </returns>
    protected override int Fill(DataTable[] dataTables, int startRecord, int maxRecords, IDbCommand command, CommandBehavior behavior)
    {
      try
      {
        SATrace.FunctionScopeEnter("<sa.SADataAdapter.Fill|API>", _objectId, "dataTables", "startRecord", "maxRecords", "command", "behavior");
                _methodName = "Fill";
        SACommand saCommand = (SACommand) command;
                CheckDataTable(dataTables[0], "dataTable");
        this.CheckCommand(saCommand, "SelectCommand", StatementType.Select);
        int num = 0;
        int index = 0;
        bool closeConnection;
        SADataReader dataReader = OpenDataReader(false, out closeConnection, saCommand);
        if (dataReader == null)
        {
          if (closeConnection)
            saCommand.Connection.Close();
          return 0;
        }
        try
        {
          saCommand.DataAdapter = this;
          do
          {
            if (index != dataTables.Length)
            {
              DataTable dataTable = dataTables[index];
                            CheckDataTable(dataTable, "dataTable");
              if (GetTableMapping(dataTable) == null)
              {
                if (this.MissingMappingAction == MissingMappingAction.Error)
                {
                  Exception e = new InvalidOperationException(SARes.GetString(11024, dataTable.TableName));
                  SATrace.Exception(e);
                  throw e;
                }
                if (this.MissingMappingAction == MissingMappingAction.Ignore)
                  goto label_12;
              }
              num += _Fill(dataTable, startRecord, maxRecords, dataReader, GetTableMapping(dataTable));
              if (!_canceled)
                ++index;
              else
                break;
            }
label_12:;
          }
          while (dataReader.NextResult());
        }
        finally
        {
          saCommand.DataAdapter = null;
          dataReader.Close();
          if (closeConnection)
            saCommand.Connection.Close();
        }
        return num;
      }
      finally
      {
        SATrace.FunctionScopeLeave();
      }
    }

    private int _Fill(DataTable dataTable, int startRecord, int maxRecords, SADataReader dataReader, DataTableMapping tm)
    {
      int fieldCount = dataReader.FieldCount;
      DataTable schemaTable = null;
      ArrayList primaryKey = null;
      ArrayList arrayList1 = new ArrayList();
      ArrayList arrayList2 = new ArrayList();
      bool hasMultipleBaseTables = false;
      string[] sourceColumnNames = GetSourceColumnNames(dataReader);
      if (this.MissingSchemaAction == MissingSchemaAction.AddWithKey && (dataTable.PrimaryKey == null || dataTable.PrimaryKey.Length < 1))
        primaryKey = new ArrayList();
      for (int index = 0; index < fieldCount; ++index)
      {
        string srcColName = sourceColumnNames[index];
        DataColumn dataCol;
        bool newCol;
                FindDataColumn(dataTable, srcColName, tm, dataReader.GetFieldType(index), out dataCol, out newCol);
        if (dataCol != null)
        {
          arrayList2.Add(index);
          arrayList1.Add((object) dataCol.Ordinal);
          if (this.MissingSchemaAction == MissingSchemaAction.AddWithKey)
          {
            if (schemaTable == null)
            {
              schemaTable = dataReader.GetSchemaTable();
              hasMultipleBaseTables = HasMultipleBaseTables(schemaTable);
            }
                        SetDataColumnProperties(dataCol, schemaTable, index, newCol, hasMultipleBaseTables);
            if (primaryKey != null && IsKeyColumn(schemaTable, index))
              primaryKey.Add((object) dataCol);
          }
        }
      }
      if (arrayList1.Count < 1)
        return 0;
      int num1 = 0;
      int num2 = 0;
      bool[] flagArray = new bool[arrayList1.Count];
      object[] values = new object[dataTable.Columns.Count];
      for (int index = 0; index < flagArray.Length; ++index)
        flagArray[index] = !dataReader.GetFieldType((int) arrayList2[index]).Equals(dataTable.Columns[(int) arrayList1[index]].DataType);
      dataTable.BeginLoadData();
      while (dataReader.Read())
      {
        if (_canceled)
          return num2;
        if (num1 < startRecord)
        {
          ++num1;
        }
        else
        {
          Array.Clear(values, 0, values.Length);
          for (int index = 0; index < arrayList1.Count; ++index)
          {
            object obj = dataReader.GetValue((int) arrayList2[index]);
            if (!flagArray[index] || obj == null || obj.Equals(DBNull.Value))
            {
              values[(int) arrayList1[index]] = obj;
            }
            else
            {
              Type dataType = dataTable.Columns[arrayList1[index]].DataType;
              if (dataType.Equals(typeof (string)))
                values[(int) arrayList1[index]] = obj.ToString();
              else
                values[(int) arrayList1[index]] = Convert.ChangeType(obj, dataType, null);
            }
          }
          try
          {
            dataTable.LoadDataRow(values, this.AcceptChangesDuringFill);
          }
          catch (Exception ex)
          {
            FillErrorEventArgs fillErrorEventArgs = new FillErrorEventArgs(dataTable, values);
            fillErrorEventArgs.Errors = ex;
            this.OnFillError(fillErrorEventArgs);
            if (!fillErrorEventArgs.Continue)
              throw ex;
          }
          ++num2;
          if (maxRecords <= 0 || num2 != maxRecords)
            ++num1;
          else
            break;
        }
      }
      dataTable.EndLoadData();
      if (this.MissingSchemaAction == MissingSchemaAction.AddWithKey && primaryKey != null && primaryKey.Count > 0)
                SetPrimaryKey(dataTable, primaryKey);
      return num2;
    }

    private bool HasMultipleBaseTables(DataTable schemaTable)
    {
      ArrayList arrayList = new ArrayList();
      foreach (DataRow row in (InternalDataCollectionBase) schemaTable.Rows)
      {
        object obj = row["BaseTableName"];
        if (!DBNull.Value.Equals(obj))
        {
          string str = (string) obj;
          if (!arrayList.Contains(str))
          {
            arrayList.Add(str);
            if (arrayList.Count > 1)
              return true;
          }
        }
      }
      return arrayList.Count > 1;
    }

    private string GetSourceTableName(string srcTable, int tableIndex)
    {
      return tableIndex != 0 ? srcTable + tableIndex.ToString() : srcTable;
    }

    private string[] GetSourceColumnNames(SADataReader dataReader)
    {
      string[] strArray = new string[dataReader.FieldCount];
      for (int ordinal1 = 0; ordinal1 < dataReader.FieldCount; ++ordinal1)
      {
        int num = 0;
        string name = dataReader.GetName(ordinal1);
        for (int ordinal2 = 0; ordinal2 < ordinal1; ++ordinal2)
        {
          if (name.Equals(dataReader.GetName(ordinal2)))
            ++num;
        }
        if (num == 0)
        {
          strArray[ordinal1] = name;
        }
        else
        {
          string str;
          while (true)
          {
            bool flag = false;
            str = name + num.ToString();
            for (int ordinal2 = 0; ordinal2 < dataReader.FieldCount; ++ordinal2)
            {
              if (str.Equals(dataReader.GetName(ordinal2)))
              {
                flag = true;
                break;
              }
            }
            if (!flag)
            {
              for (int index = 0; index < ordinal1; ++index)
              {
                if (str.Equals(strArray[index]))
                {
                  flag = true;
                  break;
                }
              }
            }
            if (flag)
              ++num;
            else
              break;
          }
          strArray[ordinal1] = str;
        }
      }
      return strArray;
    }

    /// <summary>
    ///     <para>Adds a <see cref="T:System.Data.DataTable" /> to a <see cref="T:System.Data.DataSet" /> and configures the schema to match the schema in the data source.</para>
    /// </summary>
    /// <remarks>
    ///     <para>For more information, see <see cref="M:System.Data.IDataAdapter.FillSchema(System.Data.DataSet,System.Data.SchemaType)" /> and @olink targetdoc="programming" targetptr="sadataadapter-schema-adodotnet"@Obtaining SADataAdapter schema information@/olink@.</para>
    /// </remarks>
    /// <param name="dataSet">
    ///     A <see cref="T:System.Data.DataSet" /> to fill with the schema.
    /// </param>
    /// <param name="schemaType">
    ///     One of the <see cref="T:System.Data.SchemaType" /> values that specify how to insert the schema.
    /// </param>
    /// <param name="command">
    ///     The SQL SELECT statement used to retrieve rows from the data source.
    /// </param>
    /// <param name="srcTable">
    ///     The name of the source table to use for table mapping.
    /// </param>
    /// <param name="behavior">
    ///     One of the System.Data.CommandBehavior values.
    /// </param>
    /// <returns>
    /// <para>A reference to a collection of <see cref="T:System.Data.DataTable" /> objects that were added to the <see cref="T:System.Data.DataSet" />.</para>
    /// </returns>
    protected override DataTable[] FillSchema(DataSet dataSet, SchemaType schemaType, IDbCommand command, string srcTable, CommandBehavior behavior)
    {
      try
      {
        SATrace.FunctionScopeEnter("<sa.SADataAdapter.FillSchema|API>", _objectId, "dataSet", "schemaType", "command", "srcTable", "behavior");
        SACommand saCommand = (SACommand) command;
                _methodName = "FillSchema";
                CheckDataSet(dataSet, "dataSet");
                CheckSourceTableName(srcTable, "srcTable");
        this.CheckCommand(saCommand, "SelectCommand", StatementType.Select);
        bool closeConnection;
        SADataReader dataReader = OpenDataReader(true, out closeConnection, saCommand);
        if (dataReader == null)
        {
          if (closeConnection)
            saCommand.Connection.Close();
          return null;
        }
        DataTable dataTable = null;
        int tableIndex = 0;
        bool newTable = false;
        Exception exception = null;
        ArrayList arrayList = new ArrayList();
        do
        {
          try
          {
            DataTableMapping tm = (DataTableMapping) null;
            string sourceTableName = GetSourceTableName(srcTable, tableIndex);
            if (schemaType == SchemaType.Source)
            {
              if (dataSet.Tables.Contains(sourceTableName))
              {
                dataTable = dataSet.Tables[sourceTableName];
              }
              else
              {
                dataTable = new DataTable(sourceTableName);
                newTable = true;
              }
            }
            else if (schemaType == SchemaType.Mapped)
                            FindDataTable(dataSet, sourceTableName, out dataTable, out tm, out newTable);
            if (dataTable != null)
            {
              DataTable table = FillSchema(dataTable, schemaType, dataReader, tm);
              if (table != null)
              {
                arrayList.Add(table);
                if (newTable)
                  dataSet.Tables.Add(table);
              }
            }
            ++tableIndex;
          }
          catch (Exception ex)
          {
            exception = ex;
            break;
          }
        }
        while (dataReader.NextResult());
        dataReader.Close();
        if (closeConnection)
                    SelectCommand.Connection.Close();
        if (exception != null)
          throw exception;
        DataTable[] dataTableArray = new DataTable[arrayList.Count];
        for (int index = 0; index < arrayList.Count; ++index)
          dataTableArray[index] = (DataTable) arrayList[index];
        return dataTableArray;
      }
      finally
      {
        SATrace.FunctionScopeLeave();
      }
    }

    /// <summary>
    ///     <para>Adds a <see cref="T:System.Data.DataTable" /> to a <see cref="T:System.Data.DataSet" /> and configures the schema to match the schema in the data source.</para>
    /// </summary>
    /// <remarks>
    ///     <para>For more information, see <see cref="M:System.Data.Common.DbDataAdapter.FillSchema(System.Data.DataTable,System.Data.SchemaType)" /> and @olink targetdoc="programming" targetptr="sadataadapter-schema-adodotnet"@Obtaining SADataAdapter schema information@/olink@.</para>
    /// </remarks>
    /// <param name="dataTable">
    ///     A <see cref="T:System.Data.DataTable" /> to fill with the schema.
    /// </param>
    /// <param name="schemaType">
    ///     One of the <see cref="T:System.Data.SchemaType" /> values that specify how to insert the schema.
    /// </param>
    /// <param name="command">
    ///     The SQL SELECT statement used to retrieve rows from the data source.
    /// </param>
    /// <param name="behavior">
    ///     One of the System.Data.CommandBehavior values.
    /// </param>
    /// <returns>
    /// <para>A reference to the <see cref="T:System.Data.DataTable" /> object that contains the schema.</para>
    /// </returns>
    protected override DataTable FillSchema(DataTable dataTable, SchemaType schemaType, IDbCommand command, CommandBehavior behavior)
    {
      try
      {
        SATrace.FunctionScopeEnter("<sa.SADataAdapter.FillSchema|API>", _objectId, "dataTable", "schemaType", "command", "behavior");
        SACommand saCommand = (SACommand) command;
                _methodName = "FillSchema";
                CheckDataTable(dataTable, "dataTable");
        this.CheckCommand(saCommand, "SelectCommand", StatementType.Select);
        DataTableMapping tm = (DataTableMapping) null;
        DataTable dataTable1 = null;
        if (schemaType != SchemaType.Source && schemaType == SchemaType.Mapped)
        {
          tm = GetTableMapping(dataTable);
          if (tm == null)
          {
            if (this.MissingMappingAction == MissingMappingAction.Error)
            {
              Exception e = new InvalidOperationException(SARes.GetString(11024, dataTable.TableName));
              SATrace.Exception(e);
              throw e;
            }
            if (this.MissingMappingAction == MissingMappingAction.Ignore)
              return null;
            int num = (int) this.MissingMappingAction;
          }
        }
        Exception exception = null;
        bool closeConnection;
        SADataReader dataReader = OpenDataReader(true, out closeConnection, saCommand);
        if (dataReader == null)
        {
          if (closeConnection)
            saCommand.Connection.Close();
          return null;
        }
        try
        {
          dataTable1 = FillSchema(dataTable, schemaType, dataReader, tm);
        }
        catch (Exception ex)
        {
          exception = ex;
        }
        dataReader.Close();
        if (closeConnection)
          saCommand.Connection.Close();
        if (exception != null)
          throw exception;
        return dataTable1;
      }
      finally
      {
        SATrace.FunctionScopeLeave();
      }
    }

    private DataTable FillSchema(DataTable dataTable, SchemaType schemaType, SADataReader dataReader, DataTableMapping tm)
    {
      DataColumn dataCol = (DataColumn) null;
      DataTable schemaTable = dataReader.GetSchemaTable();
      ArrayList primaryKey = null;
      bool hasMultipleBaseTables = HasMultipleBaseTables(schemaTable);
      DataTable dataTable1 = null;
      int num = 0;
      string[] sourceColumnNames = GetSourceColumnNames(dataReader);
      if (dataTable.PrimaryKey == null || dataTable.PrimaryKey.Length < 1)
        primaryKey = new ArrayList();
      for (int index1 = 0; index1 < schemaTable.Rows.Count; ++index1)
      {
        bool newCol = false;
        string index2 = sourceColumnNames[index1];
        object obj = schemaTable.Rows[index1]["IsKey"];
        if (!obj.Equals(DBNull.Value) && (bool) obj)
          ++num;
        Type type = (Type) schemaTable.Rows[index1]["DataType"];
        if (schemaType == SchemaType.Source)
        {
          if (dataTable.Columns.Contains(index2))
          {
            dataCol = dataTable.Columns[index2];
          }
          else
          {
            dataCol = dataTable.Columns.Add(index2, type);
            newCol = true;
          }
        }
        else if (schemaType == SchemaType.Mapped)
                    FindDataColumn(dataTable, index2, tm, type, out dataCol, out newCol);
        if (dataCol != null)
        {
                    SetDataColumnProperties(dataCol, schemaTable, index1, newCol, hasMultipleBaseTables);
          if (primaryKey != null && IsKeyColumn(schemaTable, index1))
            primaryKey.Add((object) dataCol);
          dataTable1 = dataTable;
        }
      }
      if (primaryKey != null && primaryKey.Count > 0 && primaryKey.Count == num)
                SetPrimaryKey(dataTable, primaryKey);
      return dataTable1;
    }

    private bool IsKeyColumn(DataTable schemaTable, int columnIndex)
    {
      object obj = schemaTable.Rows[columnIndex]["IsKey"];
      return !obj.Equals(DBNull.Value) && (bool) obj;
    }

    private void SetPrimaryKey(DataTable dataTable, ArrayList primaryKey)
    {
      DataColumn[] dataColumnArray = new DataColumn[primaryKey.Count];
      for (int index = 0; index < dataColumnArray.Length; ++index)
        dataColumnArray[index] = (DataColumn) primaryKey[index];
      dataTable.PrimaryKey = dataColumnArray;
    }

    private void SetDataColumnProperties(DataColumn dataCol, DataTable schemaTable, int fieldIndex, bool newCol, bool hasMultipleBaseTables)
    {
      object obj1 = schemaTable.Rows[fieldIndex]["AllowDBNull"];
      if (!obj1.Equals(DBNull.Value))
        dataCol.AllowDBNull = obj1;
      object obj2 = schemaTable.Rows[fieldIndex]["IsAutoIncrement"];
      if (!obj2.Equals(DBNull.Value))
      {
        if (!(bool) obj2)
          dataCol.AutoIncrement = false;
        else if ((dataCol.Expression == null || dataCol.Expression.Trim().Length < 1) && (dataCol.DefaultValue == DBNull.Value || dataCol.DefaultValue == null) && (dataCol.DataType == typeof (short) || dataCol.DataType == typeof (int) || (dataCol.DataType == typeof (long) || dataCol.DataType == typeof (Decimal))))
          dataCol.AutoIncrement = true;
      }
      Type type = typeof (string);
      if (type.Equals(dataCol.DataType) && type.Equals((Type) schemaTable.Rows[fieldIndex]["DataType"]))
      {
        object obj3 = schemaTable.Rows[fieldIndex]["ColumnSize"];
        if (!obj3.Equals(DBNull.Value))
        {
          int num = (int) obj3;
          if (num > dataCol.MaxLength)
            dataCol.MaxLength = num;
        }
      }
      object obj4 = schemaTable.Rows[fieldIndex]["IsReadOnly"];
      if (!obj4.Equals(DBNull.Value))
        dataCol.ReadOnly = obj4;
      if (hasMultipleBaseTables)
        return;
      object obj5 = schemaTable.Rows[fieldIndex]["IsUnique"];
      if (obj5.Equals(DBNull.Value))
        return;
      dataCol.Unique = obj5;
    }

    private new DataTableMapping GetTableMapping(DataTable dataTable)
    {
      foreach (DataTableMapping tableMapping in TableMappings)
      {
        if (tableMapping.DataSetTable.Equals(dataTable.TableName))
          return tableMapping;
      }
      return (DataTableMapping) null;
    }

    private DataTable GetDataTable(DataSet dataSet, string dataTableName)
    {
      foreach (DataTable table in (InternalDataCollectionBase) dataSet.Tables)
      {
        if (table.TableName.Equals(dataTableName))
          return table;
      }
      return null;
    }

    private SADataReader OpenDataReader(bool schemaOnly, out bool closeConnection)
    {
      return OpenDataReader(schemaOnly, out closeConnection, SelectCommand);
    }

    private SADataReader OpenDataReader(bool schemaOnly, out bool closeConnection, SACommand selectCommand)
    {
      if (selectCommand.Connection.GetConnectionState() == ConnectionState.Closed)
      {
        selectCommand.Connection.Open();
        closeConnection = true;
      }
      else
        closeConnection = false;
      if (schemaOnly)
        return selectCommand.ExecuteReader(CommandBehavior.SchemaOnly);
      return selectCommand.ExecuteReader();
    }

    /// <summary>
    ///     <para>Returns the parameters set by you when executing a SELECT statement.</para>
    /// </summary>
    /// <returns>
    /// <para>An array of IDataParameter objects that contains the parameters set by the user.</para>
    /// </returns>
    public SAParameter[] GetFillParameters()
    {
      try
      {
        SATrace.FunctionScopeEnter("<sa.SADataAdapter.GetFillParameters|API>", _objectId, new string[0]);
        if (SelectCommand == null)
          return new SAParameter[0];
        SAParameter[] saParameterArray = new SAParameter[SelectCommand.Parameters.Count];
        for (int index = 0; index < saParameterArray.Length; ++index)
          saParameterArray[index] = SelectCommand.Parameters[index];
        return saParameterArray;
      }
      finally
      {
        SATrace.FunctionScopeLeave();
      }
    }

    /// <summary>
    ///     <para>Initializes a new instance of the <see cref="T:System.Data.Common.RowUpdatedEventArgs" /> class.</para>
    /// </summary>
    /// <param name="dataRow">
    ///     The <see cref="T:System.Data.DataRow" /> used to update the data source.
    /// </param>
    /// <param name="command">
    ///     The <see cref="T:System.Data.IDbCommand" /> executed during the <see cref="M:System.Data.IDataAdapter.Update(System.Data.DataSet)" />.
    /// </param>
    /// <param name="statementType">
    ///     Whether the command is an UPDATE, INSERT, DELETE, or SELECT statement.
    /// </param>
    /// <param name="tableMapping">
    ///     A <see cref="T:System.Data.Common.DataTableMapping" /> object.
    /// </param>
    /// <returns>
    /// <para>A new instance of the <see cref="T:System.Data.Common.RowUpdatedEventArgs" /> class.</para>
    /// </returns>
    protected override RowUpdatedEventArgs CreateRowUpdatedEvent(DataRow dataRow, IDbCommand command, StatementType statementType, DataTableMapping tableMapping)
    {
      try
      {
        SATrace.FunctionScopeEnter("<sa.SADataAdapter.CreateRowUpdatedEvent|API>", _objectId, "dataRow", "command", "statementType", "tableMapping");
        return (RowUpdatedEventArgs) new SARowUpdatedEventArgs(dataRow, command, statementType, tableMapping);
      }
      finally
      {
        SATrace.FunctionScopeLeave();
      }
    }

    /// <summary>
    ///     <para>Initializes a new instance of the <see cref="T:System.Data.Common.RowUpdatingEventArgs" /> class.</para>
    /// </summary>
    /// <param name="dataRow">
    ///     The <see cref="T:System.Data.DataRow" /> used to update the data source.
    /// </param>
    /// <param name="command">
    ///     The <see cref="T:System.Data.IDbCommand" /> executed during the <see cref="M:System.Data.IDataAdapter.Update(System.Data.DataSet)" />.
    /// </param>
    /// <param name="statementType">
    ///     Whether the command is an UPDATE, INSERT, DELETE, or SELECT statement.
    /// </param>
    /// <param name="tableMapping">
    ///     A <see cref="T:System.Data.Common.DataTableMapping" /> object.
    /// </param>
    /// <returns>
    /// <para>A new instance of the <see cref="T:System.Data.Common.RowUpdatingEventArgs" /> class.</para>
    /// </returns>
    protected override RowUpdatingEventArgs CreateRowUpdatingEvent(DataRow dataRow, IDbCommand command, StatementType statementType, DataTableMapping tableMapping)
    {
      try
      {
        SATrace.FunctionScopeEnter("<sa.SADataAdapter.CreateRowUpdatingEvent|API>", _objectId, "dataRow", "command", "statementType", "tableMapping");
        return (RowUpdatingEventArgs) new SARowUpdatingEventArgs(dataRow, command, statementType, tableMapping);
      }
      finally
      {
        SATrace.FunctionScopeLeave();
      }
    }

    private DataRow[] GetDataRows(DataTable dataTable)
    {
      DataRow[] dataRowArray = new DataRow[dataTable.Rows.Count];
      for (int index = 0; index < dataRowArray.Length; ++index)
        dataRowArray[index] = dataTable.Rows[index];
      return dataRowArray;
    }

    /// <summary>
    ///     <para>Updates the tables in a database with the changes made to the DataSet.</para>
    /// </summary>
    /// <remarks>
    ///     <para>The Update is carried out using the InsertCommand, UpdateCommand, and DeleteCommand on each row in the data set that has been inserted, updated, or deleted.</para>
    ///     <para>For more information, see @olink targetdoc="programming" targetptr="inserting-data-adapter"@Inserting, updating, and deleting rows using the SADataAdapter object@/olink@.</para>
    /// </remarks>
    /// <param name="dataRows">
    ///     An array of <see cref="T:System.Data.DataRow" /> to update from.
    /// </param>
    /// <param name="tableMapping">
    ///     The <see cref="P:System.Data.IDataAdapter.TableMappings" /> collection to use.
    /// </param>
    /// <returns>
    /// <para>The number of rows successfully updated from the <see cref="T:System.Data.DataRow" /> array.</para>
    /// </returns>
    /// <seealso cref="P:iAnywhere.Data.SQLAnywhere.SADataAdapter.DeleteCommand" />
    /// <seealso cref="P:iAnywhere.Data.SQLAnywhere.SADataAdapter.InsertCommand" />
    /// <seealso cref="P:iAnywhere.Data.SQLAnywhere.SADataAdapter.UpdateCommand" />
    protected override int Update(DataRow[] dataRows, DataTableMapping tableMapping)
    {
      try
      {
        SATrace.FunctionScopeEnter("<sa.SADataAdapter.Update|API>", _objectId, "dataRows", "tableMapping");
        bool flag1 = true;
        bool flag2 = true;
        bool flag3 = true;
        bool flag4 = false;
        bool flag5 = false;
        bool flag6 = false;
        bool flag7 = CloseCommandConnection(SelectCommand);
        int num1 = 0;
        int num2 = 0;
        DataTable table = dataRows[0].Table;
        SACommand cmd = null;
        Hashtable srcColMapToDataCol = new Hashtable();
        StatementType statementType = StatementType.Select;
        Exception updateEx = null;
        string cmdName = null;
        bool flag8 = _batchSize != 1;
        int num3 = 0;
        if (flag8)
                    InitializeBatching();
        foreach (DataRow dataRow in dataRows)
        {
          if (dataRow.RowState != DataRowState.Detached && dataRow.RowState != DataRowState.Unchanged)
          {
            updateEx = null;
            bool flag9 = false;
            try
            {
              if (dataRow.RowState == DataRowState.Added)
              {
                cmd = InsertCommand;
                cmdName = "InsertCommand";
                statementType = StatementType.Insert;
                if (cmd == null && CommandBuilder != null)
                  cmd = CommandBuilder.GetInsertCommand();
                if (flag1)
                {
                  flag4 = CloseCommandConnection(InsertCommand);
                  if (cmd != null || RowUpdating == null)
                  {
                                        CheckCommand(cmd, cmdName, statementType);
                                        MapSrcColToDataColumn(cmd, table, tableMapping, srcColMapToDataCol);
                  }
                  flag1 = false;
                }
              }
              else if (dataRow.RowState == DataRowState.Deleted)
              {
                cmd = DeleteCommand;
                cmdName = "DeleteCommand";
                statementType = StatementType.Delete;
                if (cmd == null && CommandBuilder != null)
                  cmd = CommandBuilder.GetDeleteCommand();
                if (flag3)
                {
                  flag5 = CloseCommandConnection(DeleteCommand);
                  if (cmd != null || RowUpdating == null)
                  {
                                        CheckCommand(cmd, cmdName, statementType);
                                        MapSrcColToDataColumn(cmd, table, tableMapping, srcColMapToDataCol);
                  }
                  flag3 = false;
                }
              }
              else if (dataRow.RowState == DataRowState.Modified)
              {
                cmd = UpdateCommand;
                cmdName = "UpdateCommand";
                statementType = StatementType.Update;
                if (cmd == null && CommandBuilder != null)
                  cmd = CommandBuilder.GetUpdateCommand();
                if (flag2)
                {
                  flag6 = CloseCommandConnection(UpdateCommand);
                  if (cmd != null || RowUpdating == null)
                  {
                                        CheckCommand(cmd, cmdName, statementType);
                                        MapSrcColToDataColumn(cmd, table, tableMapping, srcColMapToDataCol);
                  }
                  flag2 = false;
                }
              }
            }
            catch (Exception ex)
            {
              updateEx = ex;
            }
            if (updateEx == null)
            {
              if (cmd != null)
              {
                try
                {
                                    CopyDataRowToInputParameters(cmd, statementType, dataRow, srcColMapToDataCol);
                }
                catch (Exception ex)
                {
                  updateEx = ex;
                }
              }
            }
            if (RowUpdating == null)
            {
              if (updateEx != null)
              {
                if (this.ContinueUpdateOnError)
                {
                  dataRow.RowError = updateEx.Message;
                  continue;
                }
                goto label_105;
              }
            }
            else
            {
              SARowUpdatingEventArgs e = new SARowUpdatingEventArgs(dataRow, cmd, statementType, tableMapping);
              e.Status = updateEx == null ? UpdateStatus.Continue : UpdateStatus.ErrorsOccurred;
              e.Errors = updateEx;
                            RowUpdating(this, e);
              if (e.Status != UpdateStatus.SkipCurrentRow)
              {
                if (e.Status == UpdateStatus.SkipAllRemainingRows)
                {
                  updateEx = null;
                  goto label_105;
                }
                else if (e.Status == UpdateStatus.ErrorsOccurred)
                {
                  updateEx = e.Errors == null ? (Exception) new DataException(SARes.GetString(11030, "RowUpdatingEvent")) : e.Errors;
                  dataRow.RowError = updateEx.Message;
                  if (this.ContinueUpdateOnError)
                    continue;
                  goto label_105;
                }
                else if (e.Status == UpdateStatus.Continue)
                {
                  try
                  {
                    if (e.Command == cmd)
                    {
                      if (cmd == null)
                                                CheckCommand(cmd, cmdName, statementType);
                      else if (updateEx != null)
                      {
                        updateEx = null;
                                                MapSrcColToDataColumn(cmd, table, tableMapping, srcColMapToDataCol);
                                                CopyDataRowToInputParameters(cmd, statementType, dataRow, srcColMapToDataCol);
                      }
                    }
                    else
                    {
                      updateEx = null;
                      cmd = e.Command;
                      flag9 = CloseCommandConnection(cmd);
                                            CheckCommand(cmd, cmdName, statementType);
                                            MapSrcColToDataColumn(cmd, table, tableMapping, srcColMapToDataCol);
                                            CopyDataRowToInputParameters(cmd, statementType, dataRow, srcColMapToDataCol);
                    }
                  }
                  catch (Exception ex)
                  {
                    updateEx = ex;
                  }
                  if (updateEx != null)
                  {
                    if (this.ContinueUpdateOnError)
                    {
                      dataRow.RowError = updateEx.Message;
                      continue;
                    }
                    goto label_105;
                  }
                }
              }
              else
                continue;
            }
            try
            {
              if (flag8 && dataRow.RowState == DataRowState.Added)
              {
                                AddToBatch(cmd, dataRow);
                ++num3;
              }
              else
              {
                if (cmd.Connection.GetConnectionState() == ConnectionState.Closed)
                  cmd.Connection.Open();
                if (cmd.UpdatedRowSource == UpdateRowSource.None)
                {
                  num1 = cmd.ExecuteNonQuery();
                  if (num1 != 0)
                    dataRow.AcceptChanges();
                }
                else if (cmd.UpdatedRowSource == UpdateRowSource.OutputParameters)
                {
                  num1 = cmd.ExecuteNonQuery();
                  if (num1 != 0)
                  {
                    dataRow.AcceptChanges();
                                        CopyOutputParmsToDataRow(cmd, dataRow, srcColMapToDataCol);
                  }
                }
                else if (cmd.UpdatedRowSource == UpdateRowSource.FirstReturnedRecord)
                {
                  SADataReader dataReader = cmd.ExecuteReader();
                  num1 = cmd.RecordsAffected;
                  if (num1 != 0)
                    dataRow.AcceptChanges();
                  if (dataReader != null)
                  {
                    if (num1 != 0)
                                            CopyFirstRecordToDataRow(dataReader, dataRow, tableMapping, srcColMapToDataCol);
                    dataReader.Close();
                  }
                }
                else if (cmd.UpdatedRowSource == UpdateRowSource.Both)
                {
                  SADataReader dataReader = cmd.ExecuteReader();
                  num1 = cmd.RecordsAffected;
                  if (num1 != 0)
                  {
                    dataRow.AcceptChanges();
                                        CopyOutputParmsToDataRow(cmd, dataRow, srcColMapToDataCol);
                  }
                  if (dataReader != null)
                  {
                    if (num1 != 0)
                                            CopyFirstRecordToDataRow(dataReader, dataRow, tableMapping, srcColMapToDataCol);
                    dataReader.Close();
                  }
                }
                if (num1 == 0)
                {
                  string parm = null;
                  if (statementType == StatementType.Delete)
                    parm = "DeleteCommand";
                  else if (statementType == StatementType.Insert)
                    parm = "InsertCommand";
                  else if (statementType == StatementType.Update)
                    parm = "UpdateCommand";
                  throw new DBConcurrencyException(SARes.GetString(13777, parm)) { Row = dataRow };
                }
              }
            }
            catch (Exception ex)
            {
              updateEx = ex;
            }
            if (flag9)
              cmd.Connection.Close();
            if (updateEx == null)
            {
              num2 += num1 > 0 ? num1 : 0;
              if (this.AcceptChangesDuringUpdate && dataRow.RowState == DataRowState.Modified)
                dataRow.AcceptChanges();
            }
            else
              dataRow.RowError = updateEx.Message;
            if (RowUpdated == null)
            {
              if (updateEx != null && !this.ContinueUpdateOnError)
                goto label_105;
            }
            else if (!flag8 || flag8 && statementType != StatementType.Insert)
            {
              SARowUpdatedEventArgs e = new SARowUpdatedEventArgs(dataRow, cmd, statementType, tableMapping);
              e.Status = updateEx == null ? UpdateStatus.Continue : UpdateStatus.ErrorsOccurred;
              e.Errors = updateEx;
              e._recordsAffected = num1;
                            RowUpdated(this, e);
              if (e.Status != UpdateStatus.Continue && e.Status != UpdateStatus.SkipCurrentRow)
              {
                if (e.Status == UpdateStatus.SkipAllRemainingRows)
                {
                  updateEx = null;
                  goto label_105;
                }
                else if (e.Status == UpdateStatus.ErrorsOccurred)
                {
                  if (e.Errors != null)
                  {
                    updateEx = e.Errors;
                  }
                  else
                  {
                    updateEx = (Exception) new DataException(SARes.GetString(11030, "RowUpdatedEvent"));
                    dataRow.RowError = updateEx.Message;
                  }
                  if (!this.ContinueUpdateOnError)
                    goto label_105;
                }
              }
            }
          }
        }
        if (!flag8 || this.ContinueUpdateOnError)
          updateEx = null;
label_105:
        if (flag8 && num3 > 0)
          num2 += SendBatchedInserts(srcColMapToDataCol, ref updateEx, tableMapping);
        if (flag4)
                    InsertCommand.Connection.Close();
        if (flag6)
                    UpdateCommand.Connection.Close();
        if (flag5)
                    DeleteCommand.Connection.Close();
        if (flag7)
                    SelectCommand.Connection.Close();
        if (updateEx != null)
          throw updateEx;
        return num2;
      }
      finally
      {
        SATrace.FunctionScopeLeave();
      }
    }

    private bool CloseCommandConnection(SACommand cmd)
    {
      return cmd != null && cmd.Connection != null && cmd.Connection.GetConnectionState() == ConnectionState.Closed;
    }

    private void CopyDataRowToInputParameters(SACommand cmd, StatementType stmtType, DataRow row, Hashtable srcColMapToDataCol)
    {
      foreach (SAParameter parameter in (DbParameterCollection) cmd.Parameters)
      {
        if (parameter.Direction == ParameterDirection.Input || parameter.Direction == ParameterDirection.InputOutput)
        {
          if (stmtType == StatementType.Delete)
            parameter.Value = row[srcColMapToDataCol[(object)parameter.SourceColumn], DataRowVersion.Original];
          else
            parameter.Value = row[srcColMapToDataCol[(object)parameter.SourceColumn], parameter.SourceVersion];
        }
      }
    }

    private void CopyOutputParmsToDataRow(SACommand cmd, DataRow row, Hashtable srcColMapToDataCol)
    {
      foreach (SAParameter parameter in (DbParameterCollection) cmd.Parameters)
      {
        if (parameter.Direction != ParameterDirection.Input)
          row[srcColMapToDataCol[(object)parameter.SourceColumn]] = parameter.Value;
      }
    }

    private void CopyFirstRecordToDataRow(SADataReader dataReader, DataRow row, DataTableMapping tm, Hashtable srcColMapToDataCol)
    {
      if (!dataReader.Read())
        return;
      for (int ordinal = 0; ordinal < dataReader.FieldCount; ++ordinal)
      {
        int dataColumn = this.MapSrcColToDataColumn(dataReader.GetName(ordinal), row.Table, tm, srcColMapToDataCol);
        row[dataColumn] = dataReader.GetValue(ordinal);
      }
    }

    private void MapSrcColToDataColumn(SACommand cmd, DataTable dataTable, DataTableMapping tm, Hashtable srcColMapToDataCol)
    {
      foreach (DbParameter parameter in (DbParameterCollection) cmd.Parameters)
                MapSrcColToDataColumn(parameter.SourceColumn, dataTable, tm, srcColMapToDataCol);
    }

    private int MapSrcColToDataColumn(string srcCol, DataTable dataTable, DataTableMapping tm, Hashtable srcColMapToDataCol)
    {
      int num;
      if (srcColMapToDataCol.Contains(srcCol))
      {
        num = (int) srcColMapToDataCol[srcCol];
      }
      else
      {
        string index;
        if (tm != null && tm.ColumnMappings.Contains(srcCol))
          index = tm.ColumnMappings[srcCol].DataSetColumn;
        else if (this.MissingMappingAction == MissingMappingAction.Passthrough)
        {
          index = srcCol;
        }
        else
        {
          Exception e = new InvalidOperationException(SARes.GetString(11025, srcCol));
          SATrace.Exception(e);
          throw e;
        }
        if (dataTable.Columns.Contains(index))
        {
          num = dataTable.Columns[index].Ordinal;
          srcColMapToDataCol[srcCol] = num;
        }
        else
        {
          Exception e = new InvalidOperationException(SARes.GetString(11022, index, dataTable.TableName, srcCol));
          SATrace.Exception(e);
          throw e;
        }
      }
      return num;
    }

    private int SendBatchedInserts(Hashtable srcColMapToDataCol, ref Exception updateEx, DataTableMapping tm)
    {
      int num1 = 0;
      int num2 = 0;
      while (_currentBatchCommand < _batchInsertCommands.Count)
      {
        DataRow lastRow = (DataRow) null;
        try
        {
          num2 = ExecuteBatch(srcColMapToDataCol, out lastRow);
          num1 += num2;
        }
        catch (Exception ex)
        {
          updateEx = ex;
        }
        if (RowUpdated != null || updateEx == null || this.ContinueUpdateOnError)
        {
          if (RowUpdated != null)
          {
            SARowUpdatedEventArgs e = new SARowUpdatedEventArgs(lastRow, (IDbCommand)InsertCommand, StatementType.Insert, tm);
            e.Status = updateEx == null ? UpdateStatus.Continue : UpdateStatus.ErrorsOccurred;
            e.Errors = updateEx;
            e._recordsAffected = num2;
                        RowUpdated(this, e);
            if (e.Status == UpdateStatus.SkipAllRemainingRows)
            {
              updateEx = null;
              break;
            }
            if (e.Status == UpdateStatus.ErrorsOccurred)
            {
              if (e.Errors != null)
              {
                updateEx = e.Errors;
              }
              else
              {
                updateEx = (Exception) new DataException(SARes.GetString(11030, "RowUpdatedEvent"));
                lastRow.RowError = updateEx.Message;
              }
              if (!this.ContinueUpdateOnError)
                break;
            }
          }
        }
        else
          break;
      }
            TerminateBatching();
      return num1;
    }

    private struct CommandRowPair
    {
      public IDbCommand Cmd;
      public DataRow Row;

      public CommandRowPair(IDbCommand cmd, DataRow row)
      {
                Cmd = cmd;
                Row = row;
      }
    }
  }
}
