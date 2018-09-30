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
            var fixture = new Fixture();
            fixture.ExpectedDatabaseId = "foo";
            fixture.ExpectedUniquePolicyPath = "/name";
            fixture.SetUp();

            var context = new ApplicationDbContext(fixture.Client, fixture.ExpectedDatabaseId, fixture.Logger);
            var policy = new UniqueKeyPolicy();
            policy.AddUniqueKey(fixture.ExpectedUniquePolicyPath);
            await context.CreateDocumentCollectionIfNotExistsAsync<Package>(policy);


            Assert.Equal("dbs/" + fixture.ExpectedDatabaseId, fixture.ActualDatabaseOriginalPath);
            Assert.Equal(fixture.ExpectedUniquePolicyPath, fixture.ActualUniqueKey[0].Paths[0]);
        }

        private class Fixture
        {
            public string ExpectedDatabaseId { get; set; }
            public string ExpectedUniquePolicyPath { get; set; }

            public string ActualDatabaseOriginalPath { get; set; }
            public Collection<UniqueKey> ActualUniqueKey {get; set;}

            public IDocumentClient Client => _clientMock.Object;
            public ILogger Logger => _loggerMock.Object;

            private  Mock<IDocumentClient> _clientMock;
            private Mock<ILogger> _loggerMock;

            public Fixture()
            {
                _clientMock = new Mock<IDocumentClient>();
                _loggerMock = new Mock<ILogger>();
            }

            public void SetUp()
            {

                _clientMock.Setup(p => p.CreateDocumentCollectionIfNotExistsAsync(It.IsAny<Uri>(), It.IsAny<DocumentCollection>(), It.IsAny<RequestOptions>()))
                    .Returns(Task.FromResult<ResourceResponse<DocumentCollection>>(new ResourceResponse<DocumentCollection>()))
                    .Callback<Uri, DocumentCollection, RequestOptions>((uri, doc, options) =>
                    {
                        ActualDatabaseOriginalPath = uri.OriginalString;
                        var uniquePolicy = doc.UniqueKeyPolicy;
                        ActualUniqueKey = uniquePolicy.UniqueKeys;
                    });
            }
                
        }

    }
}
