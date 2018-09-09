using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
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

        public const string COSMOSDB_DATABASE_ID = "COSMOSDB_DATABASE_ID";

        public const string COSMOSDB_CONNECTION_STRING = "CosmosDBConnection";

        public static string EndPointUrl => _endpointUri;
        public static string PrimaryKey => _primaryKey;
        public static string DatabaseId => _databaseId;

        static CosmosDBConfiguration()
        {
            var config = setupConfig();

            _databaseId = config[COSMOSDB_DATABASE_ID];
            _primaryKey = parsePrimaryKey(config[COSMOSDB_CONNECTION_STRING]);
            _endpointUri = parseEndpointUrl(config[COSMOSDB_CONNECTION_STRING]);
        }

        private static string parseConnectionString(string connectionString, string keyword)
        {
            foreach (var s in connectionString.Split(';'))
            {
                if (s.StartsWith(keyword))
                {
                    return s.Substring(keyword.Length);
                }
            }

            return null;
        }

        private static string parseEndpointUrl(string connectionString)
        {
            var EndpointHeader = "AccountEndpoint=";
            return parseConnectionString(connectionString, EndpointHeader);
        }
        private static string parsePrimaryKey(string connectionString)
        {
            var PrimaryKeyHeader = "AccountKey=";
            return parseConnectionString(connectionString, PrimaryKeyHeader);
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
