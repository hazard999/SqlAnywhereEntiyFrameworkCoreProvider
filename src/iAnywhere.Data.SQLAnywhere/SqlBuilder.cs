
// Type: iAnywhere.Data.SQLAnywhere.SqlBuilder
// Assembly: iAnywhere.Data.SQLAnywhere.v4.0, Version=11.0.1.27424, Culture=neutral, PublicKeyToken=f222fc4333e0d400
// MVID: CC4F9F8C-E618-49D1-9147-C06A9EF53D1F
// Assembly location: C:\Program Files\SQL Anywhere 11\Assembly\V4\iAnywhere.Data.SQLAnywhere.v4.0.dll

using System;
using System.Collections.Generic;

namespace iAnywhere.Data.SQLAnywhere
{
    /// <summary>
    /// This class is like StringBuilder.  While traversing the tree for the first time,
    /// we do not know all the strings that need to be appended e.g. things that need to be
    /// renamed, nested select statements etc.  So, we use a builder that can collect
    /// all kinds of sql fragments.
    /// </summary>
    internal sealed class SqlBuilder : ISqlFragment
    {
        private List<object> _sqlFragments;

        private List<object> sqlFragments
        {
            get
            {
                if (_sqlFragments == null)
                    _sqlFragments = new List<object>();
                return _sqlFragments;
            }
        }

        /// <summary>
        /// Whether the builder is empty.  This is used by the <see cref="M:iAnywhere.Data.SQLAnywhere.SqlGenerator.Visit(System.Data.Common.CommandTrees.DbProjectExpression)" />
        /// to determine whether a sql statement can be reused.
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                if (_sqlFragments != null)
                    return 0 == _sqlFragments.Count;
                return true;
            }
        }

        /// <summary>
        /// Add an object to the list - we do not verify that it is a proper sql fragment
        /// since this is an internal method.
        /// </summary>
        /// <param name="s"></param>
        public void Append(object s)
        {
            sqlFragments.Add(s);
        }

        /// <summary>Insert an object to the list</summary>
        /// <param name="index"></param>
        /// <param name="s"></param>
        public void Insert(int index, object s)
        {
            sqlFragments.Insert(index, s);
        }

        /// <summary>
        /// This is to pretty print the SQL.  The writer <see cref="M:iAnywhere.Data.SQLAnywhere.SqlWriter.Write(System.String)" />
        /// needs to know about new lines so that it can add the right amount of
        /// indentation at the beginning of lines.
        /// </summary>
        public void AppendLine()
        {
            sqlFragments.Add("\r\n");
        }

        /// <summary>
        /// We delegate the writing of the fragment to the appropriate type.
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="sqlGenerator"></param>
        public void WriteSql(SqlWriter writer, SqlGenerator sqlGenerator)
        {
            if (_sqlFragments == null)
                return;
            foreach (object sqlFragment1 in _sqlFragments)
            {
                string str = sqlFragment1 as string;
                if (str != null)
                {
                    writer.Write(str);
                }
                else
                {
                    ISqlFragment sqlFragment2 = sqlFragment1 as ISqlFragment;
                    if (sqlFragment2 == null)
                        throw new InvalidOperationException();
                    sqlFragment2.WriteSql(writer, sqlGenerator);
                }
            }
        }
    }
}
