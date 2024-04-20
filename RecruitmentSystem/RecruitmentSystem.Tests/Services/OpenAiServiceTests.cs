using RecruitmentSystem.Business.Services;
using RecruitmentSystem.Domain.Dtos.Decision;
using RecruitmentSystem.Domain.Models;

namespace RecruitmentSystem.Tests.Services;

public class OpenAiServiceTests
{
    
    
    [Test]
    public async Task GenerateScreeningPrompt_ShouldReturnScreeningPrompt()
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
        var expected = "Screening prompt";
        
        db.Setup(x => x.Applications.FindAsync(applicationId)).ReturnsAsync(application);
        db.Setup(x => x.StepEvaluations.Where(x => x.ApplicationId == applicationId)).Returns(evaluations);
        db.Setup(x => x.Decisions.Where(x => x.ApplicationId == applicationId)).Returns(finalDecision);

        // Act
        var result = await service.GenerateScreeningPrompt(applicationId);

        // Assert
        Assert.AreEqual(expected, result);
    }
}