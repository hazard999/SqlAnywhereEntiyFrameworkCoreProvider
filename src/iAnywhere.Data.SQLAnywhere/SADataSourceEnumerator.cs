
// Type: iAnywhere.Data.SQLAnywhere.SADataSourceEnumerator
// Assembly: iAnywhere.Data.SQLAnywhere.v4.0, Version=11.0.1.27424, Culture=neutral, PublicKeyToken=f222fc4333e0d400
// MVID: CC4F9F8C-E618-49D1-9147-C06A9EF53D1F
// Assembly location: C:\Program Files\SQL Anywhere 11\Assembly\V4\iAnywhere.Data.SQLAnywhere.v4.0.dll

using System;
using System.Data;
using System.Data.Common;

namespace iAnywhere.Data.SQLAnywhere
{
  /// <summary>
  ///     <para>Provides a mechanism for enumerating all available instances of SQL Anywhere database servers within the local network.</para>
  /// </summary>
  /// <remarks>
  ///     <para>There is no constructor for SADataSourceEnumerator.</para>
  ///     <para>The SADataSourceEnumerator class is not available in the .NET Compact Framework 2.0.</para>
  /// </remarks>
  public sealed class SADataSourceEnumerator : DbDataSourceEnumerator
  {
    private static SADataSourceEnumerator s_instance = null;
    private static SAUnmanagedDll s_unmanagedDll = SAUnmanagedDll.Instance;
    private static string[] c_ColumnNames = new string[4]{ "ServerName", "IPAddress", "PortNumber", "DataBaseNames" };
    private static Type[] c_ColumnTypes = new Type[4]{ typeof (string), typeof (string), typeof (int), typeof (string) };

    /// <summary>
    ///     <para>Gets an instance of SADataSourceEnumberator, which can be used to retrieve information about all visible SQL Anywhere database servers.</para>
    /// </summary>
    public static SADataSourceEnumerator Instance
    {
      get
      {
        if (SADataSourceEnumerator.s_instance == null)
          SADataSourceEnumerator.s_instance = new SADataSourceEnumerator();
        return SADataSourceEnumerator.s_instance;
      }
    }

    internal SADataSourceEnumerator()
    {
    }

    /// <summary>
    ///     <para>Retrieves a DataTable containing information about all visible SQL Anywhere database servers.</para>
    /// </summary>
    /// <remarks>
    ///     <para>The returned table has four columns: ServerName, IPAddress, PortNumber, and DataBaseNames. There is a row in the table for each available database server.</para>
    /// </remarks>
    /// <example>
    ///     <para>The following code fills a DataTable with information for each database server that is available.</para>
    ///     <code>DataTable servers = SADataSourceEnumerator.Instance.GetDataSources();</code>
    /// 
    /// </example>
    public override unsafe DataTable GetDataSources()
    {
      try
      {
        SATrace.FunctionScopeEnter("<sa.SADataSourceEnumerator.GetDataSources|API>");
        DataTable dataTable = EmptyResultTable();
        IntPtr result = IntPtr.Zero;
        int numResults = 0;
        int dataSources = PInvokeMethods.SADataSourceEnumerator_GetDataSources(ref numResults, ref result);
        if (SAException.IsException(dataSources))
        {
          SAException saException = new SAException(SARes.GetString(11030, "SADataSourceEnumerator.GetDataSources"));
          SAErrorCollection.FreeException(dataSources);
          SATrace.Exception(saException);
          throw saException;
        }
        if (numResults > 0)
        {
          SAServerInfo* saServerInfoPtr = (SAServerInfo*) (void*) result;
          DataRowCollection rows = dataTable.Rows;
          for (int index = 0; index < numResults; ++index)
          {
            rows.Add(saServerInfoPtr->ToObjArray());
            ++saServerInfoPtr;
          }
        }
        SAException.CheckException(PInvokeMethods.SADataSourceEnumerator_FreeResults(numResults, result));
        return dataTable;
      }
      finally
      {
        SATrace.FunctionScopeLeave();
      }
    }

    private DataTable EmptyResultTable()
    {
      DataTable dataTable = new DataTable();
      DataColumnCollection columns = dataTable.Columns;
      for (int index = 0; index < 4; ++index)
        columns.Add(SADataSourceEnumerator.c_ColumnNames[index], SADataSourceEnumerator.c_ColumnTypes[index]);
      return dataTable;
    }
  }
}
