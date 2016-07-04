
// Type: iAnywhere.Data.SQLAnywhere.SADecimal
// Assembly: iAnywhere.Data.SQLAnywhere.v4.0, Version=11.0.1.27424, Culture=neutral, PublicKeyToken=f222fc4333e0d400
// MVID: CC4F9F8C-E618-49D1-9147-C06A9EF53D1F
// Assembly location: C:\Program Files\SQL Anywhere 11\Assembly\V4\iAnywhere.Data.SQLAnywhere.v4.0.dll

namespace iAnywhere.Data.SQLAnywhere
{
  /// <summary>Summary description for SADecimal.</summary>
  internal struct SADecimal
  {
    public uint Lo;
    public uint Mid;
    public uint Hi;
    public byte Scale;
    public byte Sign;

    public SADecimal(uint lo, uint mid, uint hi, byte scale, byte sign)
    {
            Lo = lo;
            Mid = mid;
            Hi = hi;
            Scale = scale;
            Sign = sign;
    }
  }
}
