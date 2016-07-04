
// Type: iAnywhere.Data.SQLAnywhere.SAConnectionPool
// Assembly: iAnywhere.Data.SQLAnywhere.v4.0, Version=11.0.1.27424, Culture=neutral, PublicKeyToken=f222fc4333e0d400
// MVID: CC4F9F8C-E618-49D1-9147-C06A9EF53D1F
// Assembly location: C:\Program Files\SQL Anywhere 11\Assembly\V4\iAnywhere.Data.SQLAnywhere.v4.0.dll

using System;
using System.Collections;
using System.Threading;

namespace iAnywhere.Data.SQLAnywhere
{
  internal sealed class SAConnectionPool
  {
    private int _connLifetime;
    private bool _connReset;
    private bool _enlist;
    private int _maxPoolSize;
    private int _minPoolSize;
    private bool _pooling;
    private string _connStr;
    private Hashtable _connOpts;
    private ArrayList _connGroups;

    public int Count
    {
      get
      {
        int num = 0;
        for (int index = 0; index < _connGroups.Count; ++index)
        {
          SAConnectionGroup saConnectionGroup = (SAConnectionGroup)_connGroups[index];
          num += saConnectionGroup.Count;
        }
        return num;
      }
    }

    public ArrayList ConnectionGroups
    {
      get
      {
        return _connGroups;
      }
    }

    public SAConnectionGroup NonTranGroup
    {
      get
      {
        return (SAConnectionGroup)_connGroups[0];
      }
    }

    public SAConnectionPool(string connectionString, Hashtable connectionOption)
    {
            _connStr = connectionString;
            _connOpts = connectionOption;
            _connLifetime = SAConnectionOptions.GetConnectionLifetime(_connOpts);
            _connReset = SAConnectionOptions.GetConnectionReset(_connOpts);
            _enlist = SAConnectionOptions.GetEnlist(_connOpts);
            _pooling = SAConnectionOptions.GetPooling(_connOpts);
            _maxPoolSize = SAConnectionOptions.GetMaxPoolSize(_connOpts);
            _minPoolSize = SAConnectionOptions.GetMinPoolSize(_connOpts);
      SAConnectionGroup connectionGroup = new SAConnectionGroup(null);
            _connGroups = new ArrayList();
            _connGroups.Add(connectionGroup);
      int num = _minPoolSize > 0 ? _minPoolSize : 1;
      for (int index = 0; index < num; ++index)
      {
        SAInternalConnection connection = new SAInternalConnection(null, true, null, _connStr, connectionGroup);
        SATrace.AllocateConnection(connectionString);
        connectionGroup.AddConnection(connection);
      }
    }

    public void Empty()
    {
      if (_connGroups == null)
        return;
      foreach (SAConnectionGroup connGroup in _connGroups)
      {
        while (connGroup.Count != 0)
        {
          SAInternalConnection connection = connGroup[0];
          SAConnection parent = connection.Parent;
          if (parent != null)
          {
            parent.UnPool();
            connection.Pooled = false;
            connGroup.RemoveConnection(connection);
          }
          else
            connection.Close();
        }
      }
            _connGroups = null;
    }

    public SAInternalConnection AllocateConnection(SAConnection parent, object dtcTran)
    {
      SAConnectionGroup connectionGroup = null;
      for (int index = 0; index < _connGroups.Count; ++index)
      {
        SAConnectionGroup saConnectionGroup = (SAConnectionGroup)_connGroups[index];
        if (saConnectionGroup.Transaction == dtcTran)
          connectionGroup = saConnectionGroup;
      }
      if (connectionGroup == null)
      {
        connectionGroup = new SAConnectionGroup(dtcTran);
                _connGroups.Add(connectionGroup);
      }
      int num1 = 60;
      int num2 = SAConnectionOptions.GetConnectionTimeout(_connOpts);
      DateTime now = DateTime.Now;
      if (num2 == 0 || num2 > num1)
        num2 = num1;
      while ((DateTime.Now - now).TotalSeconds < num2)
      {
        for (int index = 0; index < connectionGroup.Count; ++index)
        {
          SAInternalConnection internalConnection = connectionGroup[index];
          if (!internalConnection.Busy && internalConnection.TransactionIsOver)
          {
            SATrace.ReuseConnection(parent.ConnectionString);
            internalConnection.CheckPooledConnection();
            internalConnection.Busy = true;
            internalConnection.Parent = parent;
            return internalConnection;
          }
        }
        for (int index1 = 0; index1 < _connGroups.Count; ++index1)
        {
          SAConnectionGroup saConnectionGroup = (SAConnectionGroup)_connGroups[index1];
          if (saConnectionGroup != connectionGroup)
          {
            for (int index2 = 0; index2 < saConnectionGroup.Count; ++index2)
            {
              SAInternalConnection connection = saConnectionGroup[index2];
              if (!connection.Busy && connection.TransactionIsOver)
              {
                SATrace.ReuseConnection(parent.ConnectionString);
                connection.CheckPooledConnection();
                saConnectionGroup.RemoveConnection(index2);
                connectionGroup.AddConnection(connection);
                connection.Busy = true;
                connection.Parent = parent;
                return connection;
              }
            }
          }
        }
        if (Count < SAConnectionOptions.GetMaxPoolSize(_connOpts))
        {
          SATrace.AllocateConnection(parent.ConnectionString);
          SAInternalConnection connection = new SAInternalConnection(parent, true, dtcTran, _connStr, connectionGroup);
          connectionGroup.AddConnection(connection);
          connection.Busy = true;
          return connection;
        }
        Thread.Sleep(1);
      }
      Exception e = new InvalidOperationException(SARes.GetString(10988));
      SATrace.Exception(e);
      throw e;
    }

    public void ReturnConnection(SAInternalConnection connection)
    {
      SAConnectionGroup connectionGroup = connection.ConnectionGroup;
      int connectionLifetime = SAConnectionOptions.GetConnectionLifetime(_connOpts);
      if (connectionLifetime > 0 && connection.LifeTime >= connectionLifetime)
      {
        connection.Close();
        SATrace.RemoveConnection(connection.ConnectionString);
        connectionGroup.RemoveConnection(connection);
      }
      else if (!connection.Pooled)
      {
        connection.Close();
        SATrace.RemoveConnection(connection.ConnectionString);
        connectionGroup.RemoveConnection(connection);
      }
      else
      {
        SATrace.ReturnConnection(connection.ConnectionString);
        connection.ReturnToPool();
      }
      SAConnectionGroup nonTranGroup = NonTranGroup;
      if (nonTranGroup == connectionGroup)
        return;
      bool flag = true;
      for (int index = 0; index < connectionGroup.Count; ++index)
      {
        SAInternalConnection internalConnection = connectionGroup[index];
        if (internalConnection.Busy || !internalConnection.TransactionIsOver)
        {
          flag = false;
          break;
        }
      }
      if (!flag)
        return;
      for (int index = 0; index < connectionGroup.Count; ++index)
      {
        SAInternalConnection connection1 = connectionGroup[index];
        nonTranGroup.AddConnection(connection1);
        connection1.ConnectionGroup = nonTranGroup;
      }
            _connGroups.Remove(connectionGroup);
    }
  }
}
