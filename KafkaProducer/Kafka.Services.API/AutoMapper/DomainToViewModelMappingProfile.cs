using AutoMapper;
using Kafka.Domain.Entities;
using Kafka.Infra.CrossCutting.VM;

namespace Kafka.Services.API.AutoMapper
{
    public class DomainToViewModelMappingProfile : Profile
    {
        public DomainToViewModelMappingProfile()
        {
            CreateMap<Student, StudentListVM>().ReverseMap();
            CreateMap<Student, StudentCreateVM>().ReverseMap();
            CreateMap<Student, StudentUpdateVM>().ReverseMap();
        }
    }
}