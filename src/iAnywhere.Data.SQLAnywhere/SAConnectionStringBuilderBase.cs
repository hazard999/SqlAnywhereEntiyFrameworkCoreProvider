
// Type: iAnywhere.Data.SQLAnywhere.SAConnectionStringBuilderBase
// Assembly: iAnywhere.Data.SQLAnywhere.v4.0, Version=11.0.1.27424, Culture=neutral, PublicKeyToken=f222fc4333e0d400
// MVID: CC4F9F8C-E618-49D1-9147-C06A9EF53D1F
// Assembly location: C:\Program Files\SQL Anywhere 11\Assembly\V4\iAnywhere.Data.SQLAnywhere.v4.0.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace iAnywhere.Data.SQLAnywhere
{
  /// <summary>
  ///     <para>Base class of the SAConnectionStringBuilder class.</para>
  /// </summary>
  public abstract class SAConnectionStringBuilderBase : DbConnectionStringBuilder
  {
    private bool _useLongNameAsKeyword = true;
    private Dictionary<string, ConnectionOptions> _connOptions;

    /// <summary>
    ///     <para>Gets an System.Collections.ICollection that contains the keys in the SAConnectionStringBuilder.</para>
    /// </summary>
    /// <returns>
    /// <para>An System.Collections.ICollection that contains the keys in the SAConnectionStringBuilder.</para>
    ///    </returns>
    public override ICollection Keys
    {
      get
      {
        int num = 0;
        string[] strArray = new string[_connOptions.Count];
        foreach (ConnectionOptions connOpt in _connOptions.Values)
          strArray[num++] = GetKeyword(connOpt);
        return strArray;
      }
    }

    /// <summary>
    ///     <para>Gets or sets the value of the connection keyword.</para>
    /// </summary>
    /// <value>An object representing the value of the specified connection keyword.</value>
    /// <remarks>
    ///     <para>If the keyword or type is invalid, an exception is raised. keyword is case insensitive.</para>
    ///     <para>When setting the value, passing NULL clears the value.</para>
    /// </remarks>
    /// <param name="keyword">The name of the connection keyword.</param>
    public override object this[string keyword]
    {
      get
      {
                CheckArgumentNull(keyword, "keyword");
        ConnectionOptions connectionOptions = FindConnectionOptions(keyword);
        if (connectionOptions == null)
          throw KeywordNotSupported(keyword);
        string keyword1 = GetKeyword(connectionOptions);
        if (base.ContainsKey(keyword1))
          return base[keyword1];
        return connectionOptions.DefaultValue;
      }
      set
      {
                CheckArgumentNull(keyword, "keyword");
        ConnectionOptions connectionOptions = FindConnectionOptions(keyword);
        if (connectionOptions == null)
          throw KeywordNotSupported(keyword);
        string keyword1 = GetKeyword(connectionOptions);
        if (value == null)
        {
          base.Remove(keyword1);
        }
        else
        {
          object obj = ConvertValue(keyword1, value, connectionOptions.Type);
          base[keyword1] = obj is string ? (obj as string).Trim() : obj;
        }
      }
    }

    internal SAConnectionStringBuilderBase()
      : base(false)
    {
    }

    internal void SetConnectionOptions(Dictionary<string, ConnectionOptions> connOptions)
    {
            _connOptions = connOptions;
    }

    internal void CheckArgumentNull(object value, string argName)
    {
      if (value == null)
        throw new ArgumentNullException(argName);
    }

    internal ArgumentException KeywordNotSupported(string keyword)
    {
      return new ArgumentException(string.Format(SARes.GetString(17314), keyword), "");
    }

    internal object ChangeType(object val, ConnectionOptionType connOptType)
    {
      Type type = FindType(connOptType);
      if (type == typeof (bool))
      {
        if (val is string)
        {
          string lower = (val as string).ToLower();
          if (lower == "yes" || lower == "1" || lower == "on")
            return true;
          if (lower == "no" || lower == "0" || lower == "off")
            return false;
        }
        else if (!(val is bool))
          throw new FormatException(SARes.GetString(14956, FormatForException(val.GetType().ToString()), FormatForException(type.ToString())));
      }
      else if (type == typeof (int) && !(val is string) && !(val is int))
        throw new FormatException(SARes.GetString(14956, FormatForException(val.GetType().ToString()), FormatForException(type.ToString())));
      return Convert.ChangeType(val, type);
    }

    internal Type FindType(ConnectionOptionType type)
    {
      switch (type)
      {
        case ConnectionOptionType.String:
          return typeof (string);
        case ConnectionOptionType.Int32:
          return typeof (int);
        case ConnectionOptionType.Int16:
          return typeof (short);
        case ConnectionOptionType.Bool:
          return typeof (bool);
        case ConnectionOptionType.Shorts:
          return typeof (string);
        case ConnectionOptionType.Strings:
          return typeof (string);
        default:
          return null;
      }
    }

    private string FormatForException(string str)
    {
      if (str.StartsWith("System."))
        return str.Substring(str.LastIndexOf('.') + 1);
      return str;
    }

    internal string GetKeyword(ConnectionOptions connOpt)
    {
      if (!_useLongNameAsKeyword)
        return connOpt.ShortName;
      return connOpt.LongName;
    }

    internal ConnectionOptions FindConnectionOptions(string key)
    {
      string str = RemoveSpaces(key);
      foreach (ConnectionOptions connectionOptions in _connOptions.Values)
      {
        if (str.Equals(RemoveSpaces(connectionOptions.LongName), StringComparison.InvariantCultureIgnoreCase) || str.Equals(RemoveSpaces(connectionOptions.ShortName), StringComparison.InvariantCultureIgnoreCase) || str.Equals(RemoveSpaces(connectionOptions.AlternateName), StringComparison.InvariantCultureIgnoreCase))
          return connectionOptions;
      }
      return null;
    }

    private string RemoveSpaces(string str)
    {
      if (str == null)
        return null;
      StringBuilder stringBuilder = new StringBuilder();
      for (int index = 0; index < str.Length; ++index)
      {
        if (str[index] != 32)
          stringBuilder.Append(str[index]);
      }
      return stringBuilder.ToString();
    }

    /// <summary>
    ///     <para>Sets a boolean value that indicates whether long connection parameter names are used in the connection string. Long connection parameter names are used by default.</para>
    /// </summary>
    /// <param name="useLongNameAsKeyword">
    ///     A boolean value that indicates whether the long connection parameter name is used in the connection string.
    /// </param>
    /// <seealso cref="M:iAnywhere.Data.SQLAnywhere.SAConnectionStringBuilderBase.GetUseLongNameAsKeyword" />
    public void SetUseLongNameAsKeyword(bool useLongNameAsKeyword)
    {
            _useLongNameAsKeyword = useLongNameAsKeyword;
    }

    /// <summary>
    ///     <para>Gets a boolean values that indicates whether long connection parameter names are used in the connection string. </para>
    /// </summary>
    /// <remarks>
    ///     <para>SQL Anywhere connection parameters have both long and short forms of their names. For example, to specify the name of an ODBC data source in your connection string, you can use either of the following values: DataSourceName or DSN. By default, long connection parameter names are used to build connection strings.</para>
    /// </remarks>
    /// <returns>
    /// <para>True if long connection parameter names are used to build connection strings; otherwise, false.</para>
    ///    </returns>
    /// <seealso cref="M:iAnywhere.Data.SQLAnywhere.SAConnectionStringBuilderBase.SetUseLongNameAsKeyword(System.Boolean)" />
    public bool GetUseLongNameAsKeyword()
    {
      return _useLongNameAsKeyword;
    }

    /// <summary>
    ///     <para>Gets the keyword for specified SAConnectionStringBuilder property.</para>
    /// </summary>
    /// <param name="propName">
    ///     The name of the SAConnectionStringBuilder property.
    /// </param>
    /// <returns>
    /// <para>The keyword for specified SAConnectionStringBuilder property.</para>
    ///    </returns>
    public string GetKeyword(string propName)
    {
      ConnectionOptions connectionOptions = FindConnectionOptions(propName);
      if (connectionOptions != null)
        return GetKeyword(connectionOptions);
      throw KeywordNotSupported(propName);
    }

    /// <summary>
    ///     <para>Determines whether the SAConnectionStringBuilder object contains a specific keyword.</para>
    /// </summary>
    /// <param name="keyword">
    ///     The keyword to locate in the SAConnectionStringBuilder.
    /// </param>
    /// <returns>
    /// <para>True if the value associated with keyword has been set; otherwise, false.</para>
    ///    </returns>
    /// <example>
    ///     <para>The following statement determines whether the SAConnectionStringBuilder object contains the UserID keyword.</para>
    ///     <code>connectString.ContainsKey("UserID")</code>
    /// </example>
    public override bool ContainsKey(string keyword)
    {
            CheckArgumentNull(keyword, "keyword");
      return FindConnectionOptions(keyword) != null;
    }

    /// <summary>
    ///     <para>Removes the entry with the specified key from the SAConnectionStringBuilder instance.</para>
    /// </summary>
    /// <param name="keyword">
    ///     The key of the key/value pair to be removed from the connection string in this SAConnectionStringBuilder.
    /// </param>
    /// <returns>
    /// <para>True if the key existed within the connection string and was removed; false if the key did not exist.</para>
    ///    </returns>
    public override bool Remove(string keyword)
    {
            CheckArgumentNull(keyword, "keyword");
      ConnectionOptions connectionOptions = FindConnectionOptions(keyword);
      if (connectionOptions != null)
        return base.Remove(GetKeyword(connectionOptions));
      return false;
    }

    /// <summary>
    ///     <para>Indicates whether the specified key exists in this SAConnectionStringBuilder instance.</para>
    /// </summary>
    /// <param name="keyword">
    ///     The key to locate in the SAConnectionStringBuilder.
    /// </param>
    /// <returns>
    /// <para>True if the SAConnectionStringBuilder contains an entry with the specified key; otherwise false.</para>
    ///    </returns>
    public override bool ShouldSerialize(string keyword)
    {
            CheckArgumentNull(keyword, "keyword");
      ConnectionOptions connectionOptions = FindConnectionOptions(keyword);
      if (connectionOptions != null)
      {
        object obj = this[keyword];
        if (obj != null && !obj.ToString().Equals(connectionOptions.DefaultValue.ToString(), StringComparison.InvariantCultureIgnoreCase))
          return true;
      }
      return false;
    }

    internal virtual object ConvertValue(string key, object value, ConnectionOptionType type)
    {
      return ChangeType(value, type);
    }

    /// <summary>
    ///     <para>Retrieves a value corresponding to the supplied key from this SAConnectionStringBuilder.</para>
    /// </summary>
    /// <param name="keyword">The key of the item to retrieve.</param>
    /// <param name="value">The value corresponding to keyword.</param>
    /// <returns>
    /// <para>true if keyword was found within the connection string; otherwise false.</para>
    ///    </returns>
    public override bool TryGetValue(string keyword, out object value)
    {
            CheckArgumentNull(keyword, "keyword");
      if (FindConnectionOptions(keyword) != null)
      {
        value = this[keyword];
        return true;
      }
      value = null;
      return false;
    }

    internal object GetPropertyValue(string keyword)
    {
      ConnectionOptions connectionOptions = FindConnectionOptions(keyword);
      string keyword1 = GetKeyword(connectionOptions);
      if (base.ContainsKey(keyword1))
        return ChangeType(base[keyword1], connectionOptions.Type);
      return ChangeType(connectionOptions.DefaultValue, connectionOptions.Type);
    }
  }
}
