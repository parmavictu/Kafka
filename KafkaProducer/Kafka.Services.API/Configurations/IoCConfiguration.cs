using Microsoft.Extensions.DependencyInjection;
using Kafka.Infra.CrossCutting.IoC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kafka.Services.API.Configuration
{
    public static class IoCConfiguration
    {
        public static void AddDIConfiguration(this IServiceCollection services)
        {
            NativeInjectorBootStrapper.RegisterServices(services);
        }
    }
}
