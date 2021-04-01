using System.Linq;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Kafka.Domain.Bases;
using Kafka.Domain.Core;

namespace Kafka.Services.API.Controllers
{
    [Produces("application/json")]
    public class BaseController : ControllerBase
    {
        private readonly DomainNotificationHandler _notifications;
        private readonly IMediator _mediator;

        protected BaseController(INotificationHandler<DomainNotification> notifications,
                                 //IUser user,
                                 IMediator mediator)
        {
            _notifications = (DomainNotificationHandler)notifications;
            _mediator = mediator;

        }

        protected new IActionResult Response(object result = null)
        {
            if (ValidOperation())
            {
                return Ok(new
                {
                    success = true,
                    data = result
                });
            }

            return BadRequest(new
            {
                success = false,
                errors = _notifications.GetNotifications().Select(n => n.Value)
            });
        }

        protected bool ValidOperation()
        {
            return (!_notifications.HasNotifications());
        }

        protected void NotifyErrorOfInvalidModel()
        {
            var erros = ModelState.Values.SelectMany(v => v.Errors);
            foreach (var erro in erros)
            {
                var erroMsg = erro.Exception == null ? erro.ErrorMessage : erro.Exception.Message;
                NotifyError(string.Empty, erroMsg);
            }
        }

        protected void NotifyError(string codigo, string mensagem)
        {
            _mediator.Publish(new DomainNotification(codigo, mensagem));
        }

        protected void AddErrorsIdentity(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                NotifyError(result.ToString(), error.Description);
            }
        }
        protected bool ModelStateValid()
        {
            if (ModelState.IsValid) return true;

            NotifyErrorOfInvalidModel();
            return false;
        }
    }
}
