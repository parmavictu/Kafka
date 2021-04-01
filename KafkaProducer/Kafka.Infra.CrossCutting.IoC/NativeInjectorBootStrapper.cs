using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Kafka.Domain.IoC;
using Kafka.Infra.CrossCutting.Identity.IoC;
using Kafka.Infra.Data.IoC;
using Kafka.Infra.CrossCutting.Kafka;

namespace Kafka.Infra.CrossCutting.IoC
{
    public class NativeInjectorBootStrapper
    {
        public static void RegisterServices(IServiceCollection services)
        {
            DependenciesAPI.RegisterDependencies(services);
            DependenciesData.RegisterDependencies(services);
            DependenciesDomain.RegisterDependencies(services);
            DependenciesIdentity.RegisterDependencies(services);
            DependenciesKafka.RegisterDependencies(services);
        }
    }
    public static class DependenciesAPI
    {
        public static void RegisterDependencies(IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IMapper>(sp => new Mapper(sp.GetRequiredService<IConfigurationProvider>(), sp.GetService));
        }
    }
}
