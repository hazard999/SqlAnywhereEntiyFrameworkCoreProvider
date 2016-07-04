using System.Collections;

namespace iAnywhere.Data.SQLAnywhere
{
    internal sealed class SAConnectionPoolManager
    {
        private Hashtable _connPools;

        public SAConnectionPoolManager()
        {
            _connPools = new Hashtable();
        }

        public SAInternalConnection AllocateConnection(SAConnection parent, object dtcTran, string connectionString, Hashtable connectionOptions)
        {
            SAConnectionPool saConnectionPool;
            lock (_connPools)
            {
                if (_connPools.Contains(connectionString))
                {
                    saConnectionPool = (SAConnectionPool)_connPools[connectionString];
                }
                else
                {
                    saConnectionPool = new SAConnectionPool(connectionString, connectionOptions);
                    _connPools[connectionString] = saConnectionPool;
                }
            }
            lock (saConnectionPool)
                return saConnectionPool.AllocateConnection(parent, dtcTran);
        }

        public void ReturnConnection(SAInternalConnection connection)
        {
            SAConnectionPool saConnectionPool = (SAConnectionPool)_connPools[connection.ConnectionString];
            lock (saConnectionPool)
                saConnectionPool.ReturnConnection(connection);
        }

        public void ClearAllPools()
        {
            foreach (DictionaryEntry connPool in _connPools)
                ((SAConnectionPool)connPool.Value).Empty();
            _connPools = new Hashtable();
        }

        public int GetPoolCount()
        {
            return _connPools.Count;
        }

        public int GetConnectionCount()
        {
            int num = 0;
            foreach (DictionaryEntry connPool in _connPools)
            {
                foreach (SAConnectionGroup connectionGroup in ((SAConnectionPool)connPool.Value).ConnectionGroups)
                    num += connectionGroup.Count;
            }
            return num;
        }

        public void ClearPool(SAConnection conn)
        {
            SAConnectionPool saConnectionPool = (SAConnectionPool)_connPools[conn.SAConnectionString];
            if (saConnectionPool == null)
                return;
            saConnectionPool.Empty();
            _connPools.Remove(conn.SAConnectionString);
        }
    }
}
