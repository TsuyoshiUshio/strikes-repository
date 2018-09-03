using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace StrikesLibrary
{
    public class CosmosDBConfiguration
    {
        private static readonly string _endpointUri;
        private static readonly string _primaryKey;
        private static readonly string _databaseId;

        private const string CONFIG_FILE_NAME = "appsettings.json";

        public const string COSMOSDB_ENDPOINT_URI = "COSMOSDB_ENDPOINT_URI";
        public const string COSMOSDB_PRIMARY_KEY = "COSMOSDB_PRIMARY_KEY";
        public const string COSMOSDB_DATABASE_ID = "COSMOSDB_DATABASE_ID";

        public static string EndPointUrl => _endpointUri;
        public static string PrimaryKey => _primaryKey;
        public static string DatabaseId => _databaseId;

        static CosmosDBConfiguration()
        {
            var config = setupConfig();

            _databaseId = config[COSMOSDB_DATABASE_ID];
            _primaryKey = config[COSMOSDB_PRIMARY_KEY];
            _endpointUri = config[COSMOSDB_ENDPOINT_URI];

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
    }
}
