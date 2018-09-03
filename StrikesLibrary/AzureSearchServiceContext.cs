using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Castle.Core.Logging;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using System.Linq;

namespace StrikesLibrary
{
    public class AzureSearchServiceContext
    {
        private readonly string _searchServiceName;
        private readonly string _adminApiKey;
        // NOTE: You need to use DI with Singleton. On Azure Functions, client should be static. 
        private readonly SearchServiceClient _client;
        private readonly SearchIndexClient _indexClient;
        private const string INDEX_NAME = "packages";
        private readonly string _cosmosDBEndpointUri;
        private readonly string _cosmosDBKey;
        private readonly string _databaseId;

        private readonly ILogger _logger;

        public AzureSearchServiceContext(
            string searchServiceName, 
            string adminApiKey, 
            string cosmosDBEndpointUri, 
            string cosmosDBKey, 
            string databaseId, 
            ILogger logger)
        {
            _searchServiceName = searchServiceName;
            _adminApiKey = adminApiKey;
            _client = new SearchServiceClient(searchServiceName, new SearchCredentials(adminApiKey));
            _indexClient = new SearchIndexClient(searchServiceName, INDEX_NAME, new SearchCredentials(adminApiKey));
            _cosmosDBEndpointUri = cosmosDBEndpointUri;
            _cosmosDBKey = cosmosDBKey;
            _databaseId = databaseId;

            _logger = logger;
        }

        private async Task CreateIndexAsync()
        {
            var definition = new Index()
            {
                Name = INDEX_NAME,
                Fields = FieldBuilder.BuildForType<Package>()
            };
            var index = await _client.Indexes.CreateAsync(definition);
        }
        private async Task CreateDataSourceAsync()
        {
            var dataContainer = new DataContainer("Package", null);
            var dataSourceCredentials = new DataSourceCredentials(GetCosmosDBConnectionString());
            var dataChangeDetectionPolicy = new HighWaterMarkChangeDetectionPolicy("_ts");
            
            var dataDeletionDetectionPolicy = new SoftDeleteColumnDeletionDetectionPolicy("IsDeleted", true);

            var dataSource = new DataSource(
                "cosmosDataSource", 
                DataSourceType.DocumentDb, 
                dataSourceCredentials, 
                dataContainer,"Strikes CosmosDB Settings", 
                dataChangeDetectionPolicy,
                dataDeletionDetectionPolicy);

            await _client.DataSources.CreateOrUpdateWithHttpMessagesAsync(dataSource);
        }

        // This method requires index and datasource
        private async Task CreateIndexerAsync()
        {
            var indexSchedule = new IndexingSchedule(TimeSpan.FromHours(1),DateTimeOffset.Now);
            var indexer = new Indexer("packageIndexer","cosmosDataSource",INDEX_NAME, "hourly scheduled", indexSchedule);
            await _client.Indexers.CreateOrUpdateWithHttpMessagesAsync(indexer);
        }

        public async Task CreateIndexWithCosmosIndexerAsync()
        {
            await CreateIndexAsync();
            await CreateDataSourceAsync();
            await CreateIndexerAsync();
        }


        public async Task DeleteIndexAsync()
        {
            try
            {
                _client.Indexes.Delete(INDEX_NAME);
            }
            catch (Exception e) // TODO Identify Which error has been thrown if there is no index.
            {
                _logger.WarnFormat($"Delete index failed. {INDEX_NAME} doesn't exists. Message: {e.Message}");
            }
        }

        public async Task<IEnumerable<Package>> SearchAsync(string query)
        {
            var parameters = new SearchParameters();
            var results = await _indexClient.Documents.SearchAsync<Package>(query, parameters);
            return results.Results.Select<SearchResult<Package>, Package>(p => p.Document);
        }

        private string GetCosmosDBConnectionString()
        {
            return
                $"AccountEndpoint={_cosmosDBEndpointUri};AccountKey={_cosmosDBKey};Database={_databaseId}";
        }

        



    }
}
