using FluentValidation.Results;
using MediatR;
using Kafka.Domain.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kafka.Domain.Bases
{
    public abstract class CommandHandler
    {
        //private readonly IUnitOfWork _uow;
        private readonly IMediator _mediator;
        private readonly DomainNotificationHandler _notifications;

        protected CommandHandler(IMediator mediator, INotificationHandler<DomainNotification> notifications)
        {
            _mediator = mediator;
            _notifications = (DomainNotificationHandler)notifications;
        }

        protected void NotifyValidationErrors(ValidationResult validationResult)
        {
            foreach (var error in validationResult.Errors)
            {
                _mediator.Publish(new DomainNotification(error.PropertyName, error.ErrorMessage));
            }
        }

        protected bool Commit()
        {
            if (_notifications.HasNotifications()) return false;
            return true;
        }
    }
}
