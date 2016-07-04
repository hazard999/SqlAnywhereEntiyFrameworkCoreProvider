
// Type: iAnywhere.Data.SQLAnywhere.MetadataHelpers
// Assembly: iAnywhere.Data.SQLAnywhere.v4.0, Version=11.0.1.27424, Culture=neutral, PublicKeyToken=f222fc4333e0d400
// MVID: CC4F9F8C-E618-49D1-9147-C06A9EF53D1F
// Assembly location: C:\Program Files\SQL Anywhere 11\Assembly\V4\iAnywhere.Data.SQLAnywhere.v4.0.dll

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Metadata.Edm;

namespace iAnywhere.Data.SQLAnywhere
{
  /// <summary>A set of static helpers for type metadata</summary>
  internal static class MetadataHelpers
  {
    internal static readonly int UnicodeStringMaxMaxLength = 1073741823;
    internal static readonly int AsciiStringMaxMaxLength = int.MaxValue;
    internal static readonly int BinaryMaxMaxLength = int.MaxValue;
    /// <summary>Name of the MaxLength Facet</summary>
    public static readonly string MaxLengthFacetName = "MaxLength";
    /// <summary>Name of the Unicode Facet</summary>
    public static readonly string UnicodeFacetName = "Unicode";
    /// <summary>Name of the FixedLength Facet</summary>
    public static readonly string FixedLengthFacetName = "FixedLength";
    /// <summary>Name of the PreserveSeconds Facet</summary>
    public static readonly string PreserveSecondsFacetName = "PreserveSeconds";
    /// <summary>Name of the Precision Facet</summary>
    public static readonly string PrecisionFacetName = "Precision";
    /// <summary>Name of the Scale Facet</summary>
    public static readonly string ScaleFacetName = "Scale";
    /// <summary>Name of the DefaultValue Facet</summary>
    public static readonly string DefaultValueFacetName = "DefaultValue";
    /// <summary>Name of the Nullable Facet</summary>
    internal const string NullableFacetName = "Nullable";

    /// <summary>
    /// Cast the EdmType of the given type usage to the given TEdmType
    /// </summary>
    /// <typeparam name="TEdmType"></typeparam>
    /// <param name="typeUsage"></param>
    /// <returns></returns>
    internal static TEdmType GetEdmType<TEdmType>(TypeUsage typeUsage) where TEdmType : EdmType
    {
      return (TEdmType) typeUsage.EdmType;
    }

    /// <summary>
    /// Gets the TypeUsage of the elment if the given type is a collection type
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    internal static TypeUsage GetElementTypeUsage(TypeUsage type)
    {
      if (MetadataHelpers.IsCollectionType(type))
        return ((CollectionType) type.EdmType).TypeUsage;
      return (TypeUsage) null;
    }

    /// <summary>
    /// Retrieves the properties of in the EdmType underlying the input type usage,
    ///  if that EdmType is a structured type (EntityType, RowType).
    /// </summary>
    /// <param name="typeUsage"></param>
    /// <returns></returns>
    internal static IList<EdmProperty> GetProperties(TypeUsage typeUsage)
    {
      return MetadataHelpers.GetProperties(typeUsage.EdmType);
    }

    /// <summary>
    /// Retrieves the properties of the given EdmType, if it is
    ///  a structured type (EntityType, RowType).
    /// </summary>
    /// <param name="edmType"></param>
    /// <returns></returns>
    internal static IList<EdmProperty> GetProperties(EdmType edmType)
    {
      switch (edmType.BuiltInTypeKind)
      {
        case BuiltInTypeKind.ComplexType:
          return (IList<EdmProperty>) ((ComplexType) edmType).Properties;
        case BuiltInTypeKind.EntityType:
          return (IList<EdmProperty>) ((EntityType) edmType).Properties;
        case BuiltInTypeKind.RowType:
          return (IList<EdmProperty>) ((RowType) edmType).Properties;
        default:
          return new List<EdmProperty>();
      }
    }

    /// <summary>Is the given type usage over a collection type</summary>
    /// <param name="typeUsage"></param>
    /// <returns></returns>
    internal static bool IsCollectionType(TypeUsage typeUsage)
    {
      return MetadataHelpers.IsCollectionType(typeUsage.EdmType);
    }

    /// <summary>Is the given type a collection type</summary>
    /// <param name="type"></param>
    /// <returns></returns>
    internal static bool IsCollectionType(EdmType type)
    {
      return BuiltInTypeKind.CollectionType == type.BuiltInTypeKind;
    }

    /// <summary>Is the given type usage over a primitive type</summary>
    /// <param name="type"></param>
    /// <returns></returns>
    internal static bool IsPrimitiveType(TypeUsage type)
    {
      return MetadataHelpers.IsPrimitiveType(type.EdmType);
    }

    /// <summary>Is the given type a primitive type</summary>
    /// <param name="type"></param>
    /// <returns></returns>
    internal static bool IsPrimitiveType(EdmType type)
    {
      return BuiltInTypeKind.PrimitiveType == type.BuiltInTypeKind;
    }

    /// <summary>Is the given type usage over a row type</summary>
    /// <param name="type"></param>
    /// <returns></returns>
    internal static bool IsRowType(TypeUsage type)
    {
      return MetadataHelpers.IsRowType(type.EdmType);
    }

    /// <summary>Is the given type a row type</summary>
    /// <param name="type"></param>
    /// <returns></returns>
    internal static bool IsRowType(EdmType type)
    {
      return BuiltInTypeKind.RowType == type.BuiltInTypeKind;
    }

    /// <summary>
    /// Gets the type of the given type usage if it is a primitive type
    /// </summary>
    /// <param name="type"></param>
    /// <param name="typeKind"></param>
    /// <returns></returns>
    internal static bool TryGetPrimitiveTypeKind(TypeUsage type, out PrimitiveTypeKind typeKind)
    {
      if (type != null && type.EdmType != null && type.EdmType.BuiltInTypeKind == BuiltInTypeKind.PrimitiveType)
      {
        typeKind = ((PrimitiveType) type.EdmType).PrimitiveTypeKind;
        return true;
      }
      typeKind = PrimitiveTypeKind.Binary;
      return false;
    }

    internal static PrimitiveTypeKind GetPrimitiveTypeKind(TypeUsage type)
    {
      PrimitiveTypeKind typeKind;
      if (!MetadataHelpers.TryGetPrimitiveTypeKind(type, out typeKind))
        throw new NotSupportedException("Cannot create parameter of non-primitive type");
      return typeKind;
    }

    /// <summary>
    /// Gets the value for the metadata property with the given name
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="item"></param>
    /// <param name="propertyName"></param>
    /// <returns></returns>
    internal static T TryGetValueForMetadataProperty<T>(MetadataItem item, string propertyName)
    {
      MetadataProperty metadataProperty;
      if (!item.MetadataProperties.TryGetValue(propertyName, true, out metadataProperty))
        return default (T);
      return (T) metadataProperty.Value;
    }

    internal static bool IsPrimitiveType(TypeUsage type, PrimitiveTypeKind primitiveType)
    {
      PrimitiveTypeKind typeKind;
      if (MetadataHelpers.TryGetPrimitiveTypeKind(type, out typeKind))
        return typeKind == primitiveType;
      return false;
    }

    internal static DbType GetDbType(PrimitiveTypeKind primitiveType)
    {
      switch (primitiveType)
      {
        case PrimitiveTypeKind.Binary:
          return DbType.Binary;
        case PrimitiveTypeKind.Boolean:
          return DbType.Boolean;
        case PrimitiveTypeKind.Byte:
          return DbType.Byte;
        case PrimitiveTypeKind.DateTime:
          return DbType.DateTime;
        case PrimitiveTypeKind.Decimal:
          return DbType.Decimal;
        case PrimitiveTypeKind.Double:
          return DbType.Double;
        case PrimitiveTypeKind.Guid:
          return DbType.Guid;
        case PrimitiveTypeKind.Single:
          return DbType.Single;
        case PrimitiveTypeKind.SByte:
          return DbType.SByte;
        case PrimitiveTypeKind.Int16:
          return DbType.Int16;
        case PrimitiveTypeKind.Int32:
          return DbType.Int32;
        case PrimitiveTypeKind.Int64:
          return DbType.Int64;
        case PrimitiveTypeKind.String:
          return DbType.String;
        default:
          throw new InvalidOperationException(string.Format("Unknown PrimitiveTypeKind {0}", (object) primitiveType));
      }
    }

    /// <summary>
    /// Get the value specified on the given type usage for the given facet name.
    /// If the faces does not have a value specifid or that value is null returns
    /// the default value for that facet.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="type"></param>
    /// <param name="facetName"></param>
    /// <returns></returns>
    /// <summary>
    /// Get the value specified on the given type usage for the given facet name.
    /// If the faces does not have a value specifid or that value is null returns
    /// the default value for that facet.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="type"></param>
    /// <param name="facetName"></param>
    /// <returns></returns>
    internal static T GetFacetValueOrDefault<T>(TypeUsage type, string facetName, T defaultValue)
    {
      Facet facet;
      if (type.Facets.TryGetValue(facetName, false, out facet) && facet.Value != null && !facet.IsUnbounded)
        return (T) facet.Value;
      return defaultValue;
    }

    internal static bool IsFacetValueConstant(TypeUsage type, string facetName)
    {
      return MetadataHelpers.GetFacet((IEnumerable<FacetDescription>) ((PrimitiveType) type.EdmType).FacetDescriptions, facetName).IsConstant;
    }

    private static FacetDescription GetFacet(IEnumerable<FacetDescription> facetCollection, string facetName)
    {
      foreach (FacetDescription facet in facetCollection)
      {
        if (facet.FacetName == facetName)
          return facet;
      }
      return (FacetDescription) null;
    }

    /// <summary>
    /// Given a facet name and an EdmType, tries to get that facet's description.
    /// </summary>
    /// <param name="edmType"></param>
    /// <param name="facetName"></param>
    /// <param name="facetDescription"></param>
    /// <returns></returns>
    internal static bool TryGetTypeFacetDescriptionByName(EdmType edmType, string facetName, out FacetDescription facetDescription)
    {
      facetDescription = (FacetDescription) null;
      if (MetadataHelpers.IsPrimitiveType(edmType))
      {
        foreach (FacetDescription facetDescription1 in ((PrimitiveType) edmType).FacetDescriptions)
        {
          if (facetName.Equals(facetDescription1.FacetName, StringComparison.OrdinalIgnoreCase))
          {
            facetDescription = facetDescription1;
            return true;
          }
        }
      }
      return false;
    }

    internal static bool IsNullable(TypeUsage type)
    {
      Facet facet;
      if (type.Facets.TryGetValue("Nullable", false, out facet))
        return (bool) facet.Value;
      return false;
    }

    internal static bool TryGetMaxLength(TypeUsage type, out int maxLength)
    {
      if (MetadataHelpers.IsPrimitiveType(type, PrimitiveTypeKind.String) || MetadataHelpers.IsPrimitiveType(type, PrimitiveTypeKind.Binary))
        return MetadataHelpers.TryGetIntFacetValue(type, MetadataHelpers.MaxLengthFacetName, out maxLength);
      maxLength = 0;
      return false;
    }

    internal static bool TryGetIntFacetValue(TypeUsage type, string facetName, out int intValue)
    {
      intValue = 0;
      Facet facet;
      if (!type.Facets.TryGetValue(facetName, false, out facet) || facet.Value == null || facet.IsUnbounded)
        return false;
      intValue = (int) facet.Value;
      return true;
    }

    internal static bool TryGetIsFixedLength(TypeUsage type, out bool isFixedLength)
    {
      if (MetadataHelpers.IsPrimitiveType(type, PrimitiveTypeKind.String) || MetadataHelpers.IsPrimitiveType(type, PrimitiveTypeKind.Binary))
        return MetadataHelpers.TryGetBooleanFacetValue(type, MetadataHelpers.FixedLengthFacetName, out isFixedLength);
      isFixedLength = false;
      return false;
    }

    internal static bool TryGetBooleanFacetValue(TypeUsage type, string facetName, out bool boolValue)
    {
      boolValue = false;
      Facet facet;
      if (!type.Facets.TryGetValue(facetName, false, out facet) || facet.Value == null)
        return false;
      boolValue = (bool) facet.Value;
      return true;
    }

    internal static bool TryGetIsUnicode(TypeUsage type, out bool isUnicode)
    {
      if (MetadataHelpers.IsPrimitiveType(type, PrimitiveTypeKind.String))
        return MetadataHelpers.TryGetBooleanFacetValue(type, MetadataHelpers.UnicodeFacetName, out isUnicode);
      isUnicode = false;
      return false;
    }

    internal static bool IsCanonicalFunction(EdmFunction function)
    {
      return function.NamespaceName == "Edm";
    }

    internal static bool IsStoreFunction(EdmFunction function)
    {
      return !MetadataHelpers.IsCanonicalFunction(function);
    }

    internal static ParameterDirection ParameterModeToParameterDirection(ParameterMode mode)
    {
      switch (mode)
      {
        case ParameterMode.In:
          return ParameterDirection.Input;
        case ParameterMode.Out:
          return ParameterDirection.Output;
        case ParameterMode.InOut:
          return ParameterDirection.InputOutput;
        case ParameterMode.ReturnValue:
          return ParameterDirection.ReturnValue;
        default:
          return 0;
      }
    }
  }
}
