
// Type: iAnywhere.Data.SQLAnywhere.PInvokeMethods
// Assembly: iAnywhere.Data.SQLAnywhere.v4.0, Version=11.0.1.27424, Culture=neutral, PublicKeyToken=f222fc4333e0d400
using System;
using System.Runtime.InteropServices;

namespace iAnywhere.Data.SQLAnywhere
{
    /// <summary>Summary description for PInvodeMethods.</summary>
    internal class PInvokeMethods
    {
        private const string UnmanagedDll = "dbdata11.dll";
        private const string c_kernelDll = "Kernel32.dll";

        [DllImport("Kernel32.dll")]
        public static extern IntPtr LocalAlloc(uint uFlags, UIntPtr uBytes);

        [DllImport("Kernel32.dll")]
        public static extern IntPtr LocalFree(IntPtr hMem);

        [DllImport("Kernel32.dll", CharSet = CharSet.Unicode)]
        public static extern IntPtr LoadLibrary(string fileName);

        [DllImport("Kernel32.dll")]
        public static extern bool FreeLibrary(IntPtr hModule);

        [DllImport("dbdata11.dll")]
        public static extern int Unmanaged_Init();

        [DllImport("dbdata11.dll")]
        public static extern int Unmanaged_Fini();

        [DllImport("dbdata11.dll")]
        public static extern unsafe int Unmanaged_FreeMemory(void* buffer);

        [DllImport("dbdata11.dll")]
        public static extern unsafe int Asa_GetString(int errorCode, char* buffer, int bufferLength, ref int indicator);

        [DllImport("dbdata11.dll")]
        internal static extern int AsaCommand_Fini(int idCmd);

        [DllImport("dbdata11.dll")]
        public static extern int AsaCommand_Cancel(int idCmd);

        [DllImport("dbdata11.dll", CharSet = CharSet.Unicode)]
        public static extern int AsaCommand_Prepare(ref int idCmd, int idConn, string cmdText, int parmCount, IntPtr parms, ref bool namedParms, IntPtr allParmNames, IntPtr inParmNames, IntPtr outParmNames);

        [DllImport("dbdata11.dll")]
        public static extern int AsaCommand_ExecuteNonQuery(int idCmd, int inputParmCount, IntPtr inputParmValues, ref int outputParmCount, ref IntPtr outputParmValues, ref int rowCount);

        [DllImport("dbdata11.dll")]
        public static extern int AsaCommand_ExecuteReader(int idCmd, int inputParmCount, IntPtr inputParmValues, ref int outputParmCount, ref IntPtr outputParmValues, ref int idReader, ref int rowCount);

        [DllImport("dbdata11.dll", CharSet = CharSet.Unicode)]
        public static extern int AsaCommand_BeginExecuteNonQueryDirect(ref int idCmd, int idConn, string cmdText, int parmCount, IntPtr parms, IntPtr callback);

        [DllImport("dbdata11.dll")]
        public static extern int AsaCommand_EndExecuteNonQuery(int idConn, ref int outputParmCount, ref IntPtr outputParmValues, ref int rowCount);

        [DllImport("dbdata11.dll", CharSet = CharSet.Unicode)]
        public static extern int AsaCommand_BeginExecuteReaderDirect(ref int idCmd, int idConn, string cmdText, int parmCount, IntPtr parms, IntPtr callback);

        [DllImport("dbdata11.dll")]
        public static extern int AsaCommand_EndExecuteReader(int idConn, ref int outputParmValueCount, ref IntPtr outputParmValues, ref int idReader, ref int rowCount);

        [DllImport("dbdata11.dll")]
        public static extern int AsaCommand_FreeOutputParameterValues(int count, IntPtr pValues);

        [DllImport("dbdata11.dll", CharSet = CharSet.Unicode)]
        public static extern int AsaConnection_Open(ref int idConn, string connStr);

        [DllImport("dbdata11.dll")]
        public static extern int AsaConnection_Close(int idConn);

        [DllImport("dbdata11.dll")]
        public static extern int AsaConnection_IsAlive(int idConn, ref bool isAlive);

        [DllImport("dbdata11.dll")]
        public static extern int AsaConnection_BeginTransaction(int idConn, int isolationLevel, ref int idTrans);

        [DllImport("dbdata11.dll")]
        public static extern int AsaConnection_CloseDataReaders(int idConn);

        [DllImport("dbdata11.dll")]
        public static extern int AsaConnection_SetMessageCallback(int idConn, SAInfoMessageDelegate msgDelegate);

        [DllImport("dbdata11.dll")]
        public static extern int AsaConnection_DtcEnlist(int idConn, bool enlist);

        [DllImport("dbdata11.dll")]
        public static extern unsafe int AsaConnection_SendTransactionCookie(int idConn, byte* cookie, int cookieSize);

        [DllImport("dbdata11.dll")]
        public static extern unsafe int AsaConnection_GetWhereabouts(int idConn, byte** whereabouts, uint* size);

        [DllImport("dbdata11.dll")]
        public static extern int AsaConnectionStringParser_Init(ref int idParser);

        [DllImport("dbdata11.dll")]
        public static extern int AsaConnectionStringParser_Fini(int idParser);

        [DllImport("dbdata11.dll", CharSet = CharSet.Unicode)]
        public static extern int AsaConnectionStringParser_ParseConnectionString(int idParser, string connStr, ref int indicator);

        [DllImport("dbdata11.dll")]
        public static extern int AsaConnectionStringParser_GetParameterCount(int idParser, ref int count);

        [DllImport("dbdata11.dll")]
        public static extern unsafe int AsaConnectionStringParser_GetParameter(int idParser, int index, char* keyBuffer, int keyBufferLength, ref int keyLength, char* valueBuffer, int valueBufferLength, ref int valueLength);

        [DllImport("dbdata11.dll")]
        public static extern int AsaException_Fini(int idEx);

        [DllImport("dbdata11.dll")]
        public static extern int AsaException_GetErrorCount(int idEx, ref int count);

        [DllImport("dbdata11.dll")]
        public static extern unsafe int AsaException_GetErrorInfo(int idEx, int errorIndex, ref int nativeError, char* bufSqlState, int bufLenSqlState, ref int lenSqlState, char* bufMsg, int bufLenMsg, ref int lenMsg);

        [DllImport("dbdata11.dll")]
        public static extern int AsaDataReader_Close(int idReader);

        [DllImport("dbdata11.dll")]
        public static extern int AsaDataReader_IsDBNull(int idReader, int ordinal, ref bool isDBNull);

        [DllImport("dbdata11.dll")]
        public static extern int AsaDataReader_NextResult(int idReader, ref bool nextResult);

        [DllImport("dbdata11.dll")]
        public static extern int AsaDataReader_Read(int idReader, ref bool result);

        [DllImport("dbdata11.dll")]
        public static extern int AsaDataReader_FetchRows(int idReader, ref int rowsObtained, ref IntPtr values);

        [DllImport("dbdata11.dll")]
        public static extern int AsaDataReader_GetColumnNames(int idReader, ref int count, ref IntPtr columnNames);

        [DllImport("dbdata11.dll")]
        public static extern int AsaDataReader_FreeColumnNames(int idReader, int count, IntPtr columnNames);

        [DllImport("dbdata11.dll")]
        public static extern int AsaDataReader_GetValues(int idReader, ref int count, ref IntPtr values);

        [DllImport("dbdata11.dll")]
        public static extern int AsaDataReader_FreeValues(int idReader, int count, IntPtr values);

        [DllImport("dbdata11.dll")]
        public static extern int AsaDataReader_GetValue(int idReader, int ordinal, ref IntPtr val);

        [DllImport("dbdata11.dll")]
        public static extern int AsaDataReader_FreeValue(int idReader, int ordinal, IntPtr val);

        [DllImport("dbdata11.dll")]
        public static extern int AsaDataReader_GetValueL(int idReader, int ordinal, long index, int length, ref IntPtr val);

        [DllImport("dbdata11.dll")]
        public static extern int AsaDataReader_GetSchema(int idReader, ref int count, ref IntPtr columnInfos);

        [DllImport("dbdata11.dll")]
        public static extern int AsaDataReader_FreeSchema(int idReader, int count, IntPtr columnInfos);

        [DllImport("dbdata11.dll")]
        public static extern int AsaDataReader_HasRows(int idReader, ref bool hasRows);

        [DllImport("dbdata11.dll")]
        public static extern unsafe int AsaDataReader_ReadBytes(int idReader, int ordinal, long dataIndex, byte* buffer, int bufferIndex, int length, ref int bytesRead, ref long actualSize);

        [DllImport("dbdata11.dll")]
        public static extern unsafe int AsaDataReader_ReadChars(int idReader, int ordinal, long dataIndex, char* buffer, int bufferIndex, int length, ref int charsRead, ref long actualSize);

        [DllImport("dbdata11.dll")]
        public static extern unsafe int AsaDataReader_ReadBytesCE(int idReader, int ordinal, int dataIndex, byte* buffer, int bufferIndex, int length, ref int bytesRead, ref int actualSize);

        [DllImport("dbdata11.dll")]
        public static extern unsafe int AsaDataReader_ReadCharsCE(int idReader, int ordinal, int dataIndex, char* buffer, int bufferIndex, int length, ref int charsRead, ref int actualSize);

        [DllImport("dbdata11.dll", CharSet = CharSet.Unicode)]
        public static extern int AsaTransaction_Save(int idTran, string name);

        [DllImport("dbdata11.dll")]
        public static extern int AsaTransaction_Commit(int idTran);

        [DllImport("dbdata11.dll")]
        public static extern int AsaTransaction_Rollback(int idTran);

        [DllImport("dbdata11.dll", CharSet = CharSet.Unicode)]
        public static extern int AsaTransaction_RollbackToName(int idTran, string name);

        [DllImport("dbdata11.dll", CharSet = CharSet.Unicode)]
        public static extern int SAConnectionStringBuilder_ParseLinksOptions(string connStr, ref int numResults, ref IntPtr result);

        [DllImport("dbdata11.dll")]
        public static extern int SAConnectionStringBuilder_FreeLinksOptions(int numResults, IntPtr result);

        [DllImport("dbdata11.dll")]
        public static extern int SADataSourceEnumerator_GetDataSources(ref int numResults, ref IntPtr result);

        [DllImport("dbdata11.dll")]
        public static extern int SADataSourceEnumerator_FreeResults(int numResults, IntPtr result);

        [DllImport("dbdata11.dll", CharSet = CharSet.Unicode)]
        public static extern int SATrace_FireEvent(string msg);
    }
}
