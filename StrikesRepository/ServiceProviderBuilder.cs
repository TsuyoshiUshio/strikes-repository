using DIBindings;
using Microsoft.Extensions.DependencyInjection;
using System;
using Microsoft.Azure.WebJobs.Logging;
using Microsoft.Extensions.Logging;
using StrikesLibrary;

namespace StrikesRepository
{
    public class ServiceProviderBuilder : IServiceProviderBuilder
    {
        public IServiceProvider BuildServiceProvider()
        {
            var services = new ServiceCollection();
            services.AddSingleton(typeof(AzureSearchServiceContext),
                new AzureSearchServiceContext(
                    AzureSearchConfiguration.SearchServiceName,
                    AzureSearchConfiguration.SearchAdminApiKey,
                    CosmosDBConfiguration.EndPointUrl,
                    CosmosDBConfiguration.PrimaryKey,
                    CosmosDBConfiguration.DatabaseId,
                    LoggerFactory.CreateLogger(LogCategories.CreateTriggerCategory("Http"))
                )
            );

            // SearchService
            services.AddSingleton<ISearchRepository, SearchRepository>();
            services.AddSingleton<ISearchService, SearchService>();
            // StorageAccountRepoistory
            services.AddSingleton<IStorageAccountRepository, StorageAccountRepository>();
            services.AddSingleton(typeof(IStorageAccountContext),
                new StorageAccountContext(
                    LoggerFactory.CreateLogger(LogCategories.CreateTriggerCategory("Http"))));



            return services.BuildServiceProvider(true);
        }

        public ILoggerFactory LoggerFactory { get; set; }
    }
}