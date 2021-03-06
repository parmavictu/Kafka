using Kafka.Domain.Core;

namespace Kafka.Domain.Commands
{
    public class StudentUpdateCommand : Command
    {
        public StudentUpdateCommand(string id, string name, string email)
        {
            Id = id;
            Name = name;
            Email = email;
        }
        public string Id { get; protected set; }
        public string Name { get; protected set; }
        public string Email { get; protected set; }

    }
}