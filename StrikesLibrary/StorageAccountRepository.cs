using System;
using System.Collections.Generic;
using System.Text;

namespace StrikesLibrary
{
    public interface IStorageAccountRepository
    {
        string GetStorageAccountName();
        string GetSASQueryParameterForWrite(string containerName);
    }
    public class StorageAccountRepository : IStorageAccountRepository
    {
        private IStorageAccountContext _context;
        public StorageAccountRepository(IStorageAccountContext context)
        {
            this._context = context;
        }

        public string GetSASQueryParameterForWrite(string containerName)
        {
            return this._context.GetSASQueryParameterForWrite(containerName);
        }

        public string GetStorageAccountName()
        {
            return this._context.GetStorageAccountName();
        }
    }
}
