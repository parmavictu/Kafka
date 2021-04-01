using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Kafka.Domain.Core
{
    public interface IMediatorHandler 
    {
        Task PublishEvent<T>(T newEvent) where T : Event;
        Task SendCommand<T>(T newCommand) where T : Command;
    }
}
