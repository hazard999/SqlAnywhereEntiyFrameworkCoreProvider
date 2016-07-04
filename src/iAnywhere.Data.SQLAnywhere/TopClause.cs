
// Type: iAnywhere.Data.SQLAnywhere.TopClause
// Assembly: iAnywhere.Data.SQLAnywhere.v4.0, Version=11.0.1.27424, Culture=neutral, PublicKeyToken=f222fc4333e0d400
// MVID: CC4F9F8C-E618-49D1-9147-C06A9EF53D1F
// Assembly location: C:\Program Files\SQL Anywhere 11\Assembly\V4\iAnywhere.Data.SQLAnywhere.v4.0.dll

using System;
using System.Globalization;

namespace iAnywhere.Data.SQLAnywhere
{
  /// <summary>
  /// TopClause represents the a TOP expression in a SqlSelectStatement.
  /// It has a count property, which indicates how many TOP rows should be selected and a
  /// boolen WithTies property.
  /// </summary>
  internal class TopClause : ISqlFragment
  {
    private ISqlFragment topCount;
    private bool withTies;

    /// <summary>Do we need to add a WITH_TIES to the top statement</summary>
    internal bool WithTies
    {
      get
      {
        return withTies;
      }
    }

    /// <summary>How many top rows should be selected.</summary>
    internal ISqlFragment TopCount
    {
      get
      {
        return topCount;
      }
    }

    /// <summary>
    /// Creates a TopClause with the given topCount and withTies.
    /// </summary>
    /// <param name="topCount"></param>
    /// <param name="withTies"></param>
    internal TopClause(ISqlFragment topCount, bool withTies)
    {
      this.topCount = topCount;
      this.withTies = withTies;
    }

    /// <summary>
    /// Creates a TopClause with the given topCount and withTies.
    /// </summary>
    /// <param name="topCount"></param>
    /// <param name="withTies"></param>
    internal TopClause(int topCount, bool withTies)
    {
      SqlBuilder sqlBuilder = new SqlBuilder();
      sqlBuilder.Append(topCount.ToString((IFormatProvider)CultureInfo.InvariantCulture));
      this.topCount = sqlBuilder;
      this.withTies = withTies;
    }

    /// <summary>
    /// Write out the TOP part of sql select statement
    /// It basically writes TOP (X) [WITH TIES].
    /// </summary>
    /// <param name="writer"></param>
    /// <param name="sqlGenerator"></param>
    public void WriteSql(SqlWriter writer, SqlGenerator sqlGenerator)
    {
      writer.Write("TOP (");
            TopCount.WriteSql(writer, sqlGenerator);
      writer.Write(")");
      writer.Write(" ");
      if (!WithTies)
        return;
      writer.Write("WITH TIES ");
    }
  }
}
