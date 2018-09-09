using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace StrikesLibrary.Test
{
    public class CosmosDBConfigrationTest
    {
        [Fact]
        public void Parse_CosmosDB_ConnectionString()
        {
            var ExpectedEndPointUrl = "https://foo.bar.com:443/";
            var ExpectedPrimaryKey = "foo";
            var inputConnectionString =
                $"AccountEndpoint={ExpectedEndPointUrl};AccountKey={ExpectedPrimaryKey};";
            var ExpectedDatabaseId = "RepositoryDB";

            // We can't use CosmosDbConfiguration until Environment setup has been finished.
            var CosmosDBConenctionEnvName = "CosmosDBConnection";
            var CosmosDBDatabaseIdEnvName = "COSMOSDB_DATABASE_ID";

            Environment.SetEnvironmentVariable(CosmosDBConenctionEnvName, inputConnectionString);
            Environment.SetEnvironmentVariable(CosmosDBDatabaseIdEnvName, ExpectedDatabaseId);

            Assert.Equal(ExpectedEndPointUrl, CosmosDBConfiguration.EndPointUrl);
            Assert.Equal(ExpectedPrimaryKey, CosmosDBConfiguration.PrimaryKey);
            Assert.Equal(ExpectedDatabaseId, CosmosDBConfiguration.DatabaseId);

            Assert.Equal(CosmosDBConenctionEnvName, CosmosDBConfiguration.COSMOSDB_CONNECTION_STRING);
            Assert.Equal(CosmosDBDatabaseIdEnvName, CosmosDBConfiguration.COSMOSDB_DATABASE_ID);


        }
    }
}
