
// Type: iAnywhere.Data.SQLAnywhere.SymbolTable
// Assembly: iAnywhere.Data.SQLAnywhere.v4.0, Version=11.0.1.27424, Culture=neutral, PublicKeyToken=f222fc4333e0d400
// MVID: CC4F9F8C-E618-49D1-9147-C06A9EF53D1F
// Assembly location: C:\Program Files\SQL Anywhere 11\Assembly\V4\iAnywhere.Data.SQLAnywhere.v4.0.dll

using System;
using System.Collections.Generic;

namespace iAnywhere.Data.SQLAnywhere
{
  /// <summary>
  /// The symbol table is quite primitive - it is a stack with a new entry for
  /// each scope.  Lookups search from the top of the stack to the bottom, until
  /// an entry is found.
  /// 
  /// The symbols are of the following kinds
  /// <list type="bullet">
  /// <item><see cref="T:iAnywhere.Data.SQLAnywhere.Symbol" /> represents tables (extents/nested selects/unnests)</item>
  /// <item><see cref="T:iAnywhere.Data.SQLAnywhere.JoinSymbol" /> represents Join nodes</item>
  /// <item><see cref="T:iAnywhere.Data.SQLAnywhere.Symbol" /> columns.</item>
  /// </list>
  /// 
  /// Symbols represent names <see cref="M:iAnywhere.Data.SQLAnywhere.SqlGenerator.Visit(System.Data.Common.CommandTrees.DbVariableReferenceExpression)" /> to be resolved,
  /// or things to be renamed.
  /// </summary>
  internal sealed class SymbolTable
  {
    private List<Dictionary<string, Symbol>> symbols = new List<Dictionary<string, Symbol>>();

    internal void EnterScope()
    {
            symbols.Add(new Dictionary<string, Symbol>(StringComparer.OrdinalIgnoreCase));
    }

    internal void ExitScope()
    {
            symbols.RemoveAt(symbols.Count - 1);
    }

    internal void Add(string name, Symbol value)
    {
            symbols[symbols.Count - 1][name] = value;
    }

    internal Symbol Lookup(string name)
    {
      for (int index = symbols.Count - 1; index >= 0; --index)
      {
        if (symbols[index].ContainsKey(name))
          return symbols[index][name];
      }
      return null;
    }
  }
}
