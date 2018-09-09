using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Linq;

namespace StrikesLibrary
{
    public interface IStorageAccountContext
    {
        string GetSASQueryParameterForWrite(string containerName);
        string GetStorageAccountName();
    }
    public class StorageAccountContext
    {
        private string _connectionString;
        private ILogger _logger;
        private ICloudBlobClient _blobClient;

        internal ICloudBlobClient BlobClient { get => _blobClient; set => _blobClient = value; }

        public StorageAccountContext(ILogger logger)
        {
            _connectionString = StorageAccountConfigration.RepositoryConnectionString;
            _logger = logger;
            SetUpClient();
        }

        public StorageAccountContext(string connectionString, ILogger logger)
        {
            _connectionString = connectionString;
            _logger = logger;
            SetUpClient();
        }

        public StorageAccountContext(ICloudBlobClient client, string connectionString, ILogger logger)
        {
            BlobClient = client;
            _connectionString = connectionString;
            _logger = logger;
        }

        private void SetUpClient()
        {
            CloudStorageAccount storageAccount;

            if (CloudStorageAccount.TryParse(_connectionString, out storageAccount))
            {
                ICloudStorageAccount cloudStorageAccount = new CloudStorageAccountWrapper(storageAccount);
                BlobClient = cloudStorageAccount.CreateCloudBlobClient();
            }
            else
            {
               // In case of error of connection String.
                throw new ArgumentException($"Can not parse ConnectionString {_connectionString}");
            }
        }

        public string GetSASQueryParameterForWrite(string containerName)
        {
            var container = BlobClient.GetContainerReference(containerName);
            var adHocPolicy = new SharedAccessBlobPolicy()
            {
                SharedAccessExpiryTime = DateTime.UtcNow.AddMinutes(30),
                Permissions = SharedAccessBlobPermissions.Create | SharedAccessBlobPermissions.Write |
                              SharedAccessBlobPermissions.Read
            };
            return container.GetSharedAccessSignature(adHocPolicy, null);
        }

        public string GetStorageAccountName()
        {
            return GetStringAccountName(_connectionString);
        }

        internal static string GetStringAccountName(string connectionString)
        {
            var parameterName = "AccountName=";
            return connectionString.Split(';').Where(p => p.StartsWith(parameterName))
                .Select(p => p.Substring(parameterName.Length)).First();
        }

        
    }
}
