using DIBindings;
using Microsoft.Extensions.DependencyInjection;
using System;
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
                    null
                )
            );
            services.AddSingleton<ISearchRepository, SearchRepository>();


            return services.BuildServiceProvider(true);
        }
    }
}