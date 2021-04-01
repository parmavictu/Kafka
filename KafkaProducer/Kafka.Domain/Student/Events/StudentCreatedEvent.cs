using Kafka.Domain.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kafka.Domain.Events
{
    public class StudentCreatedEvent : Event
    {
        public StudentCreatedEvent(string id, string nome)
        {
            Id = id;
            Nome = nome;
        }
        public string Id { get; set; }
        public string Nome { get; set; }
    }
}
