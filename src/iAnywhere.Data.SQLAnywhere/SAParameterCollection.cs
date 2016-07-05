using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Runtime.InteropServices;

namespace iAnywhere.Data.SQLAnywhere
{
    public sealed class SAParameterCollection : DbParameterCollection
    {
        private int _objectId = SAParameterCollection.s_CurrentId++;
        private List<SAParameter> _parms;
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
            _parms = new List<SAParameter>();
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
                throw e;
            }
        }

        private void CheckIndex(string parameterName)
        {
            if (FindIndex(parameterName) < 0)
            {
                Exception e = new IndexOutOfRangeException(SARes.GetString(11012, parameterName));
                throw e;
            }
        }

        protected override DbParameter GetParameter(int index)
        {
            CheckIndex(index);
            return _parms[index];
        }

        protected override DbParameter GetParameter(string parameterName)
        {
            CheckIndex(parameterName);
            return _parms[FindIndex(parameterName)];
        }

        protected override void SetParameter(int index, DbParameter value)
        {
            CheckIndex(index);
            CheckArgumentNull("value", value);
            _parms[index] = (SAParameter)value;
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
            _parms[index] = (SAParameter)value;
            _parms[index].ParameterName = parameterName;
        }

        private SAParameter AddParameter()
        {
            return Add(new SAParameter("p" + _parms.Count, null));
        }

        public override int Add(object value)
        {

            CheckArgumentNull("value", value);
            CheckArgumentType(value);
            if (Contains(value))
            {
                Exception e = new ArgumentException(SARes.GetString(11016, ((DbParameter)value).ParameterName), "value");

                throw e;
            }
            _parms.Add((SAParameter)value);
            return _parms.Count - 1;
        }

        public SAParameter Add(SAParameter value)
        {
            return _parms[Add((object)value)];
        }

        public SAParameter Add(string parameterName, object value)
        {
            return Add(new SAParameter(parameterName, value));
        }


        public SAParameter Add(string parameterName, SADbType saDbType)
        {
            return Add(new SAParameter(parameterName, saDbType));
        }

        public SAParameter Add(string parameterName, SADbType saDbType, int size)
        {
            return Add(new SAParameter(parameterName, saDbType, size));
        }

        public SAParameter Add(string parameterName, SADbType saDbType, int size, string sourceColumn)
        {
            return Add(new SAParameter(parameterName, saDbType, size, sourceColumn));
        }


        public SAParameter AddWithValue(string parameterName, object value)
        {
            return Add(new SAParameter(parameterName, value));
        }

        /// <summary>
        ///     <para>Adds an array of values to the end of the SAParameterCollection.</para>
        /// </summary>
        /// <param name="values">The values to add.</param>
        public override void AddRange(Array values)
        {

            if (values == null)
                throw new ArgumentNullException("values");
            foreach (object obj in values)
            {
                if (obj.GetType() != typeof(SAParameter))
                {
                    Exception e = new ArgumentException(SARes.GetString(15019, obj.GetType().ToString()), "values");

                    throw e;
                }
            }
            _parms.AddRange(values.OfType<SAParameter>());

        }

        public void AddRange(SAParameter[] values)
        {
            AddRange((Array)values);
        }

        /// <summary>
        ///     <para>Removes all items from the collection.</para>
        /// </summary>
        public override void Clear()
        {

            _parms.Clear();

        }

        public override bool Contains(object value)
        {
            return _parms.Contains(value);
        }

        public override bool Contains(string value)
        {

            return FindIndex(value) >= 0;

        }

        public override void CopyTo(Array array, int index)
        {
            var arr = _parms.ToArray() as Array;
            arr.CopyTo(arr, index);
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

            if (value == null)
                return -1;
            CheckArgumentType(value);
            return _parms.IndexOf((SAParameter)value);

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

            return FindIndex(parameterName);

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

            CheckArgumentNull("value", value);
            CheckArgumentType(value);
            _parms.Insert(index, (SAParameter)value);

        }

        /// <summary>
        ///     <para>Removes the specified SAParameter object from the collection.</para>
        /// </summary>
        /// <param name="value">
        ///     The SAParameter object to remove from the collection.
        /// </param>
        public override void Remove(object value)
        {

            CheckArgumentNull("value", value);
            CheckArgumentType(value);
            if (!Contains(value))
            {
                Exception e = new ArgumentException(SARes.GetString(11015), "value");

                throw e;
            }
            _parms.Remove((SAParameter)value);

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

            CheckIndex(index);
            _parms.RemoveAt(index);

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

            CheckIndex(parameterName);
            _parms.RemoveAt(FindIndex(parameterName));

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

            return _parms.GetEnumerator();

        }

        internal int GetRebindParameterCount()
        {
            int num = 0;
            for (int index = 0; index < _parms.Count; ++index)
            {
                if (_parms[index].Rebind)
                    ++num;
            }
            return num;
        }

        internal int GetInputParameterCount()
        {
            int num = 0;
            foreach (var parm in _parms)
            {
                if (parm.IsInputParameter())
                    ++num;
            }
            return num;
        }

        internal int GetOutputParameterCount()
        {
            int num = 0;
            foreach (var parm in _parms)
            {
                if (parm.IsOutputParameter())
                    ++num;
            }
            return num;
        }

        internal void GetParameterInfo(out int count, ref SAParameterDM[] ppParmsDM, bool addValues, bool namedParms, string[] parmNames)
        {
            count = GetRebindParameterCount();

            if (count < 1)
                return;

            if (addValues)
                MapNullParameters();
            ppParmsDM = (SAParameterDM[])Array.CreateInstance(typeof(SAParameterDM), count);

            var pParmDM = ppParmsDM[0];
            if (namedParms)
            {
                for (int index1 = 0; index1 < parmNames.GetLength(0); ++index1)
                {
                    for (int index2 = 0; index2 < _parms.Count; ++index2)
                    {
                        SAParameter parm = _parms[index2];
                        if (string.Compare(parmNames[index1], parm.ParameterName, true) == 0)
                        {
                            SetParameterInfo(parm, pParmDM, addValues);
                            break;
                        }
                    }
                    pParmDM.Ordinal = index1;
                    pParmDM = ppParmsDM[index1 + 1];
                }
            }
            else
            {
                for (int index = 0; index < _parms.Count; ++index)
                {
                    pParmDM.Ordinal = index;
                    SetParameterInfo(_parms[index] as SAParameter, pParmDM, addValues);
                    pParmDM = ppParmsDM[index + 1];
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

                    pParmDM.Scale = parm.Scale;
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

        internal void FreeParameterInfo(int count, SAParameterDM[] pParmsDMs)
        {
            if (count <= 0)
                return;

            //foreach (var param in pParmsDMs)
            //{
            //    SAUnmanagedMemory.Free(param.Value);
            //    SAUnmanagedMemory.Free(param.Name);
            //}

            //SAUnmanagedMemory.Free((IntPtr)pParmsDMs);
        }

        internal void GetInputParameterValues(out int count, ref SAValue[] ppValues, string[] allParmNames, string[] inParmNames, bool _namedParms)
        {
            count = GetInputParameterCount();
            if (count < 1)
                return;
            MapNullParameters();
            ppValues = (SAValue[])Array.CreateInstance(typeof(SAValue), count);
            SAValue pVal = ppValues[0];
            if (_namedParms)
            {
                for (int index1 = 0; index1 < inParmNames.GetLength(0); ++index1)
                {
                    for (int index2 = 0; index2 < _parms.Count; ++index2)
                    {
                        var parm = _parms[index2];
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
                            //++pVal;
                            break;
                        }
                    }
                }
            }
            else
            {
                for (int index = 0; index < _parms.Count; ++index)
                {
                    SAParameter parm = _parms[index];
                    if (parm.IsInputParameter())
                    {
                        pVal.Ordinal = index;
                        SetParameterValue(parm, pVal);
                        //++pVal;
                    }
                }
            }
        }

        void SetParameterValue(SAParameter parm, SAValue pVal)
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

        void MapNullParameters()
        {
            foreach (SAParameter parm in _parms)
            {
                if (parm.SourceColumnNullMapping && parm.IsNullable && (parm.SourceColumn != null) && (parm.SourceColumn.Trim().Length > 0 && (parm.Direction == ParameterDirection.Input || parm.Direction == ParameterDirection.InputOutput)))
                {
                    if (parm.Value == null || DBNull.Value.Equals(parm.Value))
                        parm.Value = 1;
                    else
                        parm.Value = 0;
                }
            }
        }

        internal void FreeParameterValues(int count, SAValue[] pValues)
        {
            if (count <= 0)
                return;

            //foreach (var pValue in pValues)
            //    SAUnmanagedMemory.Free(pValue.Value);

            //SAValue* saValuePtr = pValues;
            //for (int index = 0; index < count; ++index)
            //{
            //    SAUnmanagedMemory.Free(saValuePtr.Value.Value);
            //    ++saValuePtr;
            //}
            //SAUnmanagedMemory.Free((IntPtr)((void*)pValues));
        }

        int FindIndex(string parameterName)
        {
            for (int index = 0; index < _parms.Count; ++index)
            {
                if (_parms[index].ParameterName == parameterName)
                    return index;
            }
            return -1;
        }
    }
}
