using System;
using System.Data;
using System.Data.Common;

namespace iAnywhere.Data.SQLAnywhere
{
    public sealed class SAParameter : DbParameter
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

        public override DbType DbType
        {
            get
            {
                return _dbType;
            }
            set
            {
                if (value == DbType.Object || value == DbType.SByte || value == DbType.VarNumeric)
                {
                    Exception e = new ArgumentException(SARes.GetString(11008, value.ToString()), "value");
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

        public override ParameterDirection Direction
        {
            get
            {
                return _direction;
            }
            set
            {
                if (_direction == value)
                    return;
                _direction = value;
                _rebind = true;
            }
        }

        public override bool IsNullable
        {
            get
            {
                return _isNullable;
            }
            set
            {
                if (_isNullable == value)
                    return;
                _isNullable = value;
                _rebind = true;
            }
        }

        public int Offset
        {
            get
            {
                return _offset;
            }
            set
            {
                if (_offset == value)
                    return;
                _offset = value;
                _rebind = true;
            }
        }

        public override string ParameterName
        {
            get
            {
                return _name;
            }
            set
            {
                if (_name.Equals(value))
                    return;
                _name = value != null ? value : "";
                _rebind = true;
            }
        }

        public byte Precision
        {
            get
            {
                return _precision;
            }
            set
            {
                SetPrecision(value, "value");
            }
        }

        public byte Scale
        {
            get
            {
                return _scale;
            }
            set
            {
                if (_scale == value)
                    return;
                _scale = value;
                _rebind = true;
            }
        }

        public override int Size
        {
            get
            {
                return _size;
            }
            set
            {
                SetSize(value, "value");
            }
        }

        public override string SourceColumn
        {
            get
            {
                return _sourceColumn;
            }
            set
            {
                if (_sourceColumn.Equals(value))
                    return;
                _sourceColumn = value == null ? "" : value;
                _rebind = true;
            }
        }

        public SADbType SADbType
        {
            get
            {
                return _asaDbType;
            }
            set
            {
                if (_asaDbType != value)
                {
                    _asaDbType = value;
                    _dbType = SADataConvert.MapToDbType(_asaDbType);
                    _rebind = true;
                }
                _inferType = false;
            }
        }

        public override bool SourceColumnNullMapping
        {
            get
            {
                return _sourceColumnNullMapping;
            }
            set
            {
                _sourceColumnNullMapping = value;
            }
        }

        public override object Value
        {
            get
            {
                return _value;
            }
            set
            {
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

        public SAParameter()
        {
            Init();
        }

        public SAParameter(string parameterName, object value)
        {
            Init();
            ParameterName = parameterName;
            Value = value;
        }

        public SAParameter(string parameterName, SADbType dbType)
        {
            Init(parameterName, dbType);
        }

        public SAParameter(string parameterName, SADbType dbType, int size)
        {
            Init(parameterName, dbType);
            SetSize(size, "size");
        }

        public SAParameter(string parameterName, SADbType dbType, int size, string sourceColumn)
        {
            Init(parameterName, dbType);
            SetSize(size, "size");
            SourceColumn = sourceColumn;
        }

        internal SAParameter(SAParameter other)
        {
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

        public SAParameter(string parameterName, SADbType dbType, int size, ParameterDirection direction, bool isNullable, byte precision, byte scale, string sourceColumn, object value)
        {
            Init(parameterName, dbType);
            SetSize(size, "size");
            SetPrecision(precision, "precision");
            Direction = direction;
            IsNullable = isNullable;
            Scale = scale;
            SourceColumn = sourceColumn;
            Value = value;
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
            _inferType = true;
            if (!ShouldInferType())
                return;
            SetTypeFromValue();
        }

        private void SetPrecision(byte precision, string parmName)
        {
            if (precision > 38)
            {
                Exception e = new ArgumentException(SARes.GetString(11010, precision.ToString()), parmName);
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
                throw e;
            }
            if (_size == size)
                return;
            _size = size;
            _rebind = true;
        }

        private void SetParameterType(string type)
        {
            string[] names = Enum.GetNames(typeof(SADbType));
            Array values = Enum.GetValues(typeof(SADbType));
            for (int index = 0; index < names.Length; ++index)
            {
                if (names[index].Equals(type))
                {
                    SADbType = (SADbType)values.GetValue(index);
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
                throw e;
            }
        }

        public override string ToString()
        {
            return _name;
        }
    }
}
