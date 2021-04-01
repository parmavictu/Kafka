using Microsoft.Extensions.DependencyInjection;
using Kafka.Domain.Interfaces;
using Kafka.Infra.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kafka.Infra.Data.IoC
{
    public static class DependenciesData
    {
        public static void RegisterDependencies(IServiceCollection services)
        {
            services.AddScoped<IStudentRepository, StudentRepository>();
        }
    }
}
