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
            services.AddSingleton<PackageService, PackageService>();
            _provider = services.BuildServiceProvider();

        }

        private static async Task executeAsync()
        {
            Console.WriteLine("Strikes DB Seed Client\n");

            Console.WriteLine($"ENV: {CosmosDBContextFactory.COSMOSDB_ENDPOINT_URI}: {CosmosDBContextFactory.EndPointUrl}");
            Console.WriteLine($"ENV: {CosmosDBContextFactory.COSMOSDB_PRIMARY_KEY} : {CosmosDBContextFactory.PrimaryKey}");
            Console.WriteLine($"ENV: {CosmosDBContextFactory.COSMOSDB_DATABASE_ID}: {CosmosDBContextFactory.DatabaseId}");
            Console.WriteLine("\nIf one of them was wrong, Please double check Environment Variables or appsettings.json");

            var service = _provider.GetRequiredService<PackageService>();
  
            Console.WriteLine($"I'm deleting \nURL: {CosmosDBContextFactory.EndPointUrl} \n Database: {CosmosDBContextFactory.DatabaseId} \nAre you sure to delete this?");
            Console.ReadLine();

            await service.InitializeAsync();

            foreach (var package in PackageFixture.GenerateTestFixture())
            {
                await service.CreatePackageAsync(package);
            }
        }
    }
}
