using Microsoft.Extensions.DependencyInjection;
using Kafka.Domain.Bases;
using Kafka.Infra.CrossCutting.Identity.Models;
using Kafka.Infra.CrossCutting.Identity.TokenConfigurations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kafka.Infra.CrossCutting.Identity.IoC
{
    public static class DependenciesIdentity
    {
        public static void RegisterDependencies(IServiceCollection services)
        {
            services.AddScoped<IUser, AspNetUser>();
            services.AddScoped<Token>();

        }
    }
}
