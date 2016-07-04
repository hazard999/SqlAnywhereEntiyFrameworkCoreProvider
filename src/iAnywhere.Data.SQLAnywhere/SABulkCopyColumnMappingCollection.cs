
// Type: iAnywhere.Data.SQLAnywhere.SABulkCopyColumnMappingCollection
// Assembly: iAnywhere.Data.SQLAnywhere.v4.0, Version=11.0.1.27424, Culture=neutral, PublicKeyToken=f222fc4333e0d400
// MVID: CC4F9F8C-E618-49D1-9147-C06A9EF53D1F
// Assembly location: C:\Program Files\SQL Anywhere 11\Assembly\V4\iAnywhere.Data.SQLAnywhere.v4.0.dll

using System;
using System.Collections;

namespace iAnywhere.Data.SQLAnywhere
{
  /// <summary>
  ///     <para>A collection of SABulkCopyColumnMapping objects that inherits from System.Collections.CollectionBase.</para>
  /// </summary>
  /// <remarks>
  ///     <para><b>Restrictions:</b> The SABulkCopyColumnMappingCollection class is not available in the .NET Compact Framework 2.0.</para>
  /// </remarks>
  public sealed class SABulkCopyColumnMappingCollection : CollectionBase
  {
    private bool _allowChanges = true;

    /// <summary>
    ///     <para>Gets the SABulkCopyColumnMapping object at the specified index.</para>
    /// </summary>
    /// <param name="index">
    ///     The zero-based index of the SABulkCopyColumnMapping object to find.
    /// </param>
    /// <returns>
    /// <para>An SABulkCopyColumnMapping object is returned.</para>
    /// </returns>
    public SABulkCopyColumnMapping this[int index]
    {
      get
      {
        return (SABulkCopyColumnMapping)List[index];
      }
    }

    internal bool AllowChanges
    {
      get
      {
        return _allowChanges;
      }
      set
      {
                _allowChanges = value;
      }
    }

    internal SABulkCopyColumnMappingCollection()
    {
    }

    /// <summary>
    ///     <para>Adds the specified SABulkCopyColumnMapping object to the collection.</para>
    /// </summary>
    /// <remarks>
    ///     <para><b>Restrictions:</b> The SABulkCopyColumnMappingCollection class is not available in the .NET Compact Framework 2.0.</para>
    /// </remarks>
    /// <param name="bulkCopyColumnMapping">
    ///     <para>The SABulkCopyColumnMapping object that describes the mapping to be added to the collection.</para>
    /// </param>
    /// <seealso cref="T:iAnywhere.Data.SQLAnywhere.SABulkCopyColumnMapping" />
    public SABulkCopyColumnMapping Add(SABulkCopyColumnMapping bulkCopyColumnMapping)
    {
      if (bulkCopyColumnMapping == null)
      {
        Exception e = new ArgumentNullException("bulkCopyColumnMapping");
        SATrace.Exception(e);
        throw e;
      }
            CheckAllowChanges();
            CheckMappingFormat(bulkCopyColumnMapping);
            List.Add(bulkCopyColumnMapping);
      return bulkCopyColumnMapping;
    }

    private void CheckMappingFormat(SABulkCopyColumnMapping bulkcopyMapping)
    {
      if (Count > 0 && !this[0].SameFormat(bulkcopyMapping))
      {
        Exception e = new InvalidOperationException(SARes.GetString(15007));
        SATrace.Exception(e);
        throw e;
      }
    }

    /// <summary>
    ///     <para>Creates a new SABulkCopyColumnMapping object using ordinals to specify both source and destination columns, and adds the mapping to the collection.</para>
    /// </summary>
    /// <remarks>
    ///     <para><b>Restrictions:</b> The SABulkCopyColumnMappingCollection class is not available in the .NET Compact Framework 2.0.</para>
    /// </remarks>
    /// <param name="sourceColumnOrdinal">
    ///     The ordinal position of the source column within the data source.
    /// </param>
    /// <param name="destinationColumnOrdinal">
    ///     The ordinal position of the destination column within the destination table.
    /// </param>
    public SABulkCopyColumnMapping Add(int sourceColumnOrdinal, int destinationColumnOrdinal)
    {
      return Add(new SABulkCopyColumnMapping(sourceColumnOrdinal, destinationColumnOrdinal));
    }

    /// <summary>
    ///     <para>Creates a new SABulkCopyColumnMapping object using a column ordinal to refer to the source column and a column name to refer to the destination column, and adds mapping to the collection.</para>
    /// </summary>
    /// <remarks>
    ///     <para><b>Restrictions:</b> The SABulkCopyColumnMappingCollection class is not available in the .NET Compact Framework 2.0.</para>
    /// </remarks>
    /// <param name="sourceColumnOrdinal">
    ///     The ordinal position of the source column within the data source.
    /// </param>
    /// <param name="destinationColumn">
    ///     The name of the destination column within the destination table.
    /// </param>
    public SABulkCopyColumnMapping Add(int sourceColumnOrdinal, string destinationColumn)
    {
      return Add(new SABulkCopyColumnMapping(sourceColumnOrdinal, destinationColumn));
    }

    /// <summary>
    ///     <para>Creates a new SABulkCopyColumnMapping object using a column name to refer to the source column and a column ordinal to refer to the destination the column, and adds the mapping to the collection.</para>
    ///     <para>Creates a new column mapping, using column ordinals or names to refer to source and destination columns.</para>
    /// </summary>
    /// <remarks>
    ///     <para><b>Restrictions:</b> The SABulkCopyColumnMappingCollection class is not available in the .NET Compact Framework 2.0.</para>
    /// </remarks>
    /// <param name="sourceColumn">
    ///     The name of the source column within the data source.
    /// </param>
    /// <param name="destinationColumnOrdinal">
    ///     The ordinal position of the destination column within the destination table.
    /// </param>
    public SABulkCopyColumnMapping Add(string sourceColumn, int destinationColumnOrdinal)
    {
      return Add(new SABulkCopyColumnMapping(sourceColumn, destinationColumnOrdinal));
    }

    /// <summary>
    ///     <para>Creates a new SABulkCopyColumnMapping object using column names to specify both source and destination columns, and adds the mapping to the collection.</para>
    /// </summary>
    /// <remarks>
    ///     <para><b>Restrictions:</b> The SABulkCopyColumnMappingCollection class is not available in the .NET Compact Framework 2.0.</para>
    /// </remarks>
    /// <param name="sourceColumn">
    ///     The name of the source column within the data source.
    /// </param>
    /// <param name="destinationColumn">
    ///     The name of the destination column within the destination table.
    /// </param>
    public SABulkCopyColumnMapping Add(string sourceColumn, string destinationColumn)
    {
      return Add(new SABulkCopyColumnMapping(sourceColumn, destinationColumn));
    }

    /// <summary>
    ///     <para>Gets a value indicating whether a specified SABulkCopyColumnMapping object exists in the collection.</para>
    /// </summary>
    /// <param name="value">
    ///     <para>A valid SABulkCopyColumnMapping object.</para>
    /// </param>
    /// <returns>
    /// <para>True if the specified mapping exists in the collection; otherwise, false.</para>
    /// </returns>
    public bool Contains(SABulkCopyColumnMapping value)
    {
      return List.Contains(value);
    }

    /// <summary>
    ///     <para>Copies the elements of the SABulkCopyColumnMappingCollection to an array of SABulkCopyColumnMapping items, starting at a particular index.</para>
    /// </summary>
    /// <param name="array">
    ///     The one-dimensional SABulkCopyColumnMapping array that is the destination of the elements copied from SABulkCopyColumnMappingCollection. The array must have zero-based indexing.
    /// </param>
    /// <param name="index">
    ///     The zero-based index in the array at which copying begins.
    /// </param>
    public void CopyTo(SABulkCopyColumnMapping[] array, int index)
    {
            InnerList.CopyTo(array, index);
    }

    /// <summary>
    ///     <para>Gets or sets the index of the specified SABulkCopyColumnMapping object within the collection.</para>
    /// </summary>
    /// <param name="value">
    ///     The SABulkCopyColumnMapping object to search for.
    /// </param>
    /// <returns>
    /// <para>The zero-based index of the column mapping is returned, or -1 is returned if the column mapping is not found in the collection.</para>
    /// </returns>
    public int IndexOf(SABulkCopyColumnMapping value)
    {
      return List.IndexOf(value);
    }

    /// <summary>
    ///     <para>Removes the specified SABulkCopyColumnMapping element from the SABulkCopyColumnMappingCollection.</para>
    /// </summary>
    /// <param name="value">
    ///     The SABulkCopyColumnMapping object to be removed from the collection.
    /// </param>
    public void Remove(SABulkCopyColumnMapping value)
    {
            CheckAllowChanges();
            List.Remove(value);
    }

    /// <summary>
    ///     <para>Removes the mapping at the specified index from the collection.</para>
    /// </summary>
    /// <param name="index">
    ///     The zero-based index of the SABulkCopyColumnMapping object to be removed from the collection.
    /// </param>
    public new void RemoveAt(int index)
    {
            CheckAllowChanges();
      base.RemoveAt(index);
    }

    private void CheckAllowChanges()
    {
      if (!_allowChanges)
      {
        Exception e = new InvalidOperationException(SARes.GetString(15014));
        SATrace.Exception(e);
        throw e;
      }
    }

    internal void SortByDestOrdinal()
    {
      ArrayList.Adapter(List).Sort(new SABulkCopyColumnMappingCollection.DestinationOrdinalComparer());
    }

    internal SABulkCopyColumnMapping FindFromSource(object source)
    {
      for (int index = 0; index < Count; ++index)
      {
        SABulkCopyColumnMapping copyColumnMapping = this[index];
        if (copyColumnMapping.Source.Equals(source))
          return copyColumnMapping;
      }
      return null;
    }

    private class DestinationOrdinalComparer : IComparer
    {
      public int Compare(object o1, object o2)
      {
        return ((SABulkCopyColumnMapping) o1).DestinationOrdinal - ((SABulkCopyColumnMapping) o2).DestinationOrdinal;
      }
    }
  }
}
