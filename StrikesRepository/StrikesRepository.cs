
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using StrikesLibrary;
using Microsoft.Azure.Documents.Client;
using System;
using DIBindings;

namespace StrikesRepository
{
    public static class StrikesRepository
    {
        // Search Packages  Search by Name or nothing. 
        [FunctionName("GetPackages")]
            public static async Task<IActionResult> GetPackages(
                [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "packages")]HttpRequest req,
                [CosmosDB(
                    databaseName: "RepositoryDB", 
                    collectionName: "Package",
                    ConnectionStringSetting = "")] DocumentClient client,
                [Inject] IPackageRepository repository,
                ILogger log)
        {
            var name = req.Query["name"];
           // This will be replaced by Azure Search. 
            return new OkObjectResult(JsonConvert.SerializeObject("hello"));
        }
        // Get Pakcage
        [FunctionName("GetPackage")]
        public static async Task<IActionResult> GetPackage(
                [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "package")]HttpRequest req, 
                [Inject] IPackageRepository repository,
                ILogger log)
        {
            var name = req.Query["name"];
            var package = repository.GetPackage(name);
            return new OkObjectResult(JsonConvert.SerializeObject(package));
        }

        // (Eventually move) CreateOrUpdate the Package
        [FunctionName("CreateOrUpdatePackage")]
        public static async Task<IActionResult> CreateOrUpdatePackage(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "package")]HttpRequest req, ILogger log)
        {

            return new OkObjectResult($"Result!");
        }

        // (Eventually move) Delete the Package
        [FunctionName("DeletePackage")]
        public static async Task<IActionResult> DeletePackage(
        [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "package")]HttpRequest req, ILogger log)
        {

            return new OkObjectResult($"Result!");
        }

    }
}
