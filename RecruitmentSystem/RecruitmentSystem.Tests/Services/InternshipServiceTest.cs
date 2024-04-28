using AutoMapper;
using Moq;
using Moq.EntityFrameworkCore;
using RecruitmentSystem.API.Mappings;
using RecruitmentSystem.Business.Services;
using RecruitmentSystem.DataAccess;
using RecruitmentSystem.Domain.Dtos.Internship;
using RecruitmentSystem.Domain.Dtos.Setting;
using RecruitmentSystem.Domain.Dtos.Steps;
using RecruitmentSystem.Domain.Models;

namespace RecruitmentSystem.Tests.Services;

[TestFixture]
[TestOf(typeof(InternshipService))]
public class InternshipServiceTest
{
    private IInternshipService _internshipService;
    private Mock<RecruitmentDbContext> _db;

    [SetUp]
    public void Setup()
    {
        _db = new Mock<RecruitmentDbContext>();

        var myProfile = new MapperProfile();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
        var mapper = new Mapper(configuration);

        _internshipService = new InternshipService(_db.Object, mapper);
    }

    [Test]
    public async Task CreateInternshipAsync_ShouldCreateInternship()
    {
        var internshipCreateDto = new InternshipCreateDto
        {
            Name = "Hey",
            ContactEmail = "test",
            Address = "test",
            Description = "Internship Description",
            Requirements = "test",
            IsPaid = false,
            IsRemote = false,
            SlotsAvailable = 1,
            Skills = "test",
            SettingCreateDto = new SettingCreateDto(),
            InternshipStepDtos = new List<InternshipCreateStepDto>
            {
                new()
                {
                    StepType = "Screening"
                },
                new()
                {
                    StepType = "Interview"
                }
            }
        };
        
        var companys = new List<Company>()
        {
            new Company
            {
                Id = Guid.NewGuid(),
                Name = "testCompany",
                SiteUser = new SiteUser { Id = Guid.NewGuid().ToString() }
            }
        };

        var steps = new List<Step>
        {
            new() { Id = Guid.NewGuid(), StepType = StepType.Screening },
            new() { Id = Guid.NewGuid(), StepType = StepType.Interview },
        };

        _db.Setup(d => d.Internships)
            .ReturnsDbSet(new List<Internship>(){});
        
        _db.Setup(d => d.Companys)
            .ReturnsDbSet(companys);

        _db.Setup(d => d.Steps)
            .ReturnsDbSet(steps);

        var result = await _internshipService.CreateInternshipAsync(internshipCreateDto, companys[0].SiteUser.Id);

        Assert.That(result.Name, Is.EqualTo(internshipCreateDto.Name));
    }

    [Test]
    public async Task GetAllInternshipsAsDtoAsync_ShouldReturnInternshipsAsDtos()
    {
        var internships = new List<Internship>
        {
            new Internship
            {
                Id = Guid.NewGuid(),
                Name = "Hey",
                ContactEmail = "test",
                Address = "test",
                Description = "Internship Description",
                Requirements = "test",
                IsPaid = false,
                IsRemote = false,
                SlotsAvailable = 1,
                Skills = "test",
                Setting = new Setting(),
                InternshipSteps = new List<InternshipStep>
                {
                    new InternshipStep
                    {
                        Step = new Step { StepType = StepType.Screening }
                    },
                    new InternshipStep
                    {
                        Step = new Step { StepType = StepType.Interview }
                    }
                }
            }
        };
        
        _db.Setup(d => d.Internships)
            .ReturnsDbSet(internships);
        
        var result = await _internshipService.GetAllInternshipsAsDtoAsync();
        
        Assert.That(result.Count, Is.EqualTo(internships.Count));
        StringAssert.AreEqualIgnoringCase(internships[0].Name, result[0].Name);
    }

    [Test]
    public async Task GetInternshipByIdIncludeCompany_ShouldReturnInternship()
    {
        var internship = new Internship
        {
            Id = Guid.NewGuid(),
            Name = "Hey",
            ContactEmail = "test",
            Address = "test",
            Description = "Internship Description",
            Requirements = "test",
            IsPaid = false,
            IsRemote = false,
            SlotsAvailable = 1,
            Skills = "test",
            Setting = new Setting(),
            InternshipSteps = new List<InternshipStep>
            {
                new InternshipStep
                {
                    Step = new Step { StepType = StepType.Screening }
                },
                new InternshipStep
                {
                    Step = new Step { StepType = StepType.Interview }
                }
            },
            Company = new Company()
            {
                Id = Guid.NewGuid(),
                Name = "TestCompany",
            }
        };
        
        _db.Setup(d => d.Internships)
            .ReturnsDbSet(new List<Internship>{internship});
        
        var result = await _internshipService.GetInternshipByIdIncludeCompany(internship.Id);
        
        Assert.That(result.Company.Name, Is.EqualTo(internship.Company.Name));
    }

    [Test]
    public async Task GetAllInternshipsAsDtoOfCompanyAsync_ShouldReturnInternshipsAsDtos()
    {
        var companies = new List<Company>()
        {
            new Company
            {
                Id = Guid.NewGuid(),
                Name = "TestCompany",
                SiteUser = new SiteUser { Id = Guid.NewGuid().ToString() }
            }
        };
        
        var internships = new List<Internship>
        {
            new Internship
            {
                Id = Guid.NewGuid(),
                Name = "Hey",
                ContactEmail = "test",
                Address = "test",
                Description = "Internship Description",
                Requirements = "test",
                IsPaid = false,
                IsRemote = false,
                SlotsAvailable = 1,
                Skills = "test",
                Setting = new Setting(),
                InternshipSteps = new List<InternshipStep>
                {
                    new InternshipStep
                    {
                        Step = new Step { StepType = StepType.Screening }
                    },
                    new InternshipStep
                    {
                        Step = new Step { StepType = StepType.Interview }
                    }
                },
                CompanyId = companies[0].Id,
            }
        };
        
        _db.Setup(d => d.Internships)
            .ReturnsDbSet(internships);

        _db.Setup(d => d.Companys).ReturnsDbSet(companies);
        
        var result = await _internshipService.GetAllInternshipsAsDtoOfCompanyAsync(companies[0].SiteUser.Id);
        
        Assert.That(result.Count, Is.EqualTo(internships.Count));
        StringAssert.AreEqualIgnoringCase(internships[0].Name, result[0].Name);
    }
    
}