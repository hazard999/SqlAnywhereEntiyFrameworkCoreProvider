
// Type: iAnywhere.Data.SQLAnywhere.SATcpOptionsBuilder
// Assembly: iAnywhere.Data.SQLAnywhere.v4.0, Version=11.0.1.27424, Culture=neutral, PublicKeyToken=f222fc4333e0d400
// MVID: CC4F9F8C-E618-49D1-9147-C06A9EF53D1F
// Assembly location: C:\Program Files\SQL Anywhere 11\Assembly\V4\iAnywhere.Data.SQLAnywhere.v4.0.dll

using System.Collections.Generic;

namespace iAnywhere.Data.SQLAnywhere
{
  /// <summary>
  ///     <para>Provides a simple way to create and manage the TCP options portion of connection strings used by the SAConnection object.</para>
  /// </summary>
  /// <remarks>
  ///     <para><b>Restrictions:</b> The SATcpOptionsBuilder class is not available in the .NET Compact Framework 2.0.</para>
  /// </remarks>
  /// <seealso cref="T:iAnywhere.Data.SQLAnywhere.SAConnection" />
  public sealed class SATcpOptionsBuilder : SAConnectionStringBuilderBase
  {
    private static Dictionary<string, ConnectionOptions> s_LinkOptions = new Dictionary<string, ConnectionOptions>();
    private const string c_HostKey = "Host";
    private const string c_BroadcastKey = "Broadcast";
    private const string c_TimeoutKey = "Timeout";
    private const string c_DoBroadcastKey = "DoBroadcast";
    private const string c_ServerPortKey = "ServerPort";
    private const string c_MyIPKey = "MyIP";
    private const string c_ReceiveBufferSizeKey = "ReceiveBufferSize";
    private const string c_SendBufferSizeKey = "SendBufferSize";
    private const string c_TDSKey = "TDS";
    private const string c_BroadcastListenerKey = "BroadcastListener";
    private const string c_LocalOnlyKey = "LocalOnly";
    private const string c_ClientPortKey = "ClientPort";
    private const string c_VerifyServerNameKey = "VerifyServerName";
    private const string c_LDAPKey = "LDAP";
    private const string c_IPV6Key = "IPV6";

    /// <summary>
    ///     <para>Gets or sets the Host option.</para>
    /// </summary>
    public string Host
    {
      get
      {
        return (string)GetPropertyValue("Host");
      }
      set
      {
        this["Host"] = value;
      }
    }

    /// <summary>
    ///     <para>Gets or sets the Broadcast option.</para>
    /// </summary>
    public string Broadcast
    {
      get
      {
        return (string)GetPropertyValue("Broadcast");
      }
      set
      {
        this["Broadcast"] = value;
      }
    }

    /// <summary>
    ///     <para>Gets or sets the Timeout option.</para>
    /// </summary>
    public int Timeout
    {
      get
      {
        return (int)GetPropertyValue("Timeout");
      }
      set
      {
        this["Timeout"] = value;
      }
    }

    /// <summary>
    ///     <para>Gets or sets the DoBroadcast option.</para>
    /// </summary>
    public string DoBroadcast
    {
      get
      {
        return (string)GetPropertyValue("DoBroadcast");
      }
      set
      {
        this["DoBroadcast"] = value;
      }
    }

    /// <summary>
    ///     <para>Gets or sets the ServerPort option.</para>
    /// </summary>
    public string ServerPort
    {
      get
      {
        return (string)GetPropertyValue("ServerPort");
      }
      set
      {
        this["ServerPort"] = value;
      }
    }

    /// <summary>
    ///     <para>Gets or sets the MyIP option.</para>
    /// </summary>
    public string MyIP
    {
      get
      {
        return (string)GetPropertyValue("MyIP");
      }
      set
      {
        this["MyIP"] = value;
      }
    }

    /// <summary>
    ///     <para>Gets or sets the ReceiveBufferSize option.</para>
    /// </summary>
    public int ReceiveBufferSize
    {
      get
      {
        return (int)GetPropertyValue("ReceiveBufferSize");
      }
      set
      {
        this["ReceiveBufferSize"] = value;
      }
    }

    /// <summary>
    ///     <para>Gets or sets the Send BufferSize option.</para>
    /// </summary>
    public int SendBufferSize
    {
      get
      {
        return (int)GetPropertyValue("SendBufferSize");
      }
      set
      {
        this["SendBufferSize"] = value;
      }
    }

    /// <summary>
    ///     <para>Gets or sets the TDS option.</para>
    /// </summary>
    public string TDS
    {
      get
      {
        return (string)GetPropertyValue("TDS");
      }
      set
      {
        this["TDS"] = value;
      }
    }

    /// <summary>
    ///     <para>Gets or sets the BroadcastListener option.</para>
    /// </summary>
    public string BroadcastListener
    {
      get
      {
        return (string)GetPropertyValue("BroadcastListener");
      }
      set
      {
        this["BroadcastListener"] = value;
      }
    }

    /// <summary>
    ///     <para>Gets or sets the LocalOnly option.</para>
    /// </summary>
    public string LocalOnly
    {
      get
      {
        return (string)GetPropertyValue("LocalOnly");
      }
      set
      {
        this["LocalOnly"] = value;
      }
    }

    /// <summary>
    ///     <para>Gets or sets the ClientPort option.</para>
    /// </summary>
    public string ClientPort
    {
      get
      {
        return (string)GetPropertyValue("ClientPort");
      }
      set
      {
        this["ClientPort"] = value;
      }
    }

    /// <summary>
    ///     <para>Gets or sets the VerifyServerName option.</para>
    /// </summary>
    public string VerifyServerName
    {
      get
      {
        return (string)GetPropertyValue("VerifyServerName");
      }
      set
      {
        this["VerifyServerName"] = value;
      }
    }

    /// <summary>
    ///     <para>Gets or sets the LDAP option.</para>
    /// </summary>
    public string LDAP
    {
      get
      {
        return (string)GetPropertyValue("LDAP");
      }
      set
      {
        this["LDAP"] = value;
      }
    }

    /// <summary>
    ///     <para>Gets or sets the IPV6 option.</para>
    /// </summary>
    public string IPV6
    {
      get
      {
        return (string)GetPropertyValue("IPV6");
      }
      set
      {
        this["IPV6"] = value;
      }
    }

    static SATcpOptionsBuilder()
    {
      SATcpOptionsBuilder.s_LinkOptions["Host"] = new ConnectionOptions("Host", "ip", null, ConnectionOptionType.String, "");
      SATcpOptionsBuilder.s_LinkOptions["Broadcast"] = new ConnectionOptions("Broadcast", "bcast", null, ConnectionOptionType.String, "");
      SATcpOptionsBuilder.s_LinkOptions["Timeout"] = new ConnectionOptions("Timeout", "to", null, ConnectionOptionType.Int32, 5);
      SATcpOptionsBuilder.s_LinkOptions["DoBroadcast"] = new ConnectionOptions("DoBroadcast", "dobroad", null, ConnectionOptionType.String, "All");
      SATcpOptionsBuilder.s_LinkOptions["ServerPort"] = new ConnectionOptions("ServerPort", "port", null, ConnectionOptionType.Shorts, 2638);
      SATcpOptionsBuilder.s_LinkOptions["MyIP"] = new ConnectionOptions("MyIP", "me", null, ConnectionOptionType.String, "");
      SATcpOptionsBuilder.s_LinkOptions["ReceiveBufferSize"] = new ConnectionOptions("ReceiveBufferSize", "rcvbufsz", null, ConnectionOptionType.Int32, 0);
      SATcpOptionsBuilder.s_LinkOptions["SendBufferSize"] = new ConnectionOptions("SendBufferSize", "sndbufsz", null, ConnectionOptionType.Int32, 0);
      SATcpOptionsBuilder.s_LinkOptions["TDS"] = new ConnectionOptions("TDS", "tds", null, ConnectionOptionType.String, "Yes");
      SATcpOptionsBuilder.s_LinkOptions["BroadcastListener"] = new ConnectionOptions("BroadcastListener", "blistener", null, ConnectionOptionType.String, "Yes");
      SATcpOptionsBuilder.s_LinkOptions["LocalOnly"] = new ConnectionOptions("LocalOnly", "lo", null, ConnectionOptionType.String, "No");
      SATcpOptionsBuilder.s_LinkOptions["ClientPort"] = new ConnectionOptions("ClientPort", "cport", null, ConnectionOptionType.Shorts, 0);
      SATcpOptionsBuilder.s_LinkOptions["VerifyServerName"] = new ConnectionOptions("VerifyServerName", "verify", null, ConnectionOptionType.String, "Yes");
      SATcpOptionsBuilder.s_LinkOptions["LDAP"] = new ConnectionOptions("LDAP", "ldap", null, ConnectionOptionType.String, "On");
      SATcpOptionsBuilder.s_LinkOptions["IPV6"] = new ConnectionOptions("IPV6", "v6", null, ConnectionOptionType.String, "Yes");
    }

    /// <summary>
    ///     <para>Initializes an SATcpOptionsBuilder object.</para>
    /// </summary>
    /// <remarks>
    ///     <para><b>Restrictions:</b> The SATcpOptionsBuilder class is not available in the .NET Compact Framework 2.0.</para>
    /// </remarks>
    /// <example>
    ///     <para>The following statement initializes an SATcpOptionsBuilder object.</para>
    ///     <code>SATcpOptionsBuilder options = new SATcpOptionsBuilder( );</code>
    /// </example>
    public SATcpOptionsBuilder()
      : this(null)
    {
    }

    /// <summary>
    ///     <para>Initializes an SATcpOptionsBuilder object.</para>
    /// </summary>
    /// <remarks>
    ///     <para><b>Restrictions:</b> The SATcpOptionsBuilder class is not available in the .NET Compact Framework 2.0.</para>
    /// </remarks>
    /// <param name="options">
    ///     A SQL Anywhere TCP connection parameter options string.
    ///     <para>For a list of connection parameters, see @olink targetdoc="dbadmin" targetptr="conmean"@Connection parameters@/olink@.</para>
    /// </param>
    /// <example>
    ///     <para>The following statement initializes an SATcpOptionsBuilder object.</para>
    ///     <code>SATcpOptionsBuilder options = new SATcpOptionsBuilder( );</code>
    /// </example>
    public SATcpOptionsBuilder(string options)
    {
            SetConnectionOptions(SATcpOptionsBuilder.s_LinkOptions);
      if (options == null || options.Length <= 0)
        return;
            ConnectionString = options;
    }

    /// <summary>
    ///     <para>Converts the TcpOptionsBuilder object to a string representation.</para>
    /// </summary>
    /// <returns>
    /// <para>The options string being built.</para>
    ///    </returns>
    public override string ToString()
    {
      if (!GetUseLongNameAsKeyword())
        return string.Format("tcp({0})", ConnectionString);
      return string.Format("TCPIP({0})", ConnectionString);
    }
  }
}
