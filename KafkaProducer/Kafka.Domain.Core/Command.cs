using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kafka.Domain.Core
{
    public class Command : Message, IRequest
    {
        public DateTime Timestamp { get; private set; }

        public Command()
        {
            Timestamp = DateTime.Now;
        }
    }
}
