using System;
using System.Runtime.CompilerServices;
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

        private class ParameterFixture
        {
            public HttpRequest Request { get; private set; }
            public ILogger Logger { get; private set; } 
            public ParameterFixture()
            {
                Request = new Mock<HttpRequest>().Object;
                Logger = new Mock<ILogger>().Object;
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
