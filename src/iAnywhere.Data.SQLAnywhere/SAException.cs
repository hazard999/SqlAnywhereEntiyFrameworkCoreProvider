
// Type: iAnywhere.Data.SQLAnywhere.SAException
// Assembly: iAnywhere.Data.SQLAnywhere.v4.0, Version=11.0.1.27424, Culture=neutral, PublicKeyToken=f222fc4333e0d400
// MVID: CC4F9F8C-E618-49D1-9147-C06A9EF53D1F
// Assembly location: C:\Program Files\SQL Anywhere 11\Assembly\V4\iAnywhere.Data.SQLAnywhere.v4.0.dll

using System;
using System.Data.Common;
using System.Runtime.Serialization;
using System.Text;

namespace iAnywhere.Data.SQLAnywhere
{
  /// <summary>
  ///     <para>The exception that is thrown when SQL Anywhere returns a warning or error.</para>
  /// </summary>
  /// <remarks>
  ///             <para>There is no constructor for SAException. Typically, an SAException object is declared in a catch. For example:</para>
  ///             <code>...
  /// catch( SAException ex )
  /// {
  ///     MessageBox.Show( ex.Errors[0].Message, "Error" );
  /// }</code>
  ///             <para>For information about error handling, see @olink targetdoc="programming" targetptr="error-adodotnet-development"@Error handling and the SQL Anywhere .NET Data Provider@/olink@.</para>
  /// 
  ///         </remarks>
  [Serializable]
  public class SAException : DbException
  {
    private SAErrorCollection _errors;

    /// <summary>
    ///     <para>Returns a collection of one or more <see cref="T:iAnywhere.Data.SQLAnywhere.SAError" /> objects.</para>
    /// </summary>
    /// <remarks>
    ///     <para>The SAErrorCollection object always contains at least one instance of the SAError object.</para>
    /// </remarks>
    /// <seealso cref="T:iAnywhere.Data.SQLAnywhere.SAErrorCollection" />
    /// <seealso cref="T:iAnywhere.Data.SQLAnywhere.SAError" />
    public SAErrorCollection Errors
    {
      get
      {
        return _errors;
      }
    }

    /// <summary>
    ///     <para>Returns the text describing the error.</para>
    /// </summary>
    /// <remarks>
    ///     <para>This method returns a single string that contains a concatenation of all of the Message properties of all of the SAError objects in the Errors collection. Each message, except the last one, is followed by a carriage return.</para>
    /// </remarks>
    /// <seealso cref="T:iAnywhere.Data.SQLAnywhere.SAError" />
    public override string Message
    {
      get
      {
        StringBuilder stringBuilder = new StringBuilder();
        for (int index = 0; index < _errors.Count; ++index)
        {
          stringBuilder.Append(_errors[index].Message);
          if (index < _errors.Count - 1)
            stringBuilder.Append("\r\n");
        }
        return stringBuilder.ToString();
      }
    }

    /// <summary>
    ///     <para>Returns the name of the provider that generated the error.</para>
    /// </summary>
    public override string Source
    {
      get
      {
        return _errors[0].Source;
      }
    }

    /// <summary>
    ///     <para>Returns database-specific error information.</para>
    /// </summary>
    public int NativeError
    {
      get
      {
        return _errors[0].NativeError;
      }
    }

    private SAException()
    {
    }

    internal SAException(string message)
    {
            _errors = new SAErrorCollection();
            _errors.Add(new SAError(0, message, "00000"));
    }

    private SAException(SerializationInfo si, StreamingContext context)
    {
            _errors = (SAErrorCollection) si.GetValue("SAErrors", typeof (SAErrorCollection));
    }

    /// <summary>
    ///     <para>Sets the SerializationInfo with information about the exception. Overrides <see cref="M:System.Exception.GetObjectData(System.Runtime.Serialization.SerializationInfo,System.Runtime.Serialization.StreamingContext)" />.</para>
    /// </summary>
    /// <param name="info">
    ///     The SerializationInfo that holds the serialized object data about the exception being thrown.
    /// </param>
    /// <param name="context">
    ///     The StreamingContext that contains contextual information about the source or destination.
    /// </param>
    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
      if (info == null)
      {
        Exception e = new ArgumentNullException("info");
        SATrace.Exception(e);
        throw e;
      }
      info.AddValue("SAErrors", _errors, typeof (SAErrorCollection));
      base.GetObjectData(info, context);
    }

    internal static bool IsException(int idEx)
    {
      return idEx >= 1;
    }

    internal static void CheckException(int idEx)
    {
      if (SAException.IsException(idEx))
      {
        SAException saException = new SAException();
        saException._errors = SAErrorCollection.GetErrors(idEx);
        SATrace.Exception(saException);
        throw saException;
      }
    }

    internal static SAException CreateInstance(int idEx)
    {
      return new SAException() { _errors = SAErrorCollection.GetErrors(idEx) };
    }

    internal static void FreeException(int idEx)
    {
      SAErrorCollection.FreeException(idEx);
    }
  }
}
