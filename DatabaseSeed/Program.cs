using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using AutoFixture;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using StrikesLibrary;

namespace DatabaseSeed
{
    class Program
    {
        private static ServiceProvider _provider;

        static void Main(string[] args)
        {
            setup();
            executeAsync().GetAwaiter().GetResult();
        }

        private static void setup()
        {
            var services = new ServiceCollection();
            services.AddSingleton(typeof(IApplicationDbContext), CosmosDBContextFactory.Create());
            services.AddSingleton<IPackageRepository, PackageRepository>();

            services.AddSingleton(typeof(AzureSearchServiceContext),
                new AzureSearchServiceContext(
                    AzureSearchConfiguration.SearchServiceName,
                    AzureSearchConfiguration.SearchAdminApiKey,
                    CosmosDBConfiguration.EndPointUrl,
                    CosmosDBConfiguration.PrimaryKey,
                    CosmosDBConfiguration.DatabaseId,
                    LoggingFactory.Logger                
                    )
            );
            services.AddSingleton<ISearchRepository, SearchRepository>();
            services.AddSingleton<SearchService, SearchService>();

            services.AddSingleton<PackageService, PackageService>();

            _provider = services.BuildServiceProvider();

        }

        private static async Task executeAsync()
        {
            Console.WriteLine("Strikes DB Seed Client\n");

            Console.WriteLine($"ENV: {CosmosDBConfiguration.COSMOSDB_ENDPOINT_URI}: {CosmosDBConfiguration.EndPointUrl}");
            Console.WriteLine($"ENV: {CosmosDBConfiguration.COSMOSDB_PRIMARY_KEY} : {CosmosDBConfiguration.PrimaryKey}");
            Console.WriteLine($"ENV: {CosmosDBConfiguration.COSMOSDB_DATABASE_ID}: {CosmosDBConfiguration.DatabaseId}");
            Console.WriteLine($"ENV: {AzureSearchConfiguration.SEARCH_SEARCH_SERVICE_NAME}: {AzureSearchConfiguration.SearchServiceName}");
            Console.WriteLine($"ENV: {AzureSearchConfiguration.SEARCH_ADMIN_API_KEY}: {AzureSearchConfiguration.SearchAdminApiKey}");
            Console.WriteLine("\nIf one of them was wrong, Please double check Environment Variables or appsettings.json");

            var packageService = _provider.GetRequiredService<PackageService>();
            var searchService = _provider.GetRequiredService<SearchService>();

            Console.WriteLine($"I'm deleting \nURL: {CosmosDBConfiguration.EndPointUrl} \n Database: {CosmosDBConfiguration.DatabaseId} \nAre you sure to delete this?");
            Console.ReadLine();

            await packageService.InitializeAsync();

            foreach (var package in PackageFixture.GenerateTestFixture())
            {
                await packageService.CreatePackageAsync(package);
            }

            await searchService.CreateIndexWithIndexerAsync();

            Console.WriteLine("Seed has been finished.");
            Console.ReadLine();
        }
    }
}
