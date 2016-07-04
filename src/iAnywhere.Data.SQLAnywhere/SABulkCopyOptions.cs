
// Type: iAnywhere.Data.SQLAnywhere.SABulkCopyOptions
// Assembly: iAnywhere.Data.SQLAnywhere.v4.0, Version=11.0.1.27424, Culture=neutral, PublicKeyToken=f222fc4333e0d400
// MVID: CC4F9F8C-E618-49D1-9147-C06A9EF53D1F
// Assembly location: C:\Program Files\SQL Anywhere 11\Assembly\V4\iAnywhere.Data.SQLAnywhere.v4.0.dll

using System;

namespace iAnywhere.Data.SQLAnywhere
{
  /// <summary>
  ///     <para>A bitwise flag that specifies one or more options to use with an instance of SABulkCopy.</para>
  /// </summary>
  /// <remarks>
  ///     <para>The SABulkCopyOptions enumeration is used when you construct an SABulkCopy object to specify how the WriteToServer methods will behave. </para>
  ///     <para><b>Restrictions:</b> The SABulkCopyOptions class is not available in the .NET Compact Framework 2.0.</para>
  ///     <para>The CheckConstraints and KeepNulls options are not supported.</para>
  /// </remarks>
  /// <seealso cref="T:iAnywhere.Data.SQLAnywhere.SABulkCopy" />
  [Flags]
  public enum SABulkCopyOptions
  {
    Default = 0,
    DoNotFireTriggers = 1,
    KeepIdentity = 2,
    TableLock = 4,
    UseInternalTransaction = 8,
  }
}
