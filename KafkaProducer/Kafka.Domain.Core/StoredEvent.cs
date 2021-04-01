using System;
using System.Collections.Generic;
using System.Text;

namespace Kafka.Domain.Core
{
    public class StoredEvent : Event
    {
        public StoredEvent(Event eventExecuted, string data, string user)
        {
            Id = Guid.NewGuid();
            AggregateId = eventExecuted.AggregateId;
            MessageType = eventExecuted.MessageType;
            Data = data;
            User = user;
        }

        // EF Constructor
        protected StoredEvent() { }

        public Guid Id { get; private set; }

        public string Data { get; private set; }

        public string User { get; private set; }
    }
}
