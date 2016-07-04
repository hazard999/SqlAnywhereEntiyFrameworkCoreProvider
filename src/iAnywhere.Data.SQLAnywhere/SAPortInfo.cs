
// Type: iAnywhere.Data.SQLAnywhere.SAPortInfo
// Assembly: iAnywhere.Data.SQLAnywhere.v4.0, Version=11.0.1.27424, Culture=neutral, PublicKeyToken=f222fc4333e0d400
// MVID: CC4F9F8C-E618-49D1-9147-C06A9EF53D1F
// Assembly location: C:\Program Files\SQL Anywhere 11\Assembly\V4\iAnywhere.Data.SQLAnywhere.v4.0.dll

using System;

namespace iAnywhere.Data.SQLAnywhere
{
  internal struct SAPortInfo
  {
    public IntPtr Type;
    public IntPtr Options;

    public SAPortInfo(IntPtr type, IntPtr options)
    {
            Type = type;
            Options = options;
    }
  }
}
