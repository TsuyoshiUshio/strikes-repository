using DIBindings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using StrikesLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace StrikesRepository
{
    public static class StrikesRepository
    {
        private const string DATABASE_NAME = "RepositoryDB";

        private const string COLLECTION_NAME = "Package";
        // Search Packages  Search by Name or nothing. 
        [FunctionName("GetPackages")]
            public static async Task<IActionResult> GetPackages(
                [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "packages")]HttpRequest req,
                [Inject] ISearchService service,
                ILogger log)
        {
            var name = req.Query["name"];
            log.LogInformation(($"Query: {name}"));
            var results = await service.SearchNameAsync(name);
           
            return new OkObjectResult(JsonConvert.SerializeObject(results.Select(p => p.ToPackage())));
        }
        // Get Pakcage
        [FunctionName("GetPackage")]
        public static async Task<IActionResult> GetPackage(
                [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "package/name/{name}")]HttpRequest req,
                [CosmosDB(DATABASE_NAME, 
                        COLLECTION_NAME, 
                        ConnectionStringSetting = "CosmosDBConnection",
                        SqlQuery = "select * from Package p where p.Name = {name}")] IEnumerable<Package> pkgs,
                ILogger log)
        {
            // name has the unique key constratint.
            var document = pkgs.FirstOrDefault<Package>();
            if (document == null)
            {
                return new NotFoundObjectResult(document);
            }
            else
            {
                return new OkObjectResult(document);
            }
        }

        // (Eventually move) CreateOrUpdate the Package
        [FunctionName("CreatePackage")]
        public static async Task<IActionResult> CreatePackage(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "package")]HttpRequest req,
        [CosmosDB(DATABASE_NAME, COLLECTION_NAME, ConnectionStringSetting = "CosmosDBConnection")] IAsyncCollector<Package> packages,
        ILogger log)
        {
            var body = await req.GetBodyAsync<Package>();
            
            if (body.IsValid)
            {
                var model = body.Value;
                model.GenerateId();
                await packages.AddAsync(model);
             
                return new CreatedResult($"package/{model.Id}", model);
            }
            else
            {
                return new BadRequestObjectResult(JsonConvert.SerializeObject(body.ValidationResults));
            }
        }

        [FunctionName("UpdatePackage")]
        public static async Task<IActionResult> UpdatePackage(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "package/{id}")]HttpRequest req,
            [CosmosDB(DATABASE_NAME, COLLECTION_NAME, ConnectionStringSetting = "CosmosDBConnection", Id = "{id}")] object document,
            [CosmosDB(DATABASE_NAME, COLLECTION_NAME, ConnectionStringSetting = "CosmosDBConnection")] IAsyncCollector<Package> packages,
            ILogger log)
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

        [FunctionName("GetRepositoryAccessToken")]
        public static IActionResult GetRepositoryAccessToken(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "repositoryAccessToken")] HttpRequest req,
            [Inject] IStorageAccountRepository repository,
            ILogger log)
        {
            RepositoryAccessToken token;
            try
            {
                token = new RepositoryAccessToken
                {
                    StorageAccountName = repository.GetStorageAccountName(),
                    ContainerName = Repository.ContainerName,
                    SASQueryParameter = repository.GetSASQueryParameterForWrite(Repository.ContainerName)
                };
            }
            catch (Exception e)
            {
                log.LogError(e, "Can not create Repository Access Token.");
                return new ExceptionResult(e, false); 
            }

            return new OkObjectResult(token);
        }

    }
}
