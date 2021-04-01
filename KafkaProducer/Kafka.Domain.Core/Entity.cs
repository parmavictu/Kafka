using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kafka.Domain.Core
{
    public abstract class Entity<T> where T : class
    {
        public string Id { get; protected set; }
        public abstract bool IsValid();
        public ValidationResult ValidationResult { get; protected set; }
    }
}
