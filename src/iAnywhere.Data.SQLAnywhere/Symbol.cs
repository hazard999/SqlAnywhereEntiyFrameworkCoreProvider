
// Type: iAnywhere.Data.SQLAnywhere.Symbol
// Assembly: iAnywhere.Data.SQLAnywhere.v4.0, Version=11.0.1.27424, Culture=neutral, PublicKeyToken=f222fc4333e0d400
// MVID: CC4F9F8C-E618-49D1-9147-C06A9EF53D1F
// Assembly location: C:\Program Files\SQL Anywhere 11\Assembly\V4\iAnywhere.Data.SQLAnywhere.v4.0.dll

using System;
using System.Collections.Generic;
using System.Data.Metadata.Edm;
using System.Globalization;

namespace iAnywhere.Data.SQLAnywhere
{
  /// <summary>
  /// <see cref="T:iAnywhere.Data.SQLAnywhere.SymbolTable" />
  /// This class represents an extent/nested select statement,
  /// or a column.
  /// 
  /// The important fields are Name, Type and NewName.
  /// NewName starts off the same as Name, and is then modified as necessary.
  /// 
  /// 
  /// The rest are used by special symbols.
  /// e.g. NeedsRenaming is used by columns to indicate that a new name must
  /// be picked for the column in the second phase of translation.
  /// 
  /// IsUnnest is used by symbols for a collection expression used as a from clause.
  /// This allows <see cref="M:iAnywhere.Data.SQLAnywhere.SqlGenerator.AddFromSymbol(iAnywhere.Data.SQLAnywhere.SqlSelectStatement,System.String,iAnywhere.Data.SQLAnywhere.Symbol,System.Boolean)" /> to add the column list
  /// after the alias.
  /// 
  /// </summary>
  internal class Symbol : ISqlFragment
  {
    private Dictionary<string, Symbol> columns = new Dictionary<string, Symbol>(StringComparer.CurrentCultureIgnoreCase);
    private bool needsRenaming;
    private bool isUnnest;
    private string name;
    private string newName;
    private TypeUsage type;

    internal Dictionary<string, Symbol> Columns
    {
      get
      {
        return columns;
      }
    }

    internal bool NeedsRenaming
    {
      get
      {
        return needsRenaming;
      }
      set
      {
                needsRenaming = value;
      }
    }

    internal bool IsUnnest
    {
      get
      {
        return isUnnest;
      }
      set
      {
                isUnnest = value;
      }
    }

    public string Name
    {
      get
      {
        return name;
      }
    }

    public string NewName
    {
      get
      {
        return newName;
      }
      set
      {
                newName = value;
      }
    }

    internal TypeUsage Type
    {
      get
      {
        return type;
      }
      set
      {
                type = value;
      }
    }

    public Symbol(string name, TypeUsage type)
    {
      this.name = name;
            newName = name;
            Type = type;
    }

    /// <summary>
    /// Write this symbol out as a string for sql.  This is just
    /// the new name of the symbol (which could be the same as the old name).
    /// 
    /// We rename columns here if necessary.
    /// </summary>
    /// <param name="writer"></param>
    /// <param name="sqlGenerator"></param>
    public void WriteSql(SqlWriter writer, SqlGenerator sqlGenerator)
    {
      if (NeedsRenaming)
      {
        int num = sqlGenerator.AllColumnNames[NewName];
        string key;
        do
        {
          ++num;
          key = Name + num.ToString(CultureInfo.InvariantCulture);
        }
        while (sqlGenerator.AllColumnNames.ContainsKey(key));
        sqlGenerator.AllColumnNames[NewName] = num;
                NeedsRenaming = false;
                NewName = key;
        sqlGenerator.AllColumnNames[key] = 0;
      }
      writer.Write(SqlGenerator.QuoteIdentifier(NewName));
    }
  }
}
