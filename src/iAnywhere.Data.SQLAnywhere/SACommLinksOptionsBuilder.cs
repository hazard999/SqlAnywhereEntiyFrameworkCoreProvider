
// Type: iAnywhere.Data.SQLAnywhere.SACommLinksOptionsBuilder
// Assembly: iAnywhere.Data.SQLAnywhere.v4.0, Version=11.0.1.27424, Culture=neutral, PublicKeyToken=f222fc4333e0d400
// MVID: CC4F9F8C-E618-49D1-9147-C06A9EF53D1F
// Assembly location: C:\Program Files\SQL Anywhere 11\Assembly\V4\iAnywhere.Data.SQLAnywhere.v4.0.dll

using System;
using System.Collections;
using System.Text;

namespace iAnywhere.Data.SQLAnywhere
{
  /// <summary>
  ///     <para>Provides a simple way to create and manage the CommLinks options portion of connection strings used by the SAConnection class.</para>
  /// </summary>
  /// <remarks>
  ///     <para>The SACommLinksOptionsBuilder class is not available in the .NET Compact Framework 2.0.</para>
  ///     <para>For a list of connection parameters, see @olink targetdoc="dbadmin" targetptr="conmean"@Connection parameters@/olink@.</para>
  /// </remarks>
  public sealed class SACommLinksOptionsBuilder
  {
    private bool _useLongNameAsKeyword = true;
    private static SAUnmanagedDll s_unmanagedDll = SAUnmanagedDll.Instance;
    private static Hashtable s_ShortForms = new Hashtable();
    private const string c_sharedMemKey = "SharedMemory";
    private const string c_tcpipKey = "TCPIP";
    private const string c_allKey = "All";
    private Hashtable _linksOptions;

    /// <summary>
    ///     <para>Gets or sets the connection string being built.</para>
    /// </summary>
    /// <remarks>
    ///     <para>The SACommLinksOptionsBuilder class is not available in the .NET Compact Framework 2.0.</para>
    /// </remarks>
    public string ConnectionString
    {
      get
      {
        return ToString();
      }
      set
      {
                Init(value);
      }
    }

    /// <summary>
    ///     <para>Gets or sets the SharedMemory protocol.</para>
    /// </summary>
    /// <remarks>
    ///     <para>The SACommLinksOptionsBuilder class is not available in the .NET Compact Framework 2.0.</para>
    /// </remarks>
    public bool SharedMemory
    {
      get
      {
        return (bool)_linksOptions["SharedMemory"];
      }
      set
      {
                _linksOptions["SharedMemory"] = value;
      }
    }

    /// <summary>
    ///     <para>Gets or sets the ALL CommLinks option.</para>
    /// </summary>
    /// <remarks>
    ///     <para>Attempt to connect using the shared memory protocol first, followed by all remaining and available communication protocols. Use this setting if you are unsure of which communication protocol(s) to use.</para>
    ///     <para>The SACommLinksOptionsBuilder class is not available in the .NET Compact Framework 2.0.</para>
    /// </remarks>
    public bool All
    {
      get
      {
        return (bool)_linksOptions["All"];
      }
      set
      {
                _linksOptions["All"] = value;
      }
    }

    /// <summary>
    ///     <para>Gets or sets a TcpOptionsBuilder object used to create a TCP options string.</para>
    /// </summary>
    public SATcpOptionsBuilder TcpOptionsBuilder
    {
      get
      {
        return (SATcpOptionsBuilder)_linksOptions["TCPIP"];
      }
      set
      {
                _linksOptions["TCPIP"] = value;
      }
    }

    /// <summary>
    ///     <para>Gets or sets a string of TCP options.</para>
    /// </summary>
    public string TcpOptionsString
    {
      get
      {
        return _linksOptions["TCPIP"].ToString();
      }
      set
      {
        SATcpOptionsBuilder tcpOptionsBuilder = new SATcpOptionsBuilder();
        tcpOptionsBuilder.SetUseLongNameAsKeyword(_useLongNameAsKeyword);
        tcpOptionsBuilder.ConnectionString = value;
                _linksOptions["TCPIP"] = tcpOptionsBuilder;
      }
    }

    static SACommLinksOptionsBuilder()
    {
      SACommLinksOptionsBuilder.s_ShortForms["SharedMemory"] = "shmem";
      SACommLinksOptionsBuilder.s_ShortForms["TCPIP"] = "tcp";
      SACommLinksOptionsBuilder.s_ShortForms["All"] = "all";
    }

    /// <summary>
    ///     <para>Initializes an SACommLinksOptionsBuilder object.</para>
    /// </summary>
    /// <remarks>
    ///     <para>The SACommLinksOptionsBuilder class is not available in the .NET Compact Framework 2.0.</para>
    /// </remarks>
    /// <example>
    ///            <para>The following statement initializes an SACommLinksOptionsBuilder object.</para>
    ///            <code>SACommLinksOptionsBuilder commLinks =
    /// new SACommLinksOptionsBuilder( );</code>
    /// 
    ///        </example>
    public SACommLinksOptionsBuilder()
      : this("")
    {
    }

    /// <summary>
    ///     <para>Initializes an SACommLinksOptionsBuilder object.</para>
    /// </summary>
    /// <remarks>
    ///     <para>The SACommLinksOptionsBuilder class is not available in the .NET Compact Framework 2.0.</para>
    /// </remarks>
    /// <param name="options">
    ///     A SQL Anywhere CommLinks connection parameter string.
    ///     <para>For a list of connection parameters, see @olink targetdoc="dbadmin" targetptr="conmean"@Connection parameters@/olink@.</para>
    /// </param>
    /// <example>
    ///            <para>The following statement initializes an SACommLinksOptionsBuilder object.</para>
    ///            <code>SACommLinksOptionsBuilder commLinks =
    /// new SACommLinksOptionsBuilder("TCPIP(DoBroadcast=ALL;Timeout=20)" );</code>
    /// 
    ///        </example>
    public SACommLinksOptionsBuilder(string options)
    {
            Init(options);
    }

    /// <summary>
    ///     <para>Sets a boolean value that indicates whether long connection parameter names are used in the connection string. Long connection parameter names are used by default.</para>
    /// </summary>
    /// <param name="useLongNameAsKeyword">
    ///     A boolean value that indicates whether the long connection parameter name is used in the connection string.
    /// </param>
    /// <seealso cref="M:iAnywhere.Data.SQLAnywhere.SACommLinksOptionsBuilder.GetUseLongNameAsKeyword" />
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
    /// <seealso cref="M:iAnywhere.Data.SQLAnywhere.SACommLinksOptionsBuilder.SetUseLongNameAsKeyword(System.Boolean)" />
    public bool GetUseLongNameAsKeyword()
    {
      return _useLongNameAsKeyword;
    }

    private string FindKey(string key)
    {
      foreach (DictionaryEntry sShortForm in SACommLinksOptionsBuilder.s_ShortForms)
      {
        if (string.Compare(key, sShortForm.Key as string, StringComparison.InvariantCultureIgnoreCase) == 0 || string.Compare(key, sShortForm.Value as string, StringComparison.InvariantCultureIgnoreCase) == 0)
          return sShortForm.Key as string;
      }
      return null;
    }

    private string GetKeyword(string key)
    {
      if (!_useLongNameAsKeyword)
        return SACommLinksOptionsBuilder.s_ShortForms[key] as string;
      return key;
    }

    private unsafe void Init(string options)
    {
            _linksOptions = new Hashtable();
            _linksOptions["SharedMemory"] = false;
            _linksOptions["All"] = false;
            _linksOptions["TCPIP"] = null;
      int numResults = 0;
      IntPtr result = IntPtr.Zero;
      try
      {
        switch (PInvokeMethods.SAConnectionStringBuilder_ParseLinksOptions(options, ref numResults, ref result))
        {
          case 1:
            throw new SAException(SARes.GetString(7941));
          case 6:
            throw new SAException(SARes.GetString(14952, options));
          case 4:
            throw new SAException(SARes.GetString(17439, options));
          default:
            SAPortInfo* saPortInfoPtr = (SAPortInfo*) (void*) result;
            for (int index = 0; index < numResults; ++index)
            {
              string key = FindKey(new string((char*) (void*) saPortInfoPtr->Type));
              if ("SharedMemory".Equals(key, StringComparison.InvariantCultureIgnoreCase))
                                SharedMemory = true;
              else if ("TCPIP".Equals(key, StringComparison.InvariantCultureIgnoreCase))
              {
                                TcpOptionsString = new string((char*) (void*) saPortInfoPtr->Options);
              }
              else
              {
                if (!"All".Equals(key, StringComparison.InvariantCultureIgnoreCase))
                  throw new SAException(SARes.GetString(7760));
                                All = true;
              }
              ++saPortInfoPtr;
            }
            break;
        }
      }
      finally
      {
        SAException.CheckException(PInvokeMethods.SAConnectionStringBuilder_FreeLinksOptions(numResults, result));
      }
    }

    /// <summary>
    ///     <para>Converts the SACommLinksOptionsBuilder object to a string representation.</para>
    /// </summary>
    /// <returns>
    /// <para>The options string being built.</para>
    ///    </returns>
    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      if (_linksOptions["SharedMemory"].Equals(true))
        stringBuilder.Append(GetKeyword("SharedMemory"));
      if (_linksOptions["All"].Equals(true))
      {
        stringBuilder.Append(stringBuilder.Length > 0 ? "," : "");
        stringBuilder.Append(GetKeyword("All"));
      }
      if (_linksOptions["TCPIP"] != null)
      {
        stringBuilder.Append(stringBuilder.Length > 0 ? "," : "");
        stringBuilder.Append(_linksOptions["TCPIP"].ToString());
      }
      return stringBuilder.ToString();
    }
  }
}
