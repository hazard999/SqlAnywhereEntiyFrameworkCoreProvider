
// Type: iAnywhere.Data.SQLAnywhere.SAIsolationLevel
// Assembly: iAnywhere.Data.SQLAnywhere.v4.0, Version=11.0.1.27424, Culture=neutral, PublicKeyToken=f222fc4333e0d400
// MVID: CC4F9F8C-E618-49D1-9147-C06A9EF53D1F
// Assembly location: C:\Program Files\SQL Anywhere 11\Assembly\V4\iAnywhere.Data.SQLAnywhere.v4.0.dll

namespace iAnywhere.Data.SQLAnywhere
{
  /// <summary>
  ///     <para>Specifies SQL Anywhere isolation levels. This class augments the <see cref="T:System.Data.IsolationLevel" /> class.</para>
  /// </summary>
  /// <remarks>
  ///             <para>The SQL Anywhere .NET Data Provider supports all SQL Anywhere isolation levels, including the snapshot isolation levels. To use snapshot isolation, specify one of SAIsolationLevel.Snapshot, SAIsolationLevel.ReadOnlySnapshot, or SAIsolationLevel.StatementSnapshot as the parameter to BeginTransaction. BeginTransaction has been overloaded so it can take either an IsolationLevel or an SAIsolationLevel. The values in the two enumerations are the same, except for ReadOnlySnapshot and StatementS
  /// napshot which exist only in SAIsolationLevel. There is a new property in SATransaction called SAIsolationLevel that gets the SAIsolationLevel.</para>
  ///             <para>For more information, see @olink targetdoc="sqlug" targetptr="transact-s-4136352"@Snapshot isolation@/olink@.</para>
  ///         </remarks>
  public enum SAIsolationLevel
  {
    Unspecified = -1,
    Chaos = 16,
    ReadUncommitted = 256,
    ReadCommitted = 4096,
    RepeatableRead = 65536,
    Serializable = 1048576,
    Snapshot = 16777216,
    ReadOnlySnapshot = 16777217,
    StatementSnapshot = 16777218,
  }
}
