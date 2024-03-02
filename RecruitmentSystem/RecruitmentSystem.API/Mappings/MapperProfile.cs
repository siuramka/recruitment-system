using AutoMapper;
using RecruitmentSystem.Domain.Dtos.Application;
using RecruitmentSystem.Domain.Dtos.Company;
using RecruitmentSystem.Domain.Dtos.Internship;
using RecruitmentSystem.Domain.Models;

namespace RecruitmentSystem.API.Mappings;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<Internship, InternshipDto>()
            .ForMember(dest => dest.CompanyDto, opt => opt.MapFrom(src => src.Company))
            .ReverseMap();

        CreateMap<Internship, InternshipCreateDto>().ReverseMap();

        CreateMap<Company, CompanyDto>().ReverseMap();
        CreateMap<Application, ApplicationDto>().ReverseMap();
        
        CreateMap<InternshipStep, ApplicationStepDto>()
            .ForMember(dest => dest.StepType, opt => opt.MapFrom(src => src.Step.StepType.ToString()))
            .ForMember(dest => dest.PositionAscending, opt => opt.MapFrom(src => src.PositionAscending));

    }
}