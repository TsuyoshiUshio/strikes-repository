using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Moq;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents;
using System.Collections.ObjectModel;
using Microsoft.Extensions.Logging;

namespace StrikesLibrary.Test
{
    public class ApplicationDbContextTest
    {
        [Fact]
        public async Task Create_DocumentCollection_IfNotExistsAsync_NormalCase()
        {
            var ExpectedDatabaseId = "foo";
            var ExpectedUniquePolicyPath = "/name";
            var ExpectedThroughput = 2500;

            var documentClientMock = new Mock<IDocumentClient>();

            string ActualDatabaseOriginalPath = "";
            Collection<UniqueKey> ActualUniqueKey = null;
            int? ActualOfferThroughput = 0;

            documentClientMock.Setup(p => p.CreateDocumentCollectionIfNotExistsAsync(It.IsAny<Uri>(), It.IsAny<DocumentCollection>(), It.IsAny<RequestOptions>()))
                .Returns(Task.FromResult<ResourceResponse<DocumentCollection>>(new ResourceResponse<DocumentCollection>()))
                .Callback<Uri, DocumentCollection, RequestOptions>((uri, doc, options) =>
            {
                ActualDatabaseOriginalPath = uri.OriginalString;
                var uniquePolicy = doc.UniqueKeyPolicy;
                ActualUniqueKey = uniquePolicy.UniqueKeys;
            });
            var loggerMock = new Mock<ILogger>();

            var context = new ApplicationDbContext(documentClientMock.Object, ExpectedDatabaseId, loggerMock.Object);
            var policy = new UniqueKeyPolicy();
            policy.AddUniqueKey(ExpectedUniquePolicyPath);
            await context.CreateDocumentCollectionIfNotExistsAsync<Package>(policy);


            Assert.Equal("dbs/" + ExpectedDatabaseId, ActualDatabaseOriginalPath);
            Assert.Equal(ExpectedUniquePolicyPath, ActualUniqueKey[0].Paths[0]);
        }

    }
}
