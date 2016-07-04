
// Type: iAnywhere.Data.SQLAnywhere.SqlSelectStatement
// Assembly: iAnywhere.Data.SQLAnywhere.v4.0, Version=11.0.1.27424, Culture=neutral, PublicKeyToken=f222fc4333e0d400
// MVID: CC4F9F8C-E618-49D1-9147-C06A9EF53D1F
// Assembly location: C:\Program Files\SQL Anywhere 11\Assembly\V4\iAnywhere.Data.SQLAnywhere.v4.0.dll

using System;
using System.Collections.Generic;
using System.Globalization;

namespace iAnywhere.Data.SQLAnywhere
{
  /// <summary>
  /// A SqlSelectStatement represents a canonical SQL SELECT statement.
  /// It has fields for the 5 main clauses
  /// <list type="number">
  /// <item>SELECT</item>
  /// <item>FROM</item>
  /// <item>WHERE</item>
  /// <item>GROUP BY</item>
  /// <item>ORDER BY</item>
  /// </list>
  /// We do not have HAVING, since it does not correspond to anything in the DbCommandTree.
  /// Each of the fields is a SqlBuilder, so we can keep appending SQL strings
  /// or other fragments to build up the clause.
  /// 
  /// We have a IsDistinct property to indicate that we want distict columns.
  /// This is given out of band, since the input expression to the select clause
  /// may already have some columns projected out, and we use append-only SqlBuilders.
  /// The DISTINCT is inserted when we finally write the object into a string.
  /// 
  /// Also, we have a Top property, which is non-null if the number of results should
  /// be limited to certain number. It is given out of band for the same reasons as DISTINCT.
  /// 
  /// The FromExtents contains the list of inputs in use for the select statement.
  /// There is usually just one element in this - Select statements for joins may
  /// temporarily have more than one.
  /// 
  /// If the select statement is created by a Join node, we maintain a list of
  /// all the extents that have been flattened in the join in AllJoinExtents
  /// <example>
  /// in J(j1= J(a,b), c)
  /// FromExtents has 2 nodes JoinSymbol(name=j1, ...) and Symbol(name=c)
  /// AllJoinExtents has 3 nodes Symbol(name=a), Symbol(name=b), Symbol(name=c)
  /// </example>
  /// 
  /// If any expression in the non-FROM clause refers to an extent in a higher scope,
  /// we add that extent to the OuterExtents list.  This list denotes the list
  /// of extent aliases that may collide with the aliases used in this select statement.
  /// It is set by <see cref="M:iAnywhere.Data.SQLAnywhere.SqlGenerator.Visit(System.Data.Common.CommandTrees.DbVariableReferenceExpression)" />.
  /// An extent is an outer extent if it is not one of the FromExtents.
  /// 
  /// 
  /// </summary>
  internal sealed class SqlSelectStatement : ISqlFragment
  {
    private SqlBuilder select = new SqlBuilder();
    private SqlBuilder from = new SqlBuilder();
    private bool isDistinct;
    private List<Symbol> allJoinExtents;
    private List<Symbol> fromExtents;
    private Dictionary<Symbol, bool> outerExtents;
    private TopClause top;
    private SqlBuilder where;
    private SqlBuilder groupBy;
    private SqlBuilder orderBy;
    private bool isTopMost;

    /// <summary>
    /// Do we need to add a DISTINCT at the beginning of the SELECT
    /// </summary>
    internal bool IsDistinct
    {
      get
      {
        return isDistinct;
      }
      set
      {
                isDistinct = value;
      }
    }

    internal List<Symbol> AllJoinExtents
    {
      get
      {
        return allJoinExtents;
      }
      set
      {
                allJoinExtents = value;
      }
    }

    internal List<Symbol> FromExtents
    {
      get
      {
        if (fromExtents == null)
                    fromExtents = new List<Symbol>();
        return fromExtents;
      }
    }

    internal Dictionary<Symbol, bool> OuterExtents
    {
      get
      {
        if (outerExtents == null)
                    outerExtents = new Dictionary<Symbol, bool>();
        return outerExtents;
      }
    }

    internal TopClause Top
    {
      get
      {
        return top;
      }
      set
      {
                top = value;
      }
    }

    internal SqlBuilder Select
    {
      get
      {
        return select;
      }
    }

    internal SqlBuilder From
    {
      get
      {
        return from;
      }
    }

    internal SqlBuilder Where
    {
      get
      {
        if (where == null)
                    where = new SqlBuilder();
        return where;
      }
    }

    internal SqlBuilder GroupBy
    {
      get
      {
        if (groupBy == null)
                    groupBy = new SqlBuilder();
        return groupBy;
      }
    }

    public SqlBuilder OrderBy
    {
      get
      {
        if (orderBy == null)
                    orderBy = new SqlBuilder();
        return orderBy;
      }
    }

    internal bool IsTopMost
    {
      get
      {
        return isTopMost;
      }
      set
      {
                isTopMost = value;
      }
    }

    /// <summary>
    /// Write out a SQL select statement as a string.
    /// We have to
    /// <list type="number">
    /// <item>Check whether the aliases extents we use in this statement have
    /// to be renamed.
    /// We first create a list of all the aliases used by the outer extents.
    /// For each of the FromExtents( or AllJoinExtents if it is non-null),
    /// rename it if it collides with the previous list.
    /// </item>
    /// <item>Write each of the clauses (if it exists) as a string</item>
    /// </list>
    /// </summary>
    /// <param name="writer"></param>
    /// <param name="sqlGenerator"></param>
    public void WriteSql(SqlWriter writer, SqlGenerator sqlGenerator)
    {
      List<string> stringList = null;
      if (outerExtents != null && 0 < outerExtents.Count)
      {
        foreach (Symbol key in outerExtents.Keys)
        {
          JoinSymbol joinSymbol = key as JoinSymbol;
          if (joinSymbol != null)
          {
            foreach (Symbol flattenedExtent in joinSymbol.FlattenedExtentList)
            {
              if (stringList == null)
                stringList = new List<string>();
              stringList.Add(flattenedExtent.NewName);
            }
          }
          else
          {
            if (stringList == null)
              stringList = new List<string>();
            stringList.Add(key.NewName);
          }
        }
      }
      List<Symbol> symbolList = AllJoinExtents ?? fromExtents;
      if (symbolList != null)
      {
        foreach (Symbol symbol in symbolList)
        {
          if (stringList != null && stringList.Contains(symbol.Name))
          {
            int num = sqlGenerator.AllExtentNames[symbol.Name];
            string key;
            do
            {
              ++num;
              key = symbol.Name + num.ToString(CultureInfo.InvariantCulture);
            }
            while (sqlGenerator.AllExtentNames.ContainsKey(key));
            sqlGenerator.AllExtentNames[symbol.Name] = num;
            symbol.NewName = key;
            sqlGenerator.AllExtentNames[key] = 0;
          }
          if (stringList == null)
            stringList = new List<string>();
          stringList.Add(symbol.NewName);
        }
      }
      ++writer.Indent;
      writer.Write("SELECT ");
      if (IsDistinct)
        writer.Write("DISTINCT ");
      if (Top != null)
                Top.WriteSql(writer, sqlGenerator);
      if (select == null || Select.IsEmpty)
        writer.Write("*");
      else
                Select.WriteSql(writer, sqlGenerator);
      writer.WriteLine();
      writer.Write("FROM ");
            From.WriteSql(writer, sqlGenerator);
      if (where != null && !Where.IsEmpty)
      {
        writer.WriteLine();
        writer.Write("WHERE ");
                Where.WriteSql(writer, sqlGenerator);
      }
      if (groupBy != null && !GroupBy.IsEmpty)
      {
        writer.WriteLine();
        writer.Write("GROUP BY ");
                GroupBy.WriteSql(writer, sqlGenerator);
      }
      if (orderBy != null && !OrderBy.IsEmpty)
      {
        writer.WriteLine();
        writer.Write("ORDER BY ");
                OrderBy.WriteSql(writer, sqlGenerator);
      }
      --writer.Indent;
    }
  }
}
