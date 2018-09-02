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

        private static readonly DocumentClient _client;
        private static readonly IApplicationDbContext _context;

        static CosmosDBContextFactory()
        {

            _client = new DocumentClient(new Uri(CosmosDBConfiguration.EndPointUrl), CosmosDBConfiguration.PrimaryKey);
            _context = new ApplicationDbContext(_client, CosmosDBConfiguration.DatabaseId, LoggingFactory.Logger);
        }

        public static IApplicationDbContext Create()
        {
            return _context;
        }
    }
}
