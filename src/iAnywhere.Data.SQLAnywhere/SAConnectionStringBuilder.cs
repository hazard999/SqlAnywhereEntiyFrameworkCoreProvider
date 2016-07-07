using System.Collections.Generic;

namespace iAnywhere.Data.SQLAnywhere
{
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

        public SAConnectionStringBuilder()
          : this(null)
        {
        }

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
