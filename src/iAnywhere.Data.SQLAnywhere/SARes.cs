
// Type: iAnywhere.Data.SQLAnywhere.SARes
// Assembly: iAnywhere.Data.SQLAnywhere.v4.0, Version=11.0.1.27424, Culture=neutral, PublicKeyToken=f222fc4333e0d400
// MVID: CC4F9F8C-E618-49D1-9147-C06A9EF53D1F
// Assembly location: C:\Program Files\SQL Anywhere 11\Assembly\V4\iAnywhere.Data.SQLAnywhere.v4.0.dll

namespace iAnywhere.Data.SQLAnywhere
{
  /// <summary>Summary description for SARes.</summary>
  internal class SARes
  {
    private static SAUnmanagedDll s_unmanagedDll = SAUnmanagedDll.Instance;
    private const int MESSAGE_LEN = 512;
    public const int IDS_NETIO_TRYING_TO_ADD_UNKNOWN_PORT = 7760;
    public const int IDS_PARSE_ERR_NO_MEM = 7941;
    public const int IDS_AMP_NOT_SUPPORTED = 10976;
    public const int IDS_AMP_INVALID_CONN_STR = 10977;
    public const int IDS_AMP_INVALID_CONN_STR_VAL = 10978;
    public const int IDS_AMP_INVALID_CONN_STR_KEY_VAL = 10979;
    public const int IDS_AMP_INVALID_CONN_STR_POOL_SIZE = 10980;
    public const int IDS_AMP_NOT_ALLOWED_TO_CHANGE_CONN_PROP = 10981;
    public const int IDS_AMP_INVALID_OPER_ON_CLOSED_CONN = 10982;
    public const int IDS_AMP_INVALID_OPER_CHANGEDATABASE = 10983;
    public const int IDS_AMP_EMPTY_DATABASE_NAME = 10984;
    public const int IDS_AMP_CONN_IS_ALREADY_OPEN = 10985;
    public const int IDS_AMP_PROP_NOT_INITIALIZED1 = 10986;
    public const int IDS_AMP_PROP_NOT_INITIALIZED2 = 10987;
    public const int IDS_AMP_CONN_TIMEOUT = 10988;
    public const int IDS_AMP_UNKNOWN_ASA_DB_TYPE = 10989;
    public const int IDS_AMP_UNKNOWN_DB_TYPE = 10990;
    public const int IDS_AMP_UNKNOWN_DATA_TYPE = 10991;
    public const int IDS_AMP_TRAN_NOT_MATCH_CONN = 10992;
    public const int IDS_AMP_INVALID_OPER_ON_EXECUTE_CMD = 10993;
    public const int IDS_AMP_CMD_IS_ALREADY_EXECUTING = 10994;
    public const int IDS_AMP_INVALID_CMD_TIMEOUT = 10995;
    public const int IDS_AMP_CMD_TABLEDIRECT_NOT_SUPPORTED = 10996;
    public const int IDS_AMP_INVALID_TRAN_IL2 = 17016;
    public const int IDS_AMP_PARALLEL_TRAN_NOT_SUPPORTED = 10998;
    public const int IDS_AMP_TRAN_COMPLETED = 10999;
    public const int IDS_AMP_INVALID_SAVEPOINT = 11000;
    public const int IDS_AMP_CMD_TRAN_NOT_INIT = 11001;
    public const int IDS_AMP_INVALID_CAST = 11002;
    public const int IDS_AMP_INVALID_READ_NO_DATA = 11003;
    public const int IDS_AMP_INVALID_READ_ON_CLOSED_DR = 11004;
    public const int IDS_AMP_INVALID_OPER_ON_CLOSED_DR = 11005;
    public const int IDS_AMP_INVALID_CAST_GETBYTES = 11006;
    public const int IDS_AMP_INVALID_PARM_DATA_TYPE = 11007;
    public const int IDS_AMP_NO_MAPPING_FROM_DBTYPE_TO_ASADBTYPE = 11008;
    public const int IDS_AMP_INVALID_PARM_SIZE = 11009;
    public const int IDS_AMP_INVALID_PARM_PRECISION = 11010;
    public const int IDS_AMP_INVALID_PARM_INDEX_INT = 11011;
    public const int IDS_AMP_INVALID_PARM_INDEX_STRING = 11012;
    public const int IDS_AMP_NULL_PARM = 11013;
    public const int IDS_AMP_INVALID_PARM_TYPE = 11014;
    public const int IDS_AMP_REMOVE_NOT_CONTAINED_PARM = 11015;
    public const int IDS_AMP_PARM_ALREADY_CONTAINED = 11016;
    public const int IDS_AMP_DBNULL = 11017;
    public const int IDS_AMP_NULL_SRC_TABLE_NAME = 11018;
    public const int IDS_AMP_INVALID_START_RECORD = 11019;
    public const int IDS_AMP_INVALID_MAX_RECORDS = 11020;
    public const int IDS_AMP_MISSING_DATATABLE = 11021;
    public const int IDS_AMP_MISSING_DATACOLUMN = 11022;
    public const int IDS_AMP_MISSING_TABLEMAPPING_SOURCE = 11023;
    public const int IDS_AMP_MISSING_TABLEMAPPING_DATASET = 11024;
    public const int IDS_AMP_MISSING_COLUMNMAPPING_SOURCE = 11025;
    public const int IDS_AMP_UPDATE_TABLE_MAPPING_NOT_FOUND = 11026;
    public const int IDS_AMP_INVALID_INSERT_COMMAND = 11027;
    public const int IDS_AMP_INVALID_UPDATE_COMMAND = 11028;
    public const int IDS_AMP_INVALID_DELETE_COMMAND = 11029;
    public const int IDS_AMP_ERRORS_OCCURRED = 11030;
    public const int IDS_AMP_DYNAMIC_SQL_NO_KEY_COL = 11031;
    public const int IDS_AMP_DYNAMIC_SQL_NO_BASE_TABLE = 11032;
    public const int IDS_AMP_DYNAMIC_SQL_MULTI_BASE_TABLE = 11033;
    public const int IDS_AMP_DYNAMIC_SQL_DUP_COLUMN = 11034;
    public const int IDS_AMP_DYNAMIC_SQL_NO_MODIFIABLE_COLUMN = 11035;
    public const int IDS_AMP_DERIVE_PARM_CMD_TYPE_NOT_SUPPORTED = 11036;
    public const int IDS_AMP_DERIVE_PARM_PROP_NOT_INIT = 11037;
    public const int IDS_AMP_DERIVE_PARM_CONN_NOT_OPEN = 11038;
    public const int IDS_AMP_STORED_PROC_NOT_EXIST = 11039;
    public const int IDS_AMP_DB_CONCURRENCY_EX = 13777;
    public const int IDS_AMP_CONN_STR_BLDR_KEY = 14951;
    public const int IDS_AMP_CONN_STR_BLDR_MISMATCHED_PAREN = 14952;
    public const int IDS_AMP_CONN_STR_FORMAT = 14956;
    public const int IDS_AMP_INVALID_COLLECTION = 14958;
    public const int IDS_AMP_INVALID_RESTRICTIONS = 14959;
    public const int IDS_AMP_BATCH_UPDATE_ROW_SOURCE = 14970;
    public const int IDS_AMP_BATCH_NOT_INSERT = 14971;
    public const int IDS_AMP_ASYNC_COMMAND_EXECUTING = 14973;
    public const int IDS_AMP_ASYNC_COMMAND_NOT_EXECUTING = 14974;
    public const int IDS_AMP_ASYNCRESULT_NOT_MATCH = 14975;
    public const int IDS_AMP_MISMATCHED_ASYNC_COMMAND = 14976;
    public const int IDS_AMP_VARYING_BULKCOPYMAPPING = 15007;
    public const int IDS_AMP_NEGATIVE_VAL = 15008;
    public const int IDS_AMP_BULKCOPY_INVALID_COLUMN = 15009;
    public const int IDS_AMP_BULKCOPY_INVALID_SOURCE_COLUMN = 15010;
    public const int IDS_AMP_EXPECTED_NONEMPTY_STRING = 15011;
    public const int IDS_AMP_OPER_ON_CLOSED_BC = 15012;
    public const int IDS_AMP_BULKCOPY_CLOSE_IN_CALLBACK = 15013;
    public const int IDS_AMP_BULKCOPY_MODIFY_MAPPING_COLLECTION = 15014;
    public const int IDS_AMP_BULKCOPY_TIMEOUT = 15015;
    public const int IDS_AMP_BULKCOPY_SCHEMA_NOT_MATCH = 15016;
    public const int IDS_AMP_BULKCOPY_INTERNAL_TRANSACTION = 15017;
    public const int IDS_AMP_BULKCOPY_COLUMN_TYPES_NOT_MATCH = 15018;
    public const int IDS_AMP_ADD_RANGE_TYPE = 15019;
    public const int IDS_AMP_OPERATION_ABORTED = 17032;
    public const int IDS_AMP_KEYWORD_NOT_SUPPORTED = 17314;
    public const int IDS_AMP_INVALID_OUT_PARM_SIZE = 17421;
    public const int IDS_AMP_CONN_STR_BLDR_MISSING_COMMA = 17439;
    public const int IDS_AMP_INVALID_QUOTE = 17445;
    public const int IDS_AMP_CONN_CANNOT_BE_CLOSED = 17447;
    public const int IDS_AMP_CONN_CANNOT_BE_USED = 17448;
    public const int IDS_AMP_CANNOT_ENLIST_LOCAL_TRAN_IN_PROGRESS = 17882;
    public const int IDS_AMP_CANNOT_ENLIST_ALREADY_ENLISTED = 17883;
    public const int IDS_AMP_CANNOT_BEGIN_TRAN_ALREADY_ENLISTED = 17886;
    public const int IDS_AMP_DR_MUST_BE_CLOSED = 17931;
    public const int IDS_AMP_ASYNC_CMD_ALREADY_RUNNING = 18531;
    public const int IDS_AMP_NULL_OR_EMPTY_STRING = 25994;

    public static string GetString(int stringID, string parm)
    {
      return string.Format(SARes.GetString(stringID), parm);
    }

    public static string GetString(int stringID, string parm1, string parm2)
    {
      return string.Format(SARes.GetString(stringID), parm1, parm2);
    }

    public static string GetString(int stringID, string parm1, string parm2, string parm3)
    {
      return string.Format(SARes.GetString(stringID), parm1, parm2, parm3);
    }

    public static unsafe string GetString(int stringID)
    {
      int indicator = 0;
      int bufferLength = 512;
      char[] chArray1 = new char[bufferLength];
      int string1;
      fixed (char* buffer = chArray1)
        string1 = PInvokeMethods.Asa_GetString(stringID, buffer, bufferLength, ref indicator);
      SAException.CheckException(string1);
      string str;
      if (indicator <= bufferLength)
      {
        str = new string(chArray1, 0, indicator);
      }
      else
      {
        char[] chArray2 = new char[indicator];
        int string2;
        fixed (char* buffer = chArray2)
          string2 = PInvokeMethods.Asa_GetString(stringID, buffer, indicator, ref indicator);
        SAException.CheckException(string2);
        str = new string(chArray2, 0, indicator);
      }
      return str;
    }
  }
}
