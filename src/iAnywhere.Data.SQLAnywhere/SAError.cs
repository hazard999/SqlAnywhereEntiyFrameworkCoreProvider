
// Type: iAnywhere.Data.SQLAnywhere.SAError
// Assembly: iAnywhere.Data.SQLAnywhere.v4.0, Version=11.0.1.27424, Culture=neutral, PublicKeyToken=f222fc4333e0d400
// MVID: CC4F9F8C-E618-49D1-9147-C06A9EF53D1F
// Assembly location: C:\Program Files\SQL Anywhere 11\Assembly\V4\iAnywhere.Data.SQLAnywhere.v4.0.dll

using System;

namespace iAnywhere.Data.SQLAnywhere
{
  /// <summary>
  ///     <para>Collects information relevant to a warning or error returned by the data source.</para>
  /// </summary>
  /// <remarks>
  ///     <para>There is no constructor for SAError.</para>
  ///     <para>For information about error handling, see @olink targetdoc="programming" targetptr="error-adodotnet-development"@Error handling and the SQL Anywhere .NET Data Provider@/olink@.</para>
  /// </remarks>
  [Serializable]
  public sealed class SAError
  {
    private int _nativeError;
    private string _message;
    private string _sqlState;

    /// <summary>
    ///     <para>Returns database-specific error information.</para>
    /// </summary>
    public int NativeError
    {
      get
      {
        return _nativeError;
      }
    }

    /// <summary>
    ///     <para>Returns a short description of the error.</para>
    /// </summary>
    public string Message
    {
      get
      {
        return _message;
      }
    }

    /// <summary>
    ///     <para>The SQL Anywhere five-character SQLSTATE following the ANSI SQL standard.</para>
    /// </summary>
    public string SqlState
    {
      get
      {
        return _sqlState;
      }
    }

    /// <summary>
    ///     <para>Returns the name of the provider that generated the error.</para>
    /// </summary>
    public string Source
    {
      get
      {
        return "SQL Anywhere .NET Data Provider";
      }
    }

    internal SAError(int nativeError, string message, string sqlState)
    {
            _nativeError = nativeError;
            _message = message;
            _sqlState = sqlState;
    }

    /// <summary>
    ///     <para>The complete text of the error message.</para>
    /// </summary>
    /// <example>
    ///     <para>The return value is a string is in the form <b>SAError:</b>, followed by the Message. For example:</para>
    ///     <code>SAError:UserId or Password not valid.</code>
    /// 
    /// </example>
    public override string ToString()
    {
      return GetType().Name + ":" + _message;
    }
  }
}
