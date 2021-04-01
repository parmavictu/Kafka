using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Kafka.Domain.CommandsHandler;
using Kafka.Domain.Core;
using System.Reflection;

namespace Kafka.Domain.IoC
{
    public static class DependenciesDomain
    {
        public static void RegisterDependencies(IServiceCollection services)
        {
            services.AddScoped<IMediator, Mediator>();
            services.AddScoped<INotificationHandler<DomainNotification>, DomainNotificationHandler>();
            services.AddMediatR(typeof(StudentCommandHandler).GetTypeInfo().Assembly);
        }
    }
}
