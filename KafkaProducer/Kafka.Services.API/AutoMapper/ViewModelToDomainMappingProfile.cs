using System;
using AutoMapper;
using Kafka.Domain.Commands;
using Kafka.Infra.CrossCutting.VM;

namespace Kafka.Services.API.AutoMapper
{
    public class ViewModelToDomainMappingProfile : Profile
    {
        public ViewModelToDomainMappingProfile()
        {
            // Student
            CreateMap<StudentCreateVM, StudentCreateCommand>()
                .ConstructUsing(s => new StudentCreateCommand(null, s.Name, s.Email));
            CreateMap<StudentUpdateVM, StudentUpdateCommand>()
                .ConstructUsing(s => new StudentUpdateCommand(s.Id, s.Name, s.Email));
        }
    }
}