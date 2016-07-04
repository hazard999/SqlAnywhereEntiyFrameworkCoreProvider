
// Type: iAnywhere.Data.SQLAnywhere.SACommand
// Assembly: iAnywhere.Data.SQLAnywhere.v4.0, Version=11.0.1.27424, Culture=neutral, PublicKeyToken=f222fc4333e0d400
// MVID: CC4F9F8C-E618-49D1-9147-C06A9EF53D1F
// Assembly location: C:\Program Files\SQL Anywhere 11\Assembly\V4\iAnywhere.Data.SQLAnywhere.v4.0.dll

using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Drawing.Design;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace iAnywhere.Data.SQLAnywhere
{
    /// <summary>
    ///     <para>A SQL statement or stored procedure that is executed against a SQL Anywhere database.</para>
    /// </summary>
    /// <remarks>
    ///     <para><b>Implements:</b> <see cref="T:System.ICloneable" /></para>
    ///     <para>For more information, see @olink targetdoc="programming" targetptr="accessing-adodotnet-dev"@Accessing and manipulating data@/olink@.</para>
    /// </remarks>
    [ToolboxBitmap(typeof(SACommand), "Command.bmp")]
    [Designer("iAnywhere.VSIntegration.SQLAnywhere.CommandDesigner, iAnywhere.VSIntegration.SQLAnywhere, Culture=neutral, PublicKeyToken=f222fc4333e0d400, Version=11.0.1.27424", typeof(IDesigner))]
    [ToolboxItem("iAnywhere.VSIntegration.SQLAnywhere.CommandToolboxItem, iAnywhere.VSIntegration.SQLAnywhere, Culture=neutral, PublicKeyToken=f222fc4333e0d400, Version=11.0.1.27424")]
    public sealed class SACommand : DbCommand, ICloneable
    {
        private string[] _allParmNames = new string[0];
        private string[] _inParmNames = new string[0];
        private string[] _outParmNames = new string[0];
        private bool _getOutputParms = true;
        private int _objectId = SACommand.s_CurrentId++;
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
        private SADataAdapter _adapter;
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

        /// <summary>
        ///     <para>Gets or sets the text of a SQL statement or stored procedure.</para>
        /// </summary>
        /// <value>The SQL statement or the name of the stored procedure to execute. The default is an empty string.</value>
        /// <seealso cref="M:iAnywhere.Data.SQLAnywhere.SACommand.#ctor" />
        [Category("Data")]
        [DefaultValue("")]
        [Editor("iAnywhere.VSIntegration.SQLAnywhere.CommandTextEditor, iAnywhere.VSIntegration.SQLAnywhere, Culture=neutral, PublicKeyToken=f222fc4333e0d400, Version=11.0.1.27424", typeof(UITypeEditor))]
        [Description("Command text to execute.")]
        public override string CommandText
        {
            get
            {
                SATrace.PropertyCall("<sa.SACommand.get_CommandText|API>", _objectId);
                return _cmdText;
            }
            set
            {
                SATrace.PropertyCall("<sa.SACommand.set_CommandText|API>", _objectId);
                if (!_cmdText.Equals(value))
                    Unprepare();
                _cmdText = value == null ? "" : value;
                if (!SACommand.IsCreateTableStmt(_cmdText))
                    return;
                _cmdText = SACommand.ModifyCreateTableStmt(_cmdText);
            }
        }

        /// <summary>
        ///     <para>This feature is not supported by SQL Anywhere ADO.NET provider.</para>
        /// </summary>
        [DefaultValue(30)]
        [Description("Time to wait for command to execute.")]
        public override int CommandTimeout
        {
            get
            {
                SATrace.PropertyCall("<sa.SACommand.get_CommandTimeout|API>", _objectId);
                return _timeout;
            }
            set
            {
                SATrace.PropertyCall("<sa.SACommand.set_CommandTimeout|API>", _objectId);
                if (value < 0)
                {
                    Exception e = new ArgumentException(SARes.GetString(10995, value.ToString()), "value");
                    SATrace.Exception(e);
                    throw e;
                }
                _timeout = value;
            }
        }

        /// <summary>
        ///     <para>Gets or sets the type of command represented by an SACommand.</para>
        /// </summary>
        /// <value>One of the <see cref="T:System.Data.CommandType" /> values. The default is <see cref="F:System.Data.CommandType.Text" />.</value>
        /// <remarks>
        ///     <para>Supported command types are as follows: </para>
        ///     <list type="bullet">
        ///     <item>
        ///     <term><see cref="F:System.Data.CommandType.StoredProcedure" /></term> When you specify this CommandType, the command text must be the name of a stored procedure and you must supply any arguments as SAParameter objects.
        ///     </item>
        ///     <item>
        ///     <term><see cref="F:System.Data.CommandType.Text" /></term> This is the default value.
        ///     </item>
        ///     </list>
        ///     <para>When the CommandType property is set to StoredProcedure, the CommandText property should be set to the name of the stored procedure. The command executes this stored procedure when you call one of the Execute methods.</para>
        ///     <para>Use a question mark (?) placeholder to pass parameters. For example:</para>
        ///     <code>SELECT * FROM Customers WHERE ID = ?</code>
        ///     <para>The order in which SAParameter objects are added to the SAParameterCollection must directly correspond to the position of the question mark placeholder for the parameter.</para>
        /// 
        /// </remarks>
        [DefaultValue(CommandType.Text)]
        [Category("Data")]
        [Description("How to interpret the CommandText.")]
        [RefreshProperties(RefreshProperties.All)]
        public override CommandType CommandType
        {
            get
            {
                SATrace.PropertyCall("<sa.SACommand.get_CommandType|API>", _objectId);
                return _cmdType;
            }
            set
            {
                SATrace.PropertyCall("<sa.SACommand.set_CommandType|API>", _objectId);
                if (value == CommandType.TableDirect)
                {
                    Exception e = new ArgumentException(SARes.GetString(10996), "value");
                    SATrace.Exception(e);
                    throw e;
                }
                if (_cmdType != value)
                    Unprepare();
                _cmdType = value;
            }
        }

        /// <summary>
        ///     <para>Gets or sets the <see cref="T:System.Data.Common.DbConnection" /> used by this SACommand object.</para>
        /// </summary>
        /// <returns>
        /// <para>The connection to the data source.</para>
        ///    </returns>
        /// <seealso cref="T:iAnywhere.Data.SQLAnywhere.SACommand" />
        protected override DbConnection DbConnection
        {
            get
            {
                SATrace.PropertyCall("<sa.SACommand.get_DbConnection|API>", _objectId);
                return _conn;
            }
            set
            {
                SATrace.PropertyCall("<sa.SACommand.set_DbConnection|API>", _objectId);
                if (_conn != value)
                    Unprepare();
                _conn = (SAConnection)value;
            }
        }

        /// <summary>
        ///     <para>Gets or sets the connection object to which the SACommand object applies.</para>
        /// </summary>
        /// <value>The default value is a null reference. In Visual Basic it is Nothing.</value>
        [Editor("iAnywhere.VSIntegration.SQLAnywhere.ConnectionEditor, iAnywhere.VSIntegration.SQLAnywhere, Culture=neutral, PublicKeyToken=f222fc4333e0d400, Version=11.0.1.27424", typeof(UITypeEditor))]
        [DefaultValue(null)]
        [Description("Connection used by the command.")]
        [Category("Behavior")]
        public SAConnection Connection
        {
            get
            {
                SATrace.PropertyCall("<sa.SACommand.get_Connection|API>", _objectId);
                return (SAConnection)DbConnection;
            }
            set
            {
                SATrace.PropertyCall("<sa.SACommand.set_Connection|API>", _objectId);
                DbConnection = value;
            }
        }

        /// <summary>
        ///     <para>Gets or sets a value that indicates if the SACommand should be visible in a Windows Form Designer control. The default is true.</para>
        /// </summary>
        /// <value>True if this SACommand instance should be visible, false if this instance should not be visible. The default is false.</value>
        [Browsable(false)]
        [DesignOnly(true)]
        [DefaultValue(true)]
        public override bool DesignTimeVisible
        {
            get
            {
                SATrace.PropertyCall("<sa.SACommand.get_DesignTimeVisible|API>", _objectId);
                return _designTimeVisible;
            }
            set
            {
                SATrace.PropertyCall("<sa.SACommand.set_DesignTimeVisible|API>", _objectId);
                _designTimeVisible = value;
            }
        }

        /// <summary>
        ///     <para>Gets the collection of <see cref="T:System.Data.Common.DbParameter" /> objects.</para>
        /// </summary>
        /// <returns>
        /// <para>The parameters of the SQL statement or stored procedure.</para>
        ///    </returns>
        protected override DbParameterCollection DbParameterCollection
        {
            get
            {
                SATrace.PropertyCall("<sa.SACommand.get_DbParameterCollection|API>", _objectId);
                return _parms;
            }
        }

        /// <summary>
        ///     <para>A collection of parameters for the current statement. Use question marks in the CommandText to indicate parameters.</para>
        /// </summary>
        /// <value>The parameters of the SQL statement or stored procedure. The default value is an empty collection.</value>
        /// <remarks>
        ///     <para>When CommandType is set to Text, pass parameters using the question mark placeholder. For example:</para>
        ///     <code>SELECT * FROM Customers WHERE ID = ?</code>
        ///     <para>The order in which SAParameter objects are added to the SAParameterCollection must directly correspond to the position of the question mark placeholder for the parameter in the command text.</para>
        ///     <para>When the parameters in the collection do not match the requirements of the query to be executed, an error may result or an exception may be thrown.</para>
        /// 
        /// </remarks>
        /// <seealso cref="T:iAnywhere.Data.SQLAnywhere.SAParameterCollection" />
        [Description("The parameters collection.")]
        [Category("Data")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public SAParameterCollection Parameters
        {
            get
            {
                SATrace.PropertyCall("<sa.SACommand.get_Parameters|API>", _objectId);
                return (SAParameterCollection)DbParameterCollection;
            }
        }

        /// <summary>
        ///     <para>Gets or sets the <see cref="T:System.Data.Common.DbTransaction" /> within which this SACommand object executes.</para>
        /// </summary>
        /// <returns>
        /// <para>The transaction within which a Command object of a .NET Framework data provider executes. The default value is a null reference (Nothing in Visual Basic).</para>
        ///    </returns>
        protected override DbTransaction DbTransaction
        {
            get
            {
                SATrace.PropertyCall("<sa.SACommand.get_DbTransaction|API>", _objectId);
                return _asaTran;
            }
            set
            {
                SATrace.PropertyCall("<sa.SACommand.set_DbTransaction|API>", _objectId);
                _asaTran = (SATransaction)value;
            }
        }

        /// <summary>
        ///     <para>Specifies the SATransaction object in which the SACommand executes.</para>
        /// </summary>
        /// <value>The default value is a null reference. In Visual Basic, this is Nothing.</value>
        /// <remarks>
        ///     <para>You cannot set the Transaction property if it is already set to a specific value and the command is executing. If you set the transaction property to an SATransaction object that is not connected to the same SAConnection object as the SACommand object, an exception will be thrown the next time you attempt to execute a statement.</para>
        ///     <para>For more information, see @olink targetdoc="programming" targetptr="transaction-adodotnet-development"@Transaction processing@/olink@.</para>
        /// </remarks>
        /// <seealso cref="T:iAnywhere.Data.SQLAnywhere.SATransaction" />
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public SATransaction Transaction
        {
            get
            {
                SATrace.PropertyCall("<sa.SACommand.get_Transaction|API>", _objectId);
                return (SATransaction)DbTransaction;
            }
            set
            {
                SATrace.PropertyCall("<sa.SACommand.set_Transaction|API>", _objectId);
                DbTransaction = value;
            }
        }

        /// <summary>
        ///     <para>Gets or sets how command results are applied to the DataRow when used by the Update method of the SADataAdapter.</para>
        /// </summary>
        /// <value>
        /// <para>One of the UpdatedRowSource values. The default value is UpdateRowSource.OutputParameters. If the command is automatically generated, this property is UpdateRowSource.None.</para>
        ///        </value>
        /// <remarks>
        ///     <para>UpdatedRowSource.Both, which returns both resultset and output parameters, is not supported.</para>
        /// </remarks>
        [DefaultValue(UpdateRowSource.OutputParameters)]
        [Category("Behavior")]
        [Description("When used by an DataAdapter.Update, how command results are applied to the current DataRow.")]
        public override UpdateRowSource UpdatedRowSource
        {
            get
            {
                SATrace.PropertyCall("<sa.SACommand.get_UpdatedRowSource|API>", _objectId);
                return _updatedRowSrc;
            }
            set
            {
                SATrace.PropertyCall("<sa.SACommand.set_UpdatedRowSource|API>", _objectId);
                _updatedRowSrc = value;
            }
        }

        internal SADataAdapter DataAdapter
        {
            get
            {
                return _adapter;
            }
            set
            {
                _adapter = value;
            }
        }

        /// <summary>
        ///     <para>Initializes an SACommand object.</para>
        /// </summary>
        public SACommand()
        {
            Init();
        }

        /// <summary>
        ///     <para>Initializes an SACommand object.</para>
        /// </summary>
        /// <param name="cmdText">
        ///     The text of the SQL statement or stored procedure. For parameterized statements, use a question mark (?) placeholder to pass parameters.
        /// </param>
        public SACommand(string cmdText)
        {
            Init();
            CommandText = cmdText;
        }

        /// <summary>
        ///     <para>A SQL statement or stored procedure that is executed against a SQL Anywhere database.</para>
        /// </summary>
        /// <param name="cmdText">
        ///     The text of the SQL statement or stored procedure. For parameterized statements, use a question mark (?) placeholder to pass parameters.
        /// </param>
        /// <param name="connection">The current connection.</param>
        public SACommand(string cmdText, SAConnection connection)
        {
            Init();
            CommandText = cmdText;
            Connection = connection;
        }

        /// <summary>
        ///     <para>A SQL statement or stored procedure that is executed against a SQL Anywhere database.</para>
        /// </summary>
        /// <param name="cmdText">
        ///     The text of the SQL statement or stored procedure. For parameterized statements, use a question mark (?) placeholder to pass parameters.
        /// </param>
        /// <param name="connection">The current connection.</param>
        /// <param name="transaction">
        ///     The SATransaction object in which the SAConnection executes.
        /// </param>
        /// <seealso cref="T:iAnywhere.Data.SQLAnywhere.SATransaction" />
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

        /// <summary>
        ///     <para>Destructs an SACommand object.</para>
        /// </summary>
        ~SACommand()
        {
            Dispose(false);
        }

        object ICloneable.Clone()
        {
            SACommand saCommand = new SACommand();
            saCommand.CommandType = CommandType;
            saCommand.CommandText = CommandText;
            saCommand.CommandTimeout = CommandTimeout;
            saCommand.Connection = Connection;
            saCommand.Transaction = Transaction;
            saCommand.UpdatedRowSource = UpdatedRowSource;
            saCommand.DesignTimeVisible = DesignTimeVisible;
            foreach (object parameter in (DbParameterCollection)Parameters)
                saCommand.Parameters.Add((parameter as ICloneable).Clone());
            return saCommand;
        }

        /// <summary>
        ///     <para>Frees the resources associated with the object.</para>
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            ResetAsyncCommand();
            if (_disposed)
                return;
            try
            {
                if (_asyncController != null)
                    _asyncController.Close();
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

        private void FreeCommand(bool checkException)
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

        private bool IsCommandCanceled(SAException ex)
        {
            return ex.Errors[0].NativeError == -299;
        }

        internal void DataReaderClosed()
        {
            _wrReader = null;
        }

        private void CheckExistingDataReader()
        {
            if (_wrReader != null && (SADataReader)_wrReader.Target != null)
                throw new InvalidOperationException(SARes.GetString(17931));
        }

        private static bool IsCreateTableStmt(string sql)
        {
            if (!string.IsNullOrEmpty(sql))
            {
                sql = sql.TrimStart();
                if (sql.StartsWith("CREATE ", StringComparison.InvariantCultureIgnoreCase) && sql.Substring(7).TrimStart().StartsWith("TABLE ", StringComparison.InvariantCultureIgnoreCase))
                    return true;
            }
            return false;
        }

        private static string ModifyCreateTableStmt(string sql)
        {
            string format = "({0}(\\s)*\\((\\s)*-(\\s)*1(\\s)*\\))";
            string[] strArray1 = new string[3] { "nvarchar", "varbinary", "varchar" };
            string[] strArray2 = new string[3] { "long nvarchar", "long binary", "long varchar" };
            for (int index = 0; index < strArray1.Length; ++index)
                sql = Regex.Replace(sql, string.Format(format, strArray1[index]), strArray2[index], RegexOptions.IgnoreCase);
            return sql;
        }

        /// <summary>
        ///     <para>Resets the CommandTimeout property to its default value of 30 seconds.</para>
        /// </summary>
        public void ResetCommandTimeout()
        {
            try
            {
                SATrace.FunctionScopeEnter("<sa.SACommand.ResetCommandTimeout|API>", _objectId, new string[0]);
                _timeout = 30;
            }
            finally
            {
                SATrace.FunctionScopeLeave();
            }
        }

        /// <summary>
        ///     <para>Creates a new instance of a <see cref="T:System.Data.Common.DbParameter" /> object.</para>
        /// </summary>
        /// <returns>
        /// <para>A <see cref="T:System.Data.Common.DbParameter" /> object.</para>
        ///    </returns>
        protected override DbParameter CreateDbParameter()
        {
            try
            {
                SATrace.FunctionScopeEnter("<sa.SACommand.CreateDbParameter|API>", _objectId, new string[0]);
                return new SAParameter();
            }
            finally
            {
                SATrace.FunctionScopeLeave();
            }
        }

        /// <summary>
        ///     <para>Provides an SAParameter object for supplying parameters to SACommand objects.</para>
        /// </summary>
        /// <remarks>
        ///     <para>Stored procedures and some other SQL statements can take parameters, indicated in the text of a statement by a question mark (?).</para>
        ///     <para>The CreateParameter method provides an SAParameter object. You can set properties on the SAParameter to specify the value, data type, and so on for the parameter.</para>
        /// </remarks>
        /// <returns>
        /// <para>A new parameter, as an SAParameter object.</para>
        ///    </returns>
        /// <seealso cref="T:iAnywhere.Data.SQLAnywhere.SAParameter" />
        public SAParameter CreateParameter()
        {
            try
            {
                SATrace.FunctionScopeEnter("<sa.SACommand.CreateParameter|API>", _objectId, new string[0]);
                return (SAParameter)CreateDbParameter();
            }
            finally
            {
                SATrace.FunctionScopeLeave();
            }
        }

        /// <summary>
        ///     <para>Executes the command text against the connection.</para>
        /// </summary>
        /// <param name="behavior">
        ///     An instance of <see cref="T:System.Data.CommandBehavior" />.
        /// </param>
        /// <returns>
        /// <para>A <see cref="T:System.Data.Common.DbDataReader" />.</para>
        ///    </returns>
        protected override DbDataReader ExecuteDbDataReader(CommandBehavior behavior)
        {
            try
            {
                SATrace.FunctionScopeEnter("<sa.SACommand.ExecuteDbDataReader|API>", _objectId, "behaviour");
                return _ExecuteReader(behavior, false, false);
            }
            finally
            {
                SATrace.FunctionScopeLeave();
            }
        }

        /// <summary>
        ///     <para>Executes a SQL statement that returns a result set.</para>
        /// </summary>
        /// <remarks>
        ///     <para>The statement is the current SACommand object, with CommandText and Parameters as needed. The SADataReader object is a read-only, forward-only result set. For modifiable result sets, use an SADataAdapter.</para>
        /// </remarks>
        /// <returns>
        /// <para>The result set as an SADataReader object.</para>
        ///    </returns>
        /// <seealso cref="M:iAnywhere.Data.SQLAnywhere.SACommand.ExecuteNonQuery" />
        /// <seealso cref="T:iAnywhere.Data.SQLAnywhere.SADataReader" />
        /// <seealso cref="T:iAnywhere.Data.SQLAnywhere.SADataAdapter" />
        /// <seealso cref="P:iAnywhere.Data.SQLAnywhere.SACommand.CommandText" />
        /// <seealso cref="P:iAnywhere.Data.SQLAnywhere.SACommand.Parameters" />
        public SADataReader ExecuteReader()
        {
            try
            {
                SATrace.FunctionScopeEnter("<sa.SACommand.ExecuteReader|API>", _objectId, new string[0]);
                return _ExecuteReader(CommandBehavior.Default, false, false);
            }
            finally
            {
                SATrace.FunctionScopeLeave();
            }
        }

        /// <summary>
        ///     <para>Executes a SQL statement that returns a result set.</para>
        /// </summary>
        /// <remarks>
        ///     <para>The statement is the current SACommand object, with CommandText and Parameters as needed. The SADataReader object is a read-only, forward-only result set. For modifiable result sets, use an SADataAdapter.</para>
        /// </remarks>
        /// <param name="behavior">
        ///     One of CloseConnection, Default, KeyInfo, SchemaOnly, SequentialAccess, SingleResult, or SingleRow.
        ///     <para>For more information about this parameter, see the .NET Framework documentation for CommandBehavior Enumeration.</para>
        /// </param>
        /// <returns>
        /// <para>The result set as an SADataReader object.</para>
        ///    </returns>
        /// <seealso cref="M:iAnywhere.Data.SQLAnywhere.SACommand.ExecuteNonQuery" />
        /// <seealso cref="T:iAnywhere.Data.SQLAnywhere.SADataReader" />
        /// <seealso cref="T:iAnywhere.Data.SQLAnywhere.SADataAdapter" />
        /// <seealso cref="P:iAnywhere.Data.SQLAnywhere.SACommand.CommandText" />
        /// <seealso cref="P:iAnywhere.Data.SQLAnywhere.SACommand.Parameters" />
        public SADataReader ExecuteReader(CommandBehavior behavior)
        {
            try
            {
                SATrace.FunctionScopeEnter("<sa.SACommand.ExecuteReader|API>", _objectId, "behavior");
                return (SADataReader)ExecuteDbDataReader(behavior);
            }
            finally
            {
                SATrace.FunctionScopeLeave();
            }
        }

        /// <summary>
        ///     <para>Initiates the asynchronous execution of a SQL statement or stored procedure that is described by this SACommand, and retrieves one or more result sets from the database server.</para>
        /// </summary>
        /// <returns>
        /// <para>An <see cref="T:System.IAsyncResult" /> that can be used to poll, wait for results, or both; this value is also needed when invoking EndExecuteReader(IAsyncResult), which returns an SADataReader object that can be used to retrieve the returned rows.</para>
        ///    </returns>
        /// <exception cref="T:iAnywhere.Data.SQLAnywhere.SAException">
        ///     <para>Any error that occurred while executing the command text.</para>
        /// </exception>
        /// <seealso cref="M:iAnywhere.Data.SQLAnywhere.SACommand.EndExecuteReader(System.IAsyncResult)" />
        /// <seealso cref="T:iAnywhere.Data.SQLAnywhere.SADataReader" />
        /// <remarks>
        ///     <para>For asynchronous command, the order of parameters must be consistent with CommandText.</para>
        /// </remarks>
        public IAsyncResult BeginExecuteReader()
        {
            try
            {
                SATrace.FunctionScopeEnter("<sa.SACommand.BeginExecuteReader|API>", _objectId, new string[0]);
                return BeginExecuteReader(null, null, CommandBehavior.Default);
            }
            finally
            {
                SATrace.FunctionScopeLeave();
            }
        }

        /// <summary>
        ///     <para>Initiates the asynchronous execution of a SQL statement or stored procedure that is described by this SACommand, and retrieves one or more result sets from the server.</para>
        /// </summary>
        /// <param name="behavior">
        ///     A bitwise combination of <see cref="T:System.Data.CommandBehavior" /> flags describing the results of the query and its effect on the connection.
        /// </param>
        /// <returns>
        /// <para>An <see cref="T:System.IAsyncResult" /> that can be used to poll, wait for results, or both; this value is also needed when invoking EndExecuteReader(IAsyncResult), which returns an SADataReader object that can be used to retrieve the returned rows.</para>
        ///    </returns>
        /// <exception cref="T:iAnywhere.Data.SQLAnywhere.SAException">
        ///     <para>Any error that occurred while executing the command text.</para>
        /// </exception>
        /// <seealso cref="M:iAnywhere.Data.SQLAnywhere.SACommand.EndExecuteReader(System.IAsyncResult)" />
        /// <seealso cref="T:iAnywhere.Data.SQLAnywhere.SADataReader" />
        /// <remarks>
        ///     <para>For asynchronous command, the order of parameters must be consistent with CommandText.</para>
        /// </remarks>
        public IAsyncResult BeginExecuteReader(CommandBehavior behavior)
        {
            try
            {
                SATrace.FunctionScopeEnter("<sa.SACommand.BeginExecuteReader|API>", _objectId, "behavior");
                return BeginExecuteReader(null, null, behavior);
            }
            finally
            {
                SATrace.FunctionScopeLeave();
            }
        }

        /// <summary>
        ///     <para>Initiates the asynchronous execution of a SQL statement that is described by the SACommand object, and retrieves the result set, given a callback procedure and state information.</para>
        /// </summary>
        /// <param name="callback">
        ///     An <see cref="T:System.AsyncCallback" /> delegate that is invoked when the command's execution has completed. Pass null (Nothing in Microsoft Visual Basic) to indicate that no callback is required.
        /// </param>
        /// <param name="stateObject">
        ///     A user-defined state object that is passed to the callback procedure. Retrieve this object from within the callback procedure using the <see cref="P:System.IAsyncResult.AsyncState" /> property.
        /// </param>
        /// <returns>
        /// <para>An <see cref="T:System.IAsyncResult" /> that can be used to poll, wait for results, or both; this value is also needed when invoking EndExecuteReader(IAsyncResult), which returns an SADataReader object that can be used to retrieve the returned rows.</para>
        ///    </returns>
        /// <exception cref="T:iAnywhere.Data.SQLAnywhere.SAException">
        ///     <para>Any error that occurred while executing the command text.</para>
        /// </exception>
        /// <seealso cref="M:iAnywhere.Data.SQLAnywhere.SACommand.EndExecuteReader(System.IAsyncResult)" />
        /// <seealso cref="T:iAnywhere.Data.SQLAnywhere.SADataReader" />
        /// <remarks>
        ///     <para>For asynchronous command, the order of parameters must be consistent with CommandText.</para>
        /// </remarks>
        public IAsyncResult BeginExecuteReader(AsyncCallback callback, object stateObject)
        {
            try
            {
                SATrace.FunctionScopeEnter("<sa.SACommand.BeginExecuteReader|API>", _objectId, "callback", "stateObject");
                return BeginExecuteReader(callback, stateObject, CommandBehavior.Default);
            }
            finally
            {
                SATrace.FunctionScopeLeave();
            }
        }

        /// <summary>
        ///     <para>Initiates the asynchronous execution of a SQL statement or stored procedure that is described by this SACommand, and retrieves one or more result sets from the server.</para>
        /// </summary>
        /// <param name="callback">
        ///     An <see cref="T:System.AsyncCallback" /> delegate that is invoked when the command's execution has completed. Pass null (Nothing in Microsoft Visual Basic) to indicate that no callback is required.
        /// </param>
        /// <param name="stateObject">
        ///     A user-defined state object that is passed to the callback procedure. Retrieve this object from within the callback procedure using the <see cref="P:System.IAsyncResult.AsyncState" /> property.
        /// </param>
        /// <param name="behavior">
        ///     A bitwise combination of <see cref="T:System.Data.CommandBehavior" /> flags describing the results of the query and its effect on the connection.
        /// </param>
        /// <returns>
        /// <para>An <see cref="T:System.IAsyncResult" /> that can be used to poll, wait for results, or both; this value is also needed when invoking EndExecuteReader(IAsyncResult), which returns an SADataReader object that can be used to retrieve the returned rows.</para>
        ///    </returns>
        /// <exception cref="T:iAnywhere.Data.SQLAnywhere.SAException">
        ///     <para>Any error that occurred while executing the command text.</para>
        /// </exception>
        /// <seealso cref="M:iAnywhere.Data.SQLAnywhere.SACommand.EndExecuteReader(System.IAsyncResult)" />
        /// <seealso cref="T:iAnywhere.Data.SQLAnywhere.SADataReader" />
        /// <remarks>
        ///     <para>For asynchronous command, the order of parameters must be consistent with CommandText.</para>
        /// </remarks>
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

        /// <summary>
        ///     <para>Finishes asynchronous execution of a SQL statement or stored procedure, returning the requested SADataReader.</para>
        /// </summary>
        /// <remarks>
        ///             <para>You must call EndExecuteReader once for every call to BeginExecuteReader. The call must be after BeginExecuteReader has returned. ADO.NET is not thread safe; it is your responsibility to ensure that BeginExecuteReader has returned. The IAsyncResult passed to EndExecuteReader must be the same as the one returned from the BeginExecuteReader call that is being completed. It is an error to call EndExecuteReader to end a call to BeginExecuteNonQuery, and vice versa.</para>
        ///             <para>If an error occurs while executing the command, the exception is thrown when EndExecuteReader is called.</para>
        ///             <para>There are four ways to wait for execution to complete:</para>
        ///             <para>(1) Call EndExecuteReader.</para>
        /// 	    <para>Calling EndExecuteReader blocks until the command completes. For example:</para>
        ///             <code>SAConnection conn = new SAConnection("DSN=SQL Anywhere 11 Demo");
        /// conn.Open();
        /// SACommand cmd = new SACommand( "SELECT * FROM Departments",
        ///   conn );
        /// IAsyncResult res = cmd.BeginExecuteReader();
        /// // perform other work
        /// // this will block until the command completes
        /// SADataReader reader = cmd.EndExecuteReader( res );</code>
        ///             <para>(2) Poll the IsCompleted property of the IAsyncResult.</para>
        /// 	    <para> You can poll the IsCompleted property of the IAsyncResult. For example:</para>
        ///             <code>SAConnection conn = new SAConnection("DSN=SQL Anywhere 11 Demo");
        /// conn.Open();
        /// SACommand cmd = new SACommand( "SELECT * FROM Departments",
        ///   conn );
        /// IAsyncResult res = cmd.BeginExecuteReader();
        /// while( !res.IsCompleted ) {
        /// // do other work
        /// }
        /// // this will not block because the command is finished
        /// SADataReader reader = cmd.EndExecuteReader( res );</code>
        ///             <para>(3) Use the IAsyncResult.AsyncWaitHandle property to get a synchronization object.</para>
        /// 	    <para>You can use the IAsyncResult.AsyncWaitHandle property to get a synchronization object, and wait on that. For example:</para>
        ///             <code>SAConnection conn = new SAConnection("DSN=SQL Anywhere 11 Demo");
        /// conn.Open();
        /// SACommand cmd = new SACommand( "SELECT * FROM Departments",
        ///   conn );
        /// IAsyncResult res = cmd.BeginExecuteReader();
        /// // perform other work
        /// WaitHandle wh = res.AsyncWaitHandle;
        /// wh.WaitOne();
        /// // this will not block because the command is finished
        /// SADataReader reader = cmd.EndExecuteReader( res );</code>
        ///             <para>(4) Specify a callback function when calling BeginExecuteReader</para>
        /// 	    <para>You can specify a callback function when calling BeginExecuteReader. For example:</para>
        ///             <code>private void callbackFunction( IAsyncResult ar )
        /// {
        ///    SACommand cmd = (SACommand) ar.AsyncState;
        ///    // this won’t block since the command has completed
        ///             SADataReader reader = cmd.EndExecuteReader();
        /// }
        /// 
        /// // elsewhere in the code
        /// private void DoStuff()
        /// {
        ///       SAConnection conn = new SAConnection("DSN=SQL Anywhere 11 Demo");
        ///       conn.Open();
        ///             SACommand cmd = new SACommand( "SELECT * FROM Departments",
        ///         conn );
        ///    IAsyncResult res = cmd.BeginExecuteReader( callbackFunction, cmd );
        ///    // perform other work.  The callback function will be
        ///    // called when the command completes
        /// }</code>
        ///             <para>The callback function executes in a separate thread, so the usual caveats related to updating the user interface in a threaded program apply.</para>
        /// 
        /// 
        /// 
        /// 
        ///         </remarks>
        /// <param name="asyncResult">
        ///     The IAsyncResult returned by the call to SACommand.BeginExecuteReader.
        /// </param>
        /// <returns>
        /// <para>An SADataReader object that can be used to retrieve the requested rows (the same behavior as SACommand.ExecuteReader).</para>
        ///    </returns>
        /// <exception cref="T:System.ArgumentException">
        ///     <para>The asyncResult parameter is null (Nothing in Microsoft Visual Basic)</para>
        /// </exception>
        /// <exception cref="T:System.InvalidOperationException">
        ///     <para>The SACommand.EndExecuteReader(IAsyncResult) was called more than once for a single command execution, or the method was mismatched against its execution method.</para>
        /// </exception>
        /// <seealso cref="M:iAnywhere.Data.SQLAnywhere.SACommand.BeginExecuteReader" />
        /// <seealso cref="T:iAnywhere.Data.SQLAnywhere.SADataReader" />
        public unsafe SADataReader EndExecuteReader(IAsyncResult asyncResult)
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
                    _parms.FreeParameterInfo(_currentAsyncResult.ParmCount, (SAParameterDM*)_currentAsyncResult.ParmsDM.ToPointer());
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

        private void ResetAsyncCommand()
        {
            if (_conn == null || _conn.AsyncCommand != this)
                return;
            _conn.AsyncCommand = null;
        }

        internal SADataReader ExecuteReaderForSchema()
        {
            int count = _parms.Count;
            Exception e = null;
            SADataReader saDataReader = null;
            try
            {
                _getOutputParms = false;
                if (count == 0)
                {
                    if (CommandType == CommandType.StoredProcedure)
                    {
                        SACommandBuilder.DeriveParameters(this);
                    }
                    else
                    {
                        for (int index = 0; index < 50; ++index)
                            _parms.Add("p" + index.ToString(), SADbType.Char);
                    }
                    foreach (DbParameter parm in (DbParameterCollection)_parms)
                        parm.Value = DBNull.Value;
                }
                saDataReader = _ExecuteReader(CommandBehavior.SchemaOnly, false, false);
            }
            catch (Exception ex)
            {
                e = ex;
            }
            finally
            {
                _getOutputParms = true;
                if (count == 0)
                    _parms.Clear();
            }
            if (e != null)
            {
                throw e;
            }
            return saDataReader;
        }

        /// <summary>
        ///     <para>Prepares or compiles the SACommand on the data source.</para>
        /// </summary>
        /// <remarks>
        ///     <para>If you call one of the ExecuteNonQuery, ExecuteReader, or ExecuteScalar methods after calling Prepare, any parameter value that is larger than the value specified by the Size property is automatically truncated to the original specified size of the parameter, and no truncation errors are returned.</para>
        ///     <para>The truncation only happens for the following data types:</para>
        ///     <list>
        ///     <item>
        ///     <term>CHAR</term>
        ///     </item>
        ///     <item>
        ///     <term>VARCHAR</term>
        ///     </item>
        ///     <item>
        ///     <term>LONG VARCHAR</term>
        ///     </item>
        ///     <item>
        ///     <term>TEXT</term>
        ///     </item>
        ///     <item>
        ///     <term>NCHAR</term>
        ///     </item>
        ///     <item>
        ///     <term>NVARCHAR</term>
        ///     </item>
        ///     <item>
        ///     <term>LONG NVARCHAR</term>
        ///     </item>
        ///     <item>
        ///     <term>NTEXT</term>
        ///     </item>
        ///     <item>
        ///     <term>BINARY</term>
        ///     </item>
        ///     <item>
        ///     <term>LONG BINARY</term>
        ///     </item>
        ///     <item>
        ///     <term>VARBINARY</term>
        ///     </item>
        ///     <item>
        ///     <term>IMAGE</term>
        ///     </item>
        ///     </list>
        ///     <para>If the size property is not specified, and so is using the default value, the data is not truncated.</para>
        /// </remarks>
        /// <seealso cref="M:iAnywhere.Data.SQLAnywhere.SACommand.ExecuteNonQuery" />
        /// <seealso cref="M:iAnywhere.Data.SQLAnywhere.SACommand.ExecuteReader" />
        /// <seealso cref="M:iAnywhere.Data.SQLAnywhere.SACommand.ExecuteScalar" />
        public override void Prepare()
        {
            _exeMethodName = "Prepare";
            CheckAlreadyExecuting();
            CheckExistingDataReader();
            Validate();
            _Prepare();
            _isExecuting = false;
        }

        private unsafe void _Prepare()
        {
            int count = 0;
            string sqlCommand = GetSQLCommand();
            char[] arParmNames1 = new char[2048];
            char[] arParmNames2 = new char[2048];
            char[] arParmNames3 = new char[2048];
            SAParameterDM* pParmsDM = (SAParameterDM*)null;
            try
            {
                _isPrepared = false;
                FreeCommand(true);
                _parms.GetParameterInfo(out count, &pParmsDM, false, false, null);
                fixed (char* chPtr1 = arParmNames1)
                  fixed (char* chPtr2 = arParmNames2)
                    fixed (char* chPtr3 = arParmNames3)
                      SAException.CheckException(PInvokeMethods.AsaCommand_Prepare(ref _idCmd, _conn.InternalConnection.ConnectionId, sqlCommand, count, new IntPtr((void*)pParmsDM), ref _namedParms, new IntPtr((void*)chPtr1), new IntPtr((void*)chPtr2), new IntPtr((void*)chPtr3)));
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

        private void Unprepare()
        {
            _namedParms = false;
            _allParmNames = new string[0];
            _inParmNames = new string[0];
            _outParmNames = new string[0];
            FreeCommand(true);
            _isPrepared = false;
        }

        private string[] GetParameterNames(char[] arParmNames)
        {
            string str1 = new string(arParmNames);
            string str2 = str1.Substring(0, str1.IndexOf(char.MinValue));
            if (str2.Length <= 0)
                return new string[0];
            return str2.Split('\t');
        }

        private unsafe SADataReader _ExecuteReader(CommandBehavior commandBehavior, bool isExecuteScalar, bool isBeginExecuteReader)
        {
            int idEx = 0;
            int idReader = 0;
            int count1 = 0;
            SAParameterDM* pParmsDM = (SAParameterDM*)null;
            int count2 = 0;
            SAValue* pValues = (SAValue*)null;
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
                    _parms.GetParameterInfo(out count1, &pParmsDM, true, _namedParms, _allParmNames);
                    _currentAsyncResult.ParmCount = count1;
                    _currentAsyncResult.ParmsDM = new IntPtr((void*)pParmsDM);
                    _currentAsyncResult.Behavior = commandBehavior;
                    Unprepare();
                    idEx = PInvokeMethods.AsaCommand_BeginExecuteReaderDirect(ref _idCmd, _conn.InternalConnection.ConnectionId, GetSQLCommand(), count1, new IntPtr((void*)pParmsDM), Marshal.GetFunctionPointerForDelegate((Delegate)_asyncCallback));
                }
                else
                {
                    if (!_isPrepared || ParameterChanged())
                        _Prepare();
                    _parms.GetInputParameterValues(out count2, &pValues, _allParmNames, _inParmNames, _namedParms);
                    bool flag = true;
                    while (flag)
                    {
                        idEx = PInvokeMethods.AsaCommand_ExecuteReader(_idCmd, count2, new IntPtr((void*)pValues), ref outputParmCount, ref outputParmValues, ref idReader, ref _recordsAffected);
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
                SAParameter saParameter = new SAParameter(parm.ParameterName, parm.SADbType, parm.Size, parm.Direction, parm.IsNullable, parm.Precision, parm.Scale, parm.SourceColumn, parm.SourceVersion, null);
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

        /// <summary>
        ///     <para>Executes a statement that does not return a result set, such as an INSERT, UPDATE, DELETE, or data definition statement.</para>
        /// </summary>
        /// <remarks>
        ///     <para>You can use ExecuteNonQuery to change the data in a database without using a DataSet. Do this by executing UPDATE, INSERT, or DELETE statements.</para>
        ///     <para>Although ExecuteNonQuery does not return any rows, output parameters or return values that are mapped to parameters are populated with data.</para>
        ///     <para>For UPDATE, INSERT, and DELETE statements, the return value is the number of rows affected by the command. For all other types of statements, and for rollbacks, the return value is -1.</para>
        /// </remarks>
        /// <returns>
        /// <para>The number of rows affected.</para>
        ///    </returns>
        /// <seealso cref="M:iAnywhere.Data.SQLAnywhere.SACommand.ExecuteReader" />
        public override unsafe int ExecuteNonQuery()
        {
            try
            {
                int idEx = 0;
                int count1 = 0;
                SAValue* pValues = (SAValue*)null;
                int outputParmCount = 0;
                IntPtr outputParmValues = IntPtr.Zero;
                bool flag1 = _currentAsyncResult != null;
                int count2 = 0;
                SAParameterDM* pParmsDM = (SAParameterDM*)null;
                _exeMethodName = !flag1 ? "ExecuteNonQuery" : "BeginExecuteNonQuery";
                CheckAlreadyExecuting();
                CheckExistingDataReader();
                Validate();
                VerifyParameterType();
                try
                {
                    if (flag1)
                    {
                        _parms.GetParameterInfo(out count2, &pParmsDM, true, _namedParms, _allParmNames);
                        _currentAsyncResult.ParmCount = count2;
                        _currentAsyncResult.ParmsDM = new IntPtr((void*)pParmsDM);
                        Unprepare();
                        idEx = PInvokeMethods.AsaCommand_BeginExecuteNonQueryDirect(ref _idCmd, _conn.InternalConnection.ConnectionId, GetSQLCommand(), count2, new IntPtr((void*)pParmsDM), Marshal.GetFunctionPointerForDelegate((Delegate)_asyncCallback));
                    }
                    else
                    {
                        if (!_isPrepared || ParameterChanged())
                            _Prepare();
                        _parms.GetInputParameterValues(out count1, &pValues, _allParmNames, _inParmNames, _namedParms);
                        bool flag2 = true;
                        while (flag2)
                        {
                            idEx = PInvokeMethods.AsaCommand_ExecuteNonQuery(_idCmd, count1, new IntPtr((void*)pValues), ref outputParmCount, ref outputParmValues, ref _recordsAffected);
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
            finally
            {
            }
        }

        /// <summary>
        ///     <para>Initiates the asynchronous execution of a SQL statement or stored procedure that is described by this SACommand.</para>
        /// </summary>
        /// <returns>
        /// <para>An <see cref="T:System.IAsyncResult" /> that can be used to poll, wait for results, or both; this value is also needed when invoking EndExecuteNonQuery(IAsyncResult), which returns the number of affected rows.</para>
        ///    </returns>
        /// <exception cref="T:iAnywhere.Data.SQLAnywhere.SAException">
        ///     <para>Any error that occurred while executing the command text.</para>
        /// </exception>
        /// <seealso cref="M:iAnywhere.Data.SQLAnywhere.SACommand.EndExecuteNonQuery(System.IAsyncResult)" />
        /// <remarks>
        ///     <para>For asynchronous command, the order of parameters must be consistent with CommandText.</para>
        /// </remarks>
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
        
        public unsafe int EndExecuteNonQuery(IAsyncResult asyncResult)
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
                    _parms.FreeParameterValues(asyncCommandResult.InputParmCount, (SAValue*)asyncCommandResult.InputParmValues.ToPointer());
                    _parms.FreeParameterInfo(asyncCommandResult.ParmCount, (SAParameterDM*)asyncCommandResult.ParmsDM.ToPointer());
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

        /// <summary>
        ///     <para>Executes a statement that returns a single value. If this method is called on a query that returns multiple rows and columns, only the first column of the first row is returned.</para>
        /// </summary>
        /// <returns>
        /// <para>The first column of the first row in the result set, or a null reference if the result set is empty.</para>
        ///    </returns>
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
                    saDataReader.Close();
                }
                return obj;
            }
            finally
            {
                SATrace.FunctionScopeLeave();
            }
        }

        /// <summary>
        ///     <para>Cancels the execution of an SACommand object.</para>
        /// </summary>
        /// <remarks>
        ///     <para>If there is nothing to cancel, nothing happens. If there is a command in process, a "Statement interrupted by user" exception is thrown.</para>
        /// </remarks>
        public override void Cancel()
        {
            try
            {
                SATrace.FunctionScopeEnter("<sa.SACommand.Cancel|API>", _objectId, new string[0]);
                if (_isExecuting)
                {
                    _isExecuting = false;
                    if (_idCmd < 0)
                        return;
                    SAException.CheckException(PInvokeMethods.AsaCommand_Cancel(_idCmd));
                }
                else
                {
                    if (DataAdapter == null)
                        return;
                    DataAdapter.CancelFill();
                }
            }
            finally
            {
                ResetAsyncCommand();
                SATrace.FunctionScopeLeave();
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
                SATrace.Exception(e);
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
                SATrace.Exception(e);
            }
            else if (_conn.GetConnectionState() != ConnectionState.Open)
            {
                e = new InvalidOperationException(SARes.GetString(10993, _exeMethodName));
                SATrace.Exception(e);
            }
            else
            {
                if (_asaTran == null || !_asaTran.IsValid)
                {
                    if (_conn.Transaction != null)
                    {
                        e = new InvalidOperationException(SARes.GetString(11001));
                        SATrace.Exception(e);
                        goto label_16;
                    }
                }
                else if (_conn != _asaTran.Connection)
                {
                    e = new InvalidOperationException(SARes.GetString(10992));
                    SATrace.Exception(e);
                    goto label_16;
                }
                if (_cmdText == null || _cmdText.Trim().Length < 1)
                {
                    e = new InvalidOperationException(SARes.GetString(10986, _exeMethodName + ": CommandText"));
                    SATrace.Exception(e);
                }
                else
                {
                    for (int index = 0; index < Parameters.Count; ++index)
                    {
                        SAParameter saParameter = Parameters[index];
                        if (saParameter.Size == 0 && (saParameter.Direction == ParameterDirection.Output || saParameter.Direction == ParameterDirection.InputOutput) && (saParameter.DbType == DbType.AnsiString || saParameter.DbType == DbType.AnsiStringFixedLength || (saParameter.DbType == DbType.String || saParameter.DbType == DbType.StringFixedLength) || (saParameter.DbType == DbType.Binary || saParameter.DbType == DbType.Xml)))
                        {
                            e = new InvalidOperationException(SARes.GetString(17421, index.ToString()));
                            SATrace.Exception(e);
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

        private unsafe void GetParameterValues(int count, IntPtr pValues)
        {
            if (count <= 0 || _parms.GetOutputParameterCount() <= 0)
                return;
            if (_getOutputParms)
            {
                int index1 = 0;
                SAValue* saValuePtr = (SAValue*)(void*)pValues;
                if (_namedParms)
                {
                    for (int index2 = 0; index2 < _outParmNames.GetLength(0); ++index2)
                    {
                        for (int index3 = 0; index3 < _parms.Count; ++index3)
                        {
                            SAParameter saParameter = _parms[index3];
                            if (string.Compare(_outParmNames[index2], saParameter.ParameterName, true) == 0 && saParameter.IsOutputParameter())
                            {
                                saParameter.Value = SADataConvert.SAToDotNet(saValuePtr->Value, SADataConvert.MapToDotNetType(saParameter.SADbType), saParameter.Size, saParameter.Offset);
                                ++saValuePtr;
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
                        saParameter.Value = SADataConvert.SAToDotNet(saValuePtr->Value, SADataConvert.MapToDotNetType(saParameter.SADbType), saParameter.Size, saParameter.Offset);
                        ++saValuePtr;
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
                SATrace.Exception(e);
                throw e;
            }
        }

        private void CheckAsyncNotExecuting()
        {
            if (_currentAsyncResult != null)
            {
                Exception e = new InvalidOperationException(SARes.GetString(14973));
                SATrace.Exception(e);
                throw e;
            }
        }

        private void CheckAsyncResult(IAsyncResult asyncResult, AsyncCommandType type)
        {
            if (asyncResult == null)
            {
                Exception e = new ArgumentNullException("asyncResult");
                SATrace.Exception(e);
                throw e;
            }
            if (_currentAsyncResult == null)
            {
                Exception e = new InvalidOperationException(SARes.GetString(14974));
                SATrace.Exception(e);
                throw e;
            }
            if (_currentAsyncResult != asyncResult)
            {
                Exception e = new ArgumentException(SARes.GetString(14975), "asyncResult");
                SATrace.Exception(e);
                throw e;
            }
            _currentAsyncResult.CheckCommandType(type);
        }

        private delegate void AsyncCommandCallback();
    }
}
