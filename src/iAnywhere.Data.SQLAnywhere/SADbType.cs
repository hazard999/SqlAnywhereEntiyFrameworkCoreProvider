
// Type: iAnywhere.Data.SQLAnywhere.SADbType
// Assembly: iAnywhere.Data.SQLAnywhere.v4.0, Version=11.0.1.27424, Culture=neutral, PublicKeyToken=f222fc4333e0d400
// MVID: CC4F9F8C-E618-49D1-9147-C06A9EF53D1F
// Assembly location: C:\Program Files\SQL Anywhere 11\Assembly\V4\iAnywhere.Data.SQLAnywhere.v4.0.dll

namespace iAnywhere.Data.SQLAnywhere
{
  /// <summary>
  ///     <para>Enumerates the SQL Anywhere .NET database data types. </para>
  /// </summary>
  /// <remarks>
  ///     <para>The table below lists which .NET types are compatible with each SADbType. In the case of integral types, table columns can always be set using smaller integer types, but can also be set using larger types as long as the actual value is within the range of the type.</para>
  ///     <list type="table">
  ///     <listheader>
  ///         <term>SADbType</term><term>Compatible .NET type</term> <term>C# built-in type</term> <term>Visual Basic built-in type</term>
  ///     </listheader>
  ///     <item>
  ///     <term><b>BigInt</b></term> <term>System.<see cref="T:System.Int64" /></term> <term>long</term> <term>Long</term>
  ///     </item>
  ///     <item>
  ///     <term><b>Binary</b>, <b>VarBinary</b></term> <term>System.<see cref="T:System.Byte" />[], or System.<see cref="T:System.Guid" /> if size is 16</term> <term>byte[]</term> <term>Byte()</term>
  ///     </item>
  ///     <item>
  ///     <term><b>Bit</b></term> <term>System.<see cref="T:System.Boolean" /></term> <term>bool</term> <term>Boolean</term>
  ///     </item>
  ///     <item>
  ///     <term><b>Char</b>, <b>VarChar</b></term> <term>System.<see cref="T:System.String" /></term> <term>String</term> <term>String</term>
  ///     </item>
  ///     <item>
  ///     <term><b>Date</b></term> <term>System.<see cref="T:System.DateTime" /></term> <term>DateTime (no built-in type)</term> <term>Date</term>
  ///     </item>
  ///     <item>
  ///     <term><b>DateTime</b>, <b>TimeStamp</b></term> <term>System.<see cref="T:System.DateTime" /></term> <term>DateTime (no built-in type)</term> <term>Date</term>
  ///     </item>
  ///     <item>
  ///     <term><b>Decimal</b>, <b>Numeric</b></term> <term>System.<see cref="T:System.String" /></term> <term>decimal</term> <term>Decimal</term>
  ///     </item>
  ///     <item>
  ///     <term><b>Double</b></term> <term>System.<see cref="T:System.Double" /></term> <term>double</term> <term>Double</term>
  ///     </item>
  ///     <item>
  ///     <term><b>Float</b>, <b>Real</b></term> <term>System.<see cref="T:System.Single" /></term> <term>float</term> <term>Single</term>
  ///     </item>
  ///     <item>
  ///     <term><b>Image</b></term> <term>System.<see cref="T:System.Byte" />[]</term> <term>byte[]</term> <term>Byte()</term>
  ///     </item>
  ///     <item>
  ///     <term><b>Integer</b></term> <term>System.<see cref="T:System.Int32" /></term> <term>int</term> <term>Integer</term>
  ///     </item>
  ///     <item>
  ///     <term><b>LongBinary</b></term> <term>System.<see cref="T:System.Byte" />[]</term> <term>byte[]</term> <term>Byte()</term>
  ///     </item>
  ///     <item>
  ///     <term><b>LongNVarChar</b></term> <term>System.<see cref="T:System.String" /></term> <term>String</term> <term>String</term>
  ///     </item>
  ///     <item>
  ///     <term><b>LongVarChar</b></term> <term>System.<see cref="T:System.String" /></term> <term>String</term> <term>String</term>
  ///     </item>
  ///     <item>
  ///     <term><b>Money</b></term> <term>System.<see cref="T:System.String" /></term> <term>decimal</term> <term>Decimal</term>
  ///     </item>
  ///     <item>
  ///     <term><b>NChar</b></term> <term>System.<see cref="T:System.String" /></term> <term>String</term> <term>String</term>
  ///     </item>
  ///     <item>
  ///     <term><b>NText</b></term> <term>System.<see cref="T:System.String" /></term> <term>String</term> <term>String</term>
  ///     </item>
  ///     <item>
  ///     <term><b>Numeric</b></term> <term>System.<see cref="T:System.String" /></term> <term>decimal</term> <term>Decimal</term>
  ///     </item>
  ///     <item>
  ///     <term><b>NVarChar</b></term> <term>System.<see cref="T:System.String" /></term> <term>String</term> <term>String</term>
  ///     </item>
  ///     <item>
  ///     <term><b>SmallDateTime</b></term> <term>System.<see cref="T:System.DateTime" /></term>DateTime (no built-in type) <term>Date</term>
  ///     </item>
  ///     <item>
  ///     <term><b>SmallInt</b></term> <term>System.<see cref="T:System.Int16" /></term> <term>short</term> <term>Short</term>
  ///     </item>
  ///     <item>
  ///     <term><b>SmallMoney</b></term> <term>System.<see cref="T:System.String" /></term> <term>decimal</term> <term>Decimal</term>
  ///     </item>
  ///     <item>
  ///     <term><b>SysName</b></term> <term>System.<see cref="T:System.String" /></term> <term>String</term> <term>String</term>
  ///     </item>
  ///     <item>
  ///     <term><b>Text</b></term> <term>System.<see cref="T:System.String" /></term> <term>String</term> <term>String</term>
  ///     </item>
  ///     <item>
  ///     <term><b>Time</b></term> <term>System.<see cref="T:System.TimeSpan" /></term> <term>TimeSpan (no built-in type)</term> <term>TimeSpan (no built-in type)</term>
  ///     </item>
  ///     <item>
  ///     <term><b>TimeStamp</b></term> <term>System.<see cref="T:System.DateTime" /></term> <term>DateTime (no built-in type)</term> <term>Date</term>
  ///     </item>
  ///     <item>
  ///     <term><b>TinyInt</b></term> <term>System.<see cref="T:System.Byte" /></term> <term>byte</term> <term>Byte</term>
  ///     </item>
  ///     <item>
  ///     <term><b>UniqueIdentifier</b></term> <term>System.<see cref="T:System.Guid" /></term> <term>Guid (no built-in type)</term> <term>Guid (no built-in type)</term>
  ///     </item>
  ///     <item>
  ///     <term><b>UniqueIdentifierStr</b></term> <term>System.<see cref="T:System.String" /></term> <term>String</term> <term>String</term>
  ///     </item>
  ///     <item>
  ///     <term><b>UnsignedBigInt</b></term> <term>System.<see cref="T:System.UInt64" /></term> <term>ulong</term> <term>UInt64 (no built-in type)</term>
  ///     </item>
  ///     <item>
  ///     <term><b>UnsignedInt</b></term> <term>System.<see cref="T:System.UInt32" /></term> <term>uint</term> <term>UInt64 (no built-in type)</term>
  ///     </item>
  ///     <item>
  ///     <term><b>UnsignedSmallInt</b></term> <term>System.<see cref="T:System.UInt16" /></term> <term>ushort</term> <term>UInt64 (no built-in type)</term>
  ///     </item>
  ///     <item>
  ///     <term><b>Xml</b></term> <term>System.<see cref="N:System.Xml" /></term> <term>String</term> <term>String</term>
  ///     </item>
  ///     </list>
  ///     <para>Binary columns of length 16 are fully compatible with the UniqueIdentifier type.</para>
  /// </remarks>
  /// <seealso cref="M:iAnywhere.Data.SQLAnywhere.SADataReader.GetFieldType(System.Int32)" />
  /// <seealso cref="M:iAnywhere.Data.SQLAnywhere.SADataReader.GetDataTypeName(System.Int32)" />
  public enum SADbType
  {
    BigInt = 1,
    Binary = 2,
    Bit = 3,
    Char = 4,
    Date = 5,
    DateTime = 6,
    Decimal = 7,
    Double = 8,
    Float = 9,
    Image = 10,
    Integer = 11,
    LongBinary = 12,
    LongNVarchar = 13,
    LongVarbit = 14,
    LongVarchar = 15,
    Money = 16,
    NChar = 17,
    NText = 18,
    Numeric = 19,
    NVarChar = 20,
    Real = 21,
    SmallDateTime = 22,
    SmallInt = 23,
    SmallMoney = 24,
    SysName = 25,
    Text = 26,
    Time = 27,
    TimeStamp = 28,
    TinyInt = 29,
    UniqueIdentifier = 30,
    UniqueIdentifierStr = 31,
    UnsignedBigInt = 32,
    UnsignedInt = 33,
    UnsignedSmallInt = 34,
    VarBinary = 35,
    VarBit = 36,
    VarChar = 37,
    Xml = 38,
  }
}
