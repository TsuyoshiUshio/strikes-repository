using System;
using Microsoft.AspNetCore.Http;
using Xunit;
using Moq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using StrikesLibrary;
using Microsoft.AspNetCore.Mvc;

namespace StrikesRepository.Test
{
    public class StrikesRepositoryTest
    {
        [Fact]
        public async Task Get_package_with_a_parameter()
        {
            var req = new Mock<HttpRequest>();
            var log = new Mock<ILogger>();
            var document = new Package()
            {
                Name = "ushio",
            };

            var result = await StrikesRepository.GetPackage(req.Object, document , log.Object);
            Assert.Equal("OkObjectResult", result.GetType().Name);
            Assert.Equal("ushio", ((Package)((OkObjectResult)result).Value).Name);           
        }

        [Fact]
        public async Task Get_package_with_null_return_object()
        {
            var req = new Mock<HttpRequest>();
            var log = new Mock<ILogger>();
            var result = await StrikesRepository.GetPackage(req.Object, null, log.Object);
            Assert.Equal("NotFoundObjectResult", result.GetType().Name);
        }
    }
}
