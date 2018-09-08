using System;
using Moq;
using Xunit;
using StrikesLibrary;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Linq;

namespace StrikesLibrary.Test
{
    public class StorageAccountContextTest
    {
        [Fact]
        public void Get_Storage_Account_Name()
        {
            var expectedAccountName = "foo";
            var inputConnectionString = $"DefaultEndpointsProtocol=https;AccountName={expectedAccountName};AccountKey=dmVyeXZlcnlsb25nYmFzZTY0c3RyaW5ncw==;EndpointSuffix=core.windows.net";
            var actualAccountName = StorageAccountContext.GetStringAccountName(inputConnectionString);
            Assert.Equal(expectedAccountName, actualAccountName);

        }

        [Fact]
        public void Setup_NormalCase()
        {
            var inputConnectionString = $"DefaultEndpointsProtocol=https;AccountName=foo;AccountKey=dmVyeXZlcnlsb25nYmFzZTY0c3RyaW5ncw==;EndpointSuffix=core.windows.net";
            var fixture = new Fixture();
            var context = new StorageAccountContext(inputConnectionString, fixture.Logger);
            Assert.NotNull(context.BlobClient);
        }

        [Fact]
        public void Setup_WrongConnectionString()
        {
            var wrongConnectionString = $"DefaultEndpointsProtocol=https;AccountName=foo;EndpointSuffix=core.windows.net";
            var expectedErrorMessage = $"Can not parse ConnectionString {wrongConnectionString}";
            
            var fixture = new Fixture();
            var ex = Assert.Throws<ArgumentException>(
                () =>
                {
                    var context = new StorageAccountContext(wrongConnectionString, fixture.Logger);
                }
            );
            Assert.Equal(expectedErrorMessage, ex.Message);
        }

        [Fact]
        public void Sas_Query_Parameter()
        {
            var fixture = new Fixture();
            var inputContainerName = "repository";
            fixture.SetUpForGetSasQueryParameters(inputContainerName);

            var context = new StorageAccountContext(fixture.BloBClient, "foo", fixture.Logger);
 
            var sasQueryParameter = context.GetSASQueryParameterForWrite(inputContainerName);
            fixture.VerifyGetSasQueryParameter();
            Assert.Equal(fixture.ExpectedSasQueryParameter, sasQueryParameter);

            // It is ok for 5 minutes 30 min 
            Assert.InRange((DateTimeOffset)fixture.ActualSharedAccessBlobPolicy.SharedAccessExpiryTime, 
                DateTimeOffset.UtcNow.AddMinutes(25), DateTimeOffset.UtcNow.AddMinutes(35));
            AssertExtension.Include(fixture.ActualSharedAccessBlobPolicy.Permissions, SharedAccessBlobPermissions.Create);
            AssertExtension.NotInclude(fixture.ActualSharedAccessBlobPolicy.Permissions, SharedAccessBlobPermissions.Delete);
            AssertExtension.Include(fixture.ActualSharedAccessBlobPolicy.Permissions, SharedAccessBlobPermissions.Write);
            AssertExtension.NotInclude(fixture.ActualSharedAccessBlobPolicy.Permissions, SharedAccessBlobPermissions.List);
            AssertExtension.Include(fixture.ActualSharedAccessBlobPolicy.Permissions, SharedAccessBlobPermissions.Read);
            AssertExtension.NotInclude(fixture.ActualSharedAccessBlobPolicy.Permissions, SharedAccessBlobPermissions.Add);
        }

        private class Fixture
        {
            private Mock<ILogger> _logger;
            private Mock<ICloudBlobClient> _client;
            private Mock<ICloudBlobContainer> _container;

            public ILogger Logger => _logger.Object;
            public ICloudBlobClient BloBClient => _client.Object;

            public string ExpectedSasQueryParameter { get; set; }

            public Fixture()
            {
                _logger = new Mock<ILogger>();
                _client = new Mock<ICloudBlobClient>();
                _container = new Mock<ICloudBlobContainer>();

            }

            private string _sasQueryContainerName;
            public SharedAccessBlobPolicy ActualSharedAccessBlobPolicy { get; set; }

            public void SetUpForGetSasQueryParameters(string containerName)
            {
                _sasQueryContainerName = containerName;
                this.ExpectedSasQueryParameter =
                    "sv=2015-04-05&st=2015-04-29T22%3A18%3A26Z&se=2015-04-30T02%3A23%3A26Z&sr=b&sp=rw&sip=168.1.5.60-168.1.5.70&spr=https&sig=Z%2FRHIX5Xcg0Mq2rqI3OlWTjEg2tYkboXr1P9ZUXDtkk%3D";
                _container.Setup(c => c.GetSharedAccessSignature(It.IsAny<SharedAccessBlobPolicy>(), null)).Returns(
                    this.ExpectedSasQueryParameter
                ).Callback<SharedAccessBlobPolicy, string>((param, identifier) =>
                {
                    ActualSharedAccessBlobPolicy = param;
                });
                _client.Setup(c => c.GetContainerReference(containerName)).Returns(
                    _container.Object);
            }

            public void VerifyGetSasQueryParameter()
            {
                _container.Verify(c => c.GetSharedAccessSignature(It.IsAny<SharedAccessBlobPolicy>(), null));
                _client.Verify(c => c.GetContainerReference(_sasQueryContainerName));
            }

        }
    }

    public static class AssertExtension
    {
        public static void Include(SharedAccessBlobPermissions input, SharedAccessBlobPermissions target)
        {
            int i = (int)input;
            int t = (int)target;
            var result = i & t;
            Assert.True(result != 0);
        }
        public static void NotInclude(SharedAccessBlobPermissions input, SharedAccessBlobPermissions target)
        {
            int i = (int)input;
            int t = (int)target;
            var result = i & t;
            Assert.True(result == 0);
        }


    }
}

