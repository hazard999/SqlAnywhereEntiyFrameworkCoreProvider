
// Type: iAnywhere.Data.SQLAnywhere.SAParameter
// Assembly: iAnywhere.Data.SQLAnywhere.v4.0, Version=11.0.1.27424, Culture=neutral, PublicKeyToken=f222fc4333e0d400
// MVID: CC4F9F8C-E618-49D1-9147-C06A9EF53D1F
// Assembly location: C:\Program Files\SQL Anywhere 11\Assembly\V4\iAnywhere.Data.SQLAnywhere.v4.0.dll

using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Reflection;

namespace iAnywhere.Data.SQLAnywhere
{
  /// <summary>
  ///     <para>Represents a parameter to an SACommand, and optionally, its mapping to a DataSet column.</para>
  /// </summary>
  /// <remarks>
  ///     <para><b>Implements:</b> <see cref="T:System.Data.IDbDataParameter" />, <see cref="T:System.Data.IDataParameter" />, <see cref="T:System.ICloneable" /></para>
  /// </remarks>
  [TypeConverter(typeof (SAParameter.SAParameterConverter))]
  public sealed class SAParameter : DbParameter, ICloneable
  {
    private int _objectId = SAParameter.s_CurrentId++;
    private DbType _dbType;
    private ParameterDirection _direction;
    private bool _isNullable;
    private int _offset;
    private string _name;
    private byte _precision;
    private byte _scale;
    private int _size;
    private string _sourceColumn;
    private DataRowVersion _dataRowVer;
    private SADbType _asaDbType;
    private object _value;
    private bool _rebind;
    private bool _valueChanged;
    private bool _inferType;
    private bool _sourceColumnNullMapping;
    private static int s_CurrentId;

    /// <summary>
    ///     <para>Gets and sets the DbType of the parameter.</para>
    /// </summary>
    /// <remarks>
    ///     <para>The SADbType and DbType are linked. Therefore, setting the DbType changes the SADbType to a supporting SADbType.</para>
    ///     <para>The value must be a member of the SADbType enumerator.</para>
    /// </remarks>
    [RefreshProperties(RefreshProperties.All)]
    [Category("Data")]
    [Description("")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [Browsable(false)]
    public override DbType DbType
    {
      get
      {
        SATrace.PropertyCall("<sa.SAParameter.get_DbType|API>", _objectId);
        return _dbType;
      }
      set
      {
        SATrace.PropertyCall("<sa.SAParameter.set_DbType|API>", _objectId);
        if (value == DbType.Object || value == DbType.SByte || value == DbType.VarNumeric)
        {
          Exception e = new ArgumentException(SARes.GetString(11008, value.ToString()), "value");
          SATrace.Exception(e);
          throw e;
        }
        if (_dbType != value)
        {
                    _dbType = value;
                    _asaDbType = SADataConvert.MapToSADbType(_dbType);
                    _rebind = true;
        }
                _inferType = false;
      }
    }

    /// <summary>
    ///     <para>Gets and sets a value indicating whether the parameter is input-only, output-only, bidirectional, or a stored procedure return value parameter.</para>
    /// </summary>
    /// <value>One of the ParameterDirection values.</value>
    /// <remarks>
    ///     <para>If the ParameterDirection is output, and execution of the associated SACommand does not return a value, the SAParameter contains a null value. After the last row from the last result set is read, the Output, InputOut, and ReturnValue parameters are updated.</para>
    /// </remarks>
    [Category("Data")]
    [Description("")]
    [DefaultValue(ParameterDirection.Input)]
    public override ParameterDirection Direction
    {
      get
      {
        SATrace.PropertyCall("<sa.SAParameter.get_Direction|API>", _objectId);
        return _direction;
      }
      set
      {
        SATrace.PropertyCall("<sa.SAParameter.set_Direction|API>", _objectId);
        if (_direction == value)
          return;
                _direction = value;
                _rebind = true;
      }
    }

    /// <summary>
    ///     <para>Gets and sets a value indicating whether the parameter accepts null values.</para>
    /// </summary>
    /// <remarks>
    ///     <para>This property is true if null values are accepted; otherwise, it is false. The default is false. Null values are handled using the DBNull class.</para>
    /// </remarks>
    [Category("Data")]
    [Browsable(false)]
    [DesignOnly(true)]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    [Description("")]
    [DefaultValue(false)]
    public override bool IsNullable
    {
      get
      {
        SATrace.PropertyCall("<sa.SAParameter.get_IsNullable|API>", _objectId);
        return _isNullable;
      }
      set
      {
        SATrace.PropertyCall("<sa.SAParameter.set_IsNullable|API>", _objectId);
        if (_isNullable == value)
          return;
                _isNullable = value;
                _rebind = true;
      }
    }

    /// <summary>
    ///     <para>Gets and sets the offset to the Value property.</para>
    /// </summary>
    /// <value>The offset to the value. The default is 0.</value>
    [Browsable(false)]
    [Category("Data")]
    [Description("")]
    [DefaultValue(0)]
    public int Offset
    {
      get
      {
        SATrace.PropertyCall("<sa.SAParameter.get_Offset|API>", _objectId);
        return _offset;
      }
      set
      {
        SATrace.PropertyCall("<sa.SAParameter.set_Offset|API>", _objectId);
        if (_offset == value)
          return;
                _offset = value;
                _rebind = true;
      }
    }

    /// <summary>
    ///     <para>Gets and sets the name of the SAParameter.</para>
    /// </summary>
    /// <value>The default is an empty string.</value>
    /// <remarks>
    ///     <para>The SQL Anywhere .NET Data Provider uses positional parameters that are marked with a question mark (?) instead of named parameters.</para>
    /// </remarks>
    [Category("Misc")]
    [DefaultValue("")]
    [Description("")]
    public override string ParameterName
    {
      get
      {
        SATrace.PropertyCall("<sa.SAParameter.get_ParameterName|API>", _objectId);
        return _name;
      }
      set
      {
        SATrace.PropertyCall("<sa.SAParameter.set_ParameterName|API>", _objectId);
        if (_name.Equals(value))
          return;
                _name = value != null ? value : "";
                _rebind = true;
      }
    }

    /// <summary>
    ///     <para>Gets and sets the maximum number of digits used to represent the Value property.</para>
    /// </summary>
    /// <value>The value of this property is the maximum number of digits used to represent the Value property. The default value is 0, which indicates that the data provider sets the precision for the Value property.</value>
    /// <remarks>
    ///     <para>The Precision property is only used for decimal and numeric input parameters.</para>
    /// </remarks>
    [Category("Data")]
    [DefaultValue(0)]
    [Description("")]
    public byte Precision
    {
      get
      {
        SATrace.PropertyCall("<sa.SAParameter.get_Precision|API>", _objectId);
        return _precision;
      }
      set
      {
        SATrace.PropertyCall("<sa.SAParameter.set_Precision|API>", _objectId);
                SetPrecision(value, "value");
      }
    }

    /// <summary>
    ///     <para>Gets and sets the number of decimal places to which Value is resolved.</para>
    /// </summary>
    /// <value>The number of decimal places to which Value is resolved. The default is 0.</value>
    /// <remarks>
    ///     <para>The Scale property is only used for decimal and numeric input parameters.</para>
    /// </remarks>
    [DefaultValue(0)]
    [Description("")]
    [Category("Data")]
    public byte Scale
    {
      get
      {
        SATrace.PropertyCall("<sa.SAParameter.get_Scale|API>", _objectId);
        return _scale;
      }
      set
      {
        SATrace.PropertyCall("<sa.SAParameter.set_Scale|API>", _objectId);
        if (_scale == value)
          return;
                _scale = value;
                _rebind = true;
      }
    }

    /// <summary>
    ///     <para>Gets and sets the maximum size, in bytes, of the data within the column.</para>
    /// </summary>
    /// <value>The value of this property is the maximum size, in bytes, of the data within the column. The default value is inferred from the parameter value.</value>
    /// <remarks>
    ///     <para>The value of this property is the maximum size, in bytes, of the data within the column. The default value is inferred from the parameter value.</para>
    ///     <para>The Size property is used for binary and string types.</para>
    ///     <para>For variable length data types, the Size property describes the maximum amount of data to transmit to the server. For example, the Size property can be used to limit the amount of data sent to the server for a string value to the first one hundred bytes.</para>
    ///     <para>If not explicitly set, the size is inferred from the actual size of the specified parameter value. For fixed width data types, the value of Size is ignored. It can be retrieved for informational purposes, and returns the maximum amount of bytes the provider uses when transmitting the value of the parameter to the server.</para>
    /// </remarks>
    [Description("")]
    [Category("Data")]
    [DefaultValue(0)]
    public override int Size
    {
      get
      {
        SATrace.PropertyCall("<sa.SAParameter.get_Size|API>", _objectId);
        return _size;
      }
      set
      {
        SATrace.PropertyCall("<sa.SAParameter.set_Size|API>", _objectId);
                SetSize(value, "value");
      }
    }

    /// <summary>
    ///     <para>Gets and sets the name of the source column mapped to the DataSet and used for loading or returning the value.</para>
    /// </summary>
    /// <value>A string specifying the name of the source column mapped to the DataSet and used for loading or returning the value.</value>
    /// <remarks>
    ///     <para>When SourceColumn is set to anything other than an empty string, the value of the parameter is retrieved from the column with the SourceColumn name. If Direction is set to Input, the value is taken from the DataSet. If Direction is set to Output, the value is taken from the data source. A Direction of InputOutput is a combination of both.</para>
    /// </remarks>
    [DefaultValue("")]
    [Category("Data")]
    [Description("")]
    public override string SourceColumn
    {
      get
      {
        SATrace.PropertyCall("<sa.SAParameter.get_SourceColumn|API>", _objectId);
        return _sourceColumn;
      }
      set
      {
        SATrace.PropertyCall("<sa.SAParameter.set_SourceColumn|API>", _objectId);
        if (_sourceColumn.Equals(value))
          return;
                _sourceColumn = value == null ? "" : value;
                _rebind = true;
      }
    }

    /// <summary>
    ///     <para>Gets and sets the DataRowVersion to use when loading Value.</para>
    /// </summary>
    /// <remarks>
    ///     <para>Used by UpdateCommand during an Update operation to determine whether the parameter value is set to Current or Original. This allows primary keys to be updated. This property is ignored by InsertCommand and DeleteCommand. This property is set to the version of the DataRow used by the Item property, or the GetChildRows method of the DataRow object.</para>
    /// </remarks>
    [Category("Data")]
    [DefaultValue(DataRowVersion.Current)]
    [Description("")]
    public override DataRowVersion SourceVersion
    {
      get
      {
        SATrace.PropertyCall("<sa.SAParameter.get_SourceVersion|API>", _objectId);
        return _dataRowVer;
      }
      set
      {
        SATrace.PropertyCall("<sa.SAParameter.set_SourceVersion|API>", _objectId);
        if (_dataRowVer == value)
          return;
                _dataRowVer = value;
                _rebind = true;
      }
    }

    /// <summary>
    ///     <para>The SADbType of the parameter.</para>
    /// </summary>
    /// <remarks>
    ///     <para>The SADbType and DbType are linked. Therefore, setting the SADbType changes the DbType to a supporting DbType.</para>
    ///     <para>The value must be a member of the SADbType enumerator.</para>
    /// </remarks>
    [Description("")]
    [Category("Data")]
    [DbProviderSpecificTypeProperty(true)]
    [RefreshProperties(RefreshProperties.All)]
    [DefaultValue(SADbType.VarChar)]
    public SADbType SADbType
    {
      get
      {
        SATrace.PropertyCall("<sa.SAParameter.get_SADbType|API>", _objectId);
        return _asaDbType;
      }
      set
      {
        SATrace.PropertyCall("<sa.SAParameter.set_SADbType|API>", _objectId);
        if (_asaDbType != value)
        {
                    _asaDbType = value;
                    _dbType = SADataConvert.MapToDbType(_asaDbType);
                    _rebind = true;
        }
                _inferType = false;
      }
    }

    /// <summary>
    ///     <para>Gets and sets value that indicates whether the source column is nullable. This allows SACommandBuilder to generate Update statements for nullable columns correctly.</para>
    /// </summary>
    /// <remarks>
    ///     <para>If the source column is nullable, true is returned; otherwise, false.</para>
    /// </remarks>
    public override bool SourceColumnNullMapping
    {
      get
      {
        SATrace.PropertyCall("<sa.SAParameter.get_SourceColumnNullMapping|API>", _objectId);
        return _sourceColumnNullMapping;
      }
      set
      {
        SATrace.PropertyCall("<sa.SAParameter.set_SourceColumnNullMapping|API>", _objectId);
                _sourceColumnNullMapping = value;
      }
    }

    /// <summary>
    ///     <para>Gets and sets the value of the parameter.</para>
    /// </summary>
    /// <value>An Object that specifies the value of the parameter.</value>
    /// <remarks>
    ///     <para>For input parameters, the value is bound to the SACommand that is sent to the server. For output and return value parameters, the value is set on completion of the SACommand and after the SADataReader is closed.</para>
    ///     <para>When sending a null parameter value to the server, you must specify DBNull, not null. The null value in the system is an empty object that has no value. DBNull is used to represent null values.</para>
    ///     <para>If the application specifies the database type, the bound value is converted to that type when the SQL Anywhere .NET Data Provider sends the data to the server. The provider attempts to convert any type of value if it supports the IConvertible interface. Conversion errors may result if the specified type is not compatible with the value.</para>
    ///     <para>Both the DbType and SADbType properties can be inferred by setting the Value.</para>
    ///     <para>The Value property is overwritten by Update.</para>
    /// </remarks>
    [TypeConverter(typeof (StringConverter))]
    [DefaultValue(null)]
    [Description("")]
    [Category("Data")]
    public override object Value
    {
      get
      {
        SATrace.PropertyCall("<sa.SAParameter.get_Value|API>", _objectId);
        return _value;
      }
      set
      {
        SATrace.PropertyCall("<sa.SAParameter.set_Value|API>", _objectId);
        if (_value == value)
          return;
                _value = value;
                _valueChanged = true;
        if (!ShouldInferType())
          return;
                SetTypeFromValue();
      }
    }

    internal bool Rebind
    {
      get
      {
        return _rebind;
      }
      set
      {
                _rebind = value;
      }
    }

    internal bool ValueChanged
    {
      get
      {
        return _valueChanged;
      }
    }

    /// <summary>
    ///     <para>Initializes an SAParameter object with null (Nothing in Visual Basic) as its value.</para>
    /// </summary>
    public SAParameter()
    {
            Init();
    }

    /// <summary>
    ///     <para>Initializes an SAParameter object with the specified parameter name and value. This constructor is not recommended; it is provided for compatibility with other data providers.</para>
    /// </summary>
    /// <param name="parameterName">The name of the parameter.</param>
    /// <param name="value">An Object that is the value of the parameter.</param>
    public SAParameter(string parameterName, object value)
    {
            Init();
            ParameterName = parameterName;
            Value = value;
    }

    /// <summary>
    ///     <para>Initializes an SAParameter object with the specified parameter name and data type.</para>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <param name="parameterName">The name of the parameter.</param>
    /// <param name="dbType">One of the SADbType values.</param>
    /// <seealso cref="P:iAnywhere.Data.SQLAnywhere.SAParameter.SADbType" />
    public SAParameter(string parameterName, SADbType dbType)
    {
            Init(parameterName, dbType);
    }

    /// <summary>
    ///     <para>Initializes an SAParameter object with the specified parameter name and data type.</para>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <param name="parameterName">The name of the parameter.</param>
    /// <param name="dbType">One of the SADbType values</param>
    /// <param name="size">The length of the parameter.</param>
    public SAParameter(string parameterName, SADbType dbType, int size)
    {
            Init(parameterName, dbType);
            SetSize(size, "size");
    }

    /// <summary>
    ///     <para>Initializes an SAParameter object with the specified parameter name, data type, and length.</para>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <param name="parameterName">The name of the parameter.</param>
    /// <param name="dbType">One of the SADbType values</param>
    /// <param name="size">The length of the parameter.</param>
    /// <param name="sourceColumn">The name of the source column to map.</param>
    public SAParameter(string parameterName, SADbType dbType, int size, string sourceColumn)
    {
            Init(parameterName, dbType);
            SetSize(size, "size");
            SourceColumn = sourceColumn;
    }

    internal SAParameter(SAParameter other)
    {
      ICloneable cloneable = other._value as ICloneable;
            _value = cloneable == null ? other._value : cloneable.Clone();
            _name = other._name;
            _precision = other._precision;
            _scale = other._scale;
            _size = other._size;
            _sourceColumn = other._sourceColumn;
            _sourceColumnNullMapping = other._sourceColumnNullMapping;
            _direction = other._direction;
            _dataRowVer = other._dataRowVer;
            _isNullable = other._isNullable;
            _dbType = other._dbType;
            _asaDbType = other._asaDbType;
            _offset = other._offset;
            _rebind = other._rebind;
            _valueChanged = other._valueChanged;
            _inferType = other._inferType;
    }

    /// <summary>
    ///     <para>Initializes an SAParameter object with the specified parameter name, data type, length, direction, nullability, numeric precision, numeric scale, source column, source version, and value.</para>
    /// </summary>
    /// <param name="parameterName">The name of the parameter.</param>
    /// <param name="dbType">One of the SADbType values</param>
    /// <param name="size">The length of the parameter.</param>
    /// <param name="direction">One of the ParameterDirection values.</param>
    /// <param name="isNullable">
    ///     True if the value of the field can be null; otherwise, false.
    /// </param>
    /// <param name="precision">
    ///     The total number of digits to the left and right of the decimal point to which Value is resolved.
    /// </param>
    /// <param name="scale">
    ///     The total number of decimal places to which Value is resolved.
    /// </param>
    /// <param name="sourceColumn">The name of the source column to map.</param>
    /// <param name="sourceVersion">One of the DataRowVersion values.</param>
    /// <param name="value">An Object that is the value of the parameter.</param>
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public SAParameter(string parameterName, SADbType dbType, int size, ParameterDirection direction, bool isNullable, byte precision, byte scale, string sourceColumn, DataRowVersion sourceVersion, object value)
    {
            Init(parameterName, dbType);
            SetSize(size, "size");
            SetPrecision(precision, "precision");
            Direction = direction;
            IsNullable = isNullable;
            Scale = scale;
            SourceColumn = sourceColumn;
            SourceVersion = sourceVersion;
            Value = value;
    }

    object ICloneable.Clone()
    {
      return new SAParameter(this);
    }

    private void Init()
    {
            _name = "";
            _precision = 0;
            _scale = 0;
            _size = 0;
            _sourceColumn = "";
            _value = null;
            _direction = ParameterDirection.Input;
            _dataRowVer = DataRowVersion.Current;
            _isNullable = false;
            _dbType = DbType.AnsiString;
            _asaDbType = SADbType.VarChar;
            _offset = 0;
            _rebind = true;
            _valueChanged = false;
            _inferType = true;
    }

    private void Init(string parameterName, SADbType dbType)
    {
            Init();
            ParameterName = parameterName;
            SADbType = dbType;
    }

    internal bool IsInputParameter()
    {
      if (Direction != ParameterDirection.Input)
        return Direction == ParameterDirection.InputOutput;
      return true;
    }

    internal bool IsOutputParameter()
    {
      if (Direction != ParameterDirection.Output && Direction != ParameterDirection.InputOutput)
        return Direction == ParameterDirection.ReturnValue;
      return true;
    }

    /// <summary>
    ///     <para>Resets the type (the values of DbType and SADbType) associated with this SAParameter.</para>
    /// </summary>
    public override void ResetDbType()
    {
      try
      {
        SATrace.FunctionScopeEnter("<sa.SAParameter.ResetDbType|API>", _objectId, new string[0]);
                _inferType = true;
        if (!ShouldInferType())
          return;
                SetTypeFromValue();
      }
      finally
      {
        SATrace.FunctionScopeLeave();
      }
    }

    private void SetPrecision(byte precision, string parmName)
    {
      if (precision > 38)
      {
        Exception e = new ArgumentException(SARes.GetString(11010, precision.ToString()), parmName);
        SATrace.Exception(e);
        throw e;
      }
      if (_precision == precision)
        return;
            _precision = precision;
            _rebind = true;
    }

    private void SetSize(int size, string parmName)
    {
      if (size < 0)
      {
        Exception e = new ArgumentException(SARes.GetString(11009, size.ToString()), parmName);
        SATrace.Exception(e);
        throw e;
      }
      if (_size == size)
        return;
            _size = size;
            _rebind = true;
    }

    private void SetParameterType(string type)
    {
      string[] names = Enum.GetNames(typeof (SADbType));
      Array values = Enum.GetValues(typeof (SADbType));
      for (int index = 0; index < names.Length; ++index)
      {
        if (names[index].Equals(type))
        {
                    SADbType = (SADbType) values.GetValue(index);
          break;
        }
      }
    }

    private bool ShouldInferType()
    {
      if (_inferType && _value != null && !DBNull.Value.Equals(_value))
        return !SADefault.Value.Equals(_value);
      return false;
    }

    private void SetTypeFromValue()
    {
      try
      {
                _asaDbType = SADataConvert.GetSADbTypeFromValue(_value);
                _dbType = SADataConvert.GetDbTypeFromValue(_value);
      }
      catch (Exception ex)
      {
        Exception e = new ArgumentException(SARes.GetString(11007, _value.GetType().Name), "value");
        SATrace.Exception(e);
        throw e;
      }
    }

    /// <summary>
    ///     <para>Returns a string containing the ParameterName.</para>
    /// </summary>
    /// <returns>
    /// <para>The name of the parameter.</para>
    ///    </returns>
    public override string ToString()
    {
      return _name;
    }

    /// <summary>Summary description for SAParameterConverter.</summary>
    internal sealed class SAParameterConverter : ExpandableObjectConverter
    {
      public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
      {
        if (destinationType == typeof (InstanceDescriptor))
          return true;
        return base.CanConvertTo(context, destinationType);
      }

      public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
      {
        if (destinationType == null)
          throw new ArgumentNullException("destinationType");
        if (value == null)
          throw new ArgumentNullException("value");
        if (destinationType == typeof (InstanceDescriptor) && value is SAParameter)
        {
          SAParameter saParameter = value as SAParameter;
          int num = 0;
          if (!saParameter.SADbType.ToString().Equals("VarChar"))
            num |= 1;
          if (!saParameter.Size.Equals(0))
            num |= 2;
          if (!saParameter.SourceColumn.Equals(""))
            num |= 4;
          if (saParameter.Value != null)
            num |= 8;
          if (!saParameter.IsNullable.Equals(false) || !saParameter.Precision.Equals(0) || (!saParameter.Scale.Equals(0) || !saParameter.Direction.Equals(ParameterDirection.Input) || !saParameter.SourceVersion.Equals((object) DataRowVersion.Current)))
            num |= 16;
          Type[] types;
          object[] objArray;
          switch (num)
          {
            case 0:
            case 1:
              types = new Type[2]
              {
                typeof (string),
                typeof (SADbType)
              };
              objArray = new object[2]
              {
                 saParameter.ParameterName,
                 saParameter.SADbType
              };
              break;
            case 2:
            case 3:
              types = new Type[3]
              {
                typeof (string),
                typeof (SADbType),
                typeof (int)
              };
              objArray = new object[3]
              {
                 saParameter.ParameterName,
                 saParameter.SADbType,
                 saParameter.Size
              };
              break;
            case 4:
            case 5:
            case 6:
            case 7:
              types = new Type[4]
              {
                typeof (string),
                typeof (SADbType),
                typeof (int),
                typeof (string)
              };
              objArray = new object[4]
              {
                 saParameter.ParameterName,
                 saParameter.SADbType,
                 saParameter.Size,
                 saParameter.SourceColumn
              };
              break;
            case 8:
              types = new Type[2]
              {
                typeof (string),
                typeof (object)
              };
              objArray = new object[2]
              {
                 saParameter.ParameterName,
                saParameter.Value
              };
              break;
            default:
              types = new Type[10]
              {
                typeof (string),
                typeof (SADbType),
                typeof (int),
                typeof (ParameterDirection),
                typeof (bool),
                typeof (byte),
                typeof (byte),
                typeof (string),
                typeof (DataRowVersion),
                typeof (object)
              };
              objArray = new object[10]
              {
                 saParameter.ParameterName,
                 saParameter.SADbType,
                 saParameter.Size,
                 saParameter.Direction,
                 saParameter.IsNullable,
                 saParameter.Precision,
                 saParameter.Scale,
                 saParameter.SourceColumn,
                 saParameter.SourceVersion,
                saParameter.Value
              };
              break;
          }
          ConstructorInfo constructor = typeof (SAParameter).GetConstructor(types);
          if (constructor != null)
            return (object) new InstanceDescriptor(constructor, objArray);
        }
        return base.ConvertTo(context, culture, value, destinationType);
      }
    }
  }
}
