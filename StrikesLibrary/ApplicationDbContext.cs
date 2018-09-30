using Microsoft.Azure.Documents;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace StrikesLibrary
{

    public interface IApplicationDbContext
    {
        Package GetPackage(string name);
        Task CreateDocumentCollectionIfNotExistsAsync<T>();
        Task CreateDocumentCollectionIfNotExistsAsync<T>(UniqueKeyPolicy uniqueKeyPolicy);
        Task CreateDocumentAsync<T>(T document);
        Task CreateDatabaseIfNotExistsAsync();
        Task DeleteDatabaseIfExistsAsync();
        string DatabaseId { get; }

    }
    public class ApplicationDbContext : IApplicationDbContext
    {
        private readonly IDocumentClient _client;
        private readonly string _databaseId;
        private readonly ILogger _logger;

        public string DatabaseId => _databaseId;

        public ApplicationDbContext(IDocumentClient client, string databaseId, ILogger logger)
        {
            this._client = client;
            this._databaseId = databaseId;
            this._logger = logger;
        }

        // This part should be tested by integration testing.
        // Package specific logic.
        public Package GetPackage(string name)
        {
            var query = _client.CreateDocumentQuery<Package>(
                    UriFactory.CreateDocumentCollectionUri(this._databaseId, typeof(Package).Name))
                .Where(p => p.Id == name);
            return query.FirstOrDefault<Package>();
        }

        public async Task CreateDocumentCollectionIfNotExistsAsync<T>()
        {
            var packageCollection = new DocumentCollection();
            packageCollection.Id = typeof(T).Name;
            // I don't use PartitionKey until it required.
            await _client.CreateDocumentCollectionIfNotExistsAsync(
                UriFactory.CreateDatabaseUri(_databaseId),
                packageCollection);

        }
        public async Task CreateDocumentCollectionIfNotExistsAsync<T>(UniqueKeyPolicy uniqueKeyPolicy) 
        {
            var documentCollection = new DocumentCollection();
            documentCollection.Id = typeof(T).Name;
            documentCollection.UniqueKeyPolicy = uniqueKeyPolicy;
            await _client.CreateDocumentCollectionIfNotExistsAsync(
                UriFactory.CreateDatabaseUri(_databaseId),
                documentCollection
                );
        }

        public async Task CreateDocumentAsync<T>(T document)
        {
            await _client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(
                    _databaseId, 
                    typeof(T).Name),
                    document);
        }

        public async Task CreateDatabaseIfNotExistsAsync()
        {
            // This method is not exists on IDocumentClient. only on DocumentClient. 
            await _client.CreateDatabaseIfNotExistsAsync(new Database {Id = _databaseId});
        }

        public async Task DeleteDatabaseIfExistsAsync()
        {
            try
            {
                await _client.DeleteDatabaseAsync(UriFactory.CreateDatabaseUri(_databaseId));
            }
            catch (DocumentClientException e)
            {
                // If I know the exact error type, I'll validate that. 
                _logger.LogWarning($"Database {_databaseId} couldn't delete. You can ignore this message if there is no Database is there." , e);
                
            }
        }


    }
}
