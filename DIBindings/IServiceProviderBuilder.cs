using System;
using System.Collections.Generic;
using System.Text;

namespace DIBindings
{
    public interface IServiceProviderBuilder
    {
        IServiceProvider BuildServiceProvider();
    }
}
