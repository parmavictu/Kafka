using FluentValidation;
using Kafka.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kafka.Domain.Validators
{
    public class StudentValidator : AbstractValidator<Student>
    {
        public StudentValidator()
        {

            RuleFor(c => c.Name)
                .NotEmpty().WithMessage("O nome precisa ser fornecido.")
                .Length(2, 150).WithMessage("O nome precisa ter entre 2 e 150 caracteres");
        }
    }
}
