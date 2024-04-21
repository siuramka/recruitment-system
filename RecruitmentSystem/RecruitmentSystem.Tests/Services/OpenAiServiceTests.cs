using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Moq;
using Moq.EntityFrameworkCore;
using Newtonsoft.Json;
using OpenAI_API;
using OpenAI_API.Chat;
using RecruitmentSystem.Business.Interfaces;
using RecruitmentSystem.Business.Services;
using RecruitmentSystem.DataAccess;
using RecruitmentSystem.Domain.Dtos.OpenAi;
using RecruitmentSystem.Domain.Models;

namespace RecruitmentSystem.Tests.Services;

[TestFixture]
public class OpenAiServiceTests
{
    private Mock<IConfiguration> _configuration;
    private Mock<RecruitmentDbContext> _db;
    private Mock<IPdfService> _pdfService;
    private Mock<IMapper> _mapper;

    [SetUp]
    public void SetUp()
    {
        _configuration = new Mock<IConfiguration>();
        _db = new Mock<RecruitmentDbContext>();
        _pdfService = new Mock<IPdfService>();
        _mapper = new Mock<IMapper>();
    }

    [Test]
    public async Task GenerateScreeningPrompt_ShouldGeneratePrompt_ReturnsPrompt()
    {
        var cvs = new List<Cv>
        {
            new Cv
            {
                InternshipId = Guid.NewGuid(),
                ApplicationId = Guid.NewGuid(),
                FileName = "testfile.pdf",
                FileContent =
                    [],
                EvaluationId = Guid.NewGuid()
            },
        };

        var applicaitons = new List<Application>
        {
            new Application
            {
                Id = cvs[0].ApplicationId,
                InternshipId = cvs[0].InternshipId,
            }
        };

        var internships = new List<Internship>
        {
            new Internship
            {
                Id = cvs[0].InternshipId,
            }
        };

        var service = new OpenAiService(_configuration.Object, _db.Object, _pdfService.Object, _mapper.Object);

        _db.Setup(x => x.Cvs).ReturnsDbSet(cvs);
        _db.Setup(x => x.Applications).ReturnsDbSet(applicaitons);
        _db.Setup(x => x.Internships).ReturnsDbSet(internships);

        _pdfService.Setup(x => x.GetTextFromPdf(It.IsAny<byte[]>())).Returns("test");

        var result = await service.GenerateScreeningPrompt(cvs[0].ApplicationId);

        result.Should().NotBeEmpty();
        result.Should().Contain("test");
    }

    [Test]
    public async Task GenerateDecisionPrompt_ShouldGeneratePrompt_ReturnsPrompt()
    {
        var internships = new List<Internship>
        {
            new Internship
            {
                Id = Guid.NewGuid(),
            }
        };

        var applicaitons = new List<Application>
        {
            new Application
            {
                Id = Guid.NewGuid(),
                InternshipId = internships[0].Id,
            }
        };

        var decisions = new List<Decision>
        {
            new Decision
            {
                Id = Guid.NewGuid(),
                ApplicationId = applicaitons[0].Id,
                AiStagesScore = 0,
                CompanyStagesScores = 0,
                AiStagesReview = "123",
                AiCandidateSummary = "123",
                CompanySummary = "123",
            },
        };

        var evaluationId = Guid.NewGuid();

        var evaluations = new List<Evaluation>
        {
            new Evaluation
            {
                Id = evaluationId,
                AiScore = 5,
                CompanyScore = 5,
                Score = 5,
                Content = "123",
                Cv = new Cv
                {
                    Id = Guid.NewGuid(),
                    InternshipId = internships[0].Id,
                    ApplicationId = applicaitons[0].Id,
                    FileName = "test",
                    FileContent = new byte[]
                    {
                    },
                    EvaluationId = evaluationId
                },
                ApplicationId = applicaitons[0].Id,
            },
        };

        var service = new OpenAiService(_configuration.Object, _db.Object, _pdfService.Object, _mapper.Object);

        _db.Setup(x => x.Decisions).ReturnsDbSet(decisions);
        _db.Setup(x => x.Applications).ReturnsDbSet(applicaitons);
        _db.Setup(x => x.Internships).ReturnsDbSet(internships);
        _db.Setup(x => x.Evaluations).ReturnsDbSet(evaluations);

        _pdfService.Setup(x => x.GetTextFromPdf(It.IsAny<byte[]>())).Returns("test");

        var result = await service.GenerateDecisionPrompt(decisions[0].ApplicationId);

        result.Should().NotBeEmpty();
        result.Should().Contain("5");
        result.Should().Contain("123");
    }
}