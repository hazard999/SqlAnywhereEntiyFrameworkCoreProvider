
// Type: iAnywhere.Data.SQLAnywhere.SARowsCopiedEventArgs
// Assembly: iAnywhere.Data.SQLAnywhere.v4.0, Version=11.0.1.27424, Culture=neutral, PublicKeyToken=f222fc4333e0d400
// MVID: CC4F9F8C-E618-49D1-9147-C06A9EF53D1F
// Assembly location: C:\Program Files\SQL Anywhere 11\Assembly\V4\iAnywhere.Data.SQLAnywhere.v4.0.dll

namespace iAnywhere.Data.SQLAnywhere
{
  /// <summary>
  ///     <para>Represents the set of arguments passed to the SARowsCopiedEventHandler.</para>
  /// </summary>
  /// <remarks>
  ///     <para><b>Restrictions:</b> The SARowsCopiedEventArgs class is not available in the .NET Compact Framework 2.0.</para>
  /// </remarks>
  public sealed class SARowsCopiedEventArgs
  {
    private long _rowsCopied;
    private bool _shouldAbort;

    /// <summary>
    ///     <para>Gets or sets a value that indicates whether the bulk-copy operation should be aborted.</para>
    /// </summary>
    /// <remarks>
    ///     <para><b>Restrictions:</b> The SARowsCopiedEventArgs class is not available in the .NET Compact Framework 2.0.</para>
    /// </remarks>
    public bool Abort
    {
      get
      {
        return _shouldAbort;
      }
      set
      {
                _shouldAbort = value;
      }
    }

    /// <summary>
    ///     <para>Gets the number of rows copied during the current bulk-copy operation.</para>
    /// </summary>
    /// <remarks>
    ///     <para><b>Restrictions:</b> The SARowsCopiedEventArgs class is not available in the .NET Compact Framework 2.0.</para>
    /// </remarks>
    public long RowsCopied
    {
      get
      {
        return _rowsCopied;
      }
    }

    /// <summary>
    ///     <para>Creates a new instance of the SARowsCopiedEventArgs object.</para>
    /// </summary>
    /// <remarks>
    ///     <para><b>Restrictions:</b> The SARowsCopiedEventArgs class is not available in the .NET Compact Framework 2.0.</para>
    /// </remarks>
    /// <param name="rowsCopied">
    ///     An 64-bit integer value that indicates the number of rows copied during the current bulk-copy operation.
    /// </param>
    public SARowsCopiedEventArgs(long rowsCopied)
    {
            _rowsCopied = rowsCopied;
    }
  }
}
