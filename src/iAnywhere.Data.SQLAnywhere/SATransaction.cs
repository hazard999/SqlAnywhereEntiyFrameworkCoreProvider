
// Type: iAnywhere.Data.SQLAnywhere.SATransaction
// Assembly: iAnywhere.Data.SQLAnywhere.v4.0, Version=11.0.1.27424, Culture=neutral, PublicKeyToken=f222fc4333e0d400
// MVID: CC4F9F8C-E618-49D1-9147-C06A9EF53D1F
// Assembly location: C:\Program Files\SQL Anywhere 11\Assembly\V4\iAnywhere.Data.SQLAnywhere.v4.0.dll

using System;
using System.Data;
using System.Data.Common;

namespace iAnywhere.Data.SQLAnywhere
{

    public sealed class SATransaction : DbTransaction
    {
        private int _objectId = SATransaction.s_CurrentId++;
        private SAConnection _conn;
        private SAInternalConnection _internalConn;
        private SAIsolationLevel _il;
        private int _idTrans;
        private bool _isValid;
        private static int s_CurrentId;

        protected override DbConnection DbConnection
        {
            get
            {
                return _conn;
            }
        }

        /// <summary>
        ///     <para>The SAConnection object associated with the transaction, or a null reference (Nothing in Visual Basic) if the transaction is no longer valid.</para>
        /// </summary>
        /// <remarks>
        ///     <para>A single application can have multiple database connections, each with zero or more transactions. This property enables you to determine the connection object associated with a particular transaction created by BeginTransaction.</para>
        /// </remarks>
        public SAConnection Connection
        {
            get
            {
                return _conn;
            }
        }

        /// <summary>
        ///     <para>Specifies the isolation level for this transaction.</para>
        /// </summary>
        /// <value>The isolation level for this transaction. This can be one of:
        /// <list>
        /// <item>
        /// <term>ReadCommitted</term>
        /// </item>
        /// <item>
        /// <term>ReadUncommitted</term>
        /// </item>
        /// <item>
        /// <term>RepeatableRead</term>
        /// </item>
        /// <item>
        /// <term>Serializable</term>
        /// </item>
        /// <item>
        /// <term>Snapshot</term>
        /// </item>
        /// <item>
        /// <term>ReadOnlySnapshot</term>
        /// </item>
        /// <item>
        /// <term>StatementSnapshot</term>
        /// </item>
        /// </list>
        /// <para>The default is ReadCommitted.</para>
        /// </value>
        public override IsolationLevel IsolationLevel
        {
            get
            {
                return SATransaction.ConvertToIsolationLevel(_il);
            }
        }

        /// <summary>
        ///     <para>Specifies the isolation level for this transaction.</para>
        /// </summary>
        /// <value>The IsolationLevel for this transaction. This can be one of:
        /// <list>
        /// <item>
        /// <term>Chaos</term>
        /// </item>
        /// <item>
        /// Read <term>ReadCommitted</term>
        /// </item>
        /// <item>
        /// <term>ReadOnlySnapshot</term>
        /// </item>
        /// <item>
        /// <term>ReadUncommitted</term>
        /// </item>
        /// <item>
        /// <term>RepeatableRead</term>
        /// </item>
        /// <item>
        /// <term>Serializable</term>
        /// </item>
        /// <item>
        /// <term>Snapshot</term>
        /// </item>
        /// <item>
        /// <term>StatementSnapshot</term>
        /// </item>
        /// <item>
        /// <term>Unspecified</term>
        /// </item>
        /// </list>
        /// <para>The default is ReadCommitted.</para>
        /// </value>
        /// <remarks>
        ///     <para>Parallel transactions are not supported. Therefore, the IsolationLevel applies to the entire transaction.</para>
        /// </remarks>
        public SAIsolationLevel SAIsolationLevel
        {
            get
            {
                return _il;
            }
        }

        internal bool IsValid
        {
            get
            {
                return _isValid;
            }
        }

        internal int TransactionId
        {
            get
            {
                return _idTrans;
            }
        }

        internal SATransaction(SAInternalConnection internalConn, SAIsolationLevel il, int idTrans)
        {
            _internalConn = internalConn;
            _il = il;
            _idTrans = idTrans;
            _isValid = true;
        }

        /// <summary>
        ///     <para>Destructs an SATransaction object.</para>
        /// </summary>
        ~SATransaction()
        {
            _isValid = false;
        }

        private void Fini()
        {
            _internalConn.CleanUpTransaction();
            _internalConn = null;
            _conn = null;
            _isValid = false;
            GC.SuppressFinalize(this);
        }

        internal void SetConnection(SAConnection conn)
        {
            _conn = conn;
        }

        private void CheckConnection()
        {
            if (_internalConn == null)
            {
                Exception e = new InvalidOperationException(SARes.GetString(10999));
                throw e;
            }
        }

        private void CheckSavePoint(string savePoint)
        {
            if (savePoint == null || savePoint.Trim().Length < 1)
            {
                Exception e = new ArgumentException(SARes.GetString(11000), "savePoint");
                throw e;
            }
        }

        /// <summary>
        ///     <para>Commits the database transaction.</para>
        /// </summary>
        public override void Commit()
        {
            CheckConnection();
            SAException.CheckException(PInvokeMethods.AsaTransaction_Commit(_idTrans));
            Fini();
        }

        /// <summary>
        ///     <para>Rolls back a transaction from a pending state.</para>
        /// </summary>
        /// <remarks>
        ///     <para>The transaction can only be rolled back from a pending state (after BeginTransaction has been called, but before Commit is called).</para>
        /// </remarks>
        public override void Rollback()
        {
            CheckConnection();
            SAException.CheckException(PInvokeMethods.AsaTransaction_Rollback(_idTrans));
            Fini();
        }

        /// <summary>
        ///     <para>Rolls back a transaction from a pending state.</para>
        /// </summary>
        /// <remarks>
        ///     <para>The transaction can only be rolled back from a pending state (after BeginTransaction has been called, but before Commit is called).</para>
        /// </remarks>
        /// <param name="savePoint">The name of the savepoint to roll back to.</param>
        public void Rollback(string savePoint)
        {
            CheckConnection();
            CheckSavePoint(savePoint);
            SAException.CheckException(PInvokeMethods.AsaTransaction_RollbackToName(_idTrans, savePoint));
        }

        /// <summary>
        ///     <para>Creates a savepoint in the transaction that can be used to roll back a portion of the transaction, and specifies the savepoint name.</para>
        /// </summary>
        /// <param name="savePoint">
        ///     The name of the savepoint to which to roll back.
        /// </param>
        public void Save(string savePoint)
        {
            CheckConnection();
            CheckSavePoint(savePoint);
            SAException.CheckException(PInvokeMethods.AsaTransaction_Save(_idTrans, savePoint));
        }

        internal static SAIsolationLevel ConvertToSAIsolationLevel(IsolationLevel isoLevel)
        {
            return (SAIsolationLevel)isoLevel;
        }

        internal static IsolationLevel ConvertToIsolationLevel(SAIsolationLevel isoLevel)
        {
            return (IsolationLevel)isoLevel;
        }
    }
}
