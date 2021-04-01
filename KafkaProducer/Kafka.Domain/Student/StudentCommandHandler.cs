using MediatR;
using Kafka.Domain.Entities;
using Kafka.Domain.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Kafka.Domain.Core;
using Kafka.Domain.Commands;
using Kafka.Domain.Events;
using Kafka.Domain.Bases;

namespace Kafka.Domain.CommandsHandler
{
    public class StudentCommandHandler : CommandHandler,
        IRequestHandler<StudentCreateCommand>,
        IRequestHandler<StudentUpdateCommand>
    {
        private readonly IMediator _mediator;
        private readonly IStudentRepository _studentRepository;

        public StudentCommandHandler(IMediator mediator, IStudentRepository studentRepository, INotificationHandler<DomainNotification> notifications)
            : base(mediator, notifications)
        {
            _mediator = mediator;
            _studentRepository = studentRepository;
        }
        public async Task<Unit> Handle(StudentCreateCommand request, CancellationToken cancellationToken)
        {
            var foundStudent = _studentRepository.GetAll().FirstOrDefault(s => s.Name == request.Name);
            if (foundStudent != null)
            {
                await _mediator.Publish(new DomainNotification(request.MessageType, "Aluno já existe!"));
                return await Unit.Task;
            }
            var student = new Student(request.Id, request.Name, request.Email);

            if (!student.IsValid())
            {
                NotifyValidationErrors(student.ValidationResult);
                return await Unit.Task;
            }

            await _studentRepository.AddASync(student);
            await _mediator.Publish(new StudentCreatedEvent(student.Id, student.Name), CancellationToken.None);
            return await Unit.Task;
        }
        public async Task<Unit> Handle(StudentUpdateCommand request, CancellationToken cancellationToken)
        {
            var student = await StudentExists(request.Id, request.MessageType);

            if (student == null)
            {
                return await Unit.Task;
            }
            student = Student.StudentFactory.NewStudent(request.Id, request.Name, request.Email);

            if (!student.IsValid())
            {
                NotifyValidationErrors(student.ValidationResult);
                return await Unit.Task;
            }
            await _studentRepository.UpdateAsync(s => s.Id == request.Id, student);
            await _mediator.Publish(new StudentUpdatedEvent(student.Id, student.Name), CancellationToken.None);

            return await Unit.Task;
        }
        private async Task<Student> StudentExists(string id, string messageType)
        {
            var student = (await _studentRepository.FindAsync(s => s.Id == id)).FirstOrDefault();

            if (student != null) return student;

            await _mediator.Publish(new DomainNotification(messageType, "Aluno não encontrado."));
            return null;
        }
    }
}
