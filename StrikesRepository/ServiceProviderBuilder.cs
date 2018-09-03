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
            services.AddSingleton<IApplicationDbContext, ApplicationDbContext>();
            services.AddSingleton<IPackageRepository, PackageRepository>();


            return services.BuildServiceProvider(true);
        }
    }
}