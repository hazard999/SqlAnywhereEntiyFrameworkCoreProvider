
// Type: iAnywhere.Data.SQLAnywhere.SARowsCopiedEventHandler
// Assembly: iAnywhere.Data.SQLAnywhere.v4.0, Version=11.0.1.27424, Culture=neutral, PublicKeyToken=f222fc4333e0d400
// MVID: CC4F9F8C-E618-49D1-9147-C06A9EF53D1F
// Assembly location: C:\Program Files\SQL Anywhere 11\Assembly\V4\iAnywhere.Data.SQLAnywhere.v4.0.dll

namespace iAnywhere.Data.SQLAnywhere
{
  /// <summary>
  ///     <para>Represents the method that handles the SABulkCopy.SARowsCopied event of an SABulkCopy.</para>
  /// </summary>
  /// <remarks>
  ///     <para><b>Restrictions:</b> The SARowsCopiedEventHandler delegate is not available in the .NET Compact Framework 2.0.</para>
  /// </remarks>
  /// <seealso cref="T:iAnywhere.Data.SQLAnywhere.SABulkCopy" />
  public delegate void SARowsCopiedEventHandler(object sender, SARowsCopiedEventArgs rowsCopiedEventArgs);
}
