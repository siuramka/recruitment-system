using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using RecruitmentSystem.Business.Interfaces;
using RecruitmentSystem.Business.Services;
using RecruitmentSystem.DataAccess;
using RecruitmentSystem.Domain.Dtos.Decision;
using RecruitmentSystem.Domain.Dtos.Statistics;
using RecruitmentSystem.Domain.Models;

namespace RecruitmentSystem.Tests.Services;

[TestFixture]
[TestOf(typeof(StatisticsService))]
public class StatisticsServiceTests
{
    private Mock<RecruitmentDbContext> db;
    private Mock<IMapper> mapper;
    private Mock<IEvaluationService> evaluationService;
    private StatisticsService service;

    [SetUp]
    public void Setup()
    {
        db = new Mock<RecruitmentDbContext>();
        mapper = new Mock<IMapper>();
        evaluationService = new Mock<IEvaluationService>();
        service = new StatisticsService(mapper.Object, db.Object, evaluationService.Object);
    }

    [Test]
    public async Task GetEvaluationsAsync_ShouldReturnEvaluations()
    {
        // Arrange
        var applicationId = Guid.NewGuid();
        var application = new Application
        {
            Id = applicationId,
            InternshipId = Guid.NewGuid(),
            EndTime = DateTime.Now
        };
        var evaluations = new List<StepEvaluation>
        {
            new StepEvaluation
            {
                StepName = "Screening",
                AiScoreForCandidateInStep = 3,
                CompanyScoreForCandidateInStep = 3
            },
            new StepEvaluation
            {
                StepName = "Interview",
                AiScoreForCandidateInStep = 3,
                CompanyScoreForCandidateInStep = 3
            }
        };
        var finalDecision = new Decision
        {
            AiStagesScore = 3,
            CompanyStagesScores = 3,
        };
        var expected = new List<StepEvaluation>
        {
            new StepEvaluation
            {
                StepName = "Screening",
                AiScoreForCandidateInStep = 3,
                CompanyScoreForCandidateInStep = 3
            },
            new StepEvaluation
            {
                StepName = "Interview",
                AiScoreForCandidateInStep = 3,
                CompanyScoreForCandidateInStep = 3
            },
            new StepEvaluation
            {
                StepName = "Final Decision",
                AiScoreForCandidateInStep = 3,
                CompanyScoreForCandidateInStep = 3
            }
        };
        
        db.Setup(x => x.Applications.Find(applicationId)).Returns(application);
        evaluationService.Setup(x => x.GetStepEvaluations(applicationId)).ReturnsAsync(evaluations);
        evaluationService.Setup(x => x.GetFinalDecision(applicationId)).ReturnsAsync(finalDecision);
        
        // Act
        var result = await service.GetEvaluationsAsync(applicationId);
        
        // Assert
        result.Should().BeEquivalentTo(expected);
    }
    
    [Test]
    public async Task GetApplicationLineChartDataAsync_ApplicationNotEnded_ShouldReturnNull()
    {
        // Arrange
        var applicationId = Guid.NewGuid();
        var application = new Application
        {
            Id = applicationId,
            EndTime = default
        };
        
        db.Setup(x => x.Applications.Find(applicationId)).Returns(application);
        
        // Act
        var result = await service.GetApplicationLineChartDataAsync(applicationId);
        
        // Assert
        result.Should().BeNull();
    }

}