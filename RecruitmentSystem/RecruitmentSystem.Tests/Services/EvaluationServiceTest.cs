using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Moq;
using Moq.EntityFrameworkCore;
using RecruitmentSystem.API.Mappings;
using RecruitmentSystem.Business.Interfaces;
using RecruitmentSystem.Business.Services;
using RecruitmentSystem.DataAccess;
using RecruitmentSystem.DataAccess.Migrations;
using RecruitmentSystem.Domain.Dtos.Decision;
using RecruitmentSystem.Domain.Dtos.Evaluation;
using RecruitmentSystem.Domain.Dtos.OpenAi;
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
    public async Task GetStepEvaluations_ShouldReturnStepEvaluationsOfApplication()
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
                Cv = new Cv(),
            },
            new Evaluation()
            {
                ApplicationId = applications[0].Id,
                Interview = new Interview(),
            },
            new Evaluation()
            {
                ApplicationId = applications[0].Id,
                Assessment = new Assessment(),
            },
        };

        _db.Setup(x => x.Applications).ReturnsDbSet(applications);
        _db.Setup(x => x.Evaluations).ReturnsDbSet(evaluations);

        var result = await _evaluationService.GetStepEvaluations(applications[0].Id);

        Assert.That(result, Has.Count.EqualTo(3));
    }

    [Test]
    public async Task GetStepEvaluations_ShouldReturnEmptyList()
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
            },
            new Evaluation()
            {
            },
            new()
            {
            },
        };

        _db.Setup(x => x.Applications).ReturnsDbSet(applications);
        _db.Setup(x => x.Evaluations).ReturnsDbSet(evaluations);

        var result = await _evaluationService.GetStepEvaluations(applications[0].Id);

        Assert.That(result, Has.Count.EqualTo(0));
    }

    [Test]
    public async Task GetApplicationInternshipSetting_ShouldReturnApplicationInternshipSetting()
    {
        var applications = new List<Application>()
        {
            new Application()
            {
                Id = Guid.NewGuid(),
                InternshipId = Guid.NewGuid(),
            },
            new Application()
            {
                Id = Guid.NewGuid(),
                InternshipId = Guid.NewGuid(),
            },
            new Application()
            {
                Id = Guid.NewGuid(),
                InternshipId = Guid.NewGuid(),
            },
        };

        var settings = new List<Setting>()
        {
            new Setting
            {
                Id = default,
                AiScoreWeight = 123,
                CompanyScoreWeight = 123,
                TotalScoreWeight = 123,
                InternshipId = applications[0].InternshipId,
            },
        };

        _db.Setup(x => x.Applications).ReturnsDbSet(applications);
        _db.Setup(x => x.Settings).ReturnsDbSet(settings);

        var result = await _evaluationService.GetApplicationInternshipSetting(applications[0].Id);

        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(settings[0]);
    }

    [Test]
    public async Task CreateEvaluation_ShouldCreateEvaluation()
    {
        var application = new Application()
        {
            Id = Guid.NewGuid(),
        };

        var evaluation = new Evaluation()
        {
            ApplicationId = application.Id,
        };

        var evaluationCreateDto = new EvaluationCreateDto
        {
            CompanyScore = 4,
            Content = "test"
        };

        _db.Setup(x => x.Cvs).ReturnsDbSet(new List<Cv>());
        _db.Setup(x => x.Evaluations).ReturnsDbSet(new List<Evaluation>());
        _db.Setup(x => x.Applications).ReturnsDbSet(new List<Application>());

        await _evaluationService.CreateEvaluation(evaluationCreateDto, application.Id);

        _db.Verify(x => x.SaveChangesAsync(default), Times.Once);
    }

    [Test]
    public async Task AssignAssessmentEvaluation_ShouldAssing()
    {
        var applications = new List<Application>()
        {
            new Application()
            {
                Id = Guid.NewGuid(),
            }
        };

        var assessments = new List<Assessment>()
        {
            new Assessment()
            {
                Id = Guid.NewGuid(),
                ApplicationId = applications[0].Id
            },
        };

        var evaluations = new List<Evaluation>()
        {
            new Evaluation()
            {
                ApplicationId = applications[0].Id,
            }
        };

        _db.Setup(x => x.Applications).ReturnsDbSet(applications);
        _db.Setup(x => x.Assessments).ReturnsDbSet(assessments);
        _db.Setup(x => x.Evaluations).ReturnsDbSet(evaluations);

        await _evaluationService.AssignAssessmentEvaluation(assessments[0].Id, evaluations[0].Id);

        _db.Verify(x => x.SaveChangesAsync(default), Times.Once);
    }

    [Test]
    public async Task AssignInterviewEvaluation_ShouldAssign()
    {
        var applications = new List<Application>()
        {
            new Application()
            {
                Id = Guid.NewGuid(),
            }
        };

        var interviews = new List<Interview>()
        {
            new Interview()
            {
                Id = Guid.NewGuid(),
                ApplicationId = applications[0].Id
            },
        };

        var evaluations = new List<Evaluation>()
        {
            new Evaluation()
            {
                ApplicationId = applications[0].Id,
            }
        };

        _db.Setup(x => x.Applications).ReturnsDbSet(applications);
        _db.Setup(x => x.Interviews).ReturnsDbSet(interviews);
        _db.Setup(x => x.Evaluations).ReturnsDbSet(evaluations);

        await _evaluationService.AssignInterviewEvaluation(interviews[0].Id, evaluations[0].Id);

        _db.Verify(x => x.SaveChangesAsync(default), Times.Once);
    }

    [Test]
    public async Task CreateEvaluationWithAiScore_ShouldCreateEvaluationWithScores()
    {
        var application = new Application()
        {
            Id = Guid.NewGuid(),
        };

        var applications = new List<Application> { application };

        var cv = new Cv()
        {
            ApplicationId = application.Id,
        };

        var cvs = new List<Cv> { cv };

        var evaluation = new Evaluation()
        {
            ApplicationId = application.Id,
        };

        var evaluations = new List<Evaluation>() { evaluation };

        var score = 5;

        _db.Setup(x => x.Cvs).ReturnsDbSet(cvs);
        _db.Setup(x => x.Evaluations).ReturnsDbSet(evaluations);
        _db.Setup(x => x.Applications).ReturnsDbSet(applications);

        await _evaluationService.CreateEvaluationWithAiScore(application, score);

        _db.Verify(x => x.SaveChangesAsync(default), Times.Exactly(2));
    }

    [Test]
    public async Task UpdateDecisionWithAiReview_ShouldUpdateDecision()
    {
        var application = new Application()
        {
            Id = Guid.NewGuid(),
        };

        var applications = new List<Application> { application };

        var cv = new Cv()
        {
            ApplicationId = application.Id,
        };

        var cvs = new List<Cv> { cv };

        var evaluation = new Evaluation()
        {
            ApplicationId = application.Id,
        };

        var evaluations = new List<Evaluation>() { evaluation };

        var decision = new Decision()
        {
            ApplicationId = application.Id,
        };

        var decisions = new List<Decision>() { decision };

        _db.Setup(x => x.Cvs).ReturnsDbSet(cvs);
        _db.Setup(x => x.Evaluations).ReturnsDbSet(evaluations);
        _db.Setup(x => x.Applications).ReturnsDbSet(applications);
        _db.Setup(x => x.Decisions).ReturnsDbSet(decisions);

        var decisionResponse = new DecisionResponse() { finalDecision = 5, stagesReview = "test" };

        await _evaluationService.UpdateDecisionWithAiReview(decisionResponse, decision);

        _db.Verify(x => x.SaveChangesAsync(default), Times.Once);
    }

    [Test]
    public async Task UpdateScreeningCompanyEvaluation_ShouldUpdateEvaluation()
    {
        var evaluation = new Evaluation()
        {
            Id = Guid.NewGuid(),
        };

        var evaluations = new List<Evaluation>() { evaluation };

        var evaluationCreateDto = new EvaluationCreateDto()
        {
            CompanyScore = 5,
            Content = "test"
        };

        _db.Setup(x => x.Evaluations).ReturnsDbSet(evaluations);

        await _evaluationService.UpdateScreeningCompanyEvaluation(evaluation.Id, evaluationCreateDto);

        _db.Verify(x => x.SaveChangesAsync(default), Times.Once);
    }
}