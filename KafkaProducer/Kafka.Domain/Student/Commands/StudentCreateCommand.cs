using Kafka.Domain.Core;

namespace Kafka.Domain.Commands
{
    public class StudentCreateCommand : Command
    {
        public StudentCreateCommand(string id, string name, string email)
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