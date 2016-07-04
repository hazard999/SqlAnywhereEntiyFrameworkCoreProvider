
// Type: iAnywhere.Data.SQLAnywhere.ISqlFragment
// Assembly: iAnywhere.Data.SQLAnywhere.v4.0, Version=11.0.1.27424, Culture=neutral, PublicKeyToken=f222fc4333e0d400
// MVID: CC4F9F8C-E618-49D1-9147-C06A9EF53D1F
// Assembly location: C:\Program Files\SQL Anywhere 11\Assembly\V4\iAnywhere.Data.SQLAnywhere.v4.0.dll

namespace iAnywhere.Data.SQLAnywhere
{
  /// <summary>
  /// Represents the sql fragment for any node in the query tree.
  /// </summary>
  /// <remarks>
  /// The nodes in a query tree produce various kinds of sql
  /// <list type="bullet">
  /// <item>A select statement.</item>
  /// <item>A reference to an extent. (symbol)</item>
  /// <item>A raw string.</item>
  /// </list>
  /// We have this interface to allow for a common return type for the methods
  /// in the expression visitor <see cref="T:System.Data.Common.CommandTrees.DbExpressionVisitor`1" />
  /// 
  /// At the end of translation, the sql fragments are converted into real strings.
  /// </remarks>
  internal interface ISqlFragment
  {
    /// <summary>
    /// Write the string represented by this fragment into the stream.
    /// </summary>
    /// <param name="writer">The stream that collects the strings.</param>
    /// <param name="sqlGenerator">Context information used for renaming.
    /// The global lists are used to generated new names without collisions.</param>
    void WriteSql(SqlWriter writer, SqlGenerator sqlGenerator);
  }
}
