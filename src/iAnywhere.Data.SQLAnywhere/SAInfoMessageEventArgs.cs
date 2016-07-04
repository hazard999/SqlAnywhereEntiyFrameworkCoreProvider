
// Type: iAnywhere.Data.SQLAnywhere.SAInfoMessageEventArgs
// Assembly: iAnywhere.Data.SQLAnywhere.v4.0, Version=11.0.1.27424, Culture=neutral, PublicKeyToken=f222fc4333e0d400
// MVID: CC4F9F8C-E618-49D1-9147-C06A9EF53D1F
// Assembly location: C:\Program Files\SQL Anywhere 11\Assembly\V4\iAnywhere.Data.SQLAnywhere.v4.0.dll

using System;

namespace iAnywhere.Data.SQLAnywhere
{
  /// <summary>
  ///     <para>Provides data for the InfoMessage event.</para>
  /// </summary>
  /// <remarks>
  ///     <para>There is no constructor for SAInfoMessageEventArgs.</para>
  /// </remarks>
  public sealed class SAInfoMessageEventArgs : EventArgs
  {
    private int _objectId = SAInfoMessageEventArgs.s_CurrentId++;
    private int _nativeError;
    private string _msg;
    private SAMessageType _msgType;
    private static int s_CurrentId;

    /// <summary>
    ///     <para>Returns the type of the message. This can be one of: Action, Info, Status, or Warning.</para>
    /// </summary>
    public SAMessageType MessageType
    {
      get
      {
        SATrace.PropertyCall("<sa.SAInfoMessageEventArgs.get_MessageType|API>", _objectId);
        return _msgType;
      }
    }

    /// <summary>
    ///     <para>Returns the collection of messages sent from the data source.</para>
    /// </summary>
    public SAErrorCollection Errors
    {
      get
      {
        SATrace.PropertyCall("<sa.SAInfoMessageEventArgs.get_Errors|API>", _objectId);
        return null;
      }
    }

    /// <summary>
    ///     <para>Returns the full text of the error sent from the data source.</para>
    /// </summary>
    public string Message
    {
      get
      {
        SATrace.PropertyCall("<sa.SAInfoMessageEventArgs.get_Message|API>", _objectId);
        return _msg;
      }
    }

    /// <summary>
    ///     <para>Returns the name of the SQL Anywhere .NET Data Provider.</para>
    /// </summary>
    public string Source
    {
      get
      {
        SATrace.PropertyCall("<sa.SAInfoMessageEventArgs.get_Source|API>", _objectId);
        return "SQL Anywhere .NET Data Provider";
      }
    }

    /// <summary>
    ///     <para>Returns the SQL code returned by the database.</para>
    /// </summary>
    public int NativeError
    {
      get
      {
        SATrace.PropertyCall("<sa.SAInfoMessageEventArgs.get_NativeError|API>", _objectId);
        return _nativeError;
      }
    }

    internal SAInfoMessageEventArgs(SAMessageType msgType, int nativeError, string msg)
    {
            _msgType = msgType;
            _nativeError = nativeError;
            _msg = msg;
    }

    /// <summary>
    ///     <para>Retrieves a string representation of the InfoMessage event.</para>
    /// </summary>
    /// <returns>
    /// <para>A string representing the InfoMessage event.</para>
    ///    </returns>
    public override string ToString()
    {
      return Message;
    }
  }
}
