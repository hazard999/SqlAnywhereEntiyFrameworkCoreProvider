
// Type: iAnywhere.Data.SQLAnywhere.SAConnectionStringBuilder
// Assembly: iAnywhere.Data.SQLAnywhere.v4.0, Version=11.0.1.27424, Culture=neutral, PublicKeyToken=f222fc4333e0d400
// MVID: CC4F9F8C-E618-49D1-9147-C06A9EF53D1F
// Assembly location: C:\Program Files\SQL Anywhere 11\Assembly\V4\iAnywhere.Data.SQLAnywhere.v4.0.dll

using System.Collections.Generic;
using System.ComponentModel;

namespace iAnywhere.Data.SQLAnywhere
{
  /// <summary>
  ///     <para>Provides a simple way to create and manage the contents of connection strings used by the SAConnection class.</para>
  /// </summary>
  /// <remarks>
  ///     <para>The SAConnectionStringBuilder class inherits SAConnectionStringBuilderBase, which inherits DbConnectionStringBuilder. </para>
  ///     <para><b>Restrictions:</b> The SAConnectionStringBuilder class is not available in the .NET Compact Framework 2.0.</para>
  ///     <para><b>Inherits:</b> <see cref="T:iAnywhere.Data.SQLAnywhere.SAConnectionStringBuilderBase" /></para>
  ///     <para>For a list of connection parameters, see @olink targetdoc="dbadmin" targetptr="conmean"@Connection parameters@/olink@.</para>
  /// </remarks>
  public sealed class SAConnectionStringBuilder : SAConnectionStringBuilderBase
  {
    private static Dictionary<string, ConnectionOptions> s_ConnOptions = new Dictionary<string, ConnectionOptions>();
    private const string c_UserIDKey = "UserID";
    private const string c_PasswordKey = "Password";
    private const string c_NewPasswordKey = "NewPassword";
    private const string c_DatabaseNameKey = "DatabaseName";
    private const string c_DatabaseFileKey = "DatabaseFile";
    private const string c_DatabaseSwitchesKey = "DatabaseSwitches";
    private const string c_ServerNameKey = "ServerName";
    private const string c_UnconditionalKey = "Unconditional";
    private const string c_StartLineKey = "StartLine";
    private const string c_ConnectionNameKey = "ConnectionName";
    private const string c_AutoStopKey = "AutoStop";
    private const string c_DataSourceNameKey = "DataSourceName";
    private const string c_IntegratedKey = "Integrated";
    private const string c_FileDataSourceNameKey = "FileDataSourceName";
    private const string c_EncryptedPasswordKey = "EncryptedPassword";
    private const string c_CommBufferSizeKey = "CommBufferSize";
    private const string c_EncryptionKey = "Encryption";
    private const string c_LivenessTimeoutKey = "LivenessTimeout";
    private const string c_LogFileKey = "LogFile";
    private const string c_DisableMultiRowFetchKey = "DisableMultiRowFetch";
    private const string c_CommLinksKey = "CommLinks";
    private const string c_AutoStartKey = "AutoStart";
    private const string c_CharsetKey = "Charset";
    private const string c_ForceStartKey = "ForceStart";
    private const string c_AppInfoKey = "AppInfo";
    private const string c_PrefetchRowsKey = "PrefetchRows";
    private const string c_PrefetchBufferKey = "PrefetchBuffer";
    private const string c_DatabaseKeyKey = "DatabaseKey";
    private const string c_CompressKey = "Compress";
    private const string c_CompressionThresholdKey = "CompressionThreshold";
    private const string c_IdleTimeoutKey = "IdleTimeout";
    private const string c_LanguageKey = "Language";
    private const string c_LazyCloseKey = "LazyClose";
    private const string c_RetryConnectionTimeoutKey = "RetryConnectionTimeout";
    private const string c_KerberosKey = "Kerberos";
    private const string c_ElevateKey = "Elevate";
    private const string c_ConnectionTOKey = "Connection Timeout";
    private const string c_ConnectionLifetimeKey = "Connection Lifetime";
    private const string c_ConnectionResetKey = "Connection Reset";
    private const string c_EnlistKey = "Enlist";
    private const string c_MaxPoolSizeKey = "Max Pool Size";
    private const string c_MinPoolSizeKey = "Min Pool Size";
    private const string c_PersistSecInfoKey = "Persist Security Info";
    private const string c_PoolingKey = "Pooling";
    private const string c_InitStringKey = "InitString";

    /// <summary>
    ///     <para>Gets or sets the CommLinks property.</para>
    /// </summary>
    [Category("Network Protocol")]
    [RefreshProperties(RefreshProperties.All)]
    [DisplayName("CommLinks")]
    public string CommLinks
    {
      get
      {
        return (string)GetPropertyValue("CommLinks");
      }
      set
      {
        this["CommLinks"] = value;
      }
    }

    /// <summary>
    ///     <para>Gets or sets the UserID connection property.</para>
    /// </summary>
    [Category("Security")]
    [DisplayName("UserID")]
    [RefreshProperties(RefreshProperties.All)]
    public string UserID
    {
      get
      {
        return (string)GetPropertyValue("UserID");
      }
      set
      {
        this["UserID"] = value;
      }
    }

    /// <summary>
    ///     <para>Gets or sets the Password connection property.</para>
    /// </summary>
    [Category("Security")]
    [PasswordPropertyText(true)]
    [DisplayName("Password")]
    [RefreshProperties(RefreshProperties.All)]
    public string Password
    {
      get
      {
        return (string)GetPropertyValue("Password");
      }
      set
      {
        this["Password"] = value;
      }
    }

    /// <summary>
    ///     <para>Gets or sets the NewPassword connection property.</para>
    /// </summary>
    [Category("Security")]
    [PasswordPropertyText(true)]
    [DisplayName("NewPassword")]
    [RefreshProperties(RefreshProperties.All)]
    public string NewPassword
    {
      get
      {
        return (string)GetPropertyValue("NewPassword");
      }
      set
      {
        this["NewPassword"] = value;
      }
    }

    /// <summary>
    ///     <para>Gets or sets the DatabaseName connection property.</para>
    /// </summary>
    [RefreshProperties(RefreshProperties.All)]
    [DisplayName("DatabaseName")]
    [Category("Source")]
    public string DatabaseName
    {
      get
      {
        return (string)GetPropertyValue("DatabaseName");
      }
      set
      {
        this["DatabaseName"] = value;
      }
    }

    /// <summary>
    ///     <para>Gets or sets the DatabaseFile connection property.</para>
    /// </summary>
    [Category("Source")]
    [RefreshProperties(RefreshProperties.All)]
    [DisplayName("DatabaseFile")]
    public string DatabaseFile
    {
      get
      {
        return (string)GetPropertyValue("DatabaseFile");
      }
      set
      {
        this["DatabaseFile"] = value;
      }
    }

    /// <summary>
    ///     <para>Gets or sets the DatabaseSwitches connection property.</para>
    /// </summary>
    [RefreshProperties(RefreshProperties.All)]
    [Category("Source")]
    [DisplayName("DatabaseSwitches")]
    public string DatabaseSwitches
    {
      get
      {
        return (string)GetPropertyValue("DatabaseSwitches");
      }
      set
      {
        this["DatabaseSwitches"] = value;
      }
    }

    /// <summary>
    ///     <para>Gets or sets the ServerName connection property.</para>
    /// </summary>
    [Category("Source")]
    [RefreshProperties(RefreshProperties.All)]
    [DisplayName("ServerName")]
    public string ServerName
    {
      get
      {
        return (string)GetPropertyValue("ServerName");
      }
      set
      {
        this["ServerName"] = value;
      }
    }

    /// <summary>
    ///     <para>Gets or sets the Unconditional connection property.</para>
    /// </summary>
    [DisplayName("Unconditional")]
    [RefreshProperties(RefreshProperties.All)]
    [Category("Advanced")]
    public string Unconditional
    {
      get
      {
        return (string)GetPropertyValue("Unconditional");
      }
      set
      {
        this["Unconditional"] = value;
      }
    }

    /// <summary>
    ///     <para>Gets or sets the StartLine connection property.</para>
    /// </summary>
    [RefreshProperties(RefreshProperties.All)]
    [DisplayName("StartLine")]
    [Category("Initialization")]
    public string StartLine
    {
      get
      {
        return (string)GetPropertyValue("StartLine");
      }
      set
      {
        this["StartLine"] = value;
      }
    }

    /// <summary>
    ///     <para>Gets or sets the ConnectionName connection property.</para>
    /// </summary>
    [Category("Advanced")]
    [RefreshProperties(RefreshProperties.All)]
    [DisplayName("ConnectionName")]
    public string ConnectionName
    {
      get
      {
        return (string)GetPropertyValue("ConnectionName");
      }
      set
      {
        this["ConnectionName"] = value;
      }
    }

    /// <summary>
    ///     <para>Gets or sets the AutoStop connection property.</para>
    /// </summary>
    [DisplayName("AutoStop")]
    [Category("Advanced")]
    [RefreshProperties(RefreshProperties.All)]
    public string AutoStop
    {
      get
      {
        return (string)GetPropertyValue("AutoStop");
      }
      set
      {
        this["AutoStop"] = value;
      }
    }

    /// <summary>
    ///     <para>Gets or sets the DataSourceName connection property.</para>
    /// </summary>
    [DisplayName("DataSourceName")]
    [RefreshProperties(RefreshProperties.All)]
    [Category("Source")]
    public string DataSourceName
    {
      get
      {
        return (string)GetPropertyValue("DataSourceName");
      }
      set
      {
        this["DataSourceName"] = value;
      }
    }

    /// <summary>
    ///     <para>Gets or sets the Integrated connection property.</para>
    /// </summary>
    [DisplayName("Integrated")]
    [RefreshProperties(RefreshProperties.All)]
    [Category("Security")]
    public string Integrated
    {
      get
      {
        return (string)GetPropertyValue("Integrated");
      }
      set
      {
        this["Integrated"] = value;
      }
    }

    /// <summary>
    ///     <para>Gets or sets the FileDataSourceName connection property.</para>
    /// </summary>
    [RefreshProperties(RefreshProperties.All)]
    [DisplayName("FileDataSourceName")]
    [Category("Source")]
    public string FileDataSourceName
    {
      get
      {
        return (string)GetPropertyValue("FileDataSourceName");
      }
      set
      {
        this["FileDataSourceName"] = value;
      }
    }

    /// <summary>
    ///     <para>Gets or sets the EncryptedPassword connection property.</para>
    /// </summary>
    [RefreshProperties(RefreshProperties.All)]
    [Category("Security")]
    [PasswordPropertyText(true)]
    [DisplayName("EncryptedPassword")]
    public string EncryptedPassword
    {
      get
      {
        return (string)GetPropertyValue("EncryptedPassword");
      }
      set
      {
        this["EncryptedPassword"] = value;
      }
    }

    /// <summary>
    ///     <para>Gets or sets the CommBufferSize connection property.</para>
    /// </summary>
    [DisplayName("CommBufferSize")]
    [RefreshProperties(RefreshProperties.All)]
    [Category("Network Protocol")]
    public int CommBufferSize
    {
      get
      {
        return (int)GetPropertyValue("CommBufferSize");
      }
      set
      {
        this["CommBufferSize"] = value;
      }
    }

    /// <summary>
    ///     <para>Gets or sets the Encryption connection property.</para>
    /// </summary>
    [DisplayName("Encryption")]
    [Category("Security")]
    [RefreshProperties(RefreshProperties.All)]
    public string Encryption
    {
      get
      {
        return (string)GetPropertyValue("Encryption");
      }
      set
      {
        this["Encryption"] = value;
      }
    }

    /// <summary>
    ///     <para>Gets or sets the LivenessTimeout connection property.</para>
    /// </summary>
    [Category("Advanced")]
    [RefreshProperties(RefreshProperties.All)]
    [DisplayName("LivenessTimeout")]
    public int LivenessTimeout
    {
      get
      {
        return (int)GetPropertyValue("LivenessTimeout");
      }
      set
      {
        this["LivenessTimeout"] = value;
      }
    }

    /// <summary>
    ///     <para>Gets or sets the LogFile connection property.</para>
    /// </summary>
    [RefreshProperties(RefreshProperties.All)]
    [Category("Advanced")]
    [DisplayName("LogFile")]
    public string LogFile
    {
      get
      {
        return (string)GetPropertyValue("LogFile");
      }
      set
      {
        this["LogFile"] = value;
      }
    }

    /// <summary>
    ///     <para>Gets or sets the DisableMultiRowFetch connection property.</para>
    /// </summary>
    [RefreshProperties(RefreshProperties.All)]
    [Category("Advanced")]
    [DisplayName("DisableMultiRowFetch")]
    public string DisableMultiRowFetch
    {
      get
      {
        return (string)GetPropertyValue("DisableMultiRowFetch");
      }
      set
      {
        this["DisableMultiRowFetch"] = value;
      }
    }

    /// <summary>
    ///     <para>Gets or sets the AutoStart connection property.</para>
    /// </summary>
    [Category("Advanced")]
    [RefreshProperties(RefreshProperties.All)]
    [DisplayName("AutoStart")]
    public string AutoStart
    {
      get
      {
        return (string)GetPropertyValue("AutoStart");
      }
      set
      {
        this["AutoStart"] = value;
      }
    }

    /// <summary>
    ///     <para>Gets or sets the Charset connection property.</para>
    /// </summary>
    [DisplayName("Charset")]
    [Category("Initialization")]
    [RefreshProperties(RefreshProperties.All)]
    public string Charset
    {
      get
      {
        return (string)GetPropertyValue("Charset");
      }
      set
      {
        this["Charset"] = value;
      }
    }

    /// <summary>
    ///     <para>Gets or sets the ForceStart connection property.</para>
    /// </summary>
    [Category("Advanced")]
    [DisplayName("ForceStart")]
    [RefreshProperties(RefreshProperties.All)]
    public string ForceStart
    {
      get
      {
        return (string)GetPropertyValue("ForceStart");
      }
      set
      {
        this["ForceStart"] = value;
      }
    }

    /// <summary>
    ///     <para>Gets or sets the AppInfo connection property.</para>
    /// </summary>
    [DisplayName("AppInfo")]
    [RefreshProperties(RefreshProperties.All)]
    [Category("Advanced")]
    public string AppInfo
    {
      get
      {
        return (string)GetPropertyValue("AppInfo");
      }
      set
      {
        this["AppInfo"] = value;
      }
    }

    /// <summary>
    ///     <para>Gets or sets the PrefetchRows connection property. The default value is 200.</para>
    /// </summary>
    /// <remarks>
    /// </remarks>
    [Category("Advanced")]
    [DisplayName("PrefetchRows")]
    [RefreshProperties(RefreshProperties.All)]
    public int PrefetchRows
    {
      get
      {
        return (int)GetPropertyValue("PrefetchRows");
      }
      set
      {
        this["PrefetchRows"] = value;
      }
    }

    /// <summary>
    ///     <para>Gets or sets the PrefetchBuffer connection property.</para>
    /// </summary>
    [DisplayName("PrefetchBuffer")]
    [Category("Advanced")]
    [RefreshProperties(RefreshProperties.All)]
    public int PrefetchBuffer
    {
      get
      {
        return (int)GetPropertyValue("PrefetchBuffer");
      }
      set
      {
        this["PrefetchBuffer"] = value;
      }
    }

    /// <summary>
    ///     <para>Gets or sets the DatabaseKey connection property.</para>
    /// </summary>
    [Category("Security")]
    [PasswordPropertyText(true)]
    [DisplayName("DatabaseKey")]
    [RefreshProperties(RefreshProperties.All)]
    public string DatabaseKey
    {
      get
      {
        return (string)GetPropertyValue("DatabaseKey");
      }
      set
      {
        this["DatabaseKey"] = value;
      }
    }

    /// <summary>
    ///     <para>Gets or sets the Compress connection property.</para>
    /// </summary>
    [RefreshProperties(RefreshProperties.All)]
    [Category("Advanced")]
    [DisplayName("Compress")]
    public string Compress
    {
      get
      {
        return (string)GetPropertyValue("Compress");
      }
      set
      {
        this["Compress"] = value;
      }
    }

    /// <summary>
    ///     <para>Gets or sets the CompressionThreshold connection property.</para>
    /// </summary>
    [RefreshProperties(RefreshProperties.All)]
    [DisplayName("CompressionThreshold")]
    [Category("Advanced")]
    public int CompressionThreshold
    {
      get
      {
        return (int)GetPropertyValue("CompressionThreshold");
      }
      set
      {
        this["CompressionThreshold"] = value;
      }
    }

    /// <summary>
    ///     <para>Gets or sets the IdleTimeout connection property.</para>
    /// </summary>
    [Category("Advanced")]
    [RefreshProperties(RefreshProperties.All)]
    [DisplayName("IdleTimeout")]
    public int IdleTimeout
    {
      get
      {
        return (int)GetPropertyValue("IdleTimeout");
      }
      set
      {
        this["IdleTimeout"] = value;
      }
    }

    /// <summary>
    ///     <para>Gets or sets the Language connection property.</para>
    /// </summary>
    [Category("Initialization")]
    [RefreshProperties(RefreshProperties.All)]
    [DisplayName("Language")]
    public string Language
    {
      get
      {
        return (string)GetPropertyValue("Language");
      }
      set
      {
        this["Language"] = value;
      }
    }

    /// <summary>
    ///     <para>Gets or sets the LazyClose connection property.</para>
    /// </summary>
    [DisplayName("LazyClose")]
    [Category("Advanced")]
    [RefreshProperties(RefreshProperties.All)]
    public string LazyClose
    {
      get
      {
        return (string)GetPropertyValue("LazyClose");
      }
      set
      {
        this["LazyClose"] = value;
      }
    }

    /// <summary>
    ///     <para>Gets or sets the RetryConnectionTimeout property.</para>
    /// </summary>
    [RefreshProperties(RefreshProperties.All)]
    [DisplayName("RetryConnectionTimeout")]
    [Category("Advanced")]
    public int RetryConnectionTimeout
    {
      get
      {
        return (int)GetPropertyValue("RetryConnectionTimeout");
      }
      set
      {
        this["RetryConnectionTimeout"] = value;
      }
    }

    /// <summary>
    ///     <para>Gets or sets the Kerberos connection property.</para>
    /// </summary>
    [DisplayName("Kerberos")]
    [Category("Network Protocol")]
    [RefreshProperties(RefreshProperties.All)]
    public string Kerberos
    {
      get
      {
        return (string)GetPropertyValue("Kerberos");
      }
      set
      {
        this["Kerberos"] = value;
      }
    }

    /// <summary>
    ///     <para>Gets or sets the Elevate connection property.</para>
    /// </summary>
    [Category("Advanced")]
    [DisplayName("Elevate")]
    [RefreshProperties(RefreshProperties.All)]
    public string Elevate
    {
      get
      {
        return (string)GetPropertyValue("Elevate");
      }
      set
      {
        this["Elevate"] = value;
      }
    }

    /// <summary>
    ///     <para>Gets or sets the ConnectionTimeout connection property.</para>
    /// </summary>
    /// <example>
    ///     <para>The following statement displays the value of the ConnectionTimeout property.</para>
    ///     <code>MessageBox.Show( connString.ConnectionTimeout.ToString() );</code>
    /// </example>
    [Category("Pooling")]
    [DisplayName("Connection Timeout")]
    [RefreshProperties(RefreshProperties.All)]
    public int ConnectionTimeout
    {
      get
      {
        return (int)GetPropertyValue("Connection Timeout");
      }
      set
      {
        this["Connection Timeout"] = value;
      }
    }

    /// <summary>
    ///     <para>Gets or sets the PersistSecurityInfo connection property.</para>
    /// </summary>
    [RefreshProperties(RefreshProperties.All)]
    [DisplayName("Persist Security Info")]
    [Category("Security")]
    public bool PersistSecurityInfo
    {
      get
      {
        return (bool)GetPropertyValue("Persist Security Info");
      }
      set
      {
        this["Persist Security Info"] = value;
      }
    }

    /// <summary>
    ///     <para>Gets or sets the ConnectionLifetime connection property.</para>
    /// </summary>
    [DisplayName("Connection Lifetime")]
    [RefreshProperties(RefreshProperties.All)]
    [Category("Pooling")]
    public int ConnectionLifetime
    {
      get
      {
        return (int)GetPropertyValue("Connection Lifetime");
      }
      set
      {
        this["Connection Lifetime"] = value;
      }
    }

    /// <summary>
    ///     <para>Gets or sets the ConnectionReset connection property.</para>
    /// </summary>
    /// <returns>
    /// <para>A DataTable that contains schema information.</para>
    ///    </returns>
    [DisplayName("Connection Reset")]
    [RefreshProperties(RefreshProperties.All)]
    [Category("Pooling")]
    public bool ConnectionReset
    {
      get
      {
        return (bool)GetPropertyValue("Connection Reset");
      }
      set
      {
        this["Connection Reset"] = value;
      }
    }

    /// <summary>
    ///     <para>Gets or sets the Enlist connection property.</para>
    /// </summary>
    [RefreshProperties(RefreshProperties.All)]
    [DisplayName("Enlist")]
    [Category("Pooling")]
    public bool Enlist
    {
      get
      {
        return (bool)GetPropertyValue("Enlist");
      }
      set
      {
        this["Enlist"] = value;
      }
    }

    /// <summary>
    ///     <para>Gets or sets the MinPoolSize connection property.</para>
    /// </summary>
    [DisplayName("Min Pool Size")]
    [RefreshProperties(RefreshProperties.All)]
    [Category("Pooling")]
    public int MinPoolSize
    {
      get
      {
        return (int)GetPropertyValue("Min Pool Size");
      }
      set
      {
        this["Min Pool Size"] = value;
      }
    }

    /// <summary>
    ///     <para>Gets or sets the MaxPoolSize connection property.</para>
    /// </summary>
    [RefreshProperties(RefreshProperties.All)]
    [DisplayName("Max Pool Size")]
    [Category("Pooling")]
    public int MaxPoolSize
    {
      get
      {
        return (int)GetPropertyValue("Max Pool Size");
      }
      set
      {
        this["Max Pool Size"] = value;
      }
    }

    /// <summary>
    ///     <para>Gets or sets the Pooling connection property.</para>
    /// </summary>
    [RefreshProperties(RefreshProperties.All)]
    [DisplayName("Pooling")]
    [Category("Pooling")]
    public bool Pooling
    {
      get
      {
        return (bool)GetPropertyValue("Pooling");
      }
      set
      {
        this["Pooling"] = value;
      }
    }

    /// <summary>
    ///     <para>Gets or sets the InitString connection property.</para>
    /// </summary>
    [Category("Advanced")]
    [RefreshProperties(RefreshProperties.All)]
    [DisplayName("InitString")]
    public string InitString
    {
      get
      {
        return (string)GetPropertyValue("InitString");
      }
      set
      {
        this["InitString"] = value;
      }
    }

    static SAConnectionStringBuilder()
    {
      SAConnectionStringBuilder.s_ConnOptions["UserID"] = new ConnectionOptions("UserID", "uid", null, ConnectionOptionType.String, "");
      SAConnectionStringBuilder.s_ConnOptions["Password"] = new ConnectionOptions("Password", "pwd", null, ConnectionOptionType.String, "");
      SAConnectionStringBuilder.s_ConnOptions["NewPassword"] = new ConnectionOptions("NewPassword", "newpwd", null, ConnectionOptionType.String, "");
      SAConnectionStringBuilder.s_ConnOptions["DatabaseName"] = new ConnectionOptions("DatabaseName", "dbn", null, ConnectionOptionType.String, "");
      SAConnectionStringBuilder.s_ConnOptions["DatabaseFile"] = new ConnectionOptions("DatabaseFile", "dbf", null, ConnectionOptionType.String, "");
      SAConnectionStringBuilder.s_ConnOptions["DatabaseSwitches"] = new ConnectionOptions("DatabaseSwitches", "dbs", null, ConnectionOptionType.String, "");
      SAConnectionStringBuilder.s_ConnOptions["ServerName"] = new ConnectionOptions("ServerName", "eng", "enginename", ConnectionOptionType.String, "");
      SAConnectionStringBuilder.s_ConnOptions["Unconditional"] = new ConnectionOptions("Unconditional", "unc", null, ConnectionOptionType.String, "No");
      SAConnectionStringBuilder.s_ConnOptions["StartLine"] = new ConnectionOptions("StartLine", "start", null, ConnectionOptionType.String, "");
      SAConnectionStringBuilder.s_ConnOptions["ConnectionName"] = new ConnectionOptions("ConnectionName", "con", null, ConnectionOptionType.String, "");
      SAConnectionStringBuilder.s_ConnOptions["AutoStop"] = new ConnectionOptions("AutoStop", "astop", null, ConnectionOptionType.String, "Yes");
      SAConnectionStringBuilder.s_ConnOptions["DataSourceName"] = new ConnectionOptions("DataSourceName", "dsn", "data source", ConnectionOptionType.String, "");
      SAConnectionStringBuilder.s_ConnOptions["Integrated"] = new ConnectionOptions("Integrated", "int", null, ConnectionOptionType.String, "No");
      SAConnectionStringBuilder.s_ConnOptions["FileDataSourceName"] = new ConnectionOptions("FileDataSourceName", "filedsn", null, ConnectionOptionType.String, "");
      SAConnectionStringBuilder.s_ConnOptions["EncryptedPassword"] = new ConnectionOptions("EncryptedPassword", "enp", null, ConnectionOptionType.String, "");
      SAConnectionStringBuilder.s_ConnOptions["CommBufferSize"] = new ConnectionOptions("CommBufferSize", "cbsize", null, ConnectionOptionType.Int32, 0);
      SAConnectionStringBuilder.s_ConnOptions["Encryption"] = new ConnectionOptions("Encryption", "enc", null, ConnectionOptionType.String, "");
      SAConnectionStringBuilder.s_ConnOptions["LivenessTimeout"] = new ConnectionOptions("LivenessTimeout", "lto", null, ConnectionOptionType.Int32, -1);
      SAConnectionStringBuilder.s_ConnOptions["LogFile"] = new ConnectionOptions("LogFile", "log", null, ConnectionOptionType.String, "");
      SAConnectionStringBuilder.s_ConnOptions["DisableMultiRowFetch"] = new ConnectionOptions("DisableMultiRowFetch", "dmrf", null, ConnectionOptionType.String, "No");
      SAConnectionStringBuilder.s_ConnOptions["CommLinks"] = new ConnectionOptions("CommLinks", "links", null, ConnectionOptionType.String, "");
      SAConnectionStringBuilder.s_ConnOptions["AutoStart"] = new ConnectionOptions("AutoStart", "astart", null, ConnectionOptionType.String, "Yes");
      SAConnectionStringBuilder.s_ConnOptions["Charset"] = new ConnectionOptions("Charset", "cs", null, ConnectionOptionType.String, "");
      SAConnectionStringBuilder.s_ConnOptions["ForceStart"] = new ConnectionOptions("ForceStart", "force", null, ConnectionOptionType.String, "No");
      SAConnectionStringBuilder.s_ConnOptions["AppInfo"] = new ConnectionOptions("AppInfo", "app", null, ConnectionOptionType.String, "");
      SAConnectionStringBuilder.s_ConnOptions["PrefetchRows"] = new ConnectionOptions("PrefetchRows", "prows", null, ConnectionOptionType.Int32, 200);
      SAConnectionStringBuilder.s_ConnOptions["PrefetchBuffer"] = new ConnectionOptions("PrefetchBuffer", "pbuf", null, ConnectionOptionType.Int32, 64);
      SAConnectionStringBuilder.s_ConnOptions["DatabaseKey"] = new ConnectionOptions("DatabaseKey", "dbkey", null, ConnectionOptionType.String, "");
      SAConnectionStringBuilder.s_ConnOptions["Compress"] = new ConnectionOptions("Compress", "comp", null, ConnectionOptionType.String, "No");
      SAConnectionStringBuilder.s_ConnOptions["CompressionThreshold"] = new ConnectionOptions("CompressionThreshold", "compth", null, ConnectionOptionType.Int32, 120);
      SAConnectionStringBuilder.s_ConnOptions["IdleTimeout"] = new ConnectionOptions("IdleTimeout", "idle", null, ConnectionOptionType.Int32, -1);
      SAConnectionStringBuilder.s_ConnOptions["Language"] = new ConnectionOptions("Language", "lang", null, ConnectionOptionType.String, "");
      SAConnectionStringBuilder.s_ConnOptions["LazyClose"] = new ConnectionOptions("LazyClose", "lclose", null, ConnectionOptionType.String, "No");
      SAConnectionStringBuilder.s_ConnOptions["RetryConnectionTimeout"] = new ConnectionOptions("RetryConnectionTimeout", "retryconnto", null, ConnectionOptionType.Int32, 0);
      SAConnectionStringBuilder.s_ConnOptions["Kerberos"] = new ConnectionOptions("Kerberos", "krb", null, ConnectionOptionType.String, "");
      SAConnectionStringBuilder.s_ConnOptions["Elevate"] = new ConnectionOptions("Elevate", "elevate", null, ConnectionOptionType.String, "No");
      SAConnectionStringBuilder.s_ConnOptions["Connection Timeout"] = new ConnectionOptions("Connection Timeout", "Connect Timeout", null, ConnectionOptionType.Int32, 15);
      SAConnectionStringBuilder.s_ConnOptions["Connection Lifetime"] = new ConnectionOptions("Connection Lifetime", "Connection Lifetime", null, ConnectionOptionType.Int32, 0);
      SAConnectionStringBuilder.s_ConnOptions["Connection Reset"] = new ConnectionOptions("Connection Reset", "Connection Reset", null, ConnectionOptionType.Bool, true);
      SAConnectionStringBuilder.s_ConnOptions["Enlist"] = new ConnectionOptions("Enlist", "Enlist", null, ConnectionOptionType.Bool, false);
      SAConnectionStringBuilder.s_ConnOptions["Max Pool Size"] = new ConnectionOptions("Max Pool Size", "Max Pool Size", null, ConnectionOptionType.Int32, 100);
      SAConnectionStringBuilder.s_ConnOptions["Min Pool Size"] = new ConnectionOptions("Min Pool Size", "Min Pool Size", null, ConnectionOptionType.Int32, 0);
      SAConnectionStringBuilder.s_ConnOptions["Persist Security Info"] = new ConnectionOptions("Persist Security Info", "Persist Security Info", null, ConnectionOptionType.Bool, false);
      SAConnectionStringBuilder.s_ConnOptions["Pooling"] = new ConnectionOptions("Pooling", "Pooling", null, ConnectionOptionType.Bool, true);
      SAConnectionStringBuilder.s_ConnOptions["InitString"] = new ConnectionOptions("InitString", "InitString", null, ConnectionOptionType.String, "");
    }

    /// <summary>
    ///     <para>Initializes a new instance of the SAConnectionStringBuilder class.</para>
    /// </summary>
    /// <remarks>
    ///     <para><b>Restrictions:</b> The SAConnectionStringBuilder class is not available in the .NET Compact Framework 2.0.</para>
    /// </remarks>
    public SAConnectionStringBuilder()
      : this(null)
    {
    }

    /// <summary>
    ///     <para>Initializes a new instance of the SAConnectionStringBuilder class.</para>
    /// </summary>
    /// <remarks>
    ///     <para><b>Restrictions:</b> The SAConnectionStringBuilder class is not available in the .NET Compact Framework 2.0.</para>
    /// </remarks>
    /// <param name="connectionString">
    ///     The basis for the object's internal connection information. Parsed into keyword=value pairs.
    ///     <para>For a list of connection parameters, see @olink targetdoc="dbadmin" targetptr="conmean"@Connection parameters@/olink@.</para>
    /// </param>
    /// <example>
    ///     <para>The following statement initializes an SAConnection object for a connection to a database named policies running on a SQL Anywhere database server named hr. The connection uses the user ID admin and the password money.</para>
    ///     <code>SAConnectionStringBuilder conn = new SAConnectionStringBuilder("UID=admin;PWD=money;ENG=hr;DBN=policies" );</code>
    /// </example>
    public SAConnectionStringBuilder(string connectionString)
    {
            SetConnectionOptions(SAConnectionStringBuilder.s_ConnOptions);
      if (connectionString == null || connectionString.Length <= 0)
        return;
            ConnectionString = connectionString;
    }

    internal override object ConvertValue(string key, object value, ConnectionOptionType type)
    {
      if (!key.Equals(GetKeyword(SAConnectionStringBuilder.s_ConnOptions["CommLinks"])))
        return base.ConvertValue(key, value, type);
      SACommLinksOptionsBuilder linksOptionsBuilder = new SACommLinksOptionsBuilder();
      linksOptionsBuilder.SetUseLongNameAsKeyword(GetUseLongNameAsKeyword());
      linksOptionsBuilder.ConnectionString = value.ToString();
      return linksOptionsBuilder.ToString();
    }
  }
}
