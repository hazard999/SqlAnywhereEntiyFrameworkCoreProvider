
// Type: iAnywhere.Data.SQLAnywhere.SADateTime
// Assembly: iAnywhere.Data.SQLAnywhere.v4.0, Version=11.0.1.27424, Culture=neutral, PublicKeyToken=f222fc4333e0d400
// MVID: CC4F9F8C-E618-49D1-9147-C06A9EF53D1F
// Assembly location: C:\Program Files\SQL Anywhere 11\Assembly\V4\iAnywhere.Data.SQLAnywhere.v4.0.dll

namespace iAnywhere.Data.SQLAnywhere
{
  /// <summary>Summary description for SADateTime.</summary>
  internal struct SADateTime
  {
    public short Year;
    public ushort Month;
    public ushort Day;
    public ushort Hour;
    public ushort Minute;
    public ushort Second;
    public uint Microsecond;

    public SADateTime(short year, ushort month, ushort day, ushort hour, ushort minute, ushort second, uint microsecond)
    {
            Year = year;
            Month = month;
            Day = day;
            Hour = hour;
            Minute = minute;
            Second = second;
            Microsecond = microsecond;
    }
  }
}
