
// Type: iAnywhere.Data.SQLAnywhere.SACommandBuilder
// Assembly: iAnywhere.Data.SQLAnywhere.v4.0, Version=11.0.1.27424, Culture=neutral, PublicKeyToken=f222fc4333e0d400
// MVID: CC4F9F8C-E618-49D1-9147-C06A9EF53D1F
// Assembly location: C:\Program Files\SQL Anywhere 11\Assembly\V4\iAnywhere.Data.SQLAnywhere.v4.0.dll

using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Text;

namespace iAnywhere.Data.SQLAnywhere
{
  /// <summary>
  ///     <para>A way to generate single-table SQL statements that reconcile changes made to a DataSet with the data in the associated database.</para>
  /// </summary>
  public sealed class SACommandBuilder : DbCommandBuilder
  {
    private int _objectId = SACommandBuilder.s_CurrentId++;
    private static int s_CurrentId;

    /// <summary>
    ///     <para>Specifies the SADataAdapter for which to generate statements.</para>
    /// </summary>
    /// <value>An SADataAdapter object.</value>
    /// <remarks>
    ///     <para>When you create a new instance of SACommandBuilder, any existing SACommandBuilder that is associated with this SADataAdapter is released.</para>
    /// </remarks>
    [Description("The data adapter for which to automatically generate commands.")]
    [DefaultValue(null)]
    public SADataAdapter DataAdapter
    {
      get
      {
        SATrace.PropertyCall("<sa.SACommandBuilder.get_DataAdapter|API>", _objectId);
        return (SADataAdapter) base.DataAdapter;
      }
      set
      {
        SATrace.PropertyCall("<sa.SACommandBuilder.set_DataAdapter|API>", _objectId);
        if (DataAdapter != null)
                    DataAdapter.CommandBuilder = null;
        if (DataAdapter != value)
          this.RefreshSchema();
                DataAdapter = (DbDataAdapter) value;
        if (DataAdapter == null)
          return;
        value.CommandBuilder = this;
      }
    }

    /// <summary>
    ///     <para>Initializes an SACommandBuilder object.</para>
    /// </summary>
    public SACommandBuilder()
    {
            Init();
    }

    /// <summary>
    ///     <para>Initializes an SACommandBuilder object.</para>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <param name="adapter">
    ///     An SADataAdapter object for which to generate reconciliation statements.
    /// </param>
    public SACommandBuilder(SADataAdapter adapter)
    {
            Init();
            DataAdapter = adapter;
    }

    private void Init()
    {
      this.RefreshSchema();
    }

    /// <summary>
    ///     <para>Returns the generated SACommand object that performs INSERT operations on the database when an Update is called.</para>
    /// </summary>
    /// <remarks>
    ///     <para>The GetInsertCommand method returns the SACommand object to be executed, so it may be useful for informational or troubleshooting purposes.</para>
    ///     <para>You can also use GetInsertCommand as the basis of a modified command. For example, you might call GetInsertCommand and modify the CommandTimeout value, and then explicitly set that value on the SADataAdapter.</para>
    ///     <para>SQL statements are first generated either when the application calls Update or GetInsertCommand. After the SQL statement is first generated, the application must explicitly call RefreshSchema if it changes the statement in any way. Otherwise, the GetInsertCommand will be still be using information from the previous statement, which might not be correct.</para>
    /// </remarks>
    /// <param name="useColumnsForParameterNames">
    ///     If true, generate parameter names matching column names if possible. If false, generate @p1, @p2, and so on.
    /// </param>
    /// <returns>
    /// <para>The automatically generated SACommand object required to perform insertions.</para>
    ///    </returns>
    /// <seealso cref="M:iAnywhere.Data.SQLAnywhere.SACommandBuilder.GetDeleteCommand" />
    public SACommand GetInsertCommand(bool useColumnsForParameterNames)
    {
      try
      {
        SATrace.FunctionScopeEnter("<sa.SACommandBuilder.GetInsertCommand|API>", _objectId, "useColumnsForParameterNames");
                CheckAdapter();
        return (SACommand) base.GetInsertCommand(useColumnsForParameterNames);
      }
      finally
      {
        SATrace.FunctionScopeLeave();
      }
    }

    /// <summary>
    ///     <para>Returns the generated SACommand object that performs INSERT operations on the database when an Update is called.</para>
    /// </summary>
    /// <remarks>
    ///     <para>The GetInsertCommand method returns the SACommand object to be executed, so it may be useful for informational or troubleshooting purposes.</para>
    ///     <para>You can also use GetInsertCommand as the basis of a modified command. For example, you might call GetInsertCommand and modify the CommandTimeout value, and then explicitly set that value on the SADataAdapter.</para>
    ///     <para>SQL statements are first generated either when the application calls Update or GetInsertCommand. After the SQL statement is first generated, the application must explicitly call RefreshSchema if it changes the statement in any way. Otherwise, the GetInsertCommand will be still be using information from the previous statement, which might not be correct.</para>
    /// </remarks>
    /// <returns>
    /// <para>The automatically generated SACommand object required to perform insertions.</para>
    ///    </returns>
    /// <seealso cref="M:iAnywhere.Data.SQLAnywhere.SACommandBuilder.GetDeleteCommand" />
    public SACommand GetInsertCommand()
    {
      try
      {
        SATrace.FunctionScopeEnter("<sa.SACommandBuilder.GetInsertCommand|API>", _objectId, new string[0]);
                CheckAdapter();
        return (SACommand) base.GetInsertCommand();
      }
      finally
      {
        SATrace.FunctionScopeLeave();
      }
    }

    /// <summary>
    ///     <para>Returns the generated SACommand object that performs DELETE operations on the database when SADataAdapter.Update is called.</para>
    /// </summary>
    /// <remarks>
    ///     <para>The GetDeleteCommand method returns the SACommand object to be executed, so it may be useful for informational or troubleshooting purposes.</para>
    ///     <para>You can also use GetDeleteCommand as the basis of a modified command. For example, you might call GetDeleteCommand and modify the CommandTimeout value, and then explicitly set that value on the SADataAdapter.</para>
    ///     <para>SQL statements are first generated when the application calls Update or GetDeleteCommand. After the SQL statement is first generated, the application must explicitly call RefreshSchema if it changes the statement in any way. Otherwise, the GetDeleteCommand will still be using information from the previous statement.</para>
    /// </remarks>
    /// <param name="useColumnsForParameterNames">
    ///     If true, generate parameter names matching column names if possible. If false, generate @p1, @p2, and so on.
    /// </param>
    /// <returns>
    /// <para>The automatically generated SACommand object required to perform deletions.</para>
    ///    </returns>
    /// <seealso cref="M:System.Data.Common.DbCommandBuilder.RefreshSchema" />
    public SACommand GetDeleteCommand(bool useColumnsForParameterNames)
    {
      try
      {
        SATrace.FunctionScopeEnter("<sa.SACommandBuilder.GetDeleteCommand|API>", _objectId, "useColumnsForParameterNames");
                CheckAdapter();
        return (SACommand) base.GetDeleteCommand(useColumnsForParameterNames);
      }
      finally
      {
        SATrace.FunctionScopeLeave();
      }
    }

    /// <summary>
    ///     <para>Returns the generated SACommand object that performs DELETE operations on the database when SADataAdapter.Update is called.</para>
    /// </summary>
    /// <remarks>
    ///     <para>The GetDeleteCommand method returns the SACommand object to be executed, so it may be useful for informational or troubleshooting purposes.</para>
    ///     <para>You can also use GetDeleteCommand as the basis of a modified command. For example, you might call GetDeleteCommand and modify the CommandTimeout value, and then explicitly set that value on the SADataAdapter.</para>
    ///     <para>SQL statements are first generated when the application calls Update or GetDeleteCommand. After the SQL statement is first generated, the application must explicitly call RefreshSchema if it changes the statement in any way. Otherwise, the GetDeleteCommand will still be using information from the previous statement.</para>
    /// </remarks>
    /// <returns>
    /// <para>The automatically generated SACommand object required to perform deletions.</para>
    ///    </returns>
    /// <seealso cref="M:System.Data.Common.DbCommandBuilder.RefreshSchema" />
    public SACommand GetDeleteCommand()
    {
      try
      {
        SATrace.FunctionScopeEnter("<sa.SACommandBuilder.GetDeleteCommand|API>", _objectId, new string[0]);
                CheckAdapter();
        return (SACommand) base.GetDeleteCommand();
      }
      finally
      {
        SATrace.FunctionScopeLeave();
      }
    }

    /// <summary>
    ///     <para>Returns the generated SACommand object that performs UPDATE operations on the database when an Update is called.</para>
    /// </summary>
    /// <remarks>
    ///     <para>The GetUpdateCommand method returns the SACommand object to be executed, so it may be useful for informational or troubleshooting purposes.</para>
    ///     <para>You can also use GetUpdateCommand as the basis of a modified command. For example, you might call GetUpdateCommand and modify the CommandTimeout value, and then explicitly set that value on the SADataAdapter.</para>
    ///     <para>SQL statements are first generated when the application calls Update or GetUpdateCommand. After the SQL statement is first generated, the application must explicitly call RefreshSchema if it changes the statement in any way. Otherwise, the GetUpdateCommand will be still be using information from the previous statement, which might not be correct.</para>
    /// </remarks>
    /// <param name="useColumnsForParameterNames">
    ///     If true, generate parameter names matching column names if possible. If false, generate @p1, @p2, and so on.
    /// </param>
    /// <returns>
    /// <para>The automatically generated SACommand object required to perform updates.</para>
    ///    </returns>
    /// <seealso cref="M:System.Data.Common.DbCommandBuilder.RefreshSchema" />
    public SACommand GetUpdateCommand(bool useColumnsForParameterNames)
    {
      try
      {
        SATrace.FunctionScopeEnter("<sa.SACommandBuilder.GetUpdateCommand|API>", _objectId, "useColumnsForParameterNames");
                CheckAdapter();
        return (SACommand) base.GetUpdateCommand(useColumnsForParameterNames);
      }
      finally
      {
        SATrace.FunctionScopeLeave();
      }
    }

    /// <summary>
    ///     <para>Returns the generated SACommand object that performs UPDATE operations on the database when an Update is called.</para>
    /// </summary>
    /// <remarks>
    ///     <para>The GetUpdateCommand method returns the SACommand object to be executed, so it may be useful for informational or troubleshooting purposes.</para>
    ///     <para>You can also use GetUpdateCommand as the basis of a modified command. For example, you might call GetUpdateCommand and modify the CommandTimeout value, and then explicitly set that value on the SADataAdapter.</para>
    ///     <para>SQL statements are first generated when the application calls Update or GetUpdateCommand. After the SQL statement is first generated, the application must explicitly call RefreshSchema if it changes the statement in any way. Otherwise, the GetUpdateCommand will be still be using information from the previous statement, which might not be correct.</para>
    /// </remarks>
    /// <returns>
    /// <para>The automatically generated SACommand object required to perform updates.</para>
    ///    </returns>
    /// <seealso cref="M:System.Data.Common.DbCommandBuilder.RefreshSchema" />
    public SACommand GetUpdateCommand()
    {
      try
      {
        SATrace.FunctionScopeEnter("<sa.SACommandBuilder.GetUpdateCommand|API>", _objectId, new string[0]);
                CheckAdapter();
        return (SACommand) base.GetUpdateCommand();
      }
      finally
      {
        SATrace.FunctionScopeLeave();
      }
    }

    private void CheckAdapter()
    {
      if (DataAdapter == null)
      {
        Exception e = new InvalidOperationException(SARes.GetString(10986, "DataAdapter"));
        SATrace.Exception(e);
        throw e;
      }
    }

    /// <summary>
    ///     <para>Allows the provider implementation of <see cref="T:System.Data.Common.DbCommandBuilder" /> to handle additional parameter properties.</para>
    /// </summary>
    /// <param name="parameter">
    ///     A <see cref="T:System.Data.Common.DbParameter" /> to which the additional modifications are applied.
    /// </param>
    /// <param name="row">
    ///     The <see cref="T:System.Data.DataRow" /> from the schema table provided by SADataReader.GetSchemaTable.
    /// </param>
    /// <param name="statementType">
    ///     The type of command being generated: INSERT, UPDATE or DELETE.
    /// </param>
    /// <param name="whereClause">
    ///     The value is true if the parameter is part of the UPDATE or DELETE WHERE clause, and false if it is part of the INSERT or UPDATE values.
    /// </param>
    /// <seealso cref="M:iAnywhere.Data.SQLAnywhere.SADataReader.GetSchemaTable" />
    protected override void ApplyParameterInfo(DbParameter parameter, DataRow row, StatementType statementType, bool whereClause)
    {
      try
      {
        SATrace.FunctionScopeEnter("<sa.SACommandBuilder.ApplyParameterInfo|API>", _objectId, "parameter", "row", "statementType", "whereClause");
        SAParameter saParameter = (SAParameter) parameter;
        saParameter.Direction = ParameterDirection.Input;
        object obj1 = row["ColumnSize"];
        if (!obj1.Equals(DBNull.Value))
          saParameter.Size = Convert.ToInt32(obj1);
        object obj2 = row["NumericPrecision"];
        if (!obj2.Equals(DBNull.Value))
          saParameter.Precision = Convert.ToByte((short) obj2);
        object obj3 = row["NumericScale"];
        if (!obj3.Equals(DBNull.Value))
          saParameter.Scale = Convert.ToByte((short) obj3);
        object obj4 = row["ProviderType"];
        if (!obj4.Equals(DBNull.Value))
          saParameter.SADbType = (SADbType) obj4;
        object obj5 = row["AllowDBNull"];
        if (obj5.Equals(DBNull.Value))
          return;
        saParameter.IsNullable = Convert.ToBoolean(obj5);
      }
      finally
      {
        SATrace.FunctionScopeLeave();
      }
    }

    /// <summary>
    ///     <para>Returns the schema table for the SACommandBuilder object.</para>
    /// </summary>
    /// <param name="sourceCommand">
    ///     The <see cref="T:System.Data.Common.DbCommand" /> for which to retrieve the corresponding schema table.
    /// </param>
    /// <returns>
    /// <para>A <see cref="T:System.Data.DataTable" /> that represents the schema for the specific <see cref="T:System.Data.Common.DbCommand" />.</para>
    ///    </returns>
    /// <seealso cref="T:iAnywhere.Data.SQLAnywhere.SACommandBuilder" />
    protected override DataTable GetSchemaTable(DbCommand sourceCommand)
    {
      try
      {
        SATrace.FunctionScopeEnter("<sa.SACommandBuilder.ApplyParameterInfo|API>", _objectId, "sourceCommand");
        DataTable schemaTable = base.GetSchemaTable(sourceCommand);
                CheckColumnName(schemaTable);
        return schemaTable;
      }
      finally
      {
        SATrace.FunctionScopeLeave();
      }
    }

    /// <summary>
    ///     <para>Resets the <see cref="P:System.Data.Common.DbCommand.CommandTimeout" />, <see cref="P:System.Data.Common.DbCommand.Transaction" />, <see cref="P:System.Data.Common.DbCommand.CommandType" />, and <see cref="P:System.Data.Common.DbCommand.UpdatedRowSource" /> properties on the <see cref="T:System.Data.Common.DbCommand" />.</para>
    /// </summary>
    /// <param name="command">
    ///     The <see cref="T:System.Data.Common.DbCommand" /> to be used by the command builder for the corresponding insert, update, or delete command.
    /// </param>
    /// <returns>
    /// <para>A <see cref="T:System.Data.Common.DbCommand" /> instance to use for each insert, update, or delete operation. Passing a null value allows the InitializeCommand method to create a <see cref="T:System.Data.Common.DbCommand" /> object based on the SELECT statement associated with the SACommandBuilder object.</para>
    ///    </returns>
    /// <seealso cref="T:iAnywhere.Data.SQLAnywhere.SACommandBuilder" />
    protected override DbCommand InitializeCommand(DbCommand command)
    {
      try
      {
        SATrace.FunctionScopeEnter("<sa.SACommandBuilder.InitializeCommand|API>", _objectId, "command");
        return base.InitializeCommand(command);
      }
      finally
      {
        SATrace.FunctionScopeLeave();
      }
    }

    /// <summary>
    ///     <para>Returns the name of the specified parameter in the format of @p#. Use when building a custom command builder.</para>
    /// </summary>
    /// <param name="index">
    ///     The number to be included as part of the parameter's name.
    /// </param>
    /// <returns>
    /// <para>The name of the parameter with the specified number appended as part of the parameter name.</para>
    ///    </returns>
    protected override string GetParameterName(int index)
    {
      try
      {
        SATrace.FunctionScopeEnter("<sa.SACommandBuilder.GetParameterName|API>", _objectId, "index");
        return "@p" + Convert.ToString(index);
      }
      finally
      {
        SATrace.FunctionScopeLeave();
      }
    }

    /// <summary>
    ///     <para>Returns the full parameter name, given the partial parameter name.</para>
    /// </summary>
    /// <param name="parameterName">The partial name of the parameter.</param>
    /// <returns>
    /// <para>The full parameter name corresponding to the partial parameter name requested.</para>
    ///    </returns>
    protected override string GetParameterName(string parameterName)
    {
      try
      {
        SATrace.FunctionScopeEnter("<sa.SACommandBuilder.GetParameterName|API>", _objectId, "parameterName");
        return "@p" + parameterName;
      }
      finally
      {
        SATrace.FunctionScopeLeave();
      }
    }

    /// <summary>
    ///     <para>Returns the placeholder for the parameter in the associated SQL statement.</para>
    /// </summary>
    /// <param name="index">
    ///     The number to be included as part of the parameter's name.
    /// </param>
    /// <returns>
    /// <para>The name of the parameter with the specified number appended.</para>
    ///    </returns>
    protected override string GetParameterPlaceholder(int index)
    {
      try
      {
        SATrace.FunctionScopeEnter("<sa.SACommandBuilder.GetParameterPlaceholder|API>", _objectId, "index");
        return "?";
      }
      finally
      {
        SATrace.FunctionScopeLeave();
      }
    }

    private void SARowUpdatingHandler(object sender, SARowUpdatingEventArgs evnt)
    {
      this.RowUpdatingHandler((RowUpdatingEventArgs) evnt);
    }

    /// <summary>
    ///     <para>Registers the SACommandBuilder object to handle the SADataAdapter.RowUpdating event for an SADataAdapter object.</para>
    /// </summary>
    /// <param name="adapter">
    ///     The SADataAdapter object to be used for the update.
    /// </param>
    /// <seealso cref="T:iAnywhere.Data.SQLAnywhere.SACommandBuilder" />
    /// <seealso cref="T:iAnywhere.Data.SQLAnywhere.SADataAdapter" />
    /// <seealso cref="E:iAnywhere.Data.SQLAnywhere.SADataAdapter.RowUpdating" />
    protected override void SetRowUpdatingHandler(DbDataAdapter adapter)
    {
      try
      {
        SATrace.FunctionScopeEnter("<sa.SACommandBuilder.SetRowUpdatingHandler|API>", _objectId, "adapter");
      }
      finally
      {
        SATrace.FunctionScopeLeave();
      }
    }

    private void CheckColumnName(DataTable schema)
    {
      Hashtable hashtable = new Hashtable();
      for (int row = 0; row < schema.Rows.Count; ++row)
      {
        string columnName = GetColumnName(schema, row);
        if (GetBaseTableName(schema, row).Length > 0)
        {
          if (hashtable.Contains(columnName))
          {
            Exception e = new InvalidOperationException(SARes.GetString(11034));
            SATrace.Exception(e);
            throw e;
          }
          hashtable[columnName] = null;
        }
      }
    }

    private string GetBaseTableName(DataTable schema, int row)
    {
      return GetSchemaName(schema, row, "BaseTableName").ToLower();
    }

    private string GetColumnName(DataTable schema, int row)
    {
      return GetSchemaName(schema, row, "ColumnName").ToLower();
    }

    private string GetSchemaName(DataTable schema, int row, string dataReaderColumn)
    {
      object obj = schema.Rows[row][dataReaderColumn];
      return !obj.Equals(DBNull.Value) ? ((string) obj).Trim() : "";
    }

    /// <summary>
    ///     <para>Populates the Parameters collection of the specified SACommand object. This is used for the stored procedure specified in the SACommand.</para>
    /// </summary>
    /// <remarks>
    ///     <para>DeriveParameters overwrites any existing parameter information for the SACommand.</para>
    ///     <para>DeriveParameters requires an extra call to the database server. If the parameter information is known in advance, it is more efficient to populate the Parameters collection by setting the information explicitly.</para>
    /// </remarks>
    /// <param name="command">
    ///     An SACommand object for which to derive parameters.
    /// </param>
    public static void DeriveParameters(SACommand command)
    {
      try
      {
        SATrace.FunctionScopeEnter("<sa.SACommandBuilder.DeriveParameters|API>", "command");
        if (command == null)
        {
          Exception e = new ArgumentNullException("command");
          SATrace.Exception(e);
          throw e;
        }
        if (command.CommandType != CommandType.StoredProcedure)
        {
          Exception e = new InvalidOperationException(SARes.GetString(11036, command.CommandType.ToString()));
          SATrace.Exception(e);
          throw e;
        }
        if (command.Connection == null)
        {
          Exception e = new InvalidOperationException(SARes.GetString(11037, "command.Connection"));
          SATrace.Exception(e);
          throw e;
        }
        if (command.Connection.GetConnectionState() != ConnectionState.Open)
        {
          Exception e = new InvalidOperationException(SARes.GetString(11038));
          SATrace.Exception(e);
          throw e;
        }
        if (command.CommandText == null || command.CommandText.Trim().Length < 1)
        {
          Exception e = new InvalidOperationException(SARes.GetString(11037, "command.CommandText"));
          SATrace.Exception(e);
          throw e;
        }
        SAParameterCollection parameters = command.Parameters;
        parameters.Clear();
        string format = "SELECT creator, procname, parmname, parm_id, parmtype, parmmode, parmdomain, length, scale, user_type FROM sys.sysprocparms WHERE ( ( parmtype = 0 ) OR ( parmtype = 4 ) ) AND {0} ORDER BY creator, procname, parm_id";
        string creator = string.Empty;
        string name = string.Empty;
        string str1 = string.Empty;
        SACommandBuilder.ParseProcedureName(command.CommandText, out creator, out name);
        string str2 = SAUtility.EscapeQuotationMarks('\'', creator);
        name = SAUtility.EscapeQuotationMarks('\'', name);
        string str3 = str2.Length <= 0 ? string.Format("( UCASE( procname ) = UCASE( '{0}' ) )", name) : string.Format("( ( UCASE( creator ) = UCASE( '{0}' ) ) AND ( UCASE( procname ) = UCASE( '{1}' ) ) )", str2, name);
        SACommand saCommand = new SACommand(string.Format(format, str3), command.Connection);
        SADataReader saDataReader = saCommand.ExecuteReader();
        string strA1 = null;
        string strA2 = null;
        while (saDataReader.Read())
        {
          if (strA1 == null)
            strA1 = saDataReader.GetString(0);
          if (strA2 == null)
            strA2 = saDataReader.GetString(1);
          if (string.Compare(strA1, saDataReader.GetString(0), false) == 0 && string.Compare(strA2, saDataReader.GetString(1), false) == 0)
          {
            SAParameter saParameter = new SAParameter();
            saParameter.ParameterName = saDataReader.GetString(2);
            saParameter.Size = Convert.ToInt32(saDataReader[7].ToString());
            saParameter.Scale = Convert.ToByte(saDataReader[8].ToString());
            switch (Convert.ToInt32(saDataReader[4].ToString()))
            {
              case 0:
                string @string = saDataReader.GetString(5);
                if (string.Compare(@string, "INOUT", true) == 0)
                {
                  saParameter.Direction = ParameterDirection.InputOutput;
                  break;
                }
                if (string.Compare(@string, "IN", true) == 0)
                {
                  saParameter.Direction = ParameterDirection.Input;
                  break;
                }
                if (string.Compare(@string, "OUT", true) == 0)
                {
                  saParameter.Direction = ParameterDirection.Output;
                  break;
                }
                break;
              case 4:
                saParameter.Direction = ParameterDirection.ReturnValue;
                break;
            }
            saParameter.SADbType = SADataConvert.GetSADbType(saDataReader.IsDBNull(9) ? string.Empty : saDataReader.GetString(9), saDataReader.GetString(6));
            if (SADataConvert.IsDecimal((int) saParameter.SADbType))
            {
              saParameter.Precision = Convert.ToByte(saParameter.Size.ToString());
              saParameter.Size = (saParameter.Precision & byte.MaxValue) / 2 + 1;
            }
            if (saParameter.SADbType == SADbType.UniqueIdentifier)
              saParameter.Size = 16;
            if (saParameter.SADbType == SADbType.NChar || saParameter.SADbType == SADbType.NVarChar || (saParameter.SADbType == SADbType.LongNVarchar || saParameter.SADbType == SADbType.NText))
              saParameter.Scale = 0;
            bool flag = false;
            if (!saDataReader.IsDBNull(9))
              flag = SADataConvert.IsDateOrTime(saDataReader.GetString(9));
            if (!flag && !saDataReader.IsDBNull(6))
              flag = SADataConvert.IsDateOrTime(saDataReader.GetString(6));
            if (flag)
              saParameter.Scale = 6;
            parameters.Add(saParameter);
          }
          else
            break;
        }
        saDataReader.Close();
        if (parameters.Count != 0)
          return;
        saCommand.CommandText = string.Format("SELECT COUNT(*) FROM sys.sysprocedure WHERE ( proc_name = '{0}' )", name);
        if ((int) saCommand.ExecuteScalar() == 0)
        {
          Exception e = new InvalidOperationException(SARes.GetString(11039, command.CommandText));
          SATrace.Exception(e);
          throw e;
        }
      }
      finally
      {
        SATrace.FunctionScopeLeave();
      }
    }

    private static void ParseProcedureName(string cmdText, out string creator, out string name)
    {
      cmdText = cmdText.Trim();
      creator = string.Empty;
      name = string.Empty;
      int num1 = cmdText.IndexOf("\".\"");
      if (cmdText.Length >= 7 && num1 >= 2 && (cmdText[0] == 34 && cmdText[cmdText.Length - 1] == 34))
      {
        creator = cmdText.Substring(1, num1 - 1);
        name = cmdText.Substring(num1 + 3, cmdText.Length - num1 - 4);
      }
      else
      {
        int num2 = cmdText.IndexOf("\".[");
        if (cmdText.Length >= 7 && num2 >= 2 && (cmdText[0] == 34 && cmdText[cmdText.Length - 1] == 93))
        {
          creator = cmdText.Substring(1, num2 - 1);
          name = cmdText.Substring(num2 + 3, cmdText.Length - num2 - 4);
        }
        else
        {
          int num3 = cmdText.IndexOf("\".");
          if (cmdText.Length >= 5 && num3 >= 2 && cmdText[0] == 34)
          {
            creator = cmdText.Substring(1, num3 - 1);
            name = cmdText.Substring(num3 + 2, cmdText.Length - num3 - 2);
          }
          else
          {
            int num4 = cmdText.IndexOf("].\"");
            if (cmdText.Length >= 7 && num4 >= 2 && (cmdText[0] == 91 && cmdText[cmdText.Length - 1] == 34))
            {
              creator = cmdText.Substring(1, num4 - 1);
              name = cmdText.Substring(num4 + 3, cmdText.Length - num4 - 4);
            }
            else
            {
              int num5 = cmdText.IndexOf("].[");
              if (cmdText.Length >= 7 && num5 >= 2 && (cmdText[0] == 91 && cmdText[cmdText.Length - 1] == 93))
              {
                creator = cmdText.Substring(1, num5 - 1);
                name = cmdText.Substring(num5 + 3, cmdText.Length - num5 - 4);
              }
              else
              {
                int num6 = cmdText.IndexOf("].");
                if (cmdText.Length >= 5 && num6 >= 2 && cmdText[0] == 91)
                {
                  creator = cmdText.Substring(1, num6 - 1);
                  name = cmdText.Substring(num6 + 2, cmdText.Length - num6 - 2);
                }
                else if (cmdText.Length >= 3 && cmdText[0] == 34 && cmdText[cmdText.Length - 1] == 34)
                  name = cmdText.Substring(1, cmdText.Length - 2);
                else if (cmdText.Length >= 3 && cmdText[0] == 91 && cmdText[cmdText.Length - 1] == 93)
                {
                  name = cmdText.Substring(1, cmdText.Length - 2);
                }
                else
                {
                  int length1 = cmdText.IndexOf(".\"");
                  if (cmdText.Length >= 5 && length1 >= 1 && cmdText[cmdText.Length - 1] == 34)
                  {
                    creator = cmdText.Substring(0, length1);
                    name = cmdText.Substring(length1 + 2, cmdText.Length - length1 - 3);
                  }
                  else
                  {
                    int length2 = cmdText.IndexOf(".[");
                    if (cmdText.Length >= 5 && length2 >= 1 && cmdText[cmdText.Length - 1] == 93)
                    {
                      creator = cmdText.Substring(0, length2);
                      name = cmdText.Substring(length2 + 2, cmdText.Length - length2 - 3);
                    }
                    else
                    {
                      string[] strArray = cmdText.Split('.');
                      if (strArray != null && strArray.Length == 2 && (strArray[0].Trim().Length > 0 && strArray[1].Trim().Length > 0))
                      {
                        creator = strArray[0].Trim();
                        name = strArray[1].Trim();
                      }
                      else if (cmdText.Length > 0)
                        name = cmdText;
                    }
                  }
                }
              }
            }
          }
        }
      }
      creator = creator.Trim();
      name = name.Trim();
    }

    /// <summary>
    /// 	<para>Returns the correct quoted form of an unquoted identifier, including properly escaping any embedded quotes in the identifier.</para>
    /// </summary>
    /// <param name="unquotedIdentifier">
    /// 	The string representing the unquoted identifier that will have be quoted.
    /// </param>
    /// <returns>
    /// 	<para>Returns a string representing the quoted form of an unquoted identifier with embedded quotes properly escaped.</para>
    /// </returns>
    public override string QuoteIdentifier(string unquotedIdentifier)
    {
      try
      {
        SATrace.FunctionScopeEnter("<sa.SACommandBuilder.QuoteIdentifier|API>", _objectId, "unquotedIdentifier");
        if (unquotedIdentifier == null)
        {
          Exception e = new ArgumentNullException("unquotedIdentifier");
          SATrace.Exception(e);
          throw e;
        }
        string quotePrefix = this.QuotePrefix;
        string quoteSuffix = this.QuoteSuffix;
        SACommandBuilder.CheckPrefixSuffix(quotePrefix, quoteSuffix);
        if (unquotedIdentifier.IndexOf('[') >= 0 || unquotedIdentifier.IndexOf(']') >= 0)
        {
          quotePrefix = "\"";
          quoteSuffix = "\"";
        }
        StringBuilder stringBuilder = new StringBuilder();
        if (quotePrefix != null && quotePrefix.Length > 0)
          stringBuilder.Append(quotePrefix);
        if (quoteSuffix != null && quoteSuffix.Length > 0)
        {
          stringBuilder.Append(unquotedIdentifier);
          stringBuilder.Append(quoteSuffix);
        }
        else
          stringBuilder.Append(unquotedIdentifier);
        return stringBuilder.ToString();
      }
      finally
      {
        SATrace.FunctionScopeLeave();
      }
    }

    /// <summary>
    ///     <para>Returns the correct unquoted form of a quoted identifier, including properly un-escaping any embedded quotes in the identifier.</para>
    /// </summary>
    /// <param name="quotedIdentifier">
    ///     The string representing the quoted identifier that will have its embedded quotes removed.
    /// </param>
    /// <returns>
    /// <para>Returns a string representing the unquoted form of a quoted identifier with embedded quotes properly un-escaped.</para>
    ///    </returns>
    public override string UnquoteIdentifier(string quotedIdentifier)
    {
      try
      {
        SATrace.FunctionScopeEnter("<sa.SACommandBuilder.UnquoteIdentifier|API>", _objectId, "quotedIdentifier");
        if (quotedIdentifier == null)
        {
          Exception e = new ArgumentNullException("quotedIdentifier");
          SATrace.Exception(e);
          throw e;
        }
        SACommandBuilder.CheckPrefixSuffix(this.QuotePrefix, this.QuoteSuffix);
        return SACommandBuilder.RemoveIdentifierQuotes(this.QuotePrefix, this.QuoteSuffix, quotedIdentifier);
      }
      finally
      {
        SATrace.FunctionScopeLeave();
      }
    }

    internal static void CheckPrefixSuffix(string quotePrefix, string quoteSuffix)
    {
      string str1 = quotePrefix == null ? "" : quotePrefix.Trim();
      string str2 = quoteSuffix == null ? "" : quoteSuffix.Trim();
      if (str1.Length + str2.Length != 0 && (!("\"" == str1) || !("\"" == str2)) && (!("[" == str1) || !("]" == str2)))
      {
        Exception e = new ArgumentException(SARes.GetString(17445, "QuotePrefix / QuoteSuffix"), "QuotePrefix / QuoteSuffix");
        SATrace.Exception(e);
        throw e;
      }
    }

    internal static string RemoveIdentifierQuotes(string quotePrefix, string quoteSuffix, string quotedIdentifier)
    {
      string str1 = quotePrefix == null ? "" : quotePrefix.Trim();
      string str2 = quoteSuffix == null ? "" : quoteSuffix.Trim();
      string str3 = quotedIdentifier == null ? null : quotedIdentifier.Trim();
      if (str3 == null || str3.Length == 0)
        return str3;
      int length1 = str1.Length;
      int length2 = str2.Length;
      int length3 = str3.Length;
      if (length1 == 0 || length2 == 0 || length3 < length1 + length2 || (!str3.StartsWith(str1, StringComparison.Ordinal) || !str3.EndsWith(str2, StringComparison.Ordinal)))
        return str3;
      return str3.Substring(length1, length3 - (length1 + length2));
    }
  }
}
