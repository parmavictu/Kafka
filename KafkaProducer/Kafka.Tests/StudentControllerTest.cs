using AutoMapper;
using Bogus;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Kafka.Domain.Commands;
using Kafka.Domain.Core;
using Kafka.Domain.Interfaces;
using Kafka.Infra.CrossCutting.VM;
using Kafka.Services.API.Controllers;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Kafka.Tests.AutomatizationTests
{
    public class StudentControllerTests
    {
        public StudentController StudentController { get; set; }
        public StudentCreateVM StudentCreateVM { get; set; }
        public StudentCreateCommand StudentCreateCommand { get; set; }
        public Mock<IStudentRepository> MockStudentRepository { get; set; }

        public Mock<IMapper> MockMapper { get; set; }
        public Mock<IMediator> MockMediator { get; set; }
        public Mock<DomainNotificationHandler> MockNotification { get; set; }
        public StudentControllerTests()
        {
            MockMediator = new Mock<IMediator>();
            MockNotification = new Mock<DomainNotificationHandler>();
            MockStudentRepository = new Mock<IStudentRepository>();
            MockMapper = new Mock<IMapper>();

            //.RuleFor(u => u.LastName, f => f.Person.LastName)
            //.RuleFor(u => u.Email, f => f.Person.Email)
            //.RuleFor(u => u.Username, f => f.UniqueIndex + f.Person.UserName);
            StudentCreateVM = new Faker<StudentCreateVM>("pt_BR")
                   .RuleFor(u => u.Name, f => f.Person.FirstName + " " + f.Person.LastName).Generate(1).First();
            StudentCreateCommand = new StudentCreateCommand(null, StudentCreateVM.Name, StudentCreateVM.Email);

            StudentController = new StudentController(MockMediator.Object, MockNotification.Object, MockStudentRepository.Object, MockMapper.Object);
        }
        //AAA => Arrange, Act, Assert
        [Fact(DisplayName = "Register student with success")]
        [Trait("Student", "Tests Student Controller")]
        public void StudentController_CreateStudent_WithSucess()
        {
            // Arrange
            MockMapper.Setup(m => m.Map<StudentCreateCommand>(StudentCreateVM)).Returns(StudentCreateCommand);

            // Act
            var result = StudentController.Post(StudentCreateVM);

            // Assert
            MockMediator.Object.Send(StudentCreateCommand);//, Times.Once);
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact(DisplayName = "Register Student With Errors of ModelState")]
        [Trait("Category", "Tests Student Controller")]
        public void StudentController_RegisterStudent_ReturnWithErrorsOfModelState()
        {
            // Arrange
            MockMapper.Setup(m => m.Map<StudentCreateCommand>(StudentCreateVM)).Returns(StudentCreateCommand);
            var notificationList = new List<DomainNotification>
            {
                new DomainNotification("Erro","Model Error")
            };

            MockNotification.Setup(c => c.GetNotifications()).Returns(notificationList);
            MockNotification.Setup(c => c.HasNotifications()).Returns(true);

            StudentController.ModelState.AddModelError("Erro", "Model Error");

            // Act
            var result = StudentController.Post(StudentCreateVM);

            // Assert
            MockMapper.Setup(m => m.Map<StudentCreateCommand>(StudentCreateVM)).Returns(StudentCreateCommand);

            MockMediator.Object.Send(StudentCreateCommand);//, Times.Never);
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact(DisplayName = "Register Students with domain error")]
        [Trait("Category", "Testes Students Controller")]
        public void StudentsController_RegistrarEvento_RetornarComErrosDeDominio()
        {
            // Arrange
            MockMapper.Setup(m => m.Map<StudentCreateCommand>(StudentCreateCommand)).Returns(StudentCreateCommand);
            var notificationList = new List<DomainNotification>
            {
                new DomainNotification("Erro","Domain Error")
            };

            MockNotification.Setup(c => c.GetNotifications()).Returns(notificationList);
            MockNotification.Setup(c => c.HasNotifications()).Returns(true);

            // Act
            var result = StudentController.Post(StudentCreateVM);

            // Assert
            //MockMediator.Verify(m => m.Send(StudentCreateCommand), Times.Once);
            MockMediator.Object.Send(StudentCreateCommand);
            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}

