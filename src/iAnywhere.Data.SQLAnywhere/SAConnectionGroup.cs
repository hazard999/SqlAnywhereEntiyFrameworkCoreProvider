
// Type: iAnywhere.Data.SQLAnywhere.SAConnectionGroup
// Assembly: iAnywhere.Data.SQLAnywhere.v4.0, Version=11.0.1.27424, Culture=neutral, PublicKeyToken=f222fc4333e0d400
// MVID: CC4F9F8C-E618-49D1-9147-C06A9EF53D1F
// Assembly location: C:\Program Files\SQL Anywhere 11\Assembly\V4\iAnywhere.Data.SQLAnywhere.v4.0.dll

using System;
using System.Collections;

namespace iAnywhere.Data.SQLAnywhere
{
  /// <summary>Summary description for SAConnectionOptions</summary>
  internal sealed class SAConnectionGroup
  {
    private ArrayList _conns;
    private WeakReference _dtcTran;

    public object Transaction
    {
      get
      {
        if (_dtcTran != null)
          return _dtcTran.Target;
        return null;
      }
    }

    public int Count
    {
      get
      {
        return _conns.Count;
      }
    }

    public SAInternalConnection this[int index]
    {
      get
      {
        return (SAInternalConnection)_conns[index];
      }
    }

    public bool HasTransactionContext
    {
      get
      {
        return _dtcTran != null;
      }
    }

    public SAConnectionGroup(object dtcTran)
    {
            _conns = new ArrayList();
      if (dtcTran != null)
                _dtcTran = new WeakReference(dtcTran);
      else
                _dtcTran = null;
    }

    public void AddConnection(SAInternalConnection connection)
    {
            _conns.Add(connection);
    }

    public void RemoveConnection(int index)
    {
            _conns.RemoveAt(index);
    }

    public void RemoveConnection(SAInternalConnection connection)
    {
            _conns.Remove(connection);
    }
  }
}
