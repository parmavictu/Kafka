using Kafka.Domain.Core;
using Kafka.Domain.Validators;
using System;

namespace Kafka.Domain.Entities
{
    public class Student : Entity<Student>
    {
        private Student() { }

        public Student(string id, string name, string email)
        {
            Id = id;
            Name = name;
            Email = email;
        }
        public string Name { get; protected set; }
        public string Email { get; protected set; }

        public override bool IsValid()
        {
            Validate();
            return ValidationResult.IsValid;
        }

        private void Validate()
        {
            ValidationResult = new StudentValidator()
                .Validate(this);
        }
        public static class StudentFactory
        {
            public static Student NewStudent(string id, string name, string email)
            {
                var student = new Student()
                {
                    Id = id,
                    Name = name,
                    Email = email
                };

                return student;
            }
        }
    }
}