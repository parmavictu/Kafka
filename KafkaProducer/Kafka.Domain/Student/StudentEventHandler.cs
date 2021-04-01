using MediatR;
using Kafka.Domain.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using Kafka.Domain.Events;
using System;
using Kafka.Domain.Entities;
using Kafka.Infra.CrossCutting.Kafka;
using Newtonsoft.Json;

namespace Kafka.Domain.EventHandler
{
    public class StudentEventHandler : 
        INotificationHandler<StudentCreatedEvent>,
        INotificationHandler<StudentUpdatedEvent>
    {
        private readonly IMediator _mediator;
        private readonly IStudentRepository _studentRepository;
        private readonly IKafka _kafka;

        public StudentEventHandler(IMediator mediator, IStudentRepository studentRepository,IKafka kafka )
        {
            _mediator = mediator;
            _studentRepository = studentRepository;
            _kafka = kafka;
        }
        public async Task Handle(StudentCreatedEvent notification, CancellationToken cancellationToken)
        {
            //FAZER QLQ COISA DPS DE ADICIONAR O ALUNO
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Aluno {notification.Nome} criado com sucesso!");
            Console.ForegroundColor = ConsoleColor.White;
            _kafka.SendMessageByKafka(JsonConvert.SerializeObject(notification));
            await Task.FromResult($"Aluno {notification.Nome} criado com sucesso!");
            
        }

        public async Task Handle(StudentUpdatedEvent notification, CancellationToken cancellationToken)
        {
            //FAZER QLQ COISA DPS DE ADICIONAR O ALUNO
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Aluno {notification.Nome} atualizado com sucesso!");
            Console.ForegroundColor = ConsoleColor.White;
            _kafka.SendMessageByKafka(JsonConvert.SerializeObject(notification));
            await Task.FromResult($"Aluno {notification.Nome} atualizado com sucesso!");
        }
    }
}
