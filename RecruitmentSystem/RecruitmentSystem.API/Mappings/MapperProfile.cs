using AutoMapper;
using RecruitmentSystem.Domain.Dtos.Application;
using RecruitmentSystem.Domain.Dtos.Assessment;
using RecruitmentSystem.Domain.Dtos.Company;
using RecruitmentSystem.Domain.Dtos.Decision;
using RecruitmentSystem.Domain.Dtos.Evaluation;
using RecruitmentSystem.Domain.Dtos.FinalScore;
using RecruitmentSystem.Domain.Dtos.Internship;
using RecruitmentSystem.Domain.Dtos.Interview;
using RecruitmentSystem.Domain.Dtos.Screening;
using RecruitmentSystem.Domain.Dtos.Setting;
using RecruitmentSystem.Domain.Dtos.SiteUser;
using RecruitmentSystem.Domain.Dtos.Statistics;
using RecruitmentSystem.Domain.Dtos.Steps;
using RecruitmentSystem.Domain.Models;

namespace RecruitmentSystem.API.Mappings;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<Internship, InternshipDto>()
            .ForMember(dest => dest.CompanyDto, opt => opt.MapFrom(src => src.Company))
            .ForMember(dest => dest.SettingDto, opt => opt.MapFrom(src => src.Setting))
            .ReverseMap();

        CreateMap<Internship, InternshipCreateDto>()
            .ReverseMap();

        CreateMap<Internship, InternshipForScreeningPromptDto>()
            .ReverseMap();

        CreateMap<Company, CompanyDto>()
            .ReverseMap();

        CreateMap<Evaluation, EvaluationDto>()
            .ReverseMap();

        CreateMap<Evaluation, EvaluationCreateDto>()
            .ReverseMap();

        CreateMap<Cv, CvDto>()
            .ReverseMap();

        CreateMap<Interview, InterviewDto>()
            .ReverseMap();

        CreateMap<Assessment, AssessmentDto>()
            .ReverseMap();

        CreateMap<Step, StepDto>()
            .ForMember(step => step.StepType, opt => opt.MapFrom(s => s.StepType.ToString()))
            .ReverseMap();

        CreateMap<SiteUser, SiteUserDto>().ReverseMap();

        CreateMap<InternshipStep, ApplicationStepDto>()
            .ForMember(dest => dest.StepType, opt => opt.MapFrom(src => src.Step.StepType.ToString()))
            .ForMember(dest => dest.PositionAscending, opt => opt.MapFrom(src => src.PositionAscending));

        CreateMap<Application, ApplicationListItemDto>()
            .ForMember(app => app.SiteUserDto, opt => opt.MapFrom(src => src.SiteUser))
            .ForMember(app => app.StepName, opt => opt.MapFrom(src => src.InternshipStep.Step.StepType.ToString()))
            .ForMember(app => app.InternshipDto, opt => opt.MapFrom(src => src.Internship))
            .ReverseMap();

        CreateMap<Application, ApplicationDto>()
            .ForMember(dest => dest.InternshipDto, opt => opt.MapFrom(src => src.Internship))
            .ReverseMap();

        CreateMap<Setting, SettingDto>()
            .ReverseMap();

        CreateMap<Setting, SettingCreateDto>()
            .ReverseMap();

        CreateMap<Decision, DecisionDto>()
            .ReverseMap();

        CreateMap<StepEvaluation, LineStatisticsDto>()
            .ReverseMap();

        CreateMap<FinalScore, FinalScoreDto>()
            .ReverseMap();
    }
}