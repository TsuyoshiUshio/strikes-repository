using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DatabaseSeed
{
    public class AzureSearchConfiguration
    {
        private static readonly string _searchServiceName;
        private static readonly string _adminApiKey;

        private const string CONFIG_FILE_NAME = "appsettings.json";

        public const string SEARCH_SEARCH_SERVICE_NAME = "SEARCH_SEARCH_SERVICE_NAME";
        public const string SEARCH_ADMIN_API_KEY = "SEARCH_ADMIN_API_KEY";

        public static string SearchServiceName => _searchServiceName;
        public static string SearchAdminApiKey => _adminApiKey;

        static AzureSearchConfiguration()
        {
            var config = setupConfig();

            _searchServiceName = config[SEARCH_SEARCH_SERVICE_NAME];
            _adminApiKey = config[SEARCH_ADMIN_API_KEY];

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
