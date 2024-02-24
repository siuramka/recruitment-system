using AutoMapper;
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

        CreateMap<Company, CompanyDto>().ReverseMap();
    }
}