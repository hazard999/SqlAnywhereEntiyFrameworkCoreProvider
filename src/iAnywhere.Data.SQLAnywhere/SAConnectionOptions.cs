
// Type: iAnywhere.Data.SQLAnywhere.SAConnectionOptions
// Assembly: iAnywhere.Data.SQLAnywhere.v4.0, Version=11.0.1.27424, Culture=neutral, PublicKeyToken=f222fc4333e0d400
// MVID: CC4F9F8C-E618-49D1-9147-C06A9EF53D1F
// Assembly location: C:\Program Files\SQL Anywhere 11\Assembly\V4\iAnywhere.Data.SQLAnywhere.v4.0.dll

using System.Collections;

namespace iAnywhere.Data.SQLAnywhere
{
  /// <summary>Summary description for SAConnectionOptions</summary>
  internal sealed class SAConnectionOptions
  {
    public static string[] s_nonSAConnectionOptions = new string[11]{ "adodotnet testing", "connect timeout", "connection timeout", "connection lifetime", "connection reset", "enlist", "initstring", "max pool size", "min pool size", "persist security info", "pooling" };
    public static string[] s_adoConnectionOptions = new string[2]{ "user id", "uid" };
    public static string s_connectionTimeoutKeys = "connect timeout;connection timeout";
    public static string s_databaseKeys = "databasename;dbn;datasourcename;data source;dsn;databasefile;dbf;filedatasourcename;filedsn";
    public static string s_defaultDatabase = "";
    public static string s_dataSourceKeys = "enginename;servername;eng;";
    public static string s_defaultDataSource = "";
    public static string s_passwordKeys = "password;pwd";
    public static string s_defaultPassword = "";
    public static string s_persistSecurityInfoKeys = "persist security info";
    public static bool s_defaultPersistSecurityInfo = false;
    public static string s_connectionLifetimeKeys = "connection lifetime";
    public static int s_defaultConnectionLifetime = 0;
    public static string s_connectionResetKeys = "connection reset";
    public static bool s_defaultConnectionReset = true;
    public static string s_enlistKeys = "enlist";
    public static bool s_defaultEnlist = true;
    public static string s_initStringKeys = "initstring";
    public static string s_defaultInitString = "";
    public static string s_maxPoolSizeKeys = "max pool size";
    public static int s_defaultMaxPoolSize = 100;
    public static string s_minPoolSizeKeys = "min pool size";
    public static int s_defaultMinPoolSize = 0;
    public static string s_poolingKeys = "pooling";
    public static bool s_defaultPooling = true;
    public static string s_adoDotNetTestingKeys = "adodotnet testing";
    public static bool s_defaultAdoDotNetTesting = false;
    public const char keySeparator = ';';
    public const int s_defaultConnectionTimeout = 15;

    public static ArrayList GetKeys(string keysString)
    {
      ArrayList arrayList = new ArrayList();
      int num = 0;
      int startIndex = 0;
      for (int index = keysString.IndexOf(';', startIndex); index > 0; index = keysString.IndexOf(';', startIndex))
      {
        arrayList.Add(keysString.Substring(startIndex, index - startIndex));
        startIndex = index + 1;
        ++num;
      }
      arrayList.Add(keysString.Substring(startIndex));
      return arrayList;
    }

    public static int GetConnectionTimeout(Hashtable connOptions)
    {
      return SAConnectionOptions.GetIntValue(connOptions, SAConnectionOptions.s_connectionTimeoutKeys, 15);
    }

    public static string GetDatabase(Hashtable connOptions)
    {
      return SAConnectionOptions.GetStringValue(connOptions, SAConnectionOptions.s_databaseKeys, SAConnectionOptions.s_defaultDatabase);
    }

    public static string GetDataSource(Hashtable connOptions)
    {
      return SAConnectionOptions.GetStringValue(connOptions, SAConnectionOptions.s_dataSourceKeys, SAConnectionOptions.s_defaultDataSource);
    }

    public static bool GetPersistSecurityInfo(Hashtable connOptions)
    {
      return SAConnectionOptions.GetBoolValue(connOptions, SAConnectionOptions.s_persistSecurityInfoKeys, SAConnectionOptions.s_defaultPersistSecurityInfo);
    }

    public static string GetPassword(Hashtable connOptions)
    {
      return SAConnectionOptions.GetStringValue(connOptions, SAConnectionOptions.s_passwordKeys, SAConnectionOptions.s_defaultPassword);
    }

    public static int GetConnectionLifetime(Hashtable connOptions)
    {
      return SAConnectionOptions.GetIntValue(connOptions, SAConnectionOptions.s_connectionLifetimeKeys, SAConnectionOptions.s_defaultConnectionLifetime);
    }

    public static bool GetConnectionReset(Hashtable connOptions)
    {
      return SAConnectionOptions.GetBoolValue(connOptions, SAConnectionOptions.s_connectionResetKeys, SAConnectionOptions.s_defaultConnectionReset);
    }

    public static bool GetEnlist(Hashtable connOptions)
    {
      return SAConnectionOptions.GetBoolValue(connOptions, SAConnectionOptions.s_enlistKeys, SAConnectionOptions.s_defaultEnlist);
    }

    public static string GetInitString(Hashtable connOptions)
    {
      return SAConnectionOptions.GetStringValue(connOptions, SAConnectionOptions.s_initStringKeys, SAConnectionOptions.s_defaultInitString);
    }

    public static int GetMaxPoolSize(Hashtable connOptions)
    {
      return SAConnectionOptions.GetIntValue(connOptions, SAConnectionOptions.s_maxPoolSizeKeys, SAConnectionOptions.s_defaultMaxPoolSize);
    }

    public static int GetMinPoolSize(Hashtable connOptions)
    {
      return SAConnectionOptions.GetIntValue(connOptions, SAConnectionOptions.s_minPoolSizeKeys, SAConnectionOptions.s_defaultMinPoolSize);
    }

    public static bool GetPooling(Hashtable connOptions)
    {
      return SAConnectionOptions.GetBoolValue(connOptions, SAConnectionOptions.s_poolingKeys, SAConnectionOptions.s_defaultPooling);
    }

    public static bool GetAdoDotNetTesting(Hashtable connOptions)
    {
      return SAConnectionOptions.GetBoolValue(connOptions, SAConnectionOptions.s_adoDotNetTestingKeys, SAConnectionOptions.s_defaultAdoDotNetTesting);
    }

    private static int GetIntValue(Hashtable connOptions, string keysString, int defaultValue)
    {
      string s = SAConnectionOptions.GetValue(connOptions, keysString);
      if (s != null)
        return int.Parse(s);
      return defaultValue;
    }

    private static bool GetBoolValue(Hashtable connOptions, string keysString, bool defaultValue)
    {
      string str = SAConnectionOptions.GetValue(connOptions, keysString);
      if (str != null)
        return bool.Parse(str);
      return defaultValue;
    }

    private static string GetStringValue(Hashtable connOptions, string keysString, string defaultValue)
    {
      return SAConnectionOptions.GetValue(connOptions, keysString) ?? defaultValue;
    }

    private static string GetValue(Hashtable connOptions, string keysString)
    {
      if (connOptions != null)
      {
        foreach (string key in SAConnectionOptions.GetKeys(keysString))
        {
          if (connOptions.Contains(key.ToLower()))
            return (string) connOptions[key];
        }
      }
      return null;
    }
  }
}
