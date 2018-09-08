using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace StrikesLibrary
{
    class StorageAccountConfigration
    {
        private static readonly string _repositoryConnectionString;

        private const string CONFIG_FILE_NAME = "appsettings.json";
        public const string REPOSITORY_CONNECTION_STRING = "REPOSITORY_CONNECTION_STRING";

        public static string RepositoryConnectionString => _repositoryConnectionString;


        static StorageAccountConfigration()
        {
            var config = setupConfig();

            _repositoryConnectionString = config[REPOSITORY_CONNECTION_STRING];

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
