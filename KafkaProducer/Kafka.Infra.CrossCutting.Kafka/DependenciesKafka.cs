
using Microsoft.Extensions.DependencyInjection;

namespace Kafka.Infra.CrossCutting.Kafka
{
    public static class DependenciesKafka
    {
        public static void RegisterDependencies(IServiceCollection services)
        {
            services.AddScoped<IKafka, Kafka>();
        }
    }
}
