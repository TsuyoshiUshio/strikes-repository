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
            services.AddSingleton<ISearchRepository, SearchRepository>();
            services.AddSingleton<SearchService, SearchService>();


            return services.BuildServiceProvider(true);
        }

        public ILoggerFactory LoggerFactory { get; set; }
    }
}