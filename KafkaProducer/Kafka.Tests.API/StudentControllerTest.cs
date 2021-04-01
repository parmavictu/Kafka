using AutoMapper;
using Bogus;
using FizzWare.NBuilder;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Kafka.Domain.Commands;
using Kafka.Domain.Core;
using Kafka.Infra.CrossCutting.VM;
using Kafka.Infra.Repositories;
using Kafka.Services.API.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Kafka.Tests.AutomatizationTests
{
    public class StudentControllerTests
    {
        public StudentController studentController { get; set; }
        public StudentCreateVM studentCreateVM { get; set; }
        public StudentCreateCommand studentCreateCommand { get; set; }
        public Mock<StudentRepository> mockStudentRepository { get; set; }

        public Mock<IMapper> mockMapper { get; set; }
        public Mock<IMediator> mockMediator { get; set; }
        public Mock<DomainNotificationHandler> mockNotification { get; set; }
        public StudentControllerTests()
        {
            mockMediator = new Mock<IMediator>();
            mockNotification = new Mock<DomainNotificationHandler>();
            //.RuleFor(u => u.LastName, f => f.Person.LastName)
            //.RuleFor(u => u.Email, f => f.Person.Email)
            //.RuleFor(u => u.Username, f => f.UniqueIndex + f.Person.UserName);
            studentCreateVM = new Faker<StudentCreateVM>()
                   .RuleFor(u => u.Name, f => f.Person.FirstName + " " + f.Person.LastName).Generate(1).First();
            studentCreateCommand = new Faker<StudentCreateCommand>()
                    .RuleFor(u => u.Name, f => f.Person.FirstName + " " + f.Person.LastName).Generate(1).First();

            studentController = new StudentController(mockMediator.Object, mockNotification.Object, mockStudentRepository.Object, mockMapper.Object);
        }
        //AAA => Arrange, Act, Assert
        [Fact(DisplayName = "Register student with success")]
        [Trait("Student", "Tests Student Controller")]
        public void StudentController_CreateStudent_WithSucess()
        {
            // Arrange
            mockMapper.Setup(m => m.Map<StudentCreateCommand>(studentCreateVM)).Returns(studentCreateCommand);

            // Act
            var result = studentController.Post(studentCreateVM);

            // Assert
            mockMediator.Object.Send(studentCreateCommand);//, Times.Once);
            Assert.IsType<OkObjectResult>(result);
        }

        //[Fact(DisplayName = "Registrar evento com erro de ModelState")]
        //[Trait("Category", "Testes Eventos Controller")]
        //public void EventosController_RegistrarEvento_RetornarComErrosDeModelState()
        //{
        //    // Arrange
        //    mockMapper.Setup(m => m.Map<StudentCreateCommand>(studentVM)).Returns(studentCreateCommand);
        //    var notificationList = new List<DomainNotification>
        //    {
        //        new DomainNotification("Erro","Model Error")
        //    };

        //    mockNotification.Setup(c => c.GetNotifications()).Returns(notificationList);
        //    mockNotification.Setup(c => c.HasNotifications()).Returns(true);

        //    eventosController.ModelState.AddModelError("Erro", "Model Error");

        //    // Act
        //    var result = eventosController.Post(studentVM);

        //    // Assert
        //    mockMapper.Verify(m => m.Map<StudentCreateCommand>(studentVM), Times.Never);
        //    mockMediator.Verify(m => m.EnviarComando(studentCreateCommand), Times.Never);
        //    Assert.IsType<BadRequestObjectResult>(result);
        //}

        //[Fact(DisplayName = "Registrar evento com erro de Dominio")]
        //[Trait("Category", "Testes Eventos Controller")]
        //public void EventosController_RegistrarEvento_RetornarComErrosDeDominio()
        //{
        //    // Arrange
        //    mockMapper.Setup(m => m.Map<StudentCreateCommand>(studentVM)).Returns(studentCreateCommand);
        //    var notificationList = new List<DomainNotification>
        //    {
        //        new DomainNotification("Erro","Domain Error")
        //    };

        //    mockNotification.Setup(c => c.GetNotifications()).Returns(notificationList);
        //    mockNotification.Setup(c => c.HasNotifications()).Returns(true);

        //    // Act
        //    var result = eventosController.Post(studentVM);

        //    // Assert
        //    mockMediator.Verify(m => m.EnviarComando(studentCreateCommand), Times.Once);
        //    Assert.IsType<BadRequestObjectResult>(result);
        //}
    }
}

