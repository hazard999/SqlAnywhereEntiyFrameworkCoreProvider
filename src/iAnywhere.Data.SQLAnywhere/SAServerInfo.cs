
// Type: iAnywhere.Data.SQLAnywhere.SAServerInfo
// Assembly: iAnywhere.Data.SQLAnywhere.v4.0, Version=11.0.1.27424, Culture=neutral, PublicKeyToken=f222fc4333e0d400
// MVID: CC4F9F8C-E618-49D1-9147-C06A9EF53D1F
// Assembly location: C:\Program Files\SQL Anywhere 11\Assembly\V4\iAnywhere.Data.SQLAnywhere.v4.0.dll

using System;
using System.Text;

namespace iAnywhere.Data.SQLAnywhere
{
  internal struct SAServerInfo
  {
    public const int c_NumColumns = 4;
    public const string c_DbNameDelim = ";";
    public int PortType;
    public int PortNum;
    public IntPtr Name;
    public IntPtr Addr;
    public int NumDbNames;
    public IntPtr DbNames;

    public SAServerInfo(int portType, int portNum, IntPtr name, IntPtr addr, int numDbNames, IntPtr dbNames)
    {
            PortType = portType;
            PortNum = portNum;
            Name = name;
            Addr = addr;
            NumDbNames = numDbNames;
            DbNames = dbNames;
    }

    public unsafe object[] ToObjArray()
    {
      object[] objArray = new object[4]{ new string((char*)(void*)Name), new string((char*)(void*)Addr), PortNum, null };
      char** chPtr = (char**) (void*)DbNames;
      StringBuilder stringBuilder = new StringBuilder();
      for (int index = 0; index < NumDbNames; ++index)
      {
        stringBuilder.Append(new string(chPtr[index]));
        if (index != NumDbNames - 1)
          stringBuilder.Append(";");
      }
      objArray[3] = stringBuilder.ToString();
      return objArray;
    }
  }
}
