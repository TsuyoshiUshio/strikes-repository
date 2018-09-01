using System;
using System.Runtime.CompilerServices;
using System.Threading;
using Microsoft.AspNetCore.Http;
using Xunit;
using Moq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using StrikesLibrary;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Newtonsoft.Json;

namespace StrikesRepository.Test
{
    public class StrikesRepositoryTest
    {
        [Fact]
        public async Task Get_package_with_a_parameter()
        {
            var fixture = new ParameterFixture();
            var document = new Package()
            {
                Name = "ushio",
            };

            var result = await StrikesRepository.GetPackage(fixture.Request, document , fixture.Logger);
            Assert.Equal("OkObjectResult", result.GetTypeName());
            Assert.Equal("ushio", result.GetPackageName());           
        }

        [Fact]
        public async Task Get_package_with_null_return_object()
        {
            var fixture = new ParameterFixture();
            var result = await StrikesRepository.GetPackage(fixture.Request, null, fixture.Logger);
            Assert.Equal("NotFoundObjectResult", result.GetTypeName());
        }

        [Fact]
        public async Task Create_package_success()
        {
            var fixture = new ParameterFixture();
            fixture.SetUpCreated();

            var result = (CreatedAtActionResult)await StrikesRepository.CreatePackage(fixture.Request, fixture.Collector, fixture.Logger);
            fixture.VerifyCreated();
            Assert.Equal("CreatedPackage", result.ActionName);
            Assert.Equal("StrikesRepository", result.ControllerName);
            Assert.Equal($"package/{fixture.Expected.Id}", (string)result.RouteValues.Values.GetEnumerator().Current);
            Assert.Equal(fixture.Expected.Id, ((Package)result.Value).Id);

        }



        private class ParameterFixture
        {
            private Mock<HttpRequest> _requestMock;
            private Mock<ILogger> _loggerMock;
            private Mock<IAsyncCollector<Package>> _collectorMock;

            public HttpRequest Request => _requestMock.Object;

            public ILogger Logger => _loggerMock.Object;

            public IAsyncCollector<Package> Collector => _collectorMock.Object;

            public ParameterFixture()
            {
                _requestMock = new Mock<HttpRequest>();
                _loggerMock = new Mock<ILogger>();
                _collectorMock = new Mock<IAsyncCollector<Package>>();
            }

            private Package _input;
            private Package _expected;

            public Package Input => _input;
            public Package Expected => _expected;

            public void SetUpCreated()
            {
                var p = createPackage();
                // Setup HttpRequest 
                var document = JsonConvert.SerializeObject(p);
                _requestMock.Setup(some => some.ReadAsStringAsync()).Returns(Task.FromResult(document));
            }

            public void VerifyCreated()
            {
                _collectorMock.Verify(p => p.AddAsync(_expected, It.IsAny<CancellationToken>()));
            }

            private Package createPackage()
            {
                return new Package()
                {
                    Name = "hello"
                };
            }

        }


    }
    internal static class ActionResultExtension
    {
        internal static string GetTypeName(this IActionResult result)
        {
            return result.GetType().Name;
        }

        internal static string GetPackageName(this IActionResult result)
        {
            return ((Package) ((OkObjectResult) result).Value).Name;
        }

    }

}
