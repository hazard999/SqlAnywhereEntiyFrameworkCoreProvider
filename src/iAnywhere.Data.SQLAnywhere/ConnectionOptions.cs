
// Type: iAnywhere.Data.SQLAnywhere.ConnectionOptions
// Assembly: iAnywhere.Data.SQLAnywhere.v4.0, Version=11.0.1.27424, Culture=neutral, PublicKeyToken=f222fc4333e0d400
// MVID: CC4F9F8C-E618-49D1-9147-C06A9EF53D1F
// Assembly location: C:\Program Files\SQL Anywhere 11\Assembly\V4\iAnywhere.Data.SQLAnywhere.v4.0.dll

namespace iAnywhere.Data.SQLAnywhere
{
  internal class ConnectionOptions
  {
    public string LongName;
    public string ShortName;
    public string AlternateName;
    public object DefaultValue;
    public ConnectionOptionType Type;

    public ConnectionOptions(string lname, string sname, string altName, ConnectionOptionType type, object defaultValue)
    {
            LongName = lname;
            ShortName = sname;
            AlternateName = altName;
            Type = type;
            DefaultValue = defaultValue;
    }
  }
}
