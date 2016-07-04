
// Type: iAnywhere.Data.SQLAnywhere.SymbolPair
// Assembly: iAnywhere.Data.SQLAnywhere.v4.0, Version=11.0.1.27424, Culture=neutral, PublicKeyToken=f222fc4333e0d400
// MVID: CC4F9F8C-E618-49D1-9147-C06A9EF53D1F
// Assembly location: C:\Program Files\SQL Anywhere 11\Assembly\V4\iAnywhere.Data.SQLAnywhere.v4.0.dll

namespace iAnywhere.Data.SQLAnywhere
{
  /// <summary>
  /// The SymbolPair exists to solve the record flattening problem.
  /// <see cref="M:iAnywhere.Data.SQLAnywhere.SqlGenerator.Visit(System.Data.Common.CommandTrees.DbPropertyExpression)" />
  /// Consider a property expression D(v, "j3.j2.j1.a.x")
  /// where v is a VarRef, j1, j2, j3 are joins, a is an extent and x is a columns.
  /// This has to be translated eventually into {j'}.{x'}
  /// 
  /// The source field represents the outermost SqlStatement representing a join
  /// expression (say j2) - this is always a Join symbol.
  /// 
  /// The column field keeps moving from one join symbol to the next, until it
  /// stops at a non-join symbol.
  /// 
  /// This is returned by <see cref="M:iAnywhere.Data.SQLAnywhere.SqlGenerator.Visit(System.Data.Common.CommandTrees.DbPropertyExpression)" />,
  /// but never makes it into a SqlBuilder.
  /// </summary>
  internal class SymbolPair : ISqlFragment
  {
    public Symbol Source;
    public Symbol Column;

    public SymbolPair(Symbol source, Symbol column)
    {
            Source = source;
            Column = column;
    }

    public void WriteSql(SqlWriter writer, SqlGenerator sqlGenerator)
    {
    }
  }
}
