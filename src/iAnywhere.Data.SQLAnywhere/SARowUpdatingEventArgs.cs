
// Type: iAnywhere.Data.SQLAnywhere.SARowUpdatingEventArgs
// Assembly: iAnywhere.Data.SQLAnywhere.v4.0, Version=11.0.1.27424, Culture=neutral, PublicKeyToken=f222fc4333e0d400
// MVID: CC4F9F8C-E618-49D1-9147-C06A9EF53D1F
// Assembly location: C:\Program Files\SQL Anywhere 11\Assembly\V4\iAnywhere.Data.SQLAnywhere.v4.0.dll

using System.Data;
using System.Data.Common;

namespace iAnywhere.Data.SQLAnywhere
{
  /// <summary>
  ///     <para>Provides data for the RowUpdating event.</para>
  /// </summary>
  public sealed class SARowUpdatingEventArgs : RowUpdatingEventArgs
  {
    /// <summary>
    ///     <para>Specifies the SACommand to execute when performing the Update.</para>
    /// </summary>
    public SACommand Command
    {
      get
      {
        return (SACommand) base.Command;
      }
      set
      {
                Command = (IDbCommand) value;
      }
    }

    /// <summary>
    ///     <para>Initializes a new instance of the SARowUpdatingEventArgs class.</para>
    /// </summary>
    /// <param name="row">The DataRow to update.</param>
    /// <param name="command">The IDbCommand to execute during update.</param>
    /// <param name="statementType">
    ///     One of the StatementType values that specifies the type of query executed.
    /// </param>
    /// <param name="tableMapping">
    ///     The DataTableMapping sent through an Update.
    /// </param>
    public SARowUpdatingEventArgs(DataRow row, IDbCommand command, StatementType statementType, DataTableMapping tableMapping)
      : base(row, command, statementType, tableMapping)
    {
    }
  }
}
