using System;
using Microsoft.EntityFrameworkCore.Update;

namespace EntityFrameworkCore.RelationalProviderStarter.Update
{
    public class SqlAnyhwereModificationCommandBatchFactory : IModificationCommandBatchFactory
    {
        public ModificationCommandBatch Create()
        {
            throw new NotImplementedException();
        }
    }
}