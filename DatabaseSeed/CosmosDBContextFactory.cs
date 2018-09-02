using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Configuration;
using StrikesLibrary;

namespace DatabaseSeed
{
    class CosmosDBContextFactory
    {

        private static readonly string _endpointUri;
        private static readonly string _primaryKey;
        private static readonly string _databaseId;

        private static readonly DocumentClient _client;
        private static readonly IApplicationDbContext _context;

        private const string CONFIG_FILE_NAME = "appsettings.json";

        public const string COSMOSDB_ENDPOINT_URI = "COSMOSDB_ENDPOINT_URI";
        public const string COSMOSDB_PRIMARY_KEY = "COSMOSDB_PRIMARY_KEY";
        public const string COSMOSDB_DATABASE_ID = "COSMOSDB_DATABASE_ID";

        public static string EndPointUrl => _endpointUri;
        public static string PrimaryKey => _primaryKey;
        public static string DatabaseId => _databaseId;

        static CosmosDBContextFactory()
        {
            var config = setupConfig();

            _client = new DocumentClient(new Uri(config[COSMOSDB_ENDPOINT_URI]), config[COSMOSDB_PRIMARY_KEY]);

            _databaseId = config[COSMOSDB_DATABASE_ID];
            _primaryKey = config[COSMOSDB_PRIMARY_KEY];
            _endpointUri = config[COSMOSDB_ENDPOINT_URI];
            _context = new ApplicationDbContext(_client, _databaseId, LoggingFactory.Logger);
        }

        private static IConfigurationRoot setupConfig()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory());
            if (File.Exists(CONFIG_FILE_NAME))
            {
                builder.AddJsonFile(CONFIG_FILE_NAME);
            }

            builder.AddEnvironmentVariables();
            return builder.Build();
        }

        public static IApplicationDbContext Create()
        {
            return _context;
        }
    }
}
