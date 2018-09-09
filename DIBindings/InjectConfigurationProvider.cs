using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.WebJobs.Host.Config;
using System.Reflection;
using Microsoft.Azure.WebJobs.Description;
using Microsoft.Extensions.Logging;

namespace DIBindings
{
    [Extension("Inject")]
    public class InjectConfigurationProvider : IExtensionConfigProvider
    {
        private ILoggerFactory _loggerFactory;
        private IExtensionRegistry _registry;
        public InjectConfigurationProvider(ILoggerFactory loggerFactory, IExtensionRegistry registry)
        {
            _loggerFactory = loggerFactory;
            _registry = registry;
        }

        public void Initialize(ExtensionConfigContext context)
        {
            context
                .AddBindingRule<InjectAttribute>()
                .Bind(new InjectBindingProvider(_loggerFactory));
            var filter = new ScopeCleanupFilter();
            _registry.RegisterExtension(typeof(IFunctionInvocationFilter), filter);
            _registry.RegisterExtension(typeof(IFunctionExceptionFilter), filter);
        }
    }
}
