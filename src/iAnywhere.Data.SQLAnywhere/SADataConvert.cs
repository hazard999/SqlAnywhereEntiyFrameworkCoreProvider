
// Type: iAnywhere.Data.SQLAnywhere.SADataConvert
// Assembly: iAnywhere.Data.SQLAnywhere.v4.0, Version=11.0.1.27424, Culture=neutral, PublicKeyToken=f222fc4333e0d400
// MVID: CC4F9F8C-E618-49D1-9147-C06A9EF53D1F
// Assembly location: C:\Program Files\SQL Anywhere 11\Assembly\V4\iAnywhere.Data.SQLAnywhere.v4.0.dll

using System;
using System.Data;
using System.Runtime.InteropServices;

namespace iAnywhere.Data.SQLAnywhere
{
    /// <summary>Summary description for SADataConvert.</summary>
    internal class SADataConvert
    {
        private const int MAX_MAPS = 50;
        public const int OFFSET_UNSPECIFIED = 0;
        public const int SIZE_UNSPECIFIED = -1;

        static SADataConvert()
        {
            SADataConvert.GetMapDbTypeToSADbType();
            SADataConvert.GetMapSADbTypeToDbType();
            SADataConvert.GetMapDotNetTypeToSAType();
            SADataConvert.GetMapSADbTypeToDotNetType();
        }

        private static DateTime CaculateDateTime(DateTime dt, uint microsecond)
        {
            return new DateTime(dt.Ticks + microsecond * 10U);
        }

        private static uint CaculateMicrosecond(DateTime dt)
        {
            DateTime dateTime = new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second);
            return (uint)((ulong)(dt.Ticks - dateTime.Ticks) / 10UL);
        }

        private static uint CaculateMicrosecond(TimeSpan ts)
        {
            TimeSpan timeSpan = new TimeSpan(ts.Days, ts.Hours, ts.Minutes, ts.Seconds);
            return (uint)((ulong)(ts.Ticks - timeSpan.Ticks) / 10UL);
        }

        private static TimeSpan CaculateTimeSpan(TimeSpan ts, uint microsecond)
        {
            return new TimeSpan(ts.Ticks + microsecond * 10U);
        }

        private static void GetMapDbTypeToSADbType()
        {
            if (SADataConvert.s_mapDbTypeToSADbType != null)
                return;
            int num1 = 0;
            SADataConvert.s_mapDbTypeToSADbType = new SADataConvert.MapDbTypeToSADbType[50];
            SADataConvert.MapDbTypeToSADbType[] dbTypeToSaDbTypeArray1 = SADataConvert.s_mapDbTypeToSADbType;
            int index1 = num1;
            int num2 = 1;
            int num3 = index1 + num2;
            dbTypeToSaDbTypeArray1[index1] = new SADataConvert.MapDbTypeToSADbType(DbType.AnsiString, SADbType.VarChar);
            SADataConvert.MapDbTypeToSADbType[] dbTypeToSaDbTypeArray2 = SADataConvert.s_mapDbTypeToSADbType;
            int index2 = num3;
            int num4 = 1;
            int num5 = index2 + num4;
            dbTypeToSaDbTypeArray2[index2] = new SADataConvert.MapDbTypeToSADbType(DbType.AnsiStringFixedLength, SADbType.Char);
            SADataConvert.MapDbTypeToSADbType[] dbTypeToSaDbTypeArray3 = SADataConvert.s_mapDbTypeToSADbType;
            int index3 = num5;
            int num6 = 1;
            int num7 = index3 + num6;
            dbTypeToSaDbTypeArray3[index3] = new SADataConvert.MapDbTypeToSADbType(DbType.String, SADbType.NVarChar);
            SADataConvert.MapDbTypeToSADbType[] dbTypeToSaDbTypeArray4 = SADataConvert.s_mapDbTypeToSADbType;
            int index4 = num7;
            int num8 = 1;
            int num9 = index4 + num8;
            dbTypeToSaDbTypeArray4[index4] = new SADataConvert.MapDbTypeToSADbType(DbType.StringFixedLength, SADbType.NChar);
            SADataConvert.MapDbTypeToSADbType[] dbTypeToSaDbTypeArray5 = SADataConvert.s_mapDbTypeToSADbType;
            int index5 = num9;
            int num10 = 1;
            int num11 = index5 + num10;
            dbTypeToSaDbTypeArray5[index5] = new SADataConvert.MapDbTypeToSADbType(DbType.Binary, SADbType.VarBinary);
            SADataConvert.MapDbTypeToSADbType[] dbTypeToSaDbTypeArray6 = SADataConvert.s_mapDbTypeToSADbType;
            int index6 = num11;
            int num12 = 1;
            int num13 = index6 + num12;
            dbTypeToSaDbTypeArray6[index6] = new SADataConvert.MapDbTypeToSADbType(DbType.Boolean, SADbType.Bit);
            SADataConvert.MapDbTypeToSADbType[] dbTypeToSaDbTypeArray7 = SADataConvert.s_mapDbTypeToSADbType;
            int index7 = num13;
            int num14 = 1;
            int num15 = index7 + num14;
            dbTypeToSaDbTypeArray7[index7] = new SADataConvert.MapDbTypeToSADbType(DbType.Byte, SADbType.TinyInt);
            SADataConvert.MapDbTypeToSADbType[] dbTypeToSaDbTypeArray8 = SADataConvert.s_mapDbTypeToSADbType;
            int index8 = num15;
            int num16 = 1;
            int num17 = index8 + num16;
            dbTypeToSaDbTypeArray8[index8] = new SADataConvert.MapDbTypeToSADbType(DbType.Currency, SADbType.Money);
            SADataConvert.MapDbTypeToSADbType[] dbTypeToSaDbTypeArray9 = SADataConvert.s_mapDbTypeToSADbType;
            int index9 = num17;
            int num18 = 1;
            int num19 = index9 + num18;
            dbTypeToSaDbTypeArray9[index9] = new SADataConvert.MapDbTypeToSADbType(DbType.Date, SADbType.Date);
            SADataConvert.MapDbTypeToSADbType[] dbTypeToSaDbTypeArray10 = SADataConvert.s_mapDbTypeToSADbType;
            int index10 = num19;
            int num20 = 1;
            int num21 = index10 + num20;
            dbTypeToSaDbTypeArray10[index10] = new SADataConvert.MapDbTypeToSADbType(DbType.DateTime, SADbType.DateTime);
            SADataConvert.MapDbTypeToSADbType[] dbTypeToSaDbTypeArray11 = SADataConvert.s_mapDbTypeToSADbType;
            int index11 = num21;
            int num22 = 1;
            int num23 = index11 + num22;
            dbTypeToSaDbTypeArray11[index11] = new SADataConvert.MapDbTypeToSADbType(DbType.Decimal, SADbType.Decimal);
            SADataConvert.MapDbTypeToSADbType[] dbTypeToSaDbTypeArray12 = SADataConvert.s_mapDbTypeToSADbType;
            int index12 = num23;
            int num24 = 1;
            int num25 = index12 + num24;
            dbTypeToSaDbTypeArray12[index12] = new SADataConvert.MapDbTypeToSADbType(DbType.Double, SADbType.Double);
            SADataConvert.MapDbTypeToSADbType[] dbTypeToSaDbTypeArray13 = SADataConvert.s_mapDbTypeToSADbType;
            int index13 = num25;
            int num26 = 1;
            int num27 = index13 + num26;
            dbTypeToSaDbTypeArray13[index13] = new SADataConvert.MapDbTypeToSADbType(DbType.Guid, SADbType.UniqueIdentifier);
            SADataConvert.MapDbTypeToSADbType[] dbTypeToSaDbTypeArray14 = SADataConvert.s_mapDbTypeToSADbType;
            int index14 = num27;
            int num28 = 1;
            int num29 = index14 + num28;
            dbTypeToSaDbTypeArray14[index14] = new SADataConvert.MapDbTypeToSADbType(DbType.Int16, SADbType.SmallInt);
            SADataConvert.MapDbTypeToSADbType[] dbTypeToSaDbTypeArray15 = SADataConvert.s_mapDbTypeToSADbType;
            int index15 = num29;
            int num30 = 1;
            int num31 = index15 + num30;
            dbTypeToSaDbTypeArray15[index15] = new SADataConvert.MapDbTypeToSADbType(DbType.Int32, SADbType.Integer);
            SADataConvert.MapDbTypeToSADbType[] dbTypeToSaDbTypeArray16 = SADataConvert.s_mapDbTypeToSADbType;
            int index16 = num31;
            int num32 = 1;
            int num33 = index16 + num32;
            dbTypeToSaDbTypeArray16[index16] = new SADataConvert.MapDbTypeToSADbType(DbType.Int64, SADbType.BigInt);
            SADataConvert.MapDbTypeToSADbType[] dbTypeToSaDbTypeArray17 = SADataConvert.s_mapDbTypeToSADbType;
            int index17 = num33;
            int num34 = 1;
            int num35 = index17 + num34;
            dbTypeToSaDbTypeArray17[index17] = new SADataConvert.MapDbTypeToSADbType(DbType.Single, SADbType.Float);
            SADataConvert.MapDbTypeToSADbType[] dbTypeToSaDbTypeArray18 = SADataConvert.s_mapDbTypeToSADbType;
            int index18 = num35;
            int num36 = 1;
            int num37 = index18 + num36;
            dbTypeToSaDbTypeArray18[index18] = new SADataConvert.MapDbTypeToSADbType(DbType.Time, SADbType.Time);
            SADataConvert.MapDbTypeToSADbType[] dbTypeToSaDbTypeArray19 = SADataConvert.s_mapDbTypeToSADbType;
            int index19 = num37;
            int num38 = 1;
            int num39 = index19 + num38;
            dbTypeToSaDbTypeArray19[index19] = new SADataConvert.MapDbTypeToSADbType(DbType.UInt16, SADbType.UnsignedSmallInt);
            SADataConvert.MapDbTypeToSADbType[] dbTypeToSaDbTypeArray20 = SADataConvert.s_mapDbTypeToSADbType;
            int index20 = num39;
            int num40 = 1;
            int num41 = index20 + num40;
            dbTypeToSaDbTypeArray20[index20] = new SADataConvert.MapDbTypeToSADbType(DbType.UInt32, SADbType.UnsignedInt);
            SADataConvert.MapDbTypeToSADbType[] dbTypeToSaDbTypeArray21 = SADataConvert.s_mapDbTypeToSADbType;
            int index21 = num41;
            int num42 = 1;
            int num43 = index21 + num42;
            dbTypeToSaDbTypeArray21[index21] = new SADataConvert.MapDbTypeToSADbType(DbType.UInt64, SADbType.UnsignedBigInt);
            SADataConvert.MapDbTypeToSADbType[] dbTypeToSaDbTypeArray22 = SADataConvert.s_mapDbTypeToSADbType;
            int index22 = num43;
            int num44 = 1;
            int num45 = index22 + num44;
            dbTypeToSaDbTypeArray22[index22] = new SADataConvert.MapDbTypeToSADbType(DbType.VarNumeric, SADbType.Decimal);
            SADataConvert.MapDbTypeToSADbType[] dbTypeToSaDbTypeArray23 = SADataConvert.s_mapDbTypeToSADbType;
            int index23 = num45;
            int num46 = 1;
            int num47 = index23 + num46;
            dbTypeToSaDbTypeArray23[index23] = new SADataConvert.MapDbTypeToSADbType(DbType.Xml, SADbType.LongVarchar);
        }

        private static void GetMapDotNetTypeToSAType()
        {
            if (SADataConvert.s_mapDotNetTypeToSAType != null)
                return;
            int num1 = 0;
            SADataConvert.s_mapDotNetTypeToSAType = new SADataConvert.MapDotNetTypeToSAType[50];
            SADataConvert.MapDotNetTypeToSAType[] dotNetTypeToSaTypeArray1 = SADataConvert.s_mapDotNetTypeToSAType;
            int index1 = num1;
            int num2 = 1;
            int num3 = index1 + num2;
            dotNetTypeToSaTypeArray1[index1] = new SADataConvert.MapDotNetTypeToSAType(nameof(Boolean), SADbType.Bit);
            SADataConvert.MapDotNetTypeToSAType[] dotNetTypeToSaTypeArray2 = SADataConvert.s_mapDotNetTypeToSAType;
            int index2 = num3;
            int num4 = 1;
            int num5 = index2 + num4;
            dotNetTypeToSaTypeArray2[index2] = new SADataConvert.MapDotNetTypeToSAType(nameof(Byte), SADbType.TinyInt);
            SADataConvert.MapDotNetTypeToSAType[] dotNetTypeToSaTypeArray3 = SADataConvert.s_mapDotNetTypeToSAType;
            int index3 = num5;
            int num6 = 1;
            int num7 = index3 + num6;
            dotNetTypeToSaTypeArray3[index3] = new SADataConvert.MapDotNetTypeToSAType(nameof(DateTime), SADbType.DateTime);
            SADataConvert.MapDotNetTypeToSAType[] dotNetTypeToSaTypeArray4 = SADataConvert.s_mapDotNetTypeToSAType;
            int index4 = num7;
            int num8 = 1;
            int num9 = index4 + num8;
            dotNetTypeToSaTypeArray4[index4] = new SADataConvert.MapDotNetTypeToSAType(nameof(Decimal), SADbType.Decimal);
            SADataConvert.MapDotNetTypeToSAType[] dotNetTypeToSaTypeArray5 = SADataConvert.s_mapDotNetTypeToSAType;
            int index5 = num9;
            int num10 = 1;
            int num11 = index5 + num10;
            dotNetTypeToSaTypeArray5[index5] = new SADataConvert.MapDotNetTypeToSAType(nameof(Double), SADbType.Double);
            SADataConvert.MapDotNetTypeToSAType[] dotNetTypeToSaTypeArray6 = SADataConvert.s_mapDotNetTypeToSAType;
            int index6 = num11;
            int num12 = 1;
            int num13 = index6 + num12;
            dotNetTypeToSaTypeArray6[index6] = new SADataConvert.MapDotNetTypeToSAType(nameof(Guid), SADbType.UniqueIdentifier);
            SADataConvert.MapDotNetTypeToSAType[] dotNetTypeToSaTypeArray7 = SADataConvert.s_mapDotNetTypeToSAType;
            int index7 = num13;
            int num14 = 1;
            int num15 = index7 + num14;
            dotNetTypeToSaTypeArray7[index7] = new SADataConvert.MapDotNetTypeToSAType(nameof(Int16), SADbType.SmallInt);
            SADataConvert.MapDotNetTypeToSAType[] dotNetTypeToSaTypeArray8 = SADataConvert.s_mapDotNetTypeToSAType;
            int index8 = num15;
            int num16 = 1;
            int num17 = index8 + num16;
            dotNetTypeToSaTypeArray8[index8] = new SADataConvert.MapDotNetTypeToSAType(nameof(Int32), SADbType.Integer);
            SADataConvert.MapDotNetTypeToSAType[] dotNetTypeToSaTypeArray9 = SADataConvert.s_mapDotNetTypeToSAType;
            int index9 = num17;
            int num18 = 1;
            int num19 = index9 + num18;
            dotNetTypeToSaTypeArray9[index9] = new SADataConvert.MapDotNetTypeToSAType(nameof(Int64), SADbType.BigInt);
            SADataConvert.MapDotNetTypeToSAType[] dotNetTypeToSaTypeArray10 = SADataConvert.s_mapDotNetTypeToSAType;
            int index10 = num19;
            int num20 = 1;
            int num21 = index10 + num20;
            dotNetTypeToSaTypeArray10[index10] = new SADataConvert.MapDotNetTypeToSAType(nameof(Single), SADbType.Float);
            SADataConvert.MapDotNetTypeToSAType[] dotNetTypeToSaTypeArray11 = SADataConvert.s_mapDotNetTypeToSAType;
            int index11 = num21;
            int num22 = 1;
            int num23 = index11 + num22;
            dotNetTypeToSaTypeArray11[index11] = new SADataConvert.MapDotNetTypeToSAType(nameof(String), SADbType.NVarChar);
            SADataConvert.MapDotNetTypeToSAType[] dotNetTypeToSaTypeArray12 = SADataConvert.s_mapDotNetTypeToSAType;
            int index12 = num23;
            int num24 = 1;
            int num25 = index12 + num24;
            dotNetTypeToSaTypeArray12[index12] = new SADataConvert.MapDotNetTypeToSAType(nameof(TimeSpan), SADbType.Time);
            SADataConvert.MapDotNetTypeToSAType[] dotNetTypeToSaTypeArray13 = SADataConvert.s_mapDotNetTypeToSAType;
            int index13 = num25;
            int num26 = 1;
            int num27 = index13 + num26;
            dotNetTypeToSaTypeArray13[index13] = new SADataConvert.MapDotNetTypeToSAType(nameof(UInt16), SADbType.UnsignedSmallInt);
            SADataConvert.MapDotNetTypeToSAType[] dotNetTypeToSaTypeArray14 = SADataConvert.s_mapDotNetTypeToSAType;
            int index14 = num27;
            int num28 = 1;
            int num29 = index14 + num28;
            dotNetTypeToSaTypeArray14[index14] = new SADataConvert.MapDotNetTypeToSAType(nameof(UInt32), SADbType.UnsignedInt);
            SADataConvert.MapDotNetTypeToSAType[] dotNetTypeToSaTypeArray15 = SADataConvert.s_mapDotNetTypeToSAType;
            int index15 = num29;
            int num30 = 1;
            int num31 = index15 + num30;
            dotNetTypeToSaTypeArray15[index15] = new SADataConvert.MapDotNetTypeToSAType(nameof(UInt64), SADbType.UnsignedBigInt);
            SADataConvert.MapDotNetTypeToSAType[] dotNetTypeToSaTypeArray16 = SADataConvert.s_mapDotNetTypeToSAType;
            int index16 = num31;
            int num32 = 1;
            int num33 = index16 + num32;
            dotNetTypeToSaTypeArray16[index16] = new SADataConvert.MapDotNetTypeToSAType("Byte[]", SADbType.VarBinary);
            SADataConvert.MapDotNetTypeToSAType[] dotNetTypeToSaTypeArray17 = SADataConvert.s_mapDotNetTypeToSAType;
            int index17 = num33;
            int num34 = 1;
            int num35 = index17 + num34;
            dotNetTypeToSaTypeArray17[index17] = new SADataConvert.MapDotNetTypeToSAType("Char[]", SADbType.NVarChar);
        }

        private static void GetMapSADbTypeToDbType()
        {
            if (SADataConvert.s_mapSADbTypeToDbType != null)
                return;
            int num1 = 0;
            SADataConvert.s_mapSADbTypeToDbType = new SADataConvert.MapSADbTypeToDbType[50];
            SADataConvert.MapSADbTypeToDbType[] saDbTypeToDbTypeArray1 = SADataConvert.s_mapSADbTypeToDbType;
            int index1 = num1;
            int num2 = 1;
            int num3 = index1 + num2;
            saDbTypeToDbTypeArray1[index1] = new SADataConvert.MapSADbTypeToDbType(SADbType.BigInt, DbType.Int64);
            SADataConvert.MapSADbTypeToDbType[] saDbTypeToDbTypeArray2 = SADataConvert.s_mapSADbTypeToDbType;
            int index2 = num3;
            int num4 = 1;
            int num5 = index2 + num4;
            saDbTypeToDbTypeArray2[index2] = new SADataConvert.MapSADbTypeToDbType(SADbType.Binary, DbType.Binary);
            SADataConvert.MapSADbTypeToDbType[] saDbTypeToDbTypeArray3 = SADataConvert.s_mapSADbTypeToDbType;
            int index3 = num5;
            int num6 = 1;
            int num7 = index3 + num6;
            saDbTypeToDbTypeArray3[index3] = new SADataConvert.MapSADbTypeToDbType(SADbType.Bit, DbType.Boolean);
            SADataConvert.MapSADbTypeToDbType[] saDbTypeToDbTypeArray4 = SADataConvert.s_mapSADbTypeToDbType;
            int index4 = num7;
            int num8 = 1;
            int num9 = index4 + num8;
            saDbTypeToDbTypeArray4[index4] = new SADataConvert.MapSADbTypeToDbType(SADbType.Char, DbType.AnsiStringFixedLength);
            SADataConvert.MapSADbTypeToDbType[] saDbTypeToDbTypeArray5 = SADataConvert.s_mapSADbTypeToDbType;
            int index5 = num9;
            int num10 = 1;
            int num11 = index5 + num10;
            saDbTypeToDbTypeArray5[index5] = new SADataConvert.MapSADbTypeToDbType(SADbType.Date, DbType.Date);
            SADataConvert.MapSADbTypeToDbType[] saDbTypeToDbTypeArray6 = SADataConvert.s_mapSADbTypeToDbType;
            int index6 = num11;
            int num12 = 1;
            int num13 = index6 + num12;
            saDbTypeToDbTypeArray6[index6] = new SADataConvert.MapSADbTypeToDbType(SADbType.DateTime, DbType.DateTime);
            SADataConvert.MapSADbTypeToDbType[] saDbTypeToDbTypeArray7 = SADataConvert.s_mapSADbTypeToDbType;
            int index7 = num13;
            int num14 = 1;
            int num15 = index7 + num14;
            saDbTypeToDbTypeArray7[index7] = new SADataConvert.MapSADbTypeToDbType(SADbType.Decimal, DbType.Decimal);
            SADataConvert.MapSADbTypeToDbType[] saDbTypeToDbTypeArray8 = SADataConvert.s_mapSADbTypeToDbType;
            int index8 = num15;
            int num16 = 1;
            int num17 = index8 + num16;
            saDbTypeToDbTypeArray8[index8] = new SADataConvert.MapSADbTypeToDbType(SADbType.Double, DbType.Double);
            SADataConvert.MapSADbTypeToDbType[] saDbTypeToDbTypeArray9 = SADataConvert.s_mapSADbTypeToDbType;
            int index9 = num17;
            int num18 = 1;
            int num19 = index9 + num18;
            saDbTypeToDbTypeArray9[index9] = new SADataConvert.MapSADbTypeToDbType(SADbType.Float, DbType.Single);
            SADataConvert.MapSADbTypeToDbType[] saDbTypeToDbTypeArray10 = SADataConvert.s_mapSADbTypeToDbType;
            int index10 = num19;
            int num20 = 1;
            int num21 = index10 + num20;
            saDbTypeToDbTypeArray10[index10] = new SADataConvert.MapSADbTypeToDbType(SADbType.Image, DbType.Binary);
            SADataConvert.MapSADbTypeToDbType[] saDbTypeToDbTypeArray11 = SADataConvert.s_mapSADbTypeToDbType;
            int index11 = num21;
            int num22 = 1;
            int num23 = index11 + num22;
            saDbTypeToDbTypeArray11[index11] = new SADataConvert.MapSADbTypeToDbType(SADbType.Integer, DbType.Int32);
            SADataConvert.MapSADbTypeToDbType[] saDbTypeToDbTypeArray12 = SADataConvert.s_mapSADbTypeToDbType;
            int index12 = num23;
            int num24 = 1;
            int num25 = index12 + num24;
            saDbTypeToDbTypeArray12[index12] = new SADataConvert.MapSADbTypeToDbType(SADbType.LongBinary, DbType.Binary);
            SADataConvert.MapSADbTypeToDbType[] saDbTypeToDbTypeArray13 = SADataConvert.s_mapSADbTypeToDbType;
            int index13 = num25;
            int num26 = 1;
            int num27 = index13 + num26;
            saDbTypeToDbTypeArray13[index13] = new SADataConvert.MapSADbTypeToDbType(SADbType.LongVarbit, DbType.AnsiString);
            SADataConvert.MapSADbTypeToDbType[] saDbTypeToDbTypeArray14 = SADataConvert.s_mapSADbTypeToDbType;
            int index14 = num27;
            int num28 = 1;
            int num29 = index14 + num28;
            saDbTypeToDbTypeArray14[index14] = new SADataConvert.MapSADbTypeToDbType(SADbType.LongVarchar, DbType.AnsiString);
            SADataConvert.MapSADbTypeToDbType[] saDbTypeToDbTypeArray15 = SADataConvert.s_mapSADbTypeToDbType;
            int index15 = num29;
            int num30 = 1;
            int num31 = index15 + num30;
            saDbTypeToDbTypeArray15[index15] = new SADataConvert.MapSADbTypeToDbType(SADbType.LongNVarchar, DbType.String);
            SADataConvert.MapSADbTypeToDbType[] saDbTypeToDbTypeArray16 = SADataConvert.s_mapSADbTypeToDbType;
            int index16 = num31;
            int num32 = 1;
            int num33 = index16 + num32;
            saDbTypeToDbTypeArray16[index16] = new SADataConvert.MapSADbTypeToDbType(SADbType.Money, DbType.Currency);
            SADataConvert.MapSADbTypeToDbType[] saDbTypeToDbTypeArray17 = SADataConvert.s_mapSADbTypeToDbType;
            int index17 = num33;
            int num34 = 1;
            int num35 = index17 + num34;
            saDbTypeToDbTypeArray17[index17] = new SADataConvert.MapSADbTypeToDbType(SADbType.NChar, DbType.StringFixedLength);
            SADataConvert.MapSADbTypeToDbType[] saDbTypeToDbTypeArray18 = SADataConvert.s_mapSADbTypeToDbType;
            int index18 = num35;
            int num36 = 1;
            int num37 = index18 + num36;
            saDbTypeToDbTypeArray18[index18] = new SADataConvert.MapSADbTypeToDbType(SADbType.NText, DbType.String);
            SADataConvert.MapSADbTypeToDbType[] saDbTypeToDbTypeArray19 = SADataConvert.s_mapSADbTypeToDbType;
            int index19 = num37;
            int num38 = 1;
            int num39 = index19 + num38;
            saDbTypeToDbTypeArray19[index19] = new SADataConvert.MapSADbTypeToDbType(SADbType.Numeric, DbType.Decimal);
            SADataConvert.MapSADbTypeToDbType[] saDbTypeToDbTypeArray20 = SADataConvert.s_mapSADbTypeToDbType;
            int index20 = num39;
            int num40 = 1;
            int num41 = index20 + num40;
            saDbTypeToDbTypeArray20[index20] = new SADataConvert.MapSADbTypeToDbType(SADbType.NVarChar, DbType.String);
            SADataConvert.MapSADbTypeToDbType[] saDbTypeToDbTypeArray21 = SADataConvert.s_mapSADbTypeToDbType;
            int index21 = num41;
            int num42 = 1;
            int num43 = index21 + num42;
            saDbTypeToDbTypeArray21[index21] = new SADataConvert.MapSADbTypeToDbType(SADbType.Real, DbType.Single);
            SADataConvert.MapSADbTypeToDbType[] saDbTypeToDbTypeArray22 = SADataConvert.s_mapSADbTypeToDbType;
            int index22 = num43;
            int num44 = 1;
            int num45 = index22 + num44;
            saDbTypeToDbTypeArray22[index22] = new SADataConvert.MapSADbTypeToDbType(SADbType.SmallDateTime, DbType.DateTime);
            SADataConvert.MapSADbTypeToDbType[] saDbTypeToDbTypeArray23 = SADataConvert.s_mapSADbTypeToDbType;
            int index23 = num45;
            int num46 = 1;
            int num47 = index23 + num46;
            saDbTypeToDbTypeArray23[index23] = new SADataConvert.MapSADbTypeToDbType(SADbType.SmallInt, DbType.Int16);
            SADataConvert.MapSADbTypeToDbType[] saDbTypeToDbTypeArray24 = SADataConvert.s_mapSADbTypeToDbType;
            int index24 = num47;
            int num48 = 1;
            int num49 = index24 + num48;
            saDbTypeToDbTypeArray24[index24] = new SADataConvert.MapSADbTypeToDbType(SADbType.SmallMoney, DbType.Currency);
            SADataConvert.MapSADbTypeToDbType[] saDbTypeToDbTypeArray25 = SADataConvert.s_mapSADbTypeToDbType;
            int index25 = num49;
            int num50 = 1;
            int num51 = index25 + num50;
            saDbTypeToDbTypeArray25[index25] = new SADataConvert.MapSADbTypeToDbType(SADbType.SysName, DbType.AnsiString);
            SADataConvert.MapSADbTypeToDbType[] saDbTypeToDbTypeArray26 = SADataConvert.s_mapSADbTypeToDbType;
            int index26 = num51;
            int num52 = 1;
            int num53 = index26 + num52;
            saDbTypeToDbTypeArray26[index26] = new SADataConvert.MapSADbTypeToDbType(SADbType.Text, DbType.AnsiString);
            SADataConvert.MapSADbTypeToDbType[] saDbTypeToDbTypeArray27 = SADataConvert.s_mapSADbTypeToDbType;
            int index27 = num53;
            int num54 = 1;
            int num55 = index27 + num54;
            saDbTypeToDbTypeArray27[index27] = new SADataConvert.MapSADbTypeToDbType(SADbType.Time, DbType.Time);
            SADataConvert.MapSADbTypeToDbType[] saDbTypeToDbTypeArray28 = SADataConvert.s_mapSADbTypeToDbType;
            int index28 = num55;
            int num56 = 1;
            int num57 = index28 + num56;
            saDbTypeToDbTypeArray28[index28] = new SADataConvert.MapSADbTypeToDbType(SADbType.TimeStamp, DbType.DateTime);
            SADataConvert.MapSADbTypeToDbType[] saDbTypeToDbTypeArray29 = SADataConvert.s_mapSADbTypeToDbType;
            int index29 = num57;
            int num58 = 1;
            int num59 = index29 + num58;
            saDbTypeToDbTypeArray29[index29] = new SADataConvert.MapSADbTypeToDbType(SADbType.TinyInt, DbType.Byte);
            SADataConvert.MapSADbTypeToDbType[] saDbTypeToDbTypeArray30 = SADataConvert.s_mapSADbTypeToDbType;
            int index30 = num59;
            int num60 = 1;
            int num61 = index30 + num60;
            saDbTypeToDbTypeArray30[index30] = new SADataConvert.MapSADbTypeToDbType(SADbType.UniqueIdentifier, DbType.Guid);
            SADataConvert.MapSADbTypeToDbType[] saDbTypeToDbTypeArray31 = SADataConvert.s_mapSADbTypeToDbType;
            int index31 = num61;
            int num62 = 1;
            int num63 = index31 + num62;
            saDbTypeToDbTypeArray31[index31] = new SADataConvert.MapSADbTypeToDbType(SADbType.UniqueIdentifierStr, DbType.AnsiStringFixedLength);
            SADataConvert.MapSADbTypeToDbType[] saDbTypeToDbTypeArray32 = SADataConvert.s_mapSADbTypeToDbType;
            int index32 = num63;
            int num64 = 1;
            int num65 = index32 + num64;
            saDbTypeToDbTypeArray32[index32] = new SADataConvert.MapSADbTypeToDbType(SADbType.UnsignedBigInt, DbType.UInt64);
            SADataConvert.MapSADbTypeToDbType[] saDbTypeToDbTypeArray33 = SADataConvert.s_mapSADbTypeToDbType;
            int index33 = num65;
            int num66 = 1;
            int num67 = index33 + num66;
            saDbTypeToDbTypeArray33[index33] = new SADataConvert.MapSADbTypeToDbType(SADbType.UnsignedInt, DbType.UInt32);
            SADataConvert.MapSADbTypeToDbType[] saDbTypeToDbTypeArray34 = SADataConvert.s_mapSADbTypeToDbType;
            int index34 = num67;
            int num68 = 1;
            int num69 = index34 + num68;
            saDbTypeToDbTypeArray34[index34] = new SADataConvert.MapSADbTypeToDbType(SADbType.UnsignedSmallInt, DbType.UInt16);
            SADataConvert.MapSADbTypeToDbType[] saDbTypeToDbTypeArray35 = SADataConvert.s_mapSADbTypeToDbType;
            int index35 = num69;
            int num70 = 1;
            int num71 = index35 + num70;
            saDbTypeToDbTypeArray35[index35] = new SADataConvert.MapSADbTypeToDbType(SADbType.VarBinary, DbType.Binary);
            SADataConvert.MapSADbTypeToDbType[] saDbTypeToDbTypeArray36 = SADataConvert.s_mapSADbTypeToDbType;
            int index36 = num71;
            int num72 = 1;
            int num73 = index36 + num72;
            saDbTypeToDbTypeArray36[index36] = new SADataConvert.MapSADbTypeToDbType(SADbType.VarBit, DbType.AnsiString);
            SADataConvert.MapSADbTypeToDbType[] saDbTypeToDbTypeArray37 = SADataConvert.s_mapSADbTypeToDbType;
            int index37 = num73;
            int num74 = 1;
            int num75 = index37 + num74;
            saDbTypeToDbTypeArray37[index37] = new SADataConvert.MapSADbTypeToDbType(SADbType.VarChar, DbType.AnsiString);
            SADataConvert.MapSADbTypeToDbType[] saDbTypeToDbTypeArray38 = SADataConvert.s_mapSADbTypeToDbType;
            int index38 = num75;
            int num76 = 1;
            int num77 = index38 + num76;
            saDbTypeToDbTypeArray38[index38] = new SADataConvert.MapSADbTypeToDbType(SADbType.Xml, DbType.AnsiString);
        }

        private static void GetMapSADbTypeToDotNetType()
        {
            if (SADataConvert.s_mapSADbTypeToDotNetType != null)
                return;
            int num1 = 0;
            SADataConvert.s_mapSADbTypeToDotNetType = new SADataConvert.MapSADbTypeToDotNetType[50];
            SADataConvert.MapSADbTypeToDotNetType[] typeToDotNetTypeArray1 = SADataConvert.s_mapSADbTypeToDotNetType;
            int index1 = num1;
            int num2 = 1;
            int num3 = index1 + num2;
            typeToDotNetTypeArray1[index1] = new SADataConvert.MapSADbTypeToDotNetType(SADbType.BigInt, DotNetType.Int64);
            SADataConvert.MapSADbTypeToDotNetType[] typeToDotNetTypeArray2 = SADataConvert.s_mapSADbTypeToDotNetType;
            int index2 = num3;
            int num4 = 1;
            int num5 = index2 + num4;
            typeToDotNetTypeArray2[index2] = new SADataConvert.MapSADbTypeToDotNetType(SADbType.Binary, DotNetType.Bytes);
            SADataConvert.MapSADbTypeToDotNetType[] typeToDotNetTypeArray3 = SADataConvert.s_mapSADbTypeToDotNetType;
            int index3 = num5;
            int num6 = 1;
            int num7 = index3 + num6;
            typeToDotNetTypeArray3[index3] = new SADataConvert.MapSADbTypeToDotNetType(SADbType.Bit, DotNetType.Boolean);
            SADataConvert.MapSADbTypeToDotNetType[] typeToDotNetTypeArray4 = SADataConvert.s_mapSADbTypeToDotNetType;
            int index4 = num7;
            int num8 = 1;
            int num9 = index4 + num8;
            typeToDotNetTypeArray4[index4] = new SADataConvert.MapSADbTypeToDotNetType(SADbType.Char, DotNetType.String);
            SADataConvert.MapSADbTypeToDotNetType[] typeToDotNetTypeArray5 = SADataConvert.s_mapSADbTypeToDotNetType;
            int index5 = num9;
            int num10 = 1;
            int num11 = index5 + num10;
            typeToDotNetTypeArray5[index5] = new SADataConvert.MapSADbTypeToDotNetType(SADbType.Date, DotNetType.DateTime);
            SADataConvert.MapSADbTypeToDotNetType[] typeToDotNetTypeArray6 = SADataConvert.s_mapSADbTypeToDotNetType;
            int index6 = num11;
            int num12 = 1;
            int num13 = index6 + num12;
            typeToDotNetTypeArray6[index6] = new SADataConvert.MapSADbTypeToDotNetType(SADbType.DateTime, DotNetType.DateTime);
            SADataConvert.MapSADbTypeToDotNetType[] typeToDotNetTypeArray7 = SADataConvert.s_mapSADbTypeToDotNetType;
            int index7 = num13;
            int num14 = 1;
            int num15 = index7 + num14;
            typeToDotNetTypeArray7[index7] = new SADataConvert.MapSADbTypeToDotNetType(SADbType.Decimal, DotNetType.Decimal);
            SADataConvert.MapSADbTypeToDotNetType[] typeToDotNetTypeArray8 = SADataConvert.s_mapSADbTypeToDotNetType;
            int index8 = num15;
            int num16 = 1;
            int num17 = index8 + num16;
            typeToDotNetTypeArray8[index8] = new SADataConvert.MapSADbTypeToDotNetType(SADbType.Double, DotNetType.Double);
            SADataConvert.MapSADbTypeToDotNetType[] typeToDotNetTypeArray9 = SADataConvert.s_mapSADbTypeToDotNetType;
            int index9 = num17;
            int num18 = 1;
            int num19 = index9 + num18;
            typeToDotNetTypeArray9[index9] = new SADataConvert.MapSADbTypeToDotNetType(SADbType.Float, DotNetType.Single);
            SADataConvert.MapSADbTypeToDotNetType[] typeToDotNetTypeArray10 = SADataConvert.s_mapSADbTypeToDotNetType;
            int index10 = num19;
            int num20 = 1;
            int num21 = index10 + num20;
            typeToDotNetTypeArray10[index10] = new SADataConvert.MapSADbTypeToDotNetType(SADbType.Image, DotNetType.Bytes);
            SADataConvert.MapSADbTypeToDotNetType[] typeToDotNetTypeArray11 = SADataConvert.s_mapSADbTypeToDotNetType;
            int index11 = num21;
            int num22 = 1;
            int num23 = index11 + num22;
            typeToDotNetTypeArray11[index11] = new SADataConvert.MapSADbTypeToDotNetType(SADbType.Integer, DotNetType.Int32);
            SADataConvert.MapSADbTypeToDotNetType[] typeToDotNetTypeArray12 = SADataConvert.s_mapSADbTypeToDotNetType;
            int index12 = num23;
            int num24 = 1;
            int num25 = index12 + num24;
            typeToDotNetTypeArray12[index12] = new SADataConvert.MapSADbTypeToDotNetType(SADbType.LongBinary, DotNetType.Bytes);
            SADataConvert.MapSADbTypeToDotNetType[] typeToDotNetTypeArray13 = SADataConvert.s_mapSADbTypeToDotNetType;
            int index13 = num25;
            int num26 = 1;
            int num27 = index13 + num26;
            typeToDotNetTypeArray13[index13] = new SADataConvert.MapSADbTypeToDotNetType(SADbType.LongNVarchar, DotNetType.String);
            SADataConvert.MapSADbTypeToDotNetType[] typeToDotNetTypeArray14 = SADataConvert.s_mapSADbTypeToDotNetType;
            int index14 = num27;
            int num28 = 1;
            int num29 = index14 + num28;
            typeToDotNetTypeArray14[index14] = new SADataConvert.MapSADbTypeToDotNetType(SADbType.LongVarbit, DotNetType.String);
            SADataConvert.MapSADbTypeToDotNetType[] typeToDotNetTypeArray15 = SADataConvert.s_mapSADbTypeToDotNetType;
            int index15 = num29;
            int num30 = 1;
            int num31 = index15 + num30;
            typeToDotNetTypeArray15[index15] = new SADataConvert.MapSADbTypeToDotNetType(SADbType.LongVarchar, DotNetType.String);
            SADataConvert.MapSADbTypeToDotNetType[] typeToDotNetTypeArray16 = SADataConvert.s_mapSADbTypeToDotNetType;
            int index16 = num31;
            int num32 = 1;
            int num33 = index16 + num32;
            typeToDotNetTypeArray16[index16] = new SADataConvert.MapSADbTypeToDotNetType(SADbType.Money, DotNetType.Decimal);
            SADataConvert.MapSADbTypeToDotNetType[] typeToDotNetTypeArray17 = SADataConvert.s_mapSADbTypeToDotNetType;
            int index17 = num33;
            int num34 = 1;
            int num35 = index17 + num34;
            typeToDotNetTypeArray17[index17] = new SADataConvert.MapSADbTypeToDotNetType(SADbType.NChar, DotNetType.String);
            SADataConvert.MapSADbTypeToDotNetType[] typeToDotNetTypeArray18 = SADataConvert.s_mapSADbTypeToDotNetType;
            int index18 = num35;
            int num36 = 1;
            int num37 = index18 + num36;
            typeToDotNetTypeArray18[index18] = new SADataConvert.MapSADbTypeToDotNetType(SADbType.NText, DotNetType.String);
            SADataConvert.MapSADbTypeToDotNetType[] typeToDotNetTypeArray19 = SADataConvert.s_mapSADbTypeToDotNetType;
            int index19 = num37;
            int num38 = 1;
            int num39 = index19 + num38;
            typeToDotNetTypeArray19[index19] = new SADataConvert.MapSADbTypeToDotNetType(SADbType.Numeric, DotNetType.Decimal);
            SADataConvert.MapSADbTypeToDotNetType[] typeToDotNetTypeArray20 = SADataConvert.s_mapSADbTypeToDotNetType;
            int index20 = num39;
            int num40 = 1;
            int num41 = index20 + num40;
            typeToDotNetTypeArray20[index20] = new SADataConvert.MapSADbTypeToDotNetType(SADbType.NVarChar, DotNetType.String);
            SADataConvert.MapSADbTypeToDotNetType[] typeToDotNetTypeArray21 = SADataConvert.s_mapSADbTypeToDotNetType;
            int index21 = num41;
            int num42 = 1;
            int num43 = index21 + num42;
            typeToDotNetTypeArray21[index21] = new SADataConvert.MapSADbTypeToDotNetType(SADbType.Real, DotNetType.Single);
            SADataConvert.MapSADbTypeToDotNetType[] typeToDotNetTypeArray22 = SADataConvert.s_mapSADbTypeToDotNetType;
            int index22 = num43;
            int num44 = 1;
            int num45 = index22 + num44;
            typeToDotNetTypeArray22[index22] = new SADataConvert.MapSADbTypeToDotNetType(SADbType.SmallDateTime, DotNetType.DateTime);
            SADataConvert.MapSADbTypeToDotNetType[] typeToDotNetTypeArray23 = SADataConvert.s_mapSADbTypeToDotNetType;
            int index23 = num45;
            int num46 = 1;
            int num47 = index23 + num46;
            typeToDotNetTypeArray23[index23] = new SADataConvert.MapSADbTypeToDotNetType(SADbType.SmallInt, DotNetType.Int16);
            SADataConvert.MapSADbTypeToDotNetType[] typeToDotNetTypeArray24 = SADataConvert.s_mapSADbTypeToDotNetType;
            int index24 = num47;
            int num48 = 1;
            int num49 = index24 + num48;
            typeToDotNetTypeArray24[index24] = new SADataConvert.MapSADbTypeToDotNetType(SADbType.SmallMoney, DotNetType.Decimal);
            SADataConvert.MapSADbTypeToDotNetType[] typeToDotNetTypeArray25 = SADataConvert.s_mapSADbTypeToDotNetType;
            int index25 = num49;
            int num50 = 1;
            int num51 = index25 + num50;
            typeToDotNetTypeArray25[index25] = new SADataConvert.MapSADbTypeToDotNetType(SADbType.SysName, DotNetType.String);
            SADataConvert.MapSADbTypeToDotNetType[] typeToDotNetTypeArray26 = SADataConvert.s_mapSADbTypeToDotNetType;
            int index26 = num51;
            int num52 = 1;
            int num53 = index26 + num52;
            typeToDotNetTypeArray26[index26] = new SADataConvert.MapSADbTypeToDotNetType(SADbType.Text, DotNetType.String);
            SADataConvert.MapSADbTypeToDotNetType[] typeToDotNetTypeArray27 = SADataConvert.s_mapSADbTypeToDotNetType;
            int index27 = num53;
            int num54 = 1;
            int num55 = index27 + num54;
            typeToDotNetTypeArray27[index27] = new SADataConvert.MapSADbTypeToDotNetType(SADbType.Time, DotNetType.TimeSpan);
            SADataConvert.MapSADbTypeToDotNetType[] typeToDotNetTypeArray28 = SADataConvert.s_mapSADbTypeToDotNetType;
            int index28 = num55;
            int num56 = 1;
            int num57 = index28 + num56;
            typeToDotNetTypeArray28[index28] = new SADataConvert.MapSADbTypeToDotNetType(SADbType.TimeStamp, DotNetType.DateTime);
            SADataConvert.MapSADbTypeToDotNetType[] typeToDotNetTypeArray29 = SADataConvert.s_mapSADbTypeToDotNetType;
            int index29 = num57;
            int num58 = 1;
            int num59 = index29 + num58;
            typeToDotNetTypeArray29[index29] = new SADataConvert.MapSADbTypeToDotNetType(SADbType.TinyInt, DotNetType.Byte);
            SADataConvert.MapSADbTypeToDotNetType[] typeToDotNetTypeArray30 = SADataConvert.s_mapSADbTypeToDotNetType;
            int index30 = num59;
            int num60 = 1;
            int num61 = index30 + num60;
            typeToDotNetTypeArray30[index30] = new SADataConvert.MapSADbTypeToDotNetType(SADbType.UniqueIdentifier, DotNetType.Guid);
            SADataConvert.MapSADbTypeToDotNetType[] typeToDotNetTypeArray31 = SADataConvert.s_mapSADbTypeToDotNetType;
            int index31 = num61;
            int num62 = 1;
            int num63 = index31 + num62;
            typeToDotNetTypeArray31[index31] = new SADataConvert.MapSADbTypeToDotNetType(SADbType.UniqueIdentifierStr, DotNetType.String);
            SADataConvert.MapSADbTypeToDotNetType[] typeToDotNetTypeArray32 = SADataConvert.s_mapSADbTypeToDotNetType;
            int index32 = num63;
            int num64 = 1;
            int num65 = index32 + num64;
            typeToDotNetTypeArray32[index32] = new SADataConvert.MapSADbTypeToDotNetType(SADbType.UnsignedBigInt, DotNetType.UInt64);
            SADataConvert.MapSADbTypeToDotNetType[] typeToDotNetTypeArray33 = SADataConvert.s_mapSADbTypeToDotNetType;
            int index33 = num65;
            int num66 = 1;
            int num67 = index33 + num66;
            typeToDotNetTypeArray33[index33] = new SADataConvert.MapSADbTypeToDotNetType(SADbType.UnsignedInt, DotNetType.UInt32);
            SADataConvert.MapSADbTypeToDotNetType[] typeToDotNetTypeArray34 = SADataConvert.s_mapSADbTypeToDotNetType;
            int index34 = num67;
            int num68 = 1;
            int num69 = index34 + num68;
            typeToDotNetTypeArray34[index34] = new SADataConvert.MapSADbTypeToDotNetType(SADbType.UnsignedSmallInt, DotNetType.UInt16);
            SADataConvert.MapSADbTypeToDotNetType[] typeToDotNetTypeArray35 = SADataConvert.s_mapSADbTypeToDotNetType;
            int index35 = num69;
            int num70 = 1;
            int num71 = index35 + num70;
            typeToDotNetTypeArray35[index35] = new SADataConvert.MapSADbTypeToDotNetType(SADbType.VarBinary, DotNetType.Bytes);
            SADataConvert.MapSADbTypeToDotNetType[] typeToDotNetTypeArray36 = SADataConvert.s_mapSADbTypeToDotNetType;
            int index36 = num71;
            int num72 = 1;
            int num73 = index36 + num72;
            typeToDotNetTypeArray36[index36] = new SADataConvert.MapSADbTypeToDotNetType(SADbType.VarBit, DotNetType.String);
            SADataConvert.MapSADbTypeToDotNetType[] typeToDotNetTypeArray37 = SADataConvert.s_mapSADbTypeToDotNetType;
            int index37 = num73;
            int num74 = 1;
            int num75 = index37 + num74;
            typeToDotNetTypeArray37[index37] = new SADataConvert.MapSADbTypeToDotNetType(SADbType.VarChar, DotNetType.String);
            SADataConvert.MapSADbTypeToDotNetType[] typeToDotNetTypeArray38 = SADataConvert.s_mapSADbTypeToDotNetType;
            int index38 = num75;
            int num76 = 1;
            int num77 = index38 + num76;
            typeToDotNetTypeArray38[index38] = new SADataConvert.MapSADbTypeToDotNetType(SADbType.Xml, DotNetType.String);
        }

        private static SADbType GetSADbTypeFromEnumValue(object value)
        {
            Type underlyingType = Enum.GetUnderlyingType(value.GetType());
            if (underlyingType.Equals(typeof(byte)))
                return SADbType.TinyInt;
            if (underlyingType.Equals(typeof(sbyte)))
                return SADbType.Integer;
            if (underlyingType.Equals(typeof(short)))
                return SADbType.SmallInt;
            if (underlyingType.Equals(typeof(ushort)))
                return SADbType.UnsignedSmallInt;
            if (underlyingType.Equals(typeof(int)))
                return SADbType.Integer;
            if (underlyingType.Equals(typeof(uint)))
                return SADbType.UnsignedInt;
            if (underlyingType.Equals(typeof(long)))
                return SADbType.BigInt;
            return underlyingType.Equals(typeof(ulong)) ? SADbType.UnsignedBigInt : SADbType.Integer;
        }

        private static void GetStartIndexAndLength(int size, int offset, int arrayLength, out int startIndex, out int length)
        {
            startIndex = offset <= 0 ? 0 : offset;
            if (startIndex > arrayLength - 1)
                length = 0;
            else if (size <= 0)
                length = arrayLength - startIndex;
            else
                length = startIndex + size <= arrayLength ? size : arrayLength - startIndex;
        }

        private static char[] SAToChars(SADataItem valIn, DotNetType dotNetType, int size, int offset)
        {
            if (SADataConvert.IsString(valIn.SADataType))
            {
                char[] destination = new char[valIn.Length];
                Marshal.Copy(valIn.Value, destination, 0, valIn.Length);
                if (offset <= 0 && (size <= 0 || size >= valIn.Length))
                    return destination;
                int startIndex;
                int length;
                SADataConvert.GetStartIndexAndLength(size, offset, valIn.Length, out startIndex, out length);
                char[] chArray = new char[length];
                Array.Copy(destination, startIndex, chArray, 0, length);
                return chArray;
            }
            Exception e = new InvalidCastException(SARes.GetString(11002));
            SATrace.Exception(e);
            throw e;
        }

        private static void UuidSwapBytes(byte[] bytes)
        {
            byte num1 = bytes[0];
            bytes[0] = bytes[3];
            bytes[3] = num1;
            byte num2 = bytes[1];
            bytes[1] = bytes[2];
            bytes[2] = num2;
            byte num3 = bytes[4];
            bytes[4] = bytes[5];
            bytes[5] = num3;
            byte num4 = bytes[6];
            bytes[6] = bytes[7];
            bytes[7] = num4;
        }

        public static unsafe SADataItem DotNetToSA(SAParameter parm)
        {
            int size = parm.Size;
            int offset = parm.Offset;
            object obj = parm.Value;
            SADbType saDbType = parm.SADbType;
            SADataItem saDataItem = new SADataItem();
            saDataItem.SADataType = (int)saDbType;
            switch (saDbType)
            {
                case SADbType.BigInt:
                    long num1 = !(obj is long) ? Convert.ToInt64(obj) : (long)obj;
                    saDataItem.Length = Marshal.SizeOf(typeof(long));
                    saDataItem.Value = SAUnmanagedMemory.Alloc(saDataItem.Length);
                    *(long*)(void*)saDataItem.Value = num1;
                    break;
                case SADbType.Binary:
                case SADbType.Image:
                case SADbType.LongBinary:
                case SADbType.VarBinary:
                    if (obj is byte[])
                    {
                        byte[] source = (byte[])obj;
                        int startIndex;
                        int length;
                        SADataConvert.GetStartIndexAndLength(size, offset, source.Length, out startIndex, out length);
                        if (length > 0)
                        {
                            saDataItem.Length = length;
                            saDataItem.Value = SAUnmanagedMemory.Alloc(length);
                            Marshal.Copy(source, startIndex, saDataItem.Value, length);
                            break;
                        }
                        saDataItem.Value = (IntPtr)0;
                        saDataItem.Length = 0;
                        break;
                    }
                    goto default;
                case SADbType.Bit:
                    bool flag = !(obj is bool) ? Convert.ToBoolean(obj) : (bool)obj;
                    saDataItem.Length = Marshal.SizeOf(typeof(bool));
                    saDataItem.Value = SAUnmanagedMemory.Alloc(saDataItem.Length);
                    *(sbyte*)(void*)saDataItem.Value = flag;
                    break;
                case SADbType.Char:
                case SADbType.LongNVarchar:
                case SADbType.LongVarbit:
                case SADbType.LongVarchar:
                case SADbType.NChar:
                case SADbType.NText:
                case SADbType.NVarChar:
                case SADbType.SysName:
                case SADbType.Text:
                case SADbType.UniqueIdentifierStr:
                case SADbType.VarBit:
                case SADbType.VarChar:
                case SADbType.Xml:
                    string str = !(obj is string) ? Convert.ToString(obj) : (string)obj;
                    int startIndex1;
                    int length1;
                    SADataConvert.GetStartIndexAndLength(size, offset, str.Length, out startIndex1, out length1);
                    if (length1 > 0)
                    {
                        char[] chArray = new char[length1];
                        str.CopyTo(startIndex1, chArray, 0, length1);
                        saDataItem.Length = length1;
                        saDataItem.Value = SAUnmanagedMemory.Alloc(length1 * 2);
                        Marshal.Copy(chArray, 0, saDataItem.Value, length1);
                        break;
                    }
                    saDataItem.Value = (IntPtr)0;
                    saDataItem.Length = 0;
                    break;
                case SADbType.Date:
                    DateTime dateTime = !(obj is DateTime) ? Convert.ToDateTime(obj) : (DateTime)obj;
                    saDataItem.Length = Marshal.SizeOf(typeof(SADate));
                    saDataItem.Value = SAUnmanagedMemory.Alloc(saDataItem.Length);
                    SADate* saDatePtr = (SADate*)(void*)saDataItem.Value;
                    saDatePtr->Year = (short)dateTime.Year;
                    saDatePtr->Month = (ushort)dateTime.Month;
                    saDatePtr->Day = (ushort)dateTime.Day;
                    break;
                case SADbType.DateTime:
                case SADbType.SmallDateTime:
                case SADbType.TimeStamp:
                    DateTime dt1 = !(obj is DateTime) ? Convert.ToDateTime(obj) : (DateTime)obj;
                    saDataItem.Length = Marshal.SizeOf(typeof(SADateTime));
                    saDataItem.Value = SAUnmanagedMemory.Alloc(saDataItem.Length);
                    SADateTime* saDateTimePtr = (SADateTime*)(void*)saDataItem.Value;
                    saDateTimePtr->Year = (short)dt1.Year;
                    saDateTimePtr->Month = (ushort)dt1.Month;
                    saDateTimePtr->Day = (ushort)dt1.Day;
                    saDateTimePtr->Hour = (ushort)dt1.Hour;
                    saDateTimePtr->Minute = (ushort)dt1.Minute;
                    saDateTimePtr->Second = (ushort)dt1.Second;
                    saDateTimePtr->Microsecond = SADataConvert.CaculateMicrosecond(dt1);
                    break;
                case SADbType.Decimal:
                case SADbType.Money:
                case SADbType.Numeric:
                case SADbType.SmallMoney:
                    Decimal d = !(obj is Decimal) ? Convert.ToDecimal(obj) : (Decimal)obj;
                    saDataItem.Length = Marshal.SizeOf(typeof(SADecimal));
                    saDataItem.Value = SAUnmanagedMemory.Alloc(saDataItem.Length);
                    int[] bits = Decimal.GetBits(d);
                    SADecimal* saDecimalPtr = (SADecimal*)(void*)saDataItem.Value;
                    saDecimalPtr->Lo = (uint)bits[0];
                    saDecimalPtr->Mid = (uint)bits[1];
                    saDecimalPtr->Hi = (uint)bits[2];
                    saDecimalPtr->Scale = (byte)((bits[3] & 16711680) >> 16);
                    saDecimalPtr->Sign = (byte)((bits[3] & 4026531840L) >> 31);
                    break;
                case SADbType.Double:
                    double num2 = !(obj is double) ? Convert.ToDouble(obj) : (double)obj;
                    saDataItem.Length = Marshal.SizeOf(typeof(double));
                    saDataItem.Value = SAUnmanagedMemory.Alloc(saDataItem.Length);
                    *(double*)(void*)saDataItem.Value = num2;
                    break;
                case SADbType.Float:
                case SADbType.Real:
                    float num3 = !(obj is float) ? Convert.ToSingle(obj) : (float)obj;
                    saDataItem.Length = Marshal.SizeOf(typeof(float));
                    saDataItem.Value = SAUnmanagedMemory.Alloc(saDataItem.Length);
                    *(float*)(void*)saDataItem.Value = num3;
                    break;
                case SADbType.Integer:
                    int num4 = !(obj is int) ? Convert.ToInt32(obj) : (int)obj;
                    saDataItem.Length = Marshal.SizeOf(typeof(int));
                    saDataItem.Value = SAUnmanagedMemory.Alloc(saDataItem.Length);
                    *(int*)(void*)saDataItem.Value = num4;
                    break;
                case SADbType.SmallInt:
                    short num5 = !(obj is short) ? Convert.ToInt16(obj, null) : (short)obj;
                    saDataItem.Length = Marshal.SizeOf(typeof(short));
                    saDataItem.Value = SAUnmanagedMemory.Alloc(saDataItem.Length);
                    *(short*)(void*)saDataItem.Value = num5;
                    break;
                case SADbType.Time:
                    saDataItem.Length = Marshal.SizeOf(typeof(SATime));
                    saDataItem.Value = SAUnmanagedMemory.Alloc(saDataItem.Length);
                    SATime* saTimePtr = (SATime*)(void*)saDataItem.Value;
                    if (obj is TimeSpan)
                    {
                        TimeSpan ts = (TimeSpan)obj;
                        saTimePtr->Hour = (ushort)ts.Hours;
                        saTimePtr->Minute = (ushort)ts.Minutes;
                        saTimePtr->Second = (ushort)ts.Seconds;
                        saTimePtr->Microsecond = SADataConvert.CaculateMicrosecond(ts);
                        break;
                    }
                    DateTime dt2 = !(obj is DateTime) ? Convert.ToDateTime(obj) : (DateTime)obj;
                    saTimePtr->Hour = (ushort)dt2.Hour;
                    saTimePtr->Minute = (ushort)dt2.Minute;
                    saTimePtr->Second = (ushort)dt2.Second;
                    saTimePtr->Microsecond = SADataConvert.CaculateMicrosecond(dt2);
                    break;
                case SADbType.TinyInt:
                    byte num6 = !(obj is byte) ? Convert.ToByte(obj, null) : (byte)obj;
                    saDataItem.Length = Marshal.SizeOf(typeof(byte));
                    saDataItem.Value = SAUnmanagedMemory.Alloc(saDataItem.Length);
                    *(sbyte*)(void*)saDataItem.Value = (sbyte)num6;
                    break;
                case SADbType.UniqueIdentifier:
                    Guid guid;
                    if (obj is Guid)
                        guid = (Guid)obj;
                    else if (obj is string)
                        guid = new Guid((string)obj);
                    else if (obj is byte[])
                        guid = new Guid((byte[])obj);
                    else
                        goto default;
                    byte[] byteArray = guid.ToByteArray();
                    SADataConvert.UuidSwapBytes(byteArray);
                    saDataItem.Length = byteArray.Length;
                    saDataItem.Value = SAUnmanagedMemory.Alloc(saDataItem.Length);
                    Marshal.Copy(byteArray, 0, saDataItem.Value, saDataItem.Length);
                    break;
                case SADbType.UnsignedBigInt:
                    ulong num7 = !(obj is ulong) ? Convert.ToUInt64(obj) : (ulong)obj;
                    saDataItem.Length = Marshal.SizeOf(typeof(ulong));
                    saDataItem.Value = SAUnmanagedMemory.Alloc(saDataItem.Length);
                    *(long*)(void*)saDataItem.Value = (long)num7;
                    break;
                case SADbType.UnsignedInt:
                    uint num8 = !(obj is uint) ? Convert.ToUInt32(obj) : (uint)obj;
                    saDataItem.Length = Marshal.SizeOf(typeof(uint));
                    saDataItem.Value = SAUnmanagedMemory.Alloc(saDataItem.Length);
                    *(int*)(void*)saDataItem.Value = (int)num8;
                    break;
                case SADbType.UnsignedSmallInt:
                    ushort num9 = !(obj is ushort) ? Convert.ToUInt16(obj) : (ushort)obj;
                    saDataItem.Length = Marshal.SizeOf(typeof(ushort));
                    saDataItem.Value = SAUnmanagedMemory.Alloc(saDataItem.Length);
                    *(short*)(void*)saDataItem.Value = (short)num9;
                    break;
                default:
                    Exception e = new InvalidOperationException(SARes.GetString(10989));
                    throw e;
            }
            return saDataItem;
        }

        public static DbType GetDbTypeFromValue(object val)
        {
            return SADataConvert.MapToDbType(SADataConvert.GetSADbTypeFromValue(val));
        }

        public static Type GetDotNetType(SADbType asaDbType)
        {
            switch (SADataConvert.MapToDotNetType(asaDbType))
            {
                case DotNetType.Boolean:
                    return typeof(bool);
                case DotNetType.Byte:
                    return typeof(byte);
                case DotNetType.Bytes:
                    return typeof(byte[]);
                case DotNetType.Char:
                    return typeof(char);
                case DotNetType.Chars:
                    return typeof(string);
                case DotNetType.DateTime:
                    return typeof(DateTime);
                case DotNetType.Decimal:
                    return typeof(Decimal);
                case DotNetType.Double:
                    return typeof(double);
                case DotNetType.Single:
                    return typeof(float);
                case DotNetType.Guid:
                    return typeof(Guid);
                case DotNetType.Int16:
                    return typeof(short);
                case DotNetType.Int32:
                    return typeof(int);
                case DotNetType.Int64:
                    return typeof(long);
                case DotNetType.String:
                    return typeof(string);
                case DotNetType.TimeSpan:
                    return typeof(TimeSpan);
                case DotNetType.UInt16:
                    return typeof(ushort);
                case DotNetType.UInt32:
                    return typeof(uint);
                case DotNetType.UInt64:
                    return typeof(ulong);
                default:
                    return null;
            }
        }

        public static string GetSADataTypeName(SADbType saDbType)
        {
            for (int index = 0; index < SADataConvert.s_SADbTypes.GetLength(0); ++index)
                if (SADataConvert.s_SADbTypes[index] == saDbType)
                    return SADataConvert.s_SADbTypeNames[index];
            return null;
        }

        public static SADbType GetSADbType(params string[] typeNames)
        {
            foreach (string typeName in typeNames)
            {
                if (string.Compare("st_geometry", typeName, true) == 0)
                    return SADbType.LongVarchar;
                string strB = typeName.Replace(" ", "");
                if (strB.Length > 0)
                    for (int index = 0; index < SADataConvert.s_SADbTypeNames.GetLength(0); ++index)
                        if (string.Compare(SADataConvert.s_SADbTypeNames[index].Replace(" ", ""), strB, true) == 0)
                            return SADataConvert.s_SADbTypes[index];
            }
            throw new InvalidOperationException(SARes.GetString(10989));
        }

        public static SADbType GetSADbTypeFromValue(object val)
        {
            if (val is Enum)
                return SADataConvert.GetSADbTypeFromEnumValue(val);
            string name = val.GetType().Name;
            for (int index = 0; (System.ValueType)SADataConvert.s_mapDotNetTypeToSAType[index] != null; ++index)
                if (SADataConvert.s_mapDotNetTypeToSAType[index]._dotNetTypeName.Equals(name))
                    return SADataConvert.s_mapDotNetTypeToSAType[index]._asaDbType;
            Exception e = new InvalidOperationException(SARes.GetString(10989));
            throw e;
        }

        public static bool IsBinary(int asaType)
        {
            return asaType == 2 || asaType == 12 || (asaType == 35 || asaType == 10);
        }

        public static bool IsBinary(string dataType)
        {
            return SADataConvert.IsBinary((int)SADataConvert.GetSADbType(dataType));
        }

        public static bool IsDateOrTime(string dataType)
        {
            dataType = dataType.ToLower();
            return dataType.Equals("date") || dataType.Equals("datetime") || (dataType.Equals("datetimeoffset") || dataType.Equals("smalldatetime")) || (dataType.Equals("time") || dataType.Equals("timestamp") || dataType.Equals("timestamp with time zone"));
        }

        public static bool IsDecimal(int asaType)
        {
            return asaType == 7 || asaType == 19 || (asaType == 16 || asaType == 24);
        }

        public static bool IsDecimal(string dataType)
        {
            return SADataConvert.IsDecimal((int)SADataConvert.GetSADbType(dataType));
        }

        public static bool IsLong(int asaType)
        {
            return asaType == 12 || asaType == 14 || (asaType == 15 || asaType == 13) || (asaType == 10 || asaType == 18 || (asaType == 26 || asaType == 38));
        }

        public static bool IsLong(string dataType)
        {
            return SADataConvert.IsLong((int)SADataConvert.GetSADbType(dataType));
        }

        public static bool IsNumber(int asaType)
        {
            return asaType == 7 || asaType == 19 || (asaType == 16 || asaType == 24) || (asaType == 1 || asaType == 11 || (asaType == 23 || asaType == 29)) || (asaType == 32 || asaType == 33 || (asaType == 34 || asaType == 8) || (asaType == 9 || asaType == 21));
        }

        public static bool IsNumber(string dataType)
        {
            return SADataConvert.IsNumber((int)SADataConvert.GetSADbType(dataType));
        }

        public static bool IsString(int asaType)
        {
            return asaType == 4 || asaType == 17 || (asaType == 18 || asaType == 20) || (asaType == 36 || asaType == 37 || (asaType == 14 || asaType == 15)) || (asaType == 13 || asaType == 26 || (asaType == 25 || asaType == 31) || asaType == 38);
        }

        public static bool IsString(string dataType)
        {
            return SADataConvert.IsString((int)SADataConvert.GetSADbType(dataType));
        }

        public static bool IsTimeStamp(int asaType)
        {
            return asaType == 28 || asaType == 6 || asaType == 22;
        }

        public static bool IsTimeStamp(string dataType)
        {
            return SADataConvert.IsTimeStamp((int)SADataConvert.GetSADbType(dataType));
        }

        public static DbType MapToDbType(SADbType asaDbType)
        {
            for (int index = 0; (System.ValueType)SADataConvert.s_mapSADbTypeToDbType[index] != null; ++index)
                if (SADataConvert.s_mapSADbTypeToDbType[index]._asaDbType == asaDbType)
                    return SADataConvert.s_mapSADbTypeToDbType[index]._dbType;
            Exception e = new InvalidOperationException(SARes.GetString(10989));
            SATrace.Exception(e);
            throw e;
        }

        public static DotNetType MapToDotNetType(SADbType asaDbType)
        {
            for (int index = 0; (System.ValueType)SADataConvert.s_mapSADbTypeToDotNetType[index] != null; ++index)
                if (SADataConvert.s_mapSADbTypeToDotNetType[index]._asaDbType == asaDbType)
                    return SADataConvert.s_mapSADbTypeToDotNetType[index]._dotNetType;
            Exception e = new InvalidOperationException(SARes.GetString(10989));
            SATrace.Exception(e);
            throw e;
        }

        public static SADbType MapToSADbType(DbType dbType)
        {
            for (int index = 0; (System.ValueType)SADataConvert.s_mapDbTypeToSADbType[index] != null; ++index)
                if (SADataConvert.s_mapDbTypeToSADbType[index]._dbType == dbType)
                    return SADataConvert.s_mapDbTypeToSADbType[index]._asaDbType;
            Exception e = new InvalidOperationException(SARes.GetString(10990));
            SATrace.Exception(e);
            throw e;
        }

        public static unsafe object SAToDotNet(SADataItem valIn, DotNetType dotNetType)
        {
            if (valIn.IsNull == 1)
                return DBNull.Value;
            switch (dotNetType)
            {
                case DotNetType.Boolean:
                    if (valIn.SADataType == 3)
                        return (bool)*(sbyte*)(void*)valIn.Value;
                    break;
                case DotNetType.Byte:
                    if (valIn.SADataType == 29)
                        return *(byte*)(void*)valIn.Value;
                    break;
                case DotNetType.Bytes:
                    if (SADataConvert.IsBinary(valIn.SADataType))
                    {
                        byte[] destination = new byte[valIn.Length];
                        if (valIn.Length > 0)
                            Marshal.Copy(valIn.Value, destination, 0, valIn.Length);
                        return destination;
                    }
                    break;
                case DotNetType.Char:
                    if (SADataConvert.IsString(valIn.SADataType) && valIn.Length > 0)
                        return (char)*(ushort*)(void*)valIn.Value;
                    break;
                case DotNetType.Chars:
                    if (SADataConvert.IsString(valIn.SADataType))
                        return new string((char*)(void*)valIn.Value).ToCharArray();
                    break;
                case DotNetType.DateTime:
                    if (valIn.SADataType == 5)
                    {
                        SADate* saDatePtr = (SADate*)(void*)valIn.Value;
                        return new DateTime((int)saDatePtr->Year, (int)saDatePtr->Month, (int)saDatePtr->Day);
                    }
                    if (valIn.SADataType == 27)
                    {
                        SATime* saTimePtr = (SATime*)(void*)valIn.Value;
                        return SADataConvert.CaculateDateTime(new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, (int)saTimePtr->Hour, (int)saTimePtr->Minute, (int)saTimePtr->Second), saTimePtr->Microsecond);
                    }
                    if (SADataConvert.IsTimeStamp(valIn.SADataType))
                    {
                        SADateTime* saDateTimePtr = (SADateTime*)(void*)valIn.Value;
                        return SADataConvert.CaculateDateTime(new DateTime((int)saDateTimePtr->Year, (int)saDateTimePtr->Month, (int)saDateTimePtr->Day, (int)saDateTimePtr->Hour, (int)saDateTimePtr->Minute, (int)saDateTimePtr->Second), saDateTimePtr->Microsecond);
                    }
                    break;
                case DotNetType.Decimal:
                    if (SADataConvert.IsDecimal(valIn.SADataType))
                    {
                        SADecimal* saDecimalPtr = (SADecimal*)(void*)valIn.Value;
                        bool isNegative = (saDecimalPtr->Sign) == 1;
                        return new Decimal((int)saDecimalPtr->Lo, (int)saDecimalPtr->Mid, (int)saDecimalPtr->Hi, isNegative, saDecimalPtr->Scale);
                    }
                    break;
                case DotNetType.Double:
                    if (valIn.SADataType == 8)
                        return *(double*)(void*)valIn.Value;
                    break;
                case DotNetType.Single:
                    if (valIn.SADataType == 9 || valIn.SADataType == 21)
                        return *(float*)(void*)valIn.Value;
                    break;
                case DotNetType.Guid:
                    if (valIn.SADataType == 30 || valIn.SADataType == 2)
                    {
                        byte[] numArray = new byte[valIn.Length];
                        Marshal.Copy(valIn.Value, numArray, 0, valIn.Length);
                        SADataConvert.UuidSwapBytes(numArray);
                        return new Guid(numArray);
                    }
                    if (SADataConvert.IsString(valIn.SADataType))
                        return new Guid(new string((char*)(void*)valIn.Value));
                    break;
                case DotNetType.Int16:
                    if (valIn.SADataType == 23)
                        return *(short*)(void*)valIn.Value;
                    break;
                case DotNetType.Int32:
                    if (valIn.SADataType == 11)
                        return *(int*)(void*)valIn.Value;
                    break;
                case DotNetType.Int64:
                    if (valIn.SADataType == 1)
                        return *(long*)(void*)valIn.Value;
                    break;
                case DotNetType.String:
                    if (SADataConvert.IsString(valIn.SADataType))
                        return new string((char*)(void*)valIn.Value);
                    break;
                case DotNetType.TimeSpan:
                    if (valIn.SADataType == 27)
                    {
                        SATime* saTimePtr = (SATime*)(void*)valIn.Value;
                        return SADataConvert.CaculateTimeSpan(new TimeSpan(0, (int)saTimePtr->Hour, (int)saTimePtr->Minute, (int)saTimePtr->Second), saTimePtr->Microsecond);
                    }
                    if (SADataConvert.IsTimeStamp(valIn.SADataType))
                    {
                        SADateTime* saDateTimePtr = (SADateTime*)(void*)valIn.Value;
                        return SADataConvert.CaculateTimeSpan(new TimeSpan(0, (int)saDateTimePtr->Hour, (int)saDateTimePtr->Minute, (int)saDateTimePtr->Second), saDateTimePtr->Microsecond);
                    }
                    break;
                case DotNetType.UInt16:
                    if (valIn.SADataType == 34)
                        return *(ushort*)(void*)valIn.Value;
                    break;
                case DotNetType.UInt32:
                    if (valIn.SADataType == 33)
                        return *(uint*)(void*)valIn.Value;
                    break;
                case DotNetType.UInt64:
                    if (valIn.SADataType == 32)
                        return (ulong)*(long*)(void*)valIn.Value;
                    break;
            }
            Exception e = new InvalidCastException(SARes.GetString(11002));
            SATrace.Exception(e);
            throw e;
        }

        public static object SAToDotNet(SADataItem valIn, DotNetType dotNetType, int size, int offset)
        {
            if (valIn.IsNull == 1)
                return DBNull.Value;
            if (dotNetType == DotNetType.String)
                return new string(SADataConvert.SAToChars(valIn, dotNetType, size, offset));
            if (dotNetType == DotNetType.Chars)
                return SADataConvert.SAToChars(valIn, dotNetType, size, offset);
            if (dotNetType != DotNetType.Bytes)
                return SADataConvert.SAToDotNet(valIn, dotNetType);
            if (SADataConvert.IsBinary(valIn.SADataType))
            {
                byte[] destination = new byte[valIn.Length];
                if (valIn.Length == 0)
                    return destination;
                Marshal.Copy(valIn.Value, destination, 0, valIn.Length);
                if (offset <= 0 && (size <= 0 || size >= valIn.Length))
                    return destination;
                int startIndex;
                int length;
                SADataConvert.GetStartIndexAndLength(size, offset, valIn.Length, out startIndex, out length);
                byte[] numArray = new byte[length];
                Array.Copy(destination, startIndex, numArray, 0, length);
                return numArray;
            }
            Exception e = new InvalidCastException(SARes.GetString(11002));
            SATrace.Exception(e);
            throw e;
        }

        private struct MapDbTypeToSADbType
        {
            public DbType _dbType;
            public SADbType _asaDbType;

            public MapDbTypeToSADbType(DbType dbType, SADbType asaDbType)
            {
                _dbType = dbType;
                _asaDbType = asaDbType;
            }
        }

        private struct MapSADbTypeToDbType
        {
            public SADbType _asaDbType;
            public DbType _dbType;

            public MapSADbTypeToDbType(SADbType asaDbType, DbType dbType)
            {
                _asaDbType = asaDbType;
                _dbType = dbType;
            }
        }

        private struct MapDotNetTypeToSAType
        {
            public string _dotNetTypeName;
            public SADbType _asaDbType;

            public MapDotNetTypeToSAType(string dotNetTypeName, SADbType asaDbType)
            {
                _dotNetTypeName = dotNetTypeName;
                _asaDbType = asaDbType;
            }
        }

        private struct MapSADbTypeToDotNetType
        {
            public SADbType _asaDbType;
            public DotNetType _dotNetType;

            public MapSADbTypeToDotNetType(SADbType asaDbType, DotNetType dotNetType)
            {
                _asaDbType = asaDbType;
                _dotNetType = dotNetType;
            }
        }
        private static SADataConvert.MapDbTypeToSADbType[] s_mapDbTypeToSADbType = null;
        private static SADataConvert.MapSADbTypeToDbType[] s_mapSADbTypeToDbType = null;
        private static SADataConvert.MapDotNetTypeToSAType[] s_mapDotNetTypeToSAType = null;
        private static SADataConvert.MapSADbTypeToDotNetType[] s_mapSADbTypeToDotNetType = null;
        private static SADbType[] s_SADbTypes = new SADbType[38] { SADbType.BigInt, SADbType.Binary, SADbType.Bit, SADbType.Char, SADbType.Date, SADbType.DateTime, SADbType.Decimal, SADbType.Double, SADbType.Float, SADbType.Image, SADbType.Integer, SADbType.LongBinary, SADbType.LongNVarchar, SADbType.LongVarbit, SADbType.LongVarchar, SADbType.Money, SADbType.NChar, SADbType.NText, SADbType.Numeric, SADbType.NVarChar, SADbType.Real, SADbType.SmallDateTime, SADbType.SmallInt, SADbType.SmallMoney, SADbType.SysName, SADbType.Text, SADbType.Time, SADbType.TimeStamp, SADbType.TinyInt, SADbType.UniqueIdentifier, SADbType.UniqueIdentifierStr, SADbType.UnsignedBigInt, SADbType.UnsignedInt, SADbType.UnsignedSmallInt, SADbType.VarBinary, SADbType.VarBit, SADbType.VarChar, SADbType.Xml };
        private static string[] s_SADbTypeNames = new string[38] { "bigint", "binary", "bit", "char", "date", "datetime", "decimal", "double", "float", "image", "integer", "long binary", "long nvarchar", "long varbit", "long varchar", "money", "nchar", "ntext", "numeric", "nvarchar", "real", "smalldatetime", "smallint", "smallmoney", "sysname", "text", "time", "timestamp", "tinyint", "uniqueidentifier", "uniqueidentifierstr", "unsigned bigint", "unsigned int", "unsigned smallint", "varbinary", "varbit", "varchar", "xml" };
    }
}
