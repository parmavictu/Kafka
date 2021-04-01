using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Kafka.Domain.Commands;
using Kafka.Domain.Core;
using Kafka.Domain.Interfaces;
using Kafka.Infra.CrossCutting.Identity.Authorization;
using Kafka.Infra.CrossCutting.Identity.Models;
using Kafka.Infra.CrossCutting.Identity.Models.AccountViewModels;
using Kafka.Infra.CrossCutting.Identity.TokenConfigurations;

namespace Kafka.Services.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : BaseController

    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger _logger;
        private readonly IStudentRepository _studentRepository;
        private readonly IMediator _mediator;
        private readonly Token _token;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILoggerFactory loggerFactory,
            IStudentRepository studentRepository,
            Token token,
            INotificationHandler<DomainNotification> notificationHandler,
            IMediator mediator)
            : base(notificationHandler, mediator)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _studentRepository = studentRepository;
            _token = token;
            _mediator = mediator;
            _logger = loggerFactory.CreateLogger<AccountController>();
        }


        [HttpPost]
        [AllowAnonymous]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                NotifyErrorOfInvalidModel();
                return Response();
            }

            var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await _userManager.AddClaimAsync(user, new Claim("Students", "Read"));
                await _userManager.AddClaimAsync(user, new Claim("Students", "Write"));

                var registroCommand = new StudentCreateCommand(null, model.Name, model.Email);
                await _mediator.Send(registroCommand);

                if (!ValidOperation())
                {
                    await _userManager.DeleteAsync(user);
                    return Response(model);
                }

                _logger.LogInformation(1, "Usuario criado com sucesso!");
                var student = (await _studentRepository.FindAsync(s => s.Email == model.Email)).FirstOrDefault();
                var response = await _token.GenerateUserToken(new LoginViewModel { Email = model.Email, Password = model.Password }, student);
                return Response(response);
            }
            AddErrorsIdentity(result);
            return Response(model);
        }
        [HttpPost]
        [AllowAnonymous]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                NotifyErrorOfInvalidModel();
                return Response(model);
            }
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, true);

            if (result.Succeeded)
            {
                _logger.LogInformation(1, "Usuario logado com sucesso {user}", JsonConvert.SerializeObject(model));
                var student = (await _studentRepository.FindAsync(s => s.Email == model.Email)).FirstOrDefault();

                var response = await _token.GenerateUserToken(model, student);
                return Response(response);
            }
            NotifyError(result.ToString(), "Falha ao realizar o login");

            return new UnauthorizedResult();
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return Response("UserId or Code is null");

            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{userId}'.");
            }

            var result = await _userManager.ConfirmEmailAsync(user, code);
            _logger.LogInformation("ConfirmEmail userId: {userId} code:{code} result: {result}", userId, code, result);



            return Redirect(Environment.GetEnvironmentVariable("RENOVACAO_FRONTEND_URL") + "emailConfirmation");
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("Logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return Response("Logout efetuado!");

        }

        [HttpPost("ForgetPassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = await _userManager.FindByEmailAsync(model.Email);
                    if (user == null)
                    {
                        NotifyError(ModelState.ToString(), "Aluno não encontrado.");
                    }
                    else
                    {
                        var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                        var callbackUrl = $"{Environment.GetEnvironmentVariable("URL")}change-password?code={HttpUtility.UrlEncode(code)}";

                        //var student = (await _studentRepository.FindAsync(x => x.AspNetUserId == user.Id))?.First();
                        //await SendEmailWithBrand(new RegisterViewModel { Cpf = student.CpfContact, Email = student.EmailContact }, callbackUrl, "Resetar Senha", true);

                    }

                }
                else
                {
                    _logger.LogInformation(1, "Falha ao tentar redefinir a senha.");

                    NotifyErrorOfInvalidModel();

                }
            }
            catch (Exception x)
            {
                _logger.LogInformation("Ocorreu um erro ao tentar processar a recuperação de email {exception}", x);

                throw x;
            }

            return Response();
        }

        private static string CreateChangePasswordMessage(string name, string email, string callbackUrl)
        {
            return string.Format("<h1>Recupere seu acesso</h1><br /><br />Olá {0},<br /><br />" +
                                            "Seguem as informações solicitadas para recuperar seu acesso:<br /><br />" +
                                            "Login: {1}<br />Clique aqui: <a href='{2}'>{2}</a> <br /><br />Por motivos de segurança, o link acima só será válido " +
                                            "durante as próximas 24 horas.<br /><br />Se você não deseja redefinir sua senha ou não solicitou " +
                                            "estas informações, pode ignorar este e-mail com segurança.<br />", name, email, callbackUrl);
        }

        [HttpPost("ResetPassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                NotifyErrorOfInvalidModel();
            }
            else
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    NotifyError(ModelState.ToString(), "Aluno não encontrado.");
                }
                else
                {
                    var code = await _userManager.GeneratePasswordResetTokenAsync(user);

                    var result = await _userManager.ResetPasswordAsync(user, code, model.Password);

                    _logger.LogInformation("ResetPassword mensagem: {result} model:{model}", result, JsonConvert.SerializeObject(model));

                    if (!result.Succeeded)
                    {
                        _logger.LogInformation(1, "Falha ao tentar alterar a senha");

                        NotifyError(result.ToString(), "Falha ao tentar alterar a senha, verifique se a senha informada atendem os requisitos.");
                    }
                }
            }

            return Response();

        }
    }
}
