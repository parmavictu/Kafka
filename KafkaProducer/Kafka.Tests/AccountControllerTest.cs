using AutoMapper;
using Bogus;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Kafka.Domain.Commands;
using Kafka.Domain.Core;
using Kafka.Domain.Interfaces;
using Kafka.Infra.CrossCutting.Identity.Models;
using Kafka.Infra.CrossCutting.Identity.Models.AccountViewModels;
using Kafka.Infra.CrossCutting.Identity.TokenConfigurations;
using Kafka.Infra.CrossCutting.VM;
using Kafka.Services.API.Controllers;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Kafka.Tests.AutomatizationTests
{
    public class AccountControllerTest
    {
        public AccountController AccountController { get; set; }
        public RegisterViewModel RegisterVM { get; set; }
        public StudentCreateCommand StudentCreateCommand { get; set; }
        public Mock<UserManager<ApplicationUser>> MockUserManager { get; set; }
        public Mock<SignInManager<ApplicationUser>> MockSignInManager { get; set; }
        public Mock<LoggerFactory> MockFactoryLogger { get; set; }
        public Mock<ILogger> MockLogger { get; set; }
        public Mock<Token> MockToken { get; set; }

        public Mock<IStudentRepository> MockStudentRepository { get; set; }
        public Mock<IMapper> MockMapper { get; set; }
        public Mock<IMediator> MockMediator { get; set; }
        public Mock<DomainNotificationHandler> MockNotification { get; set; }
        public AccountControllerTest()
        {
            var serviceProvider = new ServiceCollection()
                   .AddLogging()
                   .BuildServiceProvider();

            MockMediator = new Mock<IMediator>();
            MockNotification = new Mock<DomainNotificationHandler>();
            MockStudentRepository = new Mock<IStudentRepository>();
            MockNotification = new Mock<DomainNotificationHandler>();
            MockFactoryLogger = new Mock<LoggerFactory>();
            MockUserManager = new Mock<UserManager<ApplicationUser>>();
            MockSignInManager = new Mock<SignInManager<ApplicationUser>>();

            RegisterVM = new Faker<RegisterViewModel>("pt_BR")
                   .RuleFor(u => u.Name, f => f.Person.FirstName + " " + f.Person.LastName).Generate(1).First();
            StudentCreateCommand = new StudentCreateCommand(null, RegisterVM.Name, RegisterVM.Email);

            AccountController = new AccountController(MockUserManager.Object, MockSignInManager.Object, MockFactoryLogger.Object, MockStudentRepository.Object, MockToken.Object, MockNotification.Object, MockMediator.Object);
        }
        //AAA => Arrange, Act, Assert
        [Fact(DisplayName = "Register student with success")]
        [Trait("Student", "Tests Student Controller")]
        public void AccountController_CreateStudent_WithSucess()
        {
            // Arrange
            MockMapper.Setup(m => m.Map<StudentCreateCommand>(RegisterVM)).Returns(StudentCreateCommand);

            // Act
            var result = AccountController.Register(RegisterVM);

            // Assert
            MockMediator.Object.Send(StudentCreateCommand);//, Times.Once);
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact(DisplayName = "Register Student With Errors of ModelState")]
        [Trait("Category", "Tests Student Controller")]
        public void AccountController_RegisterStudent_ReturnWithErrorsOfModelState()
        {
            // Arrange
            MockMapper.Setup(m => m.Map<StudentCreateCommand>(RegisterVM)).Returns(StudentCreateCommand);
            var notificationList = new List<DomainNotification>
            {
                new DomainNotification("Erro","Model Error")
            };

            MockNotification.Setup(c => c.GetNotifications()).Returns(notificationList);
            MockNotification.Setup(c => c.HasNotifications()).Returns(true);

            AccountController.ModelState.AddModelError("Erro", "Model Error");

            // Act
            var result = AccountController.Register(RegisterVM);

            // Assert
            MockMapper.Setup(m => m.Map<StudentCreateCommand>(RegisterVM)).Returns(StudentCreateCommand);

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
            var result = AccountController.Register(RegisterVM);

            // Assert
            //MockMediator.Verify(m => m.Send(StudentCreateCommand), Times.Once);
            MockMediator.Object.Send(StudentCreateCommand);
            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}

