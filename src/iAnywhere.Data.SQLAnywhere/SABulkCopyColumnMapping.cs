
// Type: iAnywhere.Data.SQLAnywhere.SABulkCopyColumnMapping
// Assembly: iAnywhere.Data.SQLAnywhere.v4.0, Version=11.0.1.27424, Culture=neutral, PublicKeyToken=f222fc4333e0d400
// MVID: CC4F9F8C-E618-49D1-9147-C06A9EF53D1F
// Assembly location: C:\Program Files\SQL Anywhere 11\Assembly\V4\iAnywhere.Data.SQLAnywhere.v4.0.dll

using System;

namespace iAnywhere.Data.SQLAnywhere
{
  /// <summary>
  ///     <para>Defines the mapping between a column in an SABulkCopy instance's data source and a column in the instance's destination table.</para>
  /// </summary>
  /// <remarks>
  ///     <para><b>Restrictions:</b> The SABulkCopyColumnMapping class is not available in the .NET Compact Framework 2.0.</para>
  /// </remarks>
  public sealed class SABulkCopyColumnMapping
  {
    private const int c_InvalidOrdinal = -1;
    private int _destinationOrdinal;
    private string _destinationColumn;
    private int _sourceOrdinal;
    private string _sourceColumn;

    /// <summary>
    ///     <para>Gets or sets the name of the column in the destination database table being mapped to.</para>
    /// </summary>
    /// <value>A string specifying the name of the column in the destination table or a null reference (Nothing in Visual Basic) if the DestinationOrdinal property has priority.</value>
    /// <remarks>
    ///     <para>The DestinationColumn property and DestinationOrdinal property are mutually exclusive. The most recently set value takes priority.</para>
    ///     <para>Setting the DestinationColumn property causes the DestinationOrdinal property to be set to -1. Setting the DestinationOrdinal property causes the DestinationColumn property to be set to a null reference (Nothing in Visual Basic).</para>
    ///     <para>It is an error to set DestinationColumn to null or the empty string.</para>
    /// </remarks>
    /// <seealso cref="P:iAnywhere.Data.SQLAnywhere.SABulkCopyColumnMapping.DestinationOrdinal" />
    public string DestinationColumn
    {
      get
      {
        return _destinationColumn;
      }
      set
      {
                CheckColumn(value, "DestinationColumn");
                _destinationColumn = value;
                _destinationOrdinal = -1;
      }
    }

    /// <summary>
    ///     <para>Gets or sets the ordinal value of the column in the destination table being mapped to.</para>
    /// </summary>
    /// <value>An integer specifying the ordinal of the column being mapped to in the destination table or -1 if the property is not set.</value>
    /// <remarks>
    ///     <para>The DestinationColumn property and DestinationOrdinal property are mutually exclusive. The most recently set value takes priority.</para>
    ///     <para>Setting the DestinationColumn property causes the DestinationOrdinal property to be set to -1. Setting the DestinationOrdinal property causes the DestinationColumn property to be set to a null reference (Nothing in Visual Basic).</para>
    /// </remarks>
    /// <seealso cref="P:iAnywhere.Data.SQLAnywhere.SABulkCopyColumnMapping.DestinationColumn" />
    public int DestinationOrdinal
    {
      get
      {
        return _destinationOrdinal;
      }
      set
      {
                CheckOrdinal(value, "DestinationOrdinal");
                _destinationOrdinal = value;
                _destinationColumn = null;
      }
    }

    /// <summary>
    ///     <para>Gets or sets the name of the column being mapped in the data source.</para>
    /// </summary>
    /// <value>A string specifying the name of the column in the data source or a null reference (Nothing in Visual Basic) if the SourceOrdinal property has priority.</value>
    /// <remarks>
    ///     <para>The SourceColumn property and SourceOrdinal property are mutually exclusive. The most recently set value takes priority.</para>
    ///     <para>Setting the SourceColumn property causes the SourceOrdinal property to be set to -1. Setting the SourceOrdinal property causes the SourceColumn property to be set to a null reference (Nothing in Visual Basic).</para>
    ///     <para>It is an error to set SourceColumn to null or the empty string.</para>
    /// </remarks>
    /// <seealso cref="P:iAnywhere.Data.SQLAnywhere.SABulkCopyColumnMapping.SourceOrdinal" />
    public string SourceColumn
    {
      get
      {
        return _sourceColumn;
      }
      set
      {
                CheckColumn(value, "SourceColumn");
                _sourceColumn = value;
                _sourceOrdinal = -1;
      }
    }

    /// <summary>
    ///     <para>Gets or sets ordinal position of the source column within the data source.</para>
    /// </summary>
    /// <value>An integer specifying the ordinal of the column in the data source or -1 if the property is not set.</value>
    /// <remarks>
    ///     <para>The SourceColumn property and SourceOrdinal property are mutually exclusive. The most recently set value takes priority.</para>
    ///     <para>Setting the SourceColumn property causes the SourceOrdinal property to be set to -1. Setting the SourceOrdinal property causes the SourceColumn property to be set to a null reference (Nothing in Visual Basic).</para>
    /// </remarks>
    /// <seealso cref="P:iAnywhere.Data.SQLAnywhere.SABulkCopyColumnMapping.SourceColumn" />
    public int SourceOrdinal
    {
      get
      {
        return _sourceOrdinal;
      }
      set
      {
                CheckOrdinal(value, "SourceOrdinal");
                _sourceOrdinal = value;
                _sourceColumn = null;
      }
    }

    internal object Source
    {
      get
      {
        if (SourceColumn != null)
          return SourceColumn;
        return SourceOrdinal;
      }
    }

    internal object Destination
    {
      get
      {
        if (DestinationColumn != null)
          return DestinationColumn;
        return DestinationOrdinal;
      }
    }

    /// <summary>
    ///     <para>Creates a new column mapping, using column ordinals or names to refer to source and destination columns.</para>
    /// </summary>
    /// <remarks>
    ///     <para><b>Restrictions:</b> The SABulkCopyColumnMapping class is not available in the .NET Compact Framework 2.0.</para>
    /// </remarks>
    public SABulkCopyColumnMapping()
    {
            Init(-1, -1, null, null);
    }

    /// <summary>
    ///     <para>Creates a new column mapping, using column ordinals to refer to source and destination columns.</para>
    /// </summary>
    /// <remarks>
    ///     <para><b>Restrictions:</b> The SABulkCopyColumnMapping class is not available in the .NET Compact Framework 2.0.</para>
    /// </remarks>
    /// <param name="sourceColumnOrdinal">
    ///     The ordinal position of the source column within the data source. The first column in a data source has ordinal position zero.
    /// </param>
    /// <param name="destinationColumnOrdinal">
    ///     The ordinal position of the destination column within the destination table. The first column in a table has ordinal position zero.
    /// </param>
    public SABulkCopyColumnMapping(int sourceColumnOrdinal, int destinationColumnOrdinal)
    {
            CheckOrdinal(sourceColumnOrdinal, "sourceColumnOrdinal");
            CheckOrdinal(destinationColumnOrdinal, "destinationColumnOrdinal");
            Init(sourceColumnOrdinal, destinationColumnOrdinal, null, null);
    }

    /// <summary>
    ///     <para>Creates a new column mapping, using a column ordinal to refer to the source column and a column name to refer to the destination column.</para>
    /// </summary>
    /// <remarks>
    ///     <para><b>Restrictions:</b> The SABulkCopyColumnMapping class is not available in the .NET Compact Framework 2.0.</para>
    /// </remarks>
    /// <param name="sourceColumnOrdinal">
    ///     The ordinal position of the source column within the data source. The first column in a data source has ordinal position zero.
    /// </param>
    /// <param name="destinationColumn">
    ///     The name of the destination column within the destination table.
    /// </param>
    public SABulkCopyColumnMapping(int sourceColumnOrdinal, string destinationColumn)
    {
            CheckOrdinal(sourceColumnOrdinal, "sourceColumnOrdinal");
            CheckColumn(destinationColumn, "destinationColumn");
            Init(sourceColumnOrdinal, -1, null, destinationColumn);
    }

    /// <summary>
    ///     <para>Creates a new column mapping, using a column name to refer to the source column and a column ordinal to refer to the destination column.</para>
    /// </summary>
    /// <remarks>
    ///     <para><b>Restrictions:</b> The SABulkCopyColumnMapping class is not available in the .NET Compact Framework 2.0.</para>
    /// </remarks>
    /// <param name="sourceColumn">
    ///     The name of the source column within the data source.
    /// </param>
    /// <param name="destinationColumnOrdinal">
    ///     The ordinal position of the destination column within the destination table. The first column in a table has ordinal position zero.
    /// </param>
    public SABulkCopyColumnMapping(string sourceColumn, int destinationColumnOrdinal)
    {
            CheckColumn(sourceColumn, "sourceColumn");
            CheckOrdinal(destinationColumnOrdinal, "destinationColumnOrdinal");
            Init(-1, destinationColumnOrdinal, sourceColumn, null);
    }

    /// <summary>
    ///     <para>Creates a new column mapping, using column names to refer to source and destination columns.</para>
    /// </summary>
    /// <remarks>
    ///     <para><b>Restrictions:</b> The SABulkCopyColumnMapping class is not available in the .NET Compact Framework 2.0.</para>
    /// </remarks>
    /// <param name="sourceColumn">
    ///     The name of the source column within the data source.
    /// </param>
    /// <param name="destinationColumn">
    ///     The name of the destination column within the destination table.
    /// </param>
    public SABulkCopyColumnMapping(string sourceColumn, string destinationColumn)
    {
            CheckColumn(sourceColumn, "sourceColumn");
            CheckColumn(destinationColumn, "destinationColumn");
            Init(-1, -1, sourceColumn, destinationColumn);
    }

    private void Init(int sourceOrdinal, int destinationOrdinal, string sourceColumn, string destinationColumn)
    {
            _destinationOrdinal = destinationOrdinal;
            _destinationColumn = destinationColumn;
            _sourceOrdinal = sourceOrdinal;
            _sourceColumn = sourceColumn;
    }

    private void CheckOrdinal(int ordinal, string parameterName)
    {
      if (ordinal < 0)
      {
        Exception e = new IndexOutOfRangeException(SARes.GetString(15008, parameterName));
        SATrace.Exception(e);
        throw e;
      }
    }

    private void CheckColumn(string columnName, string parameterName)
    {
      if (columnName == null)
      {
        Exception e = new ArgumentNullException(parameterName);
        SATrace.Exception(e);
        throw e;
      }
      if (columnName.Length == 0)
      {
        Exception e = new ArgumentException(SARes.GetString(15011, parameterName));
        SATrace.Exception(e);
        throw e;
      }
    }

    internal bool SameFormat(SABulkCopyColumnMapping other)
    {
      if ((other.DestinationColumn != null || DestinationColumn != null) && (other.DestinationColumn == null || DestinationColumn == null) || (other.SourceColumn != null || SourceColumn != null) && (other.SourceColumn == null || SourceColumn == null) || (other.DestinationOrdinal != -1 || DestinationOrdinal != -1) && (other.DestinationOrdinal == -1 || DestinationOrdinal == -1))
        return false;
      if (other.SourceOrdinal == -1 && SourceOrdinal == -1)
        return true;
      if (other.SourceOrdinal != -1)
        return SourceOrdinal != -1;
      return false;
    }
  }
}
