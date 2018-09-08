using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Azure.WebJobs;
using Microsoft.WindowsAzure.Storage;

namespace StrikesLibrary
{
    public interface ICloudStorageAccount
    {
        ICloudBlobClient CreateCloudBlobClient();
    } 
    public class CloudStorageAccountWrapper : ICloudStorageAccount
    {
        private readonly CloudStorageAccount _storageAccount;

        public CloudStorageAccountWrapper(CloudStorageAccount account)
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
        private readonly CloudBlobClient _client;
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
        private readonly CloudBlobContainer _container;
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
