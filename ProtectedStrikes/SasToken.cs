
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace ProtectedStrikes
{
    public static class SasToken
    {
        [FunctionName("CreateSasToken")]
        public static async Task<IActionResult> CreateSasToken(
[HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "sastoken")]HttpRequest req, ILogger log)
        {
            
            return new OkObjectResult($"Result!");
        }

        // CreateOrUpdate the Package
        [FunctionName("CreateOrUpdatePackage")]
        public static async Task<IActionResult> CreateOrUpdatePackage(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "package")]HttpRequest req, ILogger log)
        {

            return new OkObjectResult($"Result!");
        }

        // Delete the Package
        [FunctionName("DeletePackage")]
        public static async Task<IActionResult> DeletePackage(
        [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "package")]HttpRequest req, ILogger log)
        {

            return new OkObjectResult($"Result!");
        }

    }
}
