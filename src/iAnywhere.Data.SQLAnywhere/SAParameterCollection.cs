using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Runtime.InteropServices;

namespace iAnywhere.Data.SQLAnywhere
{
    public sealed class SAParameterCollection : DbParameterCollection
    {
        private int _objectId = SAParameterCollection.s_CurrentId++;
        private ArrayList _parms;
        private static int s_CurrentId;

        public override int Count
        {
            get
            {
                return _parms.Count;
            }
        }

        public SAParameter this[int index]
        {
            get
            {
                return (SAParameter)GetParameter(index);
            }
            set
            {
                SetParameter(index, value);
            }
        }

        public SAParameter this[string parameterName]
        {
            get
            {
                return (SAParameter)GetParameter(parameterName);
            }
            set
            {
                SetParameter(parameterName, value);
            }
        }

        public override object SyncRoot
        {
            get
            {
                return this;
            }
        }

        internal SAParameterCollection()
        {
            _parms = new ArrayList();
        }

        private void CheckArgumentNull(string arg, object value)
        {
            if (value == null)
            {
                Exception e = new ArgumentNullException(arg);
                throw e;
            }
        }

        private void CheckArgumentType(object value)
        {
            if (!(value is SAParameter))
            {
                Exception e = new InvalidCastException(SARes.GetString(11014, value.GetType().Name));
                throw e;
            }
        }

        private void CheckIndex(int index)
        {
            if (index < 0 || index >= _parms.Count)
            {
                Exception e = new IndexOutOfRangeException(SARes.GetString(11011, index.ToString(), _parms.Count.ToString()));
                SATrace.Exception(e);
                throw e;
            }
        }

        private void CheckIndex(string parameterName)
        {
            if (FindIndex(parameterName) < 0)
            {
                Exception e = new IndexOutOfRangeException(SARes.GetString(11012, parameterName));
                SATrace.Exception(e);
                throw e;
            }
        }

        /// <summary>
        ///     <para>Returns a parameter from the SAParameterCollection object.</para>
        /// </summary>
        /// <param name="index">
        ///     The zero-based index of the parameter within the collection.
        /// </param>
        /// <returns>
        /// <para>A <see cref="T:System.Data.Common.DbParameter" /> from SAParameterCollection object.</para>
        ///    </returns>
        /// <seealso cref="T:iAnywhere.Data.SQLAnywhere.SAParameterCollection" />
        protected override DbParameter GetParameter(int index)
        {
            CheckIndex(index);
            return (DbParameter)_parms[index];
        }

        /// <summary>
        ///     <para>Returns a parameter from the SAParameterCollection object.</para>
        /// </summary>
        /// <param name="parameterName">The name of the parameter to locate.</param>
        /// <returns>
        /// <para>A <see cref="T:System.Data.Common.DbParameter" /> from SAParameterCollection object.</para>
        ///    </returns>
        /// <seealso cref="T:iAnywhere.Data.SQLAnywhere.SAParameterCollection" />
        protected override DbParameter GetParameter(string parameterName)
        {
            CheckIndex(parameterName);
            return (DbParameter)_parms[FindIndex(parameterName)];
        }

        /// <summary>
        ///     <para>Sets a parameter in the SAParameterCollection object.</para>
        /// </summary>
        /// <param name="index">The zero-based index of the parameter to set.</param>
        /// <param name="value">
        /// A <see cref="T:System.Data.Common.DbParameter" /> to be inserted into the SAParameterCollection object.
        ///    </param>
        /// <seealso cref="T:iAnywhere.Data.SQLAnywhere.SAParameterCollection" />
        protected override void SetParameter(int index, DbParameter value)
        {
            CheckIndex(index);
            CheckArgumentNull("value", value);
            _parms[index] = value;
        }

        /// <summary>
        ///     <para>Sets a parameter in the SAParameterCollection object.</para>
        /// </summary>
        /// <param name="parameterName">The name of the parameter to set.</param>
        /// <param name="value">
        /// A <see cref="T:System.Data.Common.DbParameter" /> to be inserted into the SAParameterCollection object.
        ///    </param>
        /// <seealso cref="T:iAnywhere.Data.SQLAnywhere.SAParameterCollection" />
        protected override void SetParameter(string parameterName, DbParameter value)
        {
            CheckIndex(parameterName);
            CheckArgumentNull("value", value);
            int index = FindIndex(parameterName);
            _parms[index] = value;
            ((DbParameter)_parms[index]).ParameterName = parameterName;
        }

        private SAParameter AddParameter()
        {
            return Add(new SAParameter("p" + _parms.Count.ToString(), null));
        }

        /// <summary>
        ///     <para>Adds an SAParameter object to this collection.</para>
        /// </summary>
        /// <param name="value">
        ///     The SAParameter object to add to the collection.
        /// </param>
        /// <returns>
        /// <para>The index of the new SAParameter object.</para>
        ///    </returns>
        /// <seealso cref="T:iAnywhere.Data.SQLAnywhere.SAParameter" />
        public override int Add(object value)
        {
            try
            {
                SATrace.FunctionScopeEnter("<sa.SAParameterCollection.Add|API>", _objectId, "value");
                CheckArgumentNull("value", value);
                CheckArgumentType(value);
                if (Contains(value))
                {
                    Exception e = new ArgumentException(SARes.GetString(11016, ((DbParameter)value).ParameterName), "value");
                    SATrace.Exception(e);
                    throw e;
                }
                return _parms.Add(value);
            }
            finally
            {
                SATrace.FunctionScopeLeave();
            }
        }

        /// <summary>
        ///     <para>Adds an SAParameter object to this collection.</para>
        /// </summary>
        /// <param name="value">
        ///     The SAParameter object to add to the collection.
        /// </param>
        /// <returns>
        /// <para>The new SAParameter object.</para>
        ///    </returns>
        public SAParameter Add(SAParameter value)
        {
            try
            {
                SATrace.FunctionScopeEnter("<sa.SAParameterCollection.Add|API>", _objectId, "value");
                return (SAParameter)_parms[Add((object)value)];
            }
            finally
            {
                SATrace.FunctionScopeLeave();
            }
        }

        /// <summary>
        ///     <para>Adds an SAParameter object to this collection, created using the specified parameter name and value, to the collection.</para>
        /// </summary>
        /// <remarks>
        ///     <para>Because of the special treatment of the 0 and 0.0 constants and the way overloaded methods are resolved, it is highly recommended that you explicitly cast constant values to type object when using this method.</para>
        /// </remarks>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <param name="value">
        ///     The value of the parameter to add to the connection.
        /// </param>
        /// <returns>
        /// <para>The new SAParameter object.</para>
        ///    </returns>
        /// <seealso cref="T:iAnywhere.Data.SQLAnywhere.SAParameter" />
        public SAParameter Add(string parameterName, object value)
        {
            try
            {
                SATrace.FunctionScopeEnter("<sa.SAParameterCollection.Add|API>", _objectId, "parameterName", "value");
                return Add(new SAParameter(parameterName, value));
            }
            finally
            {
                SATrace.FunctionScopeLeave();
            }
        }

        /// <summary>
        ///     <para>Adds an SAParameter object to this collection, created using the specified parameter name and data type, to the collection.</para>
        /// </summary>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <param name="saDbType">One of the SADbType values.</param>
        /// <returns>
        /// <para>The new SAParameter object.</para>
        ///    </returns>
        /// <seealso cref="T:iAnywhere.Data.SQLAnywhere.SADbType" />
        /// <seealso cref="M:iAnywhere.Data.SQLAnywhere.SAParameterCollection.Add(iAnywhere.Data.SQLAnywhere.SAParameter)" />
        /// <seealso cref="M:iAnywhere.Data.SQLAnywhere.SAParameterCollection.Add(System.String,System.Object)" />
        public SAParameter Add(string parameterName, SADbType saDbType)
        {
            try
            {
                SATrace.FunctionScopeEnter("<sa.SAParameterCollection.Add|API>", _objectId, "parameterName", "saDbType");
                return Add(new SAParameter(parameterName, saDbType));
            }
            finally
            {
                SATrace.FunctionScopeLeave();
            }
        }

        /// <summary>
        ///     <para>Adds an SAParameter object to this collection, created using the specified parameter name, data type, and length, to the collection.</para>
        /// </summary>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <param name="saDbType">One of the SADbType values.</param>
        /// <param name="size">The length of the parameter.</param>
        /// <returns>
        /// <para>The new SAParameter object.</para>
        ///    </returns>
        /// <seealso cref="T:iAnywhere.Data.SQLAnywhere.SADbType" />
        /// <seealso cref="M:iAnywhere.Data.SQLAnywhere.SAParameterCollection.Add(iAnywhere.Data.SQLAnywhere.SAParameter)" />
        /// <seealso cref="M:iAnywhere.Data.SQLAnywhere.SAParameterCollection.Add(System.String,System.Object)" />
        public SAParameter Add(string parameterName, SADbType saDbType, int size)
        {
            try
            {
                SATrace.FunctionScopeEnter("<sa.SAParameterCollection.Add|API>", _objectId, "parameterName", "saDbType", "size");
                return Add(new SAParameter(parameterName, saDbType, size));
            }
            finally
            {
                SATrace.FunctionScopeLeave();
            }
        }

        /// <summary>
        ///     <para>Adds an SAParameter object to this collection, created using the specified parameter name, data type, length, and source column name, to the collection.</para>
        /// </summary>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <param name="saDbType">One of the SADbType values.</param>
        /// <param name="size">The length of the column.</param>
        /// <param name="sourceColumn">The name of the source column to map.</param>
        /// <returns>
        /// <para>The new SAParameter object.</para>
        ///    </returns>
        /// <seealso cref="T:iAnywhere.Data.SQLAnywhere.SADbType" />
        /// <seealso cref="M:iAnywhere.Data.SQLAnywhere.SAParameterCollection.Add(iAnywhere.Data.SQLAnywhere.SAParameter)" />
        /// <seealso cref="M:iAnywhere.Data.SQLAnywhere.SAParameterCollection.Add(System.String,System.Object)" />
        public SAParameter Add(string parameterName, SADbType saDbType, int size, string sourceColumn)
        {
            try
            {
                SATrace.FunctionScopeEnter("<sa.SAParameterCollection.Add|API>", _objectId, "parameterName", "saDbType", "size", "sourceColumn");
                return Add(new SAParameter(parameterName, saDbType, size, sourceColumn));
            }
            finally
            {
                SATrace.FunctionScopeLeave();
            }
        }

        /// <summary>
        ///     <para>Adds a value to the end of this collection.</para>
        /// </summary>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <param name="value">The value to be added.</param>
        /// <returns>
        ///     <para>The new SAParameter object.</para>
        /// </returns>
        public SAParameter AddWithValue(string parameterName, object value)
        {
            try
            {
                SATrace.FunctionScopeEnter("<sa.SAParameterCollection.AddWithValue|API>", _objectId, "parameterName", "value");
                return Add(new SAParameter(parameterName, value));
            }
            finally
            {
                SATrace.FunctionScopeLeave();
            }
        }

        /// <summary>
        ///     <para>Adds an array of values to the end of the SAParameterCollection.</para>
        /// </summary>
        /// <param name="values">The values to add.</param>
        public override void AddRange(Array values)
        {
            try
            {
                SATrace.FunctionScopeEnter("<sa.SAParameterCollection.AddRange|API>", _objectId, "values");
                if (values == null)
                    throw new ArgumentNullException("values");
                foreach (object obj in values)
                {
                    if (obj.GetType() != typeof(SAParameter))
                    {
                        Exception e = new ArgumentException(SARes.GetString(15019, obj.GetType().ToString()), "values");
                        SATrace.Exception(e);
                        throw e;
                    }
                }
                _parms.AddRange(values);
            }
            finally
            {
                SATrace.FunctionScopeLeave();
            }
        }

        /// <summary>
        ///     <para>Adds an array of values to the end of the SAParameterCollection.</para>
        /// </summary>
        /// <param name="values">
        ///     An array of SAParameter objects to add to the end of this collection.
        /// </param>
        public void AddRange(SAParameter[] values)
        {
            try
            {
                SATrace.FunctionScopeEnter("<sa.SAParameterCollection.AddRange|API>", _objectId, "values");
                AddRange((Array)values);
            }
            finally
            {
                SATrace.FunctionScopeLeave();
            }
        }

        /// <summary>
        ///     <para>Removes all items from the collection.</para>
        /// </summary>
        public override void Clear()
        {
            try
            {
                SATrace.FunctionScopeEnter("<sa.SAParameterCollection.Clear|API>", _objectId, new string[0]);
                _parms.Clear();
            }
            finally
            {
                SATrace.FunctionScopeLeave();
            }
        }

        /// <summary>
        ///     <para>Indicates whether an SAParameter object exists in the collection.</para>
        /// </summary>
        /// <param name="value">The SAParameter object to find.</param>
        /// <returns>
        /// <para>True if the collection contains the SAParameter object. Otherwise, false.</para>
        ///    </returns>
        /// <seealso cref="T:iAnywhere.Data.SQLAnywhere.SAParameter" />
        /// <seealso cref="M:iAnywhere.Data.SQLAnywhere.SAParameterCollection.Contains(System.String)" />
        public override bool Contains(object value)
        {
            try
            {
                SATrace.FunctionScopeEnter("<sa.SAParameterCollection.Contains|API>", _objectId, "value");
                return _parms.Contains(value);
            }
            finally
            {
                SATrace.FunctionScopeLeave();
            }
        }

        /// <summary>
        ///     <para>Indicates whether an SAParameter object exists in the collection.</para>
        /// </summary>
        /// <param name="value">The name of the parameter to search for.</param>
        /// <returns>
        /// <para>True if the collection contains the SAParameter object. Otherwise, false.</para>
        ///    </returns>
        /// <seealso cref="T:iAnywhere.Data.SQLAnywhere.SAParameter" />
        /// <seealso cref="M:iAnywhere.Data.SQLAnywhere.SAParameterCollection.Contains(System.Object)" />
        public override bool Contains(string value)
        {
            try
            {
                SATrace.FunctionScopeEnter("<sa.SAParameterCollection.Contains|API>", _objectId, "value");
                return FindIndex(value) >= 0;
            }
            finally
            {
                SATrace.FunctionScopeLeave();
            }
        }

        /// <summary>
        ///     <para>Copies SAParameter objects from the SAParameterCollection to the specified array.</para>
        /// </summary>
        /// <param name="array">The array to copy the SAParameter objects into.</param>
        /// <param name="index">The starting index of the array.</param>
        /// <seealso cref="T:iAnywhere.Data.SQLAnywhere.SAParameter" />
        /// <seealso cref="T:iAnywhere.Data.SQLAnywhere.SAParameterCollection" />
        public override void CopyTo(Array array, int index)
        {
            try
            {
                SATrace.FunctionScopeEnter("<sa.SAParameterCollection.CopyTo|API>", _objectId, "array", "index");
                _parms.CopyTo(array, index);
            }
            finally
            {
                SATrace.FunctionScopeLeave();
            }
        }

        /// <summary>
        ///     <para>Returns the location of the SAParameter object in the collection.</para>
        /// </summary>
        /// <param name="value">The SAParameter object to locate.</param>
        /// <returns>
        /// <para>The zero-based location of the SAParameter object in the collection.</para>
        ///    </returns>
        /// <seealso cref="T:iAnywhere.Data.SQLAnywhere.SAParameter" />
        /// <seealso cref="M:iAnywhere.Data.SQLAnywhere.SAParameterCollection.IndexOf(System.String)" />
        public override int IndexOf(object value)
        {
            try
            {
                SATrace.FunctionScopeEnter("<sa.SAParameterCollection.IndexOf|API>", _objectId, "value");
                if (value == null)
                    return -1;
                CheckArgumentType(value);
                return _parms.IndexOf(value);
            }
            finally
            {
                SATrace.FunctionScopeLeave();
            }
        }

        /// <summary>
        ///     <para>Returns the location of the SAParameter object in the collection.</para>
        /// </summary>
        /// <param name="parameterName">The name of the parameter to locate.</param>
        /// <returns>
        /// <para>The zero-based index of the SAParameter object in the collection.</para>
        ///    </returns>
        /// <seealso cref="T:iAnywhere.Data.SQLAnywhere.SAParameter" />
        /// <seealso cref="M:iAnywhere.Data.SQLAnywhere.SAParameterCollection.IndexOf(System.Object)" />
        public override int IndexOf(string parameterName)
        {
            try
            {
                SATrace.FunctionScopeEnter("<sa.SAParameterCollection.IndexOf|API>", _objectId, "parameterName");
                return FindIndex(parameterName);
            }
            finally
            {
                SATrace.FunctionScopeLeave();
            }
        }

        /// <summary>
        ///     <para>Inserts an SAParameter object in the collection at the specified index.</para>
        /// </summary>
        /// <param name="index">
        ///     The zero-based index where the parameter is to be inserted within the collection.
        /// </param>
        /// <param name="value">
        ///     The SAParameter object to add to the collection.
        /// </param>
        public override void Insert(int index, object value)
        {
            try
            {
                SATrace.FunctionScopeEnter("<sa.SAParameterCollection.Insert|API>", _objectId, "index", "value");
                CheckArgumentNull("value", value);
                CheckArgumentType(value);
                _parms.Insert(index, value);
            }
            finally
            {
                SATrace.FunctionScopeLeave();
            }
        }

        /// <summary>
        ///     <para>Removes the specified SAParameter object from the collection.</para>
        /// </summary>
        /// <param name="value">
        ///     The SAParameter object to remove from the collection.
        /// </param>
        public override void Remove(object value)
        {
            try
            {
                SATrace.FunctionScopeEnter("<sa.SAParameterCollection.Remove|API>", _objectId, "value");
                CheckArgumentNull("value", value);
                CheckArgumentType(value);
                if (!Contains(value))
                {
                    Exception e = new ArgumentException(SARes.GetString(11015), "value");
                    SATrace.Exception(e);
                    throw e;
                }
                _parms.Remove(value);
            }
            finally
            {
                SATrace.FunctionScopeLeave();
            }
        }

        /// <summary>
        ///     <para>Removes the specified SAParameter object from the collection.</para>
        /// </summary>
        /// <param name="index">
        ///     The zero-based index of the parameter to remove.
        /// </param>
        /// <seealso cref="M:iAnywhere.Data.SQLAnywhere.SAParameterCollection.RemoveAt(System.String)" />
        public override void RemoveAt(int index)
        {
            try
            {
                SATrace.FunctionScopeEnter("<sa.SAParameterCollection.RemoveAt|API>", _objectId, "index");
                CheckIndex(index);
                _parms.RemoveAt(index);
            }
            finally
            {
                SATrace.FunctionScopeLeave();
            }
        }

        /// <summary>
        ///     <para>Removes the specified SAParameter object from the collection.</para>
        /// </summary>
        /// <param name="parameterName">
        ///     The name of the SAParameter object to remove.
        /// </param>
        /// <seealso cref="M:iAnywhere.Data.SQLAnywhere.SAParameterCollection.RemoveAt(System.Int32)" />
        public override void RemoveAt(string parameterName)
        {
            try
            {
                SATrace.FunctionScopeEnter("<sa.SAParameterCollection.RemoveAt|API>", _objectId, "parameterName");
                CheckIndex(parameterName);
                _parms.RemoveAt(FindIndex(parameterName));
            }
            finally
            {
                SATrace.FunctionScopeLeave();
            }
        }

        /// <summary>
        ///     <para>Returns an enumerator that iterates through the SAParameterCollection.</para>
        /// </summary>
        /// <returns>
        /// <para>An <see cref="T:System.Collections.IEnumerator" /> for the SAParameterCollection object.</para>
        ///    </returns>
        /// <seealso cref="T:iAnywhere.Data.SQLAnywhere.SAParameterCollection" />
        public override IEnumerator GetEnumerator()
        {
            try
            {
                SATrace.FunctionScopeEnter("<sa.SAParameterCollection.GetEnumerator|API>", _objectId, new string[0]);
                return _parms.GetEnumerator();
            }
            finally
            {
                SATrace.FunctionScopeLeave();
            }
        }

        internal int GetRebindParameterCount()
        {
            int num = 0;
            for (int index = 0; index < _parms.Count; ++index)
            {
                if (((SAParameter)_parms[index]).Rebind)
                    ++num;
            }
            return num;
        }

        internal int GetInputParameterCount()
        {
            int num = 0;
            foreach (SAParameter parm in _parms)
            {
                if (parm.IsInputParameter())
                    ++num;
            }
            return num;
        }

        internal int GetOutputParameterCount()
        {
            int num = 0;
            foreach (SAParameter parm in _parms)
            {
                if (parm.IsOutputParameter())
                    ++num;
            }
            return num;
        }

        internal unsafe void GetParameterInfo(out int count, SAParameterDM** ppParmsDM, bool addValues, bool namedParms, string[] parmNames)
        {
            count = GetRebindParameterCount();
            if (count < 1)
                return;
            if (addValues)
                MapNullParameters();
            *ppParmsDM = (SAParameterDM*)(void*)SAUnmanagedMemory.Alloc(count * Marshal.SizeOf(typeof(SAParameterDM)));
            SAParameterDM* pParmDM = *ppParmsDM;
            if (namedParms)
            {
                for (int index1 = 0; index1 < parmNames.GetLength(0); ++index1)
                {
                    for (int index2 = 0; index2 < _parms.Count; ++index2)
                    {
                        SAParameter parm = (SAParameter)_parms[index2];
                        if (string.Compare(parmNames[index1], parm.ParameterName, true) == 0)
                        {
                            SetParameterInfo(parm, pParmDM, addValues);
                            break;
                        }
                    }
                    pParmDM.Ordinal = index1;
                    ++pParmDM;
                }
            }
            else
            {
                for (int index = 0; index < _parms.Count; ++index)
                {
                    pParmDM.Ordinal = index;
                    SetParameterInfo(_parms[index] as SAParameter, pParmDM, addValues);
                    ++pParmDM;
                }
            }
        }

        private void SetParameterInfo(SAParameter parm, SAParameterDM pParmDM, bool addValues)
        {
            if (!parm.Rebind)
                return;
            pParmDM.Size = parm.Size;
            pParmDM.IsNullable = parm.IsNullable ? 1 : 0;
            pParmDM.Direction = (int)parm.Direction;
            pParmDM.Precision = parm.Precision;
            pParmDM.Scale = parm.Scale;
            pParmDM.Value.SADataType = (int)parm.SADbType;
            if (parm.SADbType == SADbType.Decimal || parm.SADbType == SADbType.Numeric)
            {
                if (pParmDM.Precision <= 0)
                    pParmDM.Precision = 30;
            }
            else if (parm.SADbType == SADbType.Money)
            {
                pParmDM.Precision = 19;
                pParmDM.Scale = 4;
            }
            else if (parm.SADbType == SADbType.SmallMoney)
            {
                pParmDM.Precision = 10;
                pParmDM.Scale = 4;
            }
            pParmDM.Name = SAUtility.GetUnmanagedString(parm.ParameterName);
            if (addValues)
            {
                if (parm.Direction != ParameterDirection.Input && parm.Direction != ParameterDirection.InputOutput)
                    return;
                if (parm.Value != null && parm.Value != DBNull.Value && parm.Value != SADefault.Value)
                {
                    pParmDM.Value.IsNull = 0;
                    pParmDM.Value.IsDefault = 0;
                    SADataItem sa = SADataConvert.DotNetToSA(parm);
                    pParmDM.Value.SADataType = sa.SADataType;
                    pParmDM.Value.Length = sa.Length;
                    pParmDM.Value.Value = sa.Value;
                    if (!SADataConvert.IsDecimal(sa.SADataType))
                        return;
                    pParmDM.Scale = (((SADecimal*)(void*)sa.Value).Scale);
                }
                else if (parm.Value == SADefault.Value)
                {
                    pParmDM.Value.IsNull = 0;
                    pParmDM.Value.IsDefault = 1;
                    pParmDM.Value.Length = 0;
                    pParmDM.Value.Value = (IntPtr)0;
                }
                else
                {
                    pParmDM.Value.IsNull = 1;
                    pParmDM.Value.IsDefault = 0;
                    pParmDM.Value.Length = 0;
                    pParmDM.Value.Value = (IntPtr)0;
                }
            }
            else
            {
                pParmDM.Value.IsNull = 1;
                pParmDM.Value.IsDefault = 0;
                pParmDM.Value.Length = 0;
                pParmDM.Value.Value = (IntPtr)0;
            }
        }

        internal unsafe void FreeParameterInfo(int count, SAParameterDM* pParmsDM)
        {
            if (count <= 0)
                return;
            SAParameterDM* saParameterDmPtr = pParmsDM;
            for (int index = 0; index < count; ++index)
            {
                SAUnmanagedMemory.Free(saParameterDmPtr.Value.Value);
                SAUnmanagedMemory.Free(saParameterDmPtr.Name);
                ++saParameterDmPtr;
            }
            SAUnmanagedMemory.Free((IntPtr)((void*)pParmsDM));
        }

        internal unsafe void GetInputParameterValues(out int count, SAValue** ppValues, string[] allParmNames, string[] inParmNames, bool _namedParms)
        {
            count = GetInputParameterCount();
            if (count < 1)
                return;
            MapNullParameters();
            *ppValues = (SAValue*)(void*)SAUnmanagedMemory.Alloc(count * Marshal.SizeOf(typeof(SAValue)));
            SAValue* pVal = *ppValues;
            if (_namedParms)
            {
                for (int index1 = 0; index1 < inParmNames.GetLength(0); ++index1)
                {
                    for (int index2 = 0; index2 < _parms.Count; ++index2)
                    {
                        SAParameter parm = (SAParameter)_parms[index2];
                        if (string.Compare(inParmNames[index1], parm.ParameterName, true) == 0 && parm.IsInputParameter())
                        {
                            for (int index3 = 0; index3 < allParmNames.GetLength(0); ++index3)
                            {
                                if (string.Compare(inParmNames[index1], allParmNames[index3], true) == 0)
                                {
                                    pVal.Ordinal = index3;
                                    break;
                                }
                            }
                            SetParameterValue(parm, pVal);
                            ++pVal;
                            break;
                        }
                    }
                }
            }
            else
            {
                for (int index = 0; index < _parms.Count; ++index)
                {
                    SAParameter parm = (SAParameter)_parms[index];
                    if (parm.IsInputParameter())
                    {
                        pVal.Ordinal = index;
                        SetParameterValue(parm, pVal);
                        ++pVal;
                    }
                }
            }
        }

        private unsafe void SetParameterValue(SAParameter parm, SAValue* pVal)
        {
            if (parm.Value != null && !DBNull.Value.Equals(parm.Value) && !SADefault.Value.Equals(parm.Value))
            {
                SADataItem sa = SADataConvert.DotNetToSA(parm);
                pVal.Value.IsNull = 0;
                pVal.Value.IsDefault = 0;
                pVal.Value.SADataType = sa.SADataType;
                pVal.Value.Length = sa.Length;
                pVal.Value.Value = sa.Value;
            }
            else if (parm.Value == SADefault.Value)
            {
                pVal.Value.IsNull = 0;
                pVal.Value.IsDefault = 1;
                pVal.Value.Length = 0;
                pVal.Value.SADataType = (int)parm.SADbType;
                pVal.Value.Value = (IntPtr)0;
            }
            else
            {
                pVal.Value.IsNull = 1;
                pVal.Value.IsDefault = 0;
                pVal.Value.Length = 0;
                pVal.Value.SADataType = (int)parm.SADbType;
                pVal.Value.Value = (IntPtr)0;
            }
        }

        private void MapNullParameters()
        {
            foreach (SAParameter parm in _parms)
            {
                if (parm.SourceColumnNullMapping && parm.IsNullable && (parm.SourceVersion == DataRowVersion.Original && parm.SourceColumn != null) && (parm.SourceColumn.Trim().Length > 0 && (parm.Direction == ParameterDirection.Input || parm.Direction == ParameterDirection.InputOutput)))
                {
                    if (parm.Value == null || DBNull.Value.Equals(parm.Value))
                        parm.Value = 1;
                    else
                        parm.Value = 0;
                }
            }
        }

        internal unsafe void FreeParameterValues(int count, SAValue* pValues)
        {
            if (count <= 0)
                return;
            SAValue* saValuePtr = pValues;
            for (int index = 0; index < count; ++index)
            {
                SAUnmanagedMemory.Free(saValuePtr.Value.Value);
                ++saValuePtr;
            }
            SAUnmanagedMemory.Free((IntPtr)((void*)pValues));
        }

        private int FindIndex(string parameterName)
        {
            for (int index = 0; index < _parms.Count; ++index)
            {
                if (((DbParameter)_parms[index]).ParameterName == parameterName)
                    return index;
            }
            return -1;
        }
    }
}
