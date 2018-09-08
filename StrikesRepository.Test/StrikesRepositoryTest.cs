﻿using System;
using System.Collections.Generic;
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
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Primitives;

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
            fixture.SetUpCreated(fixture.CreatePackageSuccess);

            var result = await StrikesRepository.CreatePackage(fixture.Request, fixture.Collector, fixture.Logger);
            Assert.Equal("CreatedResult", result.GetTypeName());

            fixture.VerifyCreated();
            var createdResult = (CreatedResult) result;
            Assert.Equal($"package/{fixture.Expected.Id}", (string)createdResult.Location);
            Assert.Equal(fixture.Expected.Id, ((Package)createdResult.Value).Id);
            fixture.Cleanup(); // Only in case you use Stream. 
        }

        [Fact]
        public async Task Create_package_invalid()
        {
            var fixture = new ParameterFixture();
            fixture.SetUpCreated(fixture.CreatePackageFail);
            var result = await StrikesRepository.CreatePackage(fixture.Request, fixture.Collector, fixture.Logger);
            Assert.Equal("BadRequestObjectResult", result.GetTypeName());

            var expected = "[{\"MemberNames\":[\"Name\"],\"ErrorMessage\":\"The Name field is required.\"},{\"MemberNames\":[\"ProjectPage\"],\"ErrorMessage\":\"The ProjectPage field is not a valid fully-qualified http, https, or ftp URL.\"}]";
            Assert.Equal(expected, ((BadRequestObjectResult)result).Value);
        }

        [Fact]
        public async Task Get_Packages_normal_case()
        {
            var fixture = new ParameterFixture();
            fixture.SetUpGetPackages();
            var result = await StrikesRepository.GetPackages(fixture.Request, fixture.SearchService, fixture.Logger);
            fixture.VerifyGetPackages();
            Assert.Equal(fixture.ExpectedGetPackagesName, result.GetSearchPackages().ToArray()[0].Name);
        }

        private class ParameterFixture
        {
            private Mock<HttpRequest> _requestMock;
            private Mock<ILogger> _loggerMock;
            private Mock<IAsyncCollector<Package>> _collectorMock;
            private Mock<ISearchService> _searchServiceMock;

            public HttpRequest Request => _requestMock.Object;

            public ILogger Logger => _loggerMock.Object;

            public IAsyncCollector<Package> Collector => _collectorMock.Object;

            public ISearchService SearchService => _searchServiceMock.Object;

            public ParameterFixture()
            {
                _requestMock = new Mock<HttpRequest>();
                _loggerMock = new Mock<ILogger>();
                _collectorMock = new Mock<IAsyncCollector<Package>>();
                _searchServiceMock = new Mock<ISearchService>();
            }

            private Package _input;
            private Package _expected;

            public Package Input => _input;
            public Package Expected => _expected;

            private Stream _stream;

            public void SetUpCreated(Func<Package> createPackage)
            {
                var _input = createPackage();
                // Setup HttpRequest 
                var document = JsonConvert.SerializeObject(_input);
                _stream = GenerateStreamFromString(document); // try not to use using. 
            
                // ReadAsStreamAsync is extention method. we can't mock it. 
                _requestMock.Setup(r => r.Body).Returns(_stream);
                _collectorMock.Setup(c => c.AddAsync(It.IsAny<Package>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask)
                    .Callback<Package, CancellationToken>((p, c) =>
                    {
                        _expected = p;
                    }); 

            }

            private const string ExpectedPackageName = "foo";
            public string ExpectedGetPackagesName => ExpectedPackageName;

            public void SetUpGetPackages()
            {
                var queryCollection = new Mock<IQueryCollection>();
                queryCollection.Setup(q => q["name"]).Returns(new Microsoft.Extensions.Primitives.StringValues("h*"));
                _requestMock.Setup(r => r.Query).Returns(queryCollection.Object);
                var packageList = new List<SearchPackage>
                {
                    new SearchPackage()
                    {
                        Name = ExpectedPackageName
                    }
                };
                _searchServiceMock.Setup(s => s.SearchNameAsync("h*")).ReturnsAsync(packageList);

            }

            public void VerifyGetPackages()
            {
                _searchServiceMock.Verify(s => s.SearchNameAsync("h*"));
            }

            public void Cleanup()
            {
                _stream.Close();
            }

            private static Stream GenerateStreamFromString(string s)
            {
                var stream = new MemoryStream();
                var writer = new StreamWriter(stream);
                writer.Write(s);
                writer.Flush();
                stream.Position = 0;
                return stream;
            }

            public void VerifyCreated()
            {
                _collectorMock.Verify(p => p.AddAsync(_expected, It.IsAny<CancellationToken>()));
            }

            internal Package CreatePackageSuccess()
            {
                return new Package()
                {
                    Name = "hello"
                };
            }

            internal Package CreatePackageFail()
            {
                return new Package()
                {
                    // No Name (required)
                    ProjectPage = "abc" // this should be URL
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

        internal static IEnumerable<SearchPackage> GetSearchPackages(this IActionResult result)
        {
             var json =  (string)((OkObjectResult) result).Value;
            return JsonConvert.DeserializeObject<IEnumerable<SearchPackage>>(json);

        }

    }

}
