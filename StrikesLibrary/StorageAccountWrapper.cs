using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Azure.WebJobs;

namespace StrikesLibrary
{
    public interface IStorageAccount
    {
        ICloudBlobClient CreateCloudBlobClient();
    } 
    public class StorageAccountWrapper : IStorageAccount
    {
        private StorageAccount _storageAccount;

        public StorageAccountWrapper(StorageAccount account)
        {
            _storageAccount = account;
        }
        public ICloudBlobClient CreateCloudBlobClient()
        {
            return new CloudBlobClientWrapper(_storageAccount.CreateCloudBlobClient());
        }
    }

    public interface ICloudBlobClient
    {
        ICloudBlobContainer GetContainerReference(string containerName);
    }

    public class CloudBlobClientWrapper : ICloudBlobClient
    {
        private CloudBlobClient _client;
        public CloudBlobClientWrapper(CloudBlobClient client)
        {
            _client = client;
        }
        public ICloudBlobContainer GetContainerReference(string containerName)
        {
            return new CloudBlobContainerWrapper(_client.GetContainerReference(containerName));
        }
    }

    public interface ICloudBlobContainer
    {
        string GetSharedAccessSignature(SharedAccessBlobPolicy policy, string groupPolicyIdentifier);
    }

    public class CloudBlobContainerWrapper : ICloudBlobContainer
    {
        private CloudBlobContainer _container;
        public CloudBlobContainerWrapper(CloudBlobContainer container)
        {
            _container = container;
        }

        public string GetSharedAccessSignature(SharedAccessBlobPolicy policy, string groupPolicyIdentifier)
        {
            return _container.GetSharedAccessSignature(policy, groupPolicyIdentifier);
        }
    }
}
