using System;
using System.Collections.Generic;

namespace iAnywhere.Data.SQLAnywhere
{
    /// <summary>
    /// A Join symbol is a special kind of Symbol.
    /// It has to carry additional information
    /// <list type="bullet">
    /// <item>ColumnList for the list of columns in the select clause if this
    /// symbol represents a sql select statement.  This is set by <see cref="M:iAnywhere.Data.SQLAnywhere.SqlGenerator.AddDefaultColumns(iAnywhere.Data.SQLAnywhere.SqlSelectStatement)" />. </item>
    /// <item>ExtentList is the list of extents in the select clause.</item>
    /// <item>FlattenedExtentList - if the Join has multiple extents flattened at the
    /// top level, we need this information to ensure that extent aliases are renamed
    /// correctly in <see cref="M:iAnywhere.Data.SQLAnywhere.SqlSelectStatement.WriteSql(iAnywhere.Data.SQLAnywhere.SqlWriter,iAnywhere.Data.SQLAnywhere.SqlGenerator)" /></item>
    /// <item>NameToExtent has all the extents in ExtentList as a dictionary.
    /// This is used by <see cref="M:iAnywhere.Data.SQLAnywhere.SqlGenerator.Visit(System.Data.Common.CommandTrees.DbPropertyExpression)" /> to flatten
    /// record accesses.</item>
    /// <item>IsNestedJoin - is used to determine whether a JoinSymbol is an
    /// ordinary join symbol, or one that has a corresponding SqlSelectStatement.</item>
    /// </list>
    /// 
    /// All the lists are set exactly once, and then used for lookups/enumerated.
    /// </summary>
    internal sealed class JoinSymbol : Symbol
    {
        private List<Symbol> columnList;
        private List<Symbol> extentList;
        private List<Symbol> flattenedExtentList;
        private Dictionary<string, Symbol> nameToExtent;
        private bool isNestedJoin;

        internal List<Symbol> ColumnList
        {
            get
            {
                if (columnList == null)
                    columnList = new List<Symbol>();
                return columnList;
            }
            set
            {
                columnList = value;
            }
        }

        internal List<Symbol> ExtentList
        {
            get
            {
                return extentList;
            }
        }

        internal List<Symbol> FlattenedExtentList
        {
            get
            {
                if (flattenedExtentList == null)
                    flattenedExtentList = new List<Symbol>();
                return flattenedExtentList;
            }
            set
            {
                flattenedExtentList = value;
            }
        }

        internal Dictionary<string, Symbol> NameToExtent
        {
            get
            {
                return nameToExtent;
            }
        }

        internal bool IsNestedJoin
        {
            get
            {
                return isNestedJoin;
            }
            set
            {
                isNestedJoin = value;
            }
        }

        public JoinSymbol(string name, TypeUsage type, List<Symbol> extents)
          : base(name, type)
        {
            extentList = new List<Symbol>(extents.Count);
            nameToExtent = new Dictionary<string, Symbol>(extents.Count, StringComparer.OrdinalIgnoreCase);
            foreach (Symbol extent in extents)
            {
                nameToExtent[extent.Name] = extent;
                ExtentList.Add(extent);
            }
        }
    }
}
