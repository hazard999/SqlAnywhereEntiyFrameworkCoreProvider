
// Type: iAnywhere.Data.SQLAnywhere.SARowUpdatedEventArgs
// Assembly: iAnywhere.Data.SQLAnywhere.v4.0, Version=11.0.1.27424, Culture=neutral, PublicKeyToken=f222fc4333e0d400
// MVID: CC4F9F8C-E618-49D1-9147-C06A9EF53D1F
// Assembly location: C:\Program Files\SQL Anywhere 11\Assembly\V4\iAnywhere.Data.SQLAnywhere.v4.0.dll

using System.Data;
using System.Data.Common;

namespace iAnywhere.Data.SQLAnywhere
{
  /// <summary>
  ///     <para>Provides data for the RowUpdated event.</para>
  /// </summary>
  public sealed class SARowUpdatedEventArgs : RowUpdatedEventArgs
  {
    internal int _recordsAffected;

    /// <summary>
    ///     <para>Gets the SACommand that is executed when <see cref="M:System.Data.Common.DataAdapter.Update(System.Data.DataSet)" /> is called.</para>
    /// </summary>
    public SACommand Command
    {
      get
      {
        return (SACommand) base.Command;
      }
    }

    /// <summary>
    ///     <para>Returns the number of rows changed, inserted, or deleted by execution of the SQL statement.</para>
    /// </summary>
    /// <value>The number of rows changed, inserted, or deleted; 0 if no rows were affected or the statement failed; and -1 for SELECT statements.</value>
    public new int RecordsAffected
    {
      get
      {
        return _recordsAffected;
      }
    }

    /// <summary>
    ///     <para>Initializes a new instance of the SARowUpdatedEventArgs class.</para>
    /// </summary>
    /// <param name="row">The DataRow sent through an Update.</param>
    /// <param name="command">
    ///     The IDbCommand executed when Update is called.
    /// </param>
    /// <param name="statementType">
    ///     One of the StatementType values that specifies the type of query executed.
    /// </param>
    /// <param name="tableMapping">
    ///     The DataTableMapping sent through an Update.
    /// </param>
    public SARowUpdatedEventArgs(DataRow row, IDbCommand command, StatementType statementType, DataTableMapping tableMapping)
      : base(row, command, statementType, tableMapping)
    {
    }
  }
}
