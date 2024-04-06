using AutoMapper;
using Microsoft.Extensions.Configuration;
using Moq;
using Moq.EntityFrameworkCore;
using RecruitmentSystem.API.Mappings;
using RecruitmentSystem.Business.Services;
using RecruitmentSystem.DataAccess;
using RecruitmentSystem.Domain.Dtos.Decision;
using RecruitmentSystem.Domain.Models;

namespace RecruitmentSystem.Tests.Services;

[TestFixture]
[TestOf(typeof(EvaluationService))]
public class EvaluationServiceTest
{
    private IEvaluationService _evaluationService;
    private Mock<RecruitmentDbContext> _db;

    [SetUp]
    public void Setup()
    {
        _db = new Mock<RecruitmentDbContext>();
        var pdfService = new Mock<IPdfService>();
        var openaiService = new Mock<IOpenAiService>();
        var config = new Mock<IConfiguration>();
        
        var myProfile = new MapperProfile();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
        var mapper = new Mapper(configuration);
        
        _evaluationService = new EvaluationService(_db.Object, mapper, openaiService.Object);
    }
    
    [Test]
    public async Task GetEvaluationStepName_ShouldReturnScreeningStep()
    {
        var evaluation = new Evaluation()
        {
            Id = Guid.NewGuid(),
            Cv = new Cv(),
        };
        
        var result = await _evaluationService.GetEvaluationStepName(evaluation);
        
        Assert.That(result, Is.EqualTo(StepType.Screening.ToString()));
    }
    
    [Test]
    public async Task GetEvaluationStepName_ShouldReturnInterviewStep()
    {
        var evaluation = new Evaluation()
        {
            Id = Guid.NewGuid(),
            Interview = new Interview(),
        };
        
        var result = await _evaluationService.GetEvaluationStepName(evaluation);
        
        Assert.That(result, Is.EqualTo(StepType.Interview.ToString()));
    }
    
    [Test]
    public async Task GetEvaluationStepName_ShouldReturnAssessmentStep()
    {
        var evaluation = new Evaluation()
        {
            Id = Guid.NewGuid(),
            Assessment = new Assessment(),
        };
        
        var result = await _evaluationService.GetEvaluationStepName(evaluation);
        
        Assert.That(result, Is.EqualTo(StepType.Assessment.ToString()));
    }
    
    [Test]
    public async Task GetApplicationEvaluations_ShouldReturnApplicationEvaluations()
    {
        var applications = new List<Application>()
        {
            new Application()
            {
                Id = Guid.NewGuid(),
            },
            new Application()
            {
                Id = Guid.NewGuid(),
            },
            new Application()
            {
                Id = Guid.NewGuid(),
            },
        };

        var evaluations = new List<Evaluation>()
        {
            new Evaluation()
            {
                ApplicationId = applications[0].Id,
            },
        };
        
        _db.Setup(x => x.Applications).ReturnsDbSet(applications);
        _db.Setup(x => x.Evaluations).ReturnsDbSet(evaluations);
        
        var result = await _evaluationService.GetApplicationEvaluations(applications[0].Id);
        
        Assert.That(result, Has.Count.EqualTo(1));
    }
    
    [Test]
    public async Task GetFinalDecision_ShouldReturnFinalDecision()
    {
        var applications = new List<Application>()
        {
            new Application()
            {
                Id = Guid.NewGuid(),
            },
            new Application()
            {
                Id = Guid.NewGuid(),
            },
            new Application()
            {
                Id = Guid.NewGuid(),
            },
        };

        var decisions = new List<Decision>
        {
            new Decision()
            {
                Id = Guid.NewGuid(),
                ApplicationId = applications[0].Id,
            }
        };
        
        _db.Setup(x => x.Applications).ReturnsDbSet(applications);
        _db.Setup(x => x.Decisions).ReturnsDbSet(decisions);
        
        var result = await _evaluationService.GetFinalDecision(applications[0].Id);
        
        Assert.That(result.ApplicationId, Is.EqualTo(applications[0].Id));
    }

    [Test]
    public async Task CalculateCorrelation_CorrelationShouldBe1()
    {
        int[] aiScores = { 1, 2, 3, 4, 5 };
        int[] companyScores = { 1, 2, 3, 4, 5 };
        
        var result = _evaluationService.CalculateCorrelation(aiScores, companyScores);
        
        Assert.That(result, Is.EqualTo(1));
    }

    [Test]
    public async Task GetCompanyScores_ShouldReturnScores()
    {
        var stepEvaluations = new List<StepEvaluation>
        {
            new StepEvaluation()
            {
                CompanyScoreForCandidateInStep = 1,
            },
            new StepEvaluation()
            {
                CompanyScoreForCandidateInStep = 2,
            },
        };
        
        var finalDecision = new Decision()
        {
            CompanyStagesScores = 5,
        };
        
        var result = _evaluationService.GetCompanyScores(stepEvaluations, finalDecision);
        var shouldBeResult = new List<int> { 1, 2, 5 };
        
        Assert.That(result, Is.EqualTo(shouldBeResult));
    }
    
    [Test]
    public async Task GetAiScores_ShouldReturnScores()
    {
        var stepEvaluations = new List<StepEvaluation>
        {
            new StepEvaluation()
            {
                AiScoreForCandidateInStep = 1,
            },
            new StepEvaluation()
            {
                AiScoreForCandidateInStep = 2,
            },
        };
        
        var finalDecision = new Decision()
        {
            AiStagesScore = 5,
        };
        
        var result = _evaluationService.GetAiScores(stepEvaluations, finalDecision);
        var shouldBeResult = new List<int> { 1, 2, 5 };
        
        Assert.That(result, Is.EqualTo(shouldBeResult));
    }

    [Test]
    public async Task GetApplicationInternshipSetting_ShouldReturnSetting()
    {
        var internships = new List<Internship>
        {
            new Internship()
            {
                Id = Guid.NewGuid(),
            },
        };
        
        var applications = new List<Application>
        {
            new Application()
            {
                Id = Guid.NewGuid(),
                InternshipId = internships[0].Id,
            }
        };
        
        var settings = new List<Setting>
        {
            new Setting()
            {
                InternshipId = internships[0].Id,
            }
        };
        
        _db.Setup(x => x.Internships).ReturnsDbSet(internships);
        _db.Setup(x => x.Applications).ReturnsDbSet(applications);
        _db.Setup(x => x.Settings).ReturnsDbSet(settings);
        
        var result = await _evaluationService.GetApplicationInternshipSetting(applications[0].Id);
        
        Assert.That(result.InternshipId, Is.EqualTo(settings[0].InternshipId));
    }

    [Test]
    public async Task EvaluateInterviewAiScore_ShouldEvaluate()
    {
        
    }
}